using System.CommandLine;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Dafny;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Type = System.Type;

namespace IntegrationTests;

public class GenerateParsedAst {
  private static HashSet<Type> excludedTypes = [typeof(DafnyOptions)];
  private static Dictionary<Type,Type> mappedTypes = new() {
    { typeof(Guid), typeof(string) }
  };
  
  public static Command GetCommand() {
    var result = new Command("generate-parsed-ast", "");
    var fileArgument = new Argument<FileInfo>();
    result.AddArgument(fileArgument);
    result.SetHandler(file => Handle(file.Name), fileArgument);
    return result;
  }

  public static async Task Handle(string file) {
    var program = typeof(LiteralModuleDecl);
    await File.WriteAllTextAsync(file, GenerateAll(program));
  }
  
  public static string GenerateAll(Type rootType) {
    var assembly = rootType.Assembly;
    var inheritors = assembly.GetTypes().Where(t => t.BaseType != null).GroupBy(t => t.BaseType!).ToDictionary(
      g => g.Key, 
      g => (ISet<Type>)g.ToHashSet());
    
      // Create a namespace
      var namespaceDeclaration = NamespaceDeclaration(
          IdentifierName("GeneratedCode"))
          .NormalizeWhitespace();

      var toVisit = new Stack<Type>();
      toVisit.Push(rootType);
      var visited = new HashSet<Type>();
      while (toVisit.Any()) {
        var current = toVisit.Pop();
        if (!visited.Add(current)) {
          continue;
        }

        if (current.Namespace != rootType.Namespace) {
          continue;
        }
        var classDeclaration = GenerateClass(current, toVisit, inheritors);
        if (classDeclaration != null) {
          namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);
        }
      }

      // Create the compilation unit
      var compilationUnit = CompilationUnit()
          .AddUsings(UsingDirective(IdentifierName("System")))
          .AddMembers(namespaceDeclaration)
          .NormalizeWhitespace();

      // using (var workspace = new AdhocWorkspace())
      // {
      //   // Get formatting options
      //   OptionSet options = workspace.Options;
      //       
      //   // Format the compilation unit
      //   SyntaxNode formattedNode = Formatter.Format(compilationUnit, workspace, options);
      //       
      //   // Return the formatted code
      //   return formattedNode.ToFullString();
      // }
      return compilationUnit.ToFullString();
  }

  private static BaseTypeDeclarationSyntax? GenerateClass(Type type, Stack<Type> toVisit, IDictionary<Type, ISet<Type>> inheritors)
  {
    if (type.IsEnum) {
      var enumm = EnumDeclaration(type.Name);
      foreach (var name in Enum.GetNames(type)) {
        enumm = enumm.AddMembers(EnumMemberDeclaration(name));
      }

      return enumm;
    }
    
    // Create a class
    var classDeclaration = ClassDeclaration(type.Name)
      .AddModifiers(Token(SyntaxKind.PublicKeyword));
    List<MemberDeclarationSyntax> newFields = new();

    if (type.BaseType != null) {
      toVisit.Push(type.BaseType);
      classDeclaration = classDeclaration.WithBaseList(BaseList(SeparatedList(new List<BaseTypeSyntax> {
        SimpleBaseType(ParseTypeName(type.BaseType.ToGenericTypeString())) })));
    }

    if (inheritors.TryGetValue(type, out var children)) {
      foreach (var child in children) {
        toVisit.Push(child);
      }
    }

    var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
    var constructor = constructors.MaxBy(c => c.GetParameters().Length);
    if (constructor == null) {
      return null;
    }

    var fields = type.GetFields().ToDictionary(f => f.Name.ToLower(), f => f);
    var properties = type.GetProperties().ToDictionary(p => p.Name.ToLower(), p => p);
    
    foreach (var parameter in constructor.GetParameters()) {
      if (excludedTypes.Contains(parameter.ParameterType)) {
        continue;
      }

      var memberInfo = fields.GetValueOrDefault(parameter.Name!.ToLower()) ??
                       (MemberInfo?)properties.GetValueOrDefault(parameter.Name.ToLower());

      if (memberInfo != null && memberInfo.DeclaringType != type) {
        continue;
      }
      
      var usedTyped = parameter.ParameterType;
      if (mappedTypes.TryGetValue(usedTyped, out var newType)) {
        usedTyped = newType;
      }
      
      newFields.Add(FieldDeclaration(VariableDeclaration(
          
          ParseTypeName(usedTyped.ToGenericTypeString()),
        SeparatedList([VariableDeclarator(Identifier(parameter.Name!))]))));
      
      toVisit.Push(parameter.ParameterType);
    }

    // Combine everything
    classDeclaration = classDeclaration
      .AddMembers(newFields.ToArray());
    return classDeclaration;
  }
}
  
public static class TypeExtensions
{
  public static string ToGenericTypeString(this Type t)
  {
    if (!t.IsGenericType) {
      return t.Name;
    }

    string genericTypeName = t.GetGenericTypeDefinition().Name;
    genericTypeName = genericTypeName.Substring(0,
      genericTypeName.IndexOf('`'));
    string genericArgs = string.Join(",",
      t.GetGenericArguments()
        .Select(ta => ToGenericTypeString(ta)).ToArray());
    return genericTypeName + "<" + genericArgs + ">";
  }
}