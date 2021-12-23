using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Text.Json;
using System.Threading;
using Microsoft.CodeAnalysis;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.JsonRpc.Client;
using OmniSharp.Extensions.JsonRpc.Serialization;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.Dafny.LanguageServer.IntegrationTest.Various {    
  class AlwaysOutputFilter : IOutputFilter { public bool ShouldOutput(object value) => true; }

  [TestClass]
  public class ShutdownTest {
    
    [TestMethod]
    public async Task LanguageServerStaysAliveIfNoParentIdIsProvided() {
      var process = await StartLanguageServerRunnerProcess();
      
      var languageServerProcessId = await process.StandardOutput.ReadLineAsync();
      
      var initializeMessage = GetLspInitializeMessage(null);
      await process.StandardInput.WriteAsync(initializeMessage);
      
      var initializedResponseFirstLine = await process.StandardOutput.ReadLineAsync();
      Assert.IsFalse(string.IsNullOrEmpty(initializedResponseFirstLine));
      
      var error = await process.StandardError.ReadToEndAsync();
      var didExit = process.WaitForExit(-1);
      Assert.IsTrue(didExit);
      Assert.IsNotNull(languageServerProcessId, error);
      
      Thread.Sleep(100); // Give the process some time to die
      
      Assert.IsFalse(string.IsNullOrEmpty(initializedResponseFirstLine));
      try {
        var languageServer = Process.GetProcessById(int.Parse(languageServerProcessId));
        languageServer.Kill();
      } catch (ArgumentException e) {
        Assert.Fail("Language server should not have killed itself if it doesn't know the parent.");
      }
    }

    [TestMethod]
    public async Task LanguageServerShutsDownIfParentDies() {
      var process = await StartLanguageServerRunnerProcess();
      
      var languageServerProcessId = await process.StandardOutput.ReadLineAsync();
      
      var initializeMessage = GetLspInitializeMessage(process.Id);
      await process.StandardInput.WriteAsync(initializeMessage);
      
      var initializedResponseFirstLine = await process.StandardOutput.ReadLineAsync();
      Assert.IsFalse(string.IsNullOrEmpty(initializedResponseFirstLine));
      
      var error = await process.StandardError.ReadToEndAsync();
      var didExit = process.WaitForExit(-1);
      Assert.IsTrue(didExit);
      Assert.IsNotNull(languageServerProcessId, error);
      Assert.IsTrue(string.IsNullOrEmpty(error), error);
      
      // Wait for the language server to kill itself by waiting until it closes the output stream.
      await process.StandardOutput.ReadToEndAsync();
      Thread.Sleep(100); // Give the process some time to die
      try {
        var languageServer = Process.GetProcessById(int.Parse(languageServerProcessId));
        languageServer.Kill();
        Assert.Fail("Language server should have killed itself if the parent is gone.");
      } catch (ArgumentException e) {
        // Language server process is not running, pass the test.
      }
    }

    private static async Task<Process> StartLanguageServerRunnerProcess() {
      var languageServerBinary = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DafnyLanguageServer");
      var languageServerRunnerPath = await CreateDotNetDllThatStartsGivenFilepath(languageServerBinary);

      var processInfo = new ProcessStartInfo("dotnet", languageServerRunnerPath)
      {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        RedirectStandardInput = true,
        UseShellExecute = false
      };
      return Process.Start(processInfo)!;
    }

    private static string GetLspInitializeMessage(int? processId)
    {
      var buffer = new MemoryStream();
      OutputHandler outputHandler = new OutputHandler(PipeWriter.Create(buffer), new JsonRpcSerializer(),
        new List<IOutputFilter> { new AlwaysOutputFilter() },
        TaskPoolScheduler.Default, null);
      outputHandler.Send(new OutgoingRequest {
        Id = 1,
        Method = "initialize",
        Params = new InitializeParams
        {
          ProcessId = processId,
          ClientInfo = new ClientInfo(),
          Capabilities = new ClientCapabilities(),
        }
      });
      outputHandler.StopAsync().Wait();
      return Encoding.ASCII.GetString(buffer.ToArray());
    }
    
    private static async Task<string> CreateDotNetDllThatStartsGivenFilepath(string filePathToStart)
    {
      var code = @$"using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class Foo {{
  public static async Task<int> Main(string[] args) {{
    var processInfo = new ProcessStartInfo(""{filePathToStart}"") {{
      // Prevents keeping stdio open after the outer process closes. 
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      UseShellExecute = false
    }};
    using var process = Process.Start(processInfo)!;
    await Console.Out.WriteLineAsync(process.Id.ToString());
    var firstLine = await process.StandardOutput.ReadLineAsync();
    await Console.Out.WriteLineAsync(firstLine);
    return 0;
  }}
}}";
      var temp = Path.GetTempFileName();
      var compilation = CSharpCompilation.Create("tempAssembly");
      var standardLibraries = new List<string>()
      {
        "System.Runtime",
        "System.Console",
        "System.Diagnostics.Process",
        "System.ComponentModel.Primitives"
      };
      compilation = compilation.AddReferences(standardLibraries.Select(fileName =>
        MetadataReference.CreateFromFile(Assembly.Load(fileName).Location)));
      compilation = compilation.WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication))
        .AddReferences(
          MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
          MetadataReference.CreateFromFile(Assembly.Load("mscorlib").Location));
      var syntaxTree = CSharpSyntaxTree.ParseText(code);
      compilation = compilation.AddSyntaxTrees(syntaxTree);
      var assemblyPath = temp + ".dll";
      var result = compilation.Emit(assemblyPath);

      var configuration = JsonSerializer.Serialize(
        new
        {
          runtimeOptions = new
          {
            tfm = "net5.0",
            framework = new
            {
              name = "Microsoft.NETCore.App",
              version = "5.0.0",
              rollForward = "LatestMinor"
            }
          }
        }, new JsonSerializerOptions() { WriteIndented = true });
      await File.WriteAllTextAsync(temp + ".runtimeconfig.json", configuration + Environment.NewLine);

      Assert.IsTrue(result.Success, string.Join("\n", result.Diagnostics.Select(d => d.ToString())));
      return assemblyPath;
    }
  }
}