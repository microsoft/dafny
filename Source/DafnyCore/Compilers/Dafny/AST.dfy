module {:extern "DAST"} DAST {
  datatype Module = Module(name: string, body: seq<ModuleItem>)

  datatype ModuleItem = Module(Module) | Class(Class) | Newtype(Newtype) | Datatype(Datatype)

  datatype Newtype = Newtype(name: string, base: Type)

  datatype Type = Path(seq<Ident>) | Passthrough(string) | TypeArg(Ident)

  datatype Ident = Ident(id: string)

  datatype Class = Class(name: string, body: seq<ClassItem>)

  datatype Datatype = Datatype(name: string, ctors: seq<DatatypeCtor>, body: seq<ClassItem>)

  datatype DatatypeCtor = DatatypeCtor(name: string, args: seq<Formal>, hasAnyArgs: bool /* includes ghost */)

  datatype ClassItem = Method(Method) | Field(Formal)

  datatype Formal = Formal(name: string, typ: Type)

  datatype Method = Method(isStatic: bool, name: string, typeArgs: seq<Type>, params: seq<Formal>, body: seq<Statement>, outTypes: seq<Type>, outVars: Optional<seq<Ident>>)

  datatype Optional<T> = Some(T) | None

  datatype Statement =
    DeclareVar(name: string, typ: Type, maybeValue: Optional<Expression>) |
    Assign(name: string, value: Expression) |
    If(cond: Expression, thn: seq<Statement>, els: seq<Statement>) |
    Call(enclosing: Optional<Type>, name: string, args: seq<Expression>, outs: Optional<seq<Ident>>) |
    Return(expr: Expression) |
    Print(Expression) |
    Todo(reason: string)

  datatype Expression =
    Literal(Literal) |
    Ident(string) |
    Tuple(seq<Expression>) |
    DatatypeValue(typ: Type, variant: string, contents: seq<(string, Expression)>) |
    BinOp(op: string, left: Expression, right: Expression) |
    Call(enclosing: Optional<Type>, on: Optional<Expression>, name: string, args: seq<Expression>) |
    InitializationValue(typ: Type) |
    Todo(reason: string)

  datatype Literal = BoolLiteral(bool) | IntLiteral(int) | DecLiteral(string) | StringLiteral(string)
}
