// Dafny program the_program compiled into C#
// To recompile, you will need the libraries
//     System.Runtime.Numerics.dll System.Collections.Immutable.dll
// but the 'dotnet' tool in net6.0 should pick those up automatically.
// Optionally, you may want to include compiler switches like
//     /debug /nowarn:162,164,168,183,219,436,1717,1718

using System;
using System.Numerics;
using System.Collections;
#pragma warning disable CS0164 // This label has not been referenced
#pragma warning disable CS0162 // Unreachable code detected
#pragma warning disable CS1717 // Assignment made to same variable

namespace DCOMP {


  public partial class COMP {
    public COMP() {
      this.error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.Default();
      this.optimizations = Dafny.Sequence<Func<RAST._IMod, RAST._IMod>>.Empty;
      this._rootType = Defs.RootType.Default();
      this._charType = Defs.CharType.Default();
      this._pointerType = Defs.PointerType.Default();
    }
    public RAST._IType Object(RAST._IType underlying) {
      if (((this).pointerType).is_Raw) {
        return RAST.__default.PtrType(underlying);
      } else {
        return RAST.__default.ObjectType(underlying);
      }
    }
    public Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> error {get; set;}
    public Dafny.ISequence<Func<RAST._IMod, RAST._IMod>> optimizations {get; set;}
    public void __ctor(Defs._ICharType charType, Defs._IPointerType pointerType, Defs._IRootType rootType)
    {
      (this)._charType = charType;
      (this)._pointerType = pointerType;
      (this)._rootType = rootType;
      (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
      (this).optimizations = Dafny.Sequence<Func<RAST._IMod, RAST._IMod>>.FromElements(ExpressionOptimization.__default.apply, FactorPathsOptimization.__default.apply((this).thisFile));
    }
    public bool HasAttribute(Dafny.ISequence<DAST._IAttribute> attributes, Dafny.ISequence<Dafny.Rune> name)
    {
      return Dafny.Helpers.Id<Func<Dafny.ISequence<DAST._IAttribute>, Dafny.ISequence<Dafny.Rune>, bool>>((_0_attributes, _1_name) => Dafny.Helpers.Quantifier<DAST._IAttribute>((_0_attributes).UniqueElements, false, (((_exists_var_0) => {
        DAST._IAttribute _2_attribute = (DAST._IAttribute)_exists_var_0;
        return ((_0_attributes).Contains(_2_attribute)) && ((((_2_attribute).dtor_name).Equals(_1_name)) && ((new BigInteger(((_2_attribute).dtor_args).Count)).Sign == 0));
      }))))(attributes, name);
    }
    public DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> GenModule(DAST._IModule mod, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> containingPath)
    {
      DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> s = DafnyCompilerRustUtils.SeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule>.Default();
      _System._ITuple2<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs0 = DafnyCompilerRustUtils.__default.DafnyNameToContainingPathAndName((mod).dtor_name, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements());
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _0_innerPath = _let_tmp_rhs0.dtor__0;
      Dafny.ISequence<Dafny.Rune> _1_innerName = _let_tmp_rhs0.dtor__1;
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2_containingPath;
      _2_containingPath = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(containingPath, _0_innerPath);
      Dafny.ISequence<Dafny.Rune> _3_modName;
      _3_modName = Defs.__default.escapeName(_1_innerName);
      if (((mod).dtor_body).is_None) {
        s = DafnyCompilerRustUtils.GatheringModule.Wrap(Defs.__default.ContainingPathToRust(_2_containingPath), RAST.Mod.create_ExternMod(_3_modName));
      } else {
        Defs._IExternAttribute _4_optExtern;
        _4_optExtern = Defs.__default.ExtractExternMod(mod);
        Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _5_attributes;
        _5_attributes = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
        if ((this).HasAttribute((mod).dtor_attributes, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("rust_cfg_test"))) {
          _5_attributes = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#[cfg(test)]"));
        }
        Dafny.ISequence<RAST._IModDecl> _6_body;
        DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _7_allmodules;
        Dafny.ISequence<RAST._IModDecl> _out0;
        DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _out1;
        (this).GenModuleBody(((mod).dtor_body).dtor_value, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_2_containingPath, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_1_innerName)), out _out0, out _out1);
        _6_body = _out0;
        _7_allmodules = _out1;
        if ((_4_optExtern).is_SimpleExtern) {
          if ((mod).dtor_requiresExterns) {
            _6_body = Dafny.Sequence<RAST._IModDecl>.Concat(Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_UseDecl(RAST.Use.create(RAST.Visibility.create_PUB(), ((((this).thisFile).MSel(Defs.__default.DAFNY__EXTERN__MODULE)).MSels(Defs.__default.SplitRustPathElement(Defs.__default.ReplaceDotByDoubleColon((_4_optExtern).dtor_overrideName), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"))))), _6_body);
          }
        } else if ((_4_optExtern).is_AdvancedExtern) {
          (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Externs on modules can only have 1 string argument"));
        } else if ((_4_optExtern).is_UnsupportedExtern) {
          (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some((_4_optExtern).dtor_reason);
        }
        s = DafnyCompilerRustUtils.GatheringModule.MergeSeqMap(DafnyCompilerRustUtils.GatheringModule.Wrap(Defs.__default.ContainingPathToRust(_2_containingPath), RAST.Mod.create_Mod((mod).dtor_docString, _5_attributes, _3_modName, _6_body)), _7_allmodules);
      }
      return s;
    }
    public void GenModuleBody(Dafny.ISequence<DAST._IModuleItem> body, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> containingPath, out Dafny.ISequence<RAST._IModDecl> s, out DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> allmodules)
    {
      s = Dafny.Sequence<RAST._IModDecl>.Empty;
      allmodules = DafnyCompilerRustUtils.SeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule>.Default();
      s = Dafny.Sequence<RAST._IModDecl>.FromElements();
      allmodules = DafnyCompilerRustUtils.SeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule>.Empty();
      BigInteger _hi0 = new BigInteger((body).Count);
      for (BigInteger _0_i = BigInteger.Zero; _0_i < _hi0; _0_i++) {
        Dafny.ISequence<RAST._IModDecl> _1_generated = Dafny.Sequence<RAST._IModDecl>.Empty;
        DAST._IModuleItem _source0 = (body).Select(_0_i);
        {
          if (_source0.is_Module) {
            DAST._IModule _2_m = _source0.dtor_Module_a0;
            DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _3_mm;
            DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _out0;
            _out0 = (this).GenModule(_2_m, containingPath);
            _3_mm = _out0;
            allmodules = DafnyCompilerRustUtils.GatheringModule.MergeSeqMap(allmodules, _3_mm);
            _1_generated = Dafny.Sequence<RAST._IModDecl>.FromElements();
            goto after_match0;
          }
        }
        {
          if (_source0.is_Class) {
            DAST._IClass _4_c = _source0.dtor_Class_a0;
            Dafny.ISequence<RAST._IModDecl> _out1;
            _out1 = (this).GenClass(_4_c, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(containingPath, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements((_4_c).dtor_name)));
            _1_generated = _out1;
            goto after_match0;
          }
        }
        {
          if (_source0.is_Trait) {
            DAST._ITrait _5_t = _source0.dtor_Trait_a0;
            Dafny.ISequence<RAST._IModDecl> _out2;
            _out2 = (this).GenTrait(_5_t, containingPath);
            _1_generated = _out2;
            goto after_match0;
          }
        }
        {
          if (_source0.is_Newtype) {
            DAST._INewtype _6_n = _source0.dtor_Newtype_a0;
            Dafny.ISequence<RAST._IModDecl> _out3;
            _out3 = (this).GenNewtype(_6_n, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(containingPath, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements((_6_n).dtor_name)));
            _1_generated = _out3;
            goto after_match0;
          }
        }
        {
          if (_source0.is_SynonymType) {
            DAST._ISynonymType _7_s = _source0.dtor_SynonymType_a0;
            Dafny.ISequence<RAST._IModDecl> _out4;
            _out4 = (this).GenSynonymType(_7_s);
            _1_generated = _out4;
            goto after_match0;
          }
        }
        {
          DAST._IDatatype _8_d = _source0.dtor_Datatype_a0;
          Dafny.ISequence<RAST._IModDecl> _out5;
          _out5 = (this).GenDatatype(_8_d, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(containingPath, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements((_8_d).dtor_name)));
          _1_generated = _out5;
        }
      after_match0: ;
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, _1_generated);
      }
    }
    public void GenTypeParam(DAST._ITypeArgDecl tp, out DAST._IType typeArg, out RAST._ITypeParamDecl typeParam)
    {
      typeArg = DAST.Type.Default();
      typeParam = RAST.TypeParamDecl.Default();
      typeArg = DAST.Type.create_TypeArg((tp).dtor_name);
      bool _0_supportsEquality;
      _0_supportsEquality = false;
      bool _1_supportsDefault;
      _1_supportsDefault = false;
      Dafny.ISequence<RAST._IType> _2_genTpConstraint;
      _2_genTpConstraint = Dafny.Sequence<RAST._IType>.FromElements();
      BigInteger _hi0 = new BigInteger(((tp).dtor_bounds).Count);
      for (BigInteger _3_i = BigInteger.Zero; _3_i < _hi0; _3_i++) {
        DAST._ITypeArgBound _4_bound;
        _4_bound = ((tp).dtor_bounds).Select(_3_i);
        DAST._ITypeArgBound _source0 = _4_bound;
        {
          if (_source0.is_SupportsEquality) {
            _0_supportsEquality = true;
            goto after_match0;
          }
        }
        {
          if (_source0.is_SupportsDefault) {
            _1_supportsDefault = true;
            goto after_match0;
          }
        }
        {
          DAST._IType _5_typ = _source0.dtor_typ;
          RAST._IType _6_tpe;
          RAST._IType _out0;
          _out0 = (this).GenType(_5_typ, Defs.GenTypeContext.ForTraitParents());
          _6_tpe = _out0;
          RAST._IType _7_upcast__tpe;
          _7_upcast__tpe = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("UpcastBox"))).AsType()).Apply(Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_DynType(_6_tpe)));
          _2_genTpConstraint = Dafny.Sequence<RAST._IType>.Concat(_2_genTpConstraint, Dafny.Sequence<RAST._IType>.FromElements(_7_upcast__tpe));
        }
      after_match0: ;
      }
      if (_1_supportsDefault) {
        _2_genTpConstraint = Dafny.Sequence<RAST._IType>.Concat(Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.DefaultTrait), _2_genTpConstraint);
      }
      RAST._IType _8_dafnyType;
      if (_0_supportsEquality) {
        _8_dafnyType = RAST.__default.DafnyTypeEq;
      } else {
        _8_dafnyType = RAST.__default.DafnyType;
      }
      _2_genTpConstraint = Dafny.Sequence<RAST._IType>.Concat(Dafny.Sequence<RAST._IType>.FromElements(_8_dafnyType), _2_genTpConstraint);
      typeParam = RAST.TypeParamDecl.create(Defs.__default.escapeName(((tp).dtor_name)), _2_genTpConstraint);
    }
    public void GenTypeParameters(Dafny.ISequence<DAST._ITypeArgDecl> @params, out Dafny.ISequence<DAST._IType> typeParamsSeq, out Dafny.ISequence<RAST._IType> rTypeParams, out Dafny.ISequence<RAST._ITypeParamDecl> rTypeParamsDecls)
    {
      typeParamsSeq = Dafny.Sequence<DAST._IType>.Empty;
      rTypeParams = Dafny.Sequence<RAST._IType>.Empty;
      rTypeParamsDecls = Dafny.Sequence<RAST._ITypeParamDecl>.Empty;
      typeParamsSeq = Dafny.Sequence<DAST._IType>.FromElements();
      rTypeParams = Dafny.Sequence<RAST._IType>.FromElements();
      rTypeParamsDecls = Dafny.Sequence<RAST._ITypeParamDecl>.FromElements();
      if ((new BigInteger((@params).Count)).Sign == 1) {
        BigInteger _hi0 = new BigInteger((@params).Count);
        for (BigInteger _0_tpI = BigInteger.Zero; _0_tpI < _hi0; _0_tpI++) {
          DAST._ITypeArgDecl _1_tp;
          _1_tp = (@params).Select(_0_tpI);
          DAST._IType _2_typeArg;
          RAST._ITypeParamDecl _3_typeParam;
          DAST._IType _out0;
          RAST._ITypeParamDecl _out1;
          (this).GenTypeParam(_1_tp, out _out0, out _out1);
          _2_typeArg = _out0;
          _3_typeParam = _out1;
          RAST._IType _4_rType;
          RAST._IType _out2;
          _out2 = (this).GenType(_2_typeArg, Defs.GenTypeContext.@default());
          _4_rType = _out2;
          typeParamsSeq = Dafny.Sequence<DAST._IType>.Concat(typeParamsSeq, Dafny.Sequence<DAST._IType>.FromElements(_2_typeArg));
          rTypeParams = Dafny.Sequence<RAST._IType>.Concat(rTypeParams, Dafny.Sequence<RAST._IType>.FromElements(_4_rType));
          rTypeParamsDecls = Dafny.Sequence<RAST._ITypeParamDecl>.Concat(rTypeParamsDecls, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(_3_typeParam));
        }
      }
    }
    public bool IsSameResolvedTypeAnyArgs(DAST._IResolvedType r1, DAST._IResolvedType r2)
    {
      return (((r1).dtor_path).Equals((r2).dtor_path)) && (object.Equals((r1).dtor_kind, (r2).dtor_kind));
    }
    public bool IsSameResolvedType(DAST._IResolvedType r1, DAST._IResolvedType r2)
    {
      return ((this).IsSameResolvedTypeAnyArgs(r1, r2)) && (((r1).dtor_typeArgs).Equals((r2).dtor_typeArgs));
    }
    public Dafny.ISet<Dafny.ISequence<Dafny.Rune>> GatherTypeParamNames(Dafny.ISet<Dafny.ISequence<Dafny.Rune>> types, RAST._IType typ)
    {
      return (typ).Fold<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>(types, ((System.Func<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>, RAST._IType, Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>)((_0_types, _1_currentType) => {
        return (((_1_currentType).is_TIdentifier) ? (Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_0_types, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements((_1_currentType).dtor_name))) : (_0_types));
      })));
    }
    public void GenField(DAST._IField field, out RAST._IField rfield, out RAST._IAssignIdentifier fieldInit, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> usedTypeParams)
    {
      rfield = RAST.Field.Default();
      fieldInit = RAST.AssignIdentifier.Default();
      usedTypeParams = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      usedTypeParams = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      RAST._IType _0_fieldType;
      RAST._IType _out0;
      _out0 = (this).GenType(((field).dtor_formal).dtor_typ, Defs.GenTypeContext.@default());
      _0_fieldType = _out0;
      if (!((field).dtor_isConstant)) {
        _0_fieldType = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Field"))).AsType()).Apply(Dafny.Sequence<RAST._IType>.FromElements(_0_fieldType));
      }
      usedTypeParams = (this).GatherTypeParamNames(usedTypeParams, _0_fieldType);
      Dafny.ISequence<Dafny.Rune> _1_fieldRustName;
      _1_fieldRustName = Defs.__default.escapeVar(((field).dtor_formal).dtor_name);
      rfield = RAST.Field.create(RAST.Visibility.create_PUB(), RAST.Formal.create(_1_fieldRustName, _0_fieldType));
      Std.Wrappers._IOption<DAST._IExpression> _source0 = (field).dtor_defaultValue;
      {
        if (_source0.is_Some) {
          DAST._IExpression _2_e = _source0.dtor_value;
          {
            RAST._IExpr _3_expr;
            Defs._IOwnership _4___v0;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _5___v1;
            RAST._IExpr _out1;
            Defs._IOwnership _out2;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out3;
            (this).GenExpr(_2_e, Defs.SelfInfo.create_NoSelf(), Defs.Environment.Empty(), Defs.Ownership.create_OwnershipOwned(), out _out1, out _out2, out _out3);
            _3_expr = _out1;
            _4___v0 = _out2;
            _5___v1 = _out3;
            fieldInit = RAST.AssignIdentifier.create(_1_fieldRustName, _3_expr);
          }
          goto after_match0;
        }
      }
      {
        {
          RAST._IExpr _6_default;
          _6_default = RAST.__default.std__default__Default__default;
          if ((_0_fieldType).IsObjectOrPointer()) {
            _6_default = (_0_fieldType).ToNullExpr();
          }
          fieldInit = RAST.AssignIdentifier.create(_1_fieldRustName, _6_default);
        }
      }
    after_match0: ;
    }
    public void GetName(Dafny.ISequence<DAST._IAttribute> attributes, Dafny.ISequence<Dafny.Rune> name, Dafny.ISequence<Dafny.Rune> kind, out Dafny.ISequence<Dafny.Rune> rName, out Defs._IExternAttribute @extern)
    {
      rName = Dafny.Sequence<Dafny.Rune>.Empty;
      @extern = Defs.ExternAttribute.Default();
      @extern = Defs.__default.ExtractExtern(attributes, name);
      if ((@extern).is_SimpleExtern) {
        rName = (@extern).dtor_overrideName;
      } else {
        rName = Defs.__default.escapeName(name);
        if ((@extern).is_AdvancedExtern) {
          (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Multi-argument externs not supported for "), kind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" yet")));
        }
      }
    }
    public Dafny.ISequence<RAST._IModDecl> GenTraitImplementations(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> path, Dafny.ISequence<RAST._IType> rTypeParams, Dafny.ISequence<RAST._ITypeParamDecl> rTypeParamsDecls, Dafny.ISequence<DAST._IType> superTraitTypes, Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> traitBodies, Defs._IExternAttribute @extern, bool supportsEquality, Dafny.ISequence<Dafny.Rune> kind)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      s = Dafny.Sequence<RAST._IModDecl>.FromElements();
      RAST._IPath _0_genPath;
      RAST._IPath _out0;
      _out0 = (this).GenPath(path, true);
      _0_genPath = _out0;
      RAST._IType _1_genSelfPath;
      _1_genSelfPath = (_0_genPath).AsType();
      RAST._IExpr _2_genPathExpr;
      _2_genPathExpr = (_0_genPath).AsExpr();
      BigInteger _hi0 = new BigInteger((superTraitTypes).Count);
      for (BigInteger _3_i = BigInteger.Zero; _3_i < _hi0; _3_i++) {
        DAST._IType _4_superTraitType;
        _4_superTraitType = (superTraitTypes).Select(_3_i);
        DAST._IType _source0 = _4_superTraitType;
        {
          if (_source0.is_UserDefined) {
            DAST._IResolvedType resolved0 = _source0.dtor_resolved;
            Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _5_traitPath = resolved0.dtor_path;
            Dafny.ISequence<DAST._IType> _6_typeArgs = resolved0.dtor_typeArgs;
            DAST._IResolvedTypeBase kind0 = resolved0.dtor_kind;
            if (kind0.is_Trait) {
              DAST._ITraitType _7_traitType = kind0.dtor_traitType;
              Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _8_properMethods = resolved0.dtor_properMethods;
              {
                RAST._IPath _9_path;
                RAST._IPath _out1;
                _out1 = (this).GenPath(_5_traitPath, true);
                _9_path = _out1;
                RAST._IType _10_pathType;
                _10_pathType = (_9_path).AsType();
                Dafny.ISequence<RAST._IType> _11_typeArgs;
                Dafny.ISequence<RAST._IType> _out2;
                _out2 = (this).GenTypeArgs(_6_typeArgs, Defs.GenTypeContext.@default());
                _11_typeArgs = _out2;
                Dafny.ISequence<RAST._IImplMember> _12_body;
                _12_body = Dafny.Sequence<RAST._IImplMember>.FromElements();
                if ((traitBodies).Contains(_5_traitPath)) {
                  _12_body = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.Select(traitBodies,_5_traitPath);
                }
                RAST._IType _13_fullTraitPath;
                _13_fullTraitPath = RAST.Type.create_TypeApp(_10_pathType, _11_typeArgs);
                RAST._IExpr _14_fullTraitExpr;
                _14_fullTraitExpr = ((_9_path).AsExpr()).ApplyType(_11_typeArgs);
                if (!((@extern).is_NoExtern)) {
                  if (((new BigInteger((_12_body).Count)).Sign == 0) && ((new BigInteger((_8_properMethods).Count)).Sign != 0)) {
                    goto continue_0;
                  }
                  if ((new BigInteger((_12_body).Count)) != (new BigInteger((_8_properMethods).Count))) {
                    (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Error: In the "), kind), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" ")), RAST.__default.SeqToString<Dafny.ISequence<Dafny.Rune>>(_5_traitPath, ((System.Func<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>)((_15_s) => {
  return ((_15_s));
})), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("."))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", some proper methods of ")), (_13_fullTraitPath)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" are marked {:extern} and some are not.")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" For the Rust compiler, please make all methods (")), RAST.__default.SeqToString<Dafny.ISequence<Dafny.Rune>>(_8_properMethods, ((System.Func<Dafny.ISequence<Dafny.Rune>, Dafny.ISequence<Dafny.Rune>>)((_16_s) => {
  return (_16_s);
})), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")  bodiless and mark as {:extern} and implement them in a Rust file, ")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("or mark none of them as {:extern} and implement them in Dafny. ")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Alternatively, you can insert an intermediate trait that performs the partial implementation if feasible.")));
                  }
                }
                if ((_7_traitType).is_GeneralTrait) {
                  _12_body = Dafny.Sequence<RAST._IImplMember>.Concat(_12_body, Dafny.Sequence<RAST._IImplMember>.FromElements(Defs.__default.clone__trait(_13_fullTraitPath), Defs.__default.print__trait, Defs.__default.hasher__trait(supportsEquality, (this).pointerType), Defs.__default.eq__trait(_13_fullTraitPath, _14_fullTraitExpr, supportsEquality, (this).pointerType), Defs.__default.as__any__trait));
                } else {
                  if (((kind).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("datatype"))) || ((kind).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("newtype")))) {
                    RAST._IExpr _17_dummy;
                    RAST._IExpr _out3;
                    _out3 = (this).Error(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Cannot extend non-general traits"), (this).InitEmptyExpr());
                    _17_dummy = _out3;
                  }
                }
                RAST._IModDecl _18_x;
                _18_x = RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(rTypeParamsDecls, _13_fullTraitPath, RAST.Type.create_TypeApp(_1_genSelfPath, rTypeParams), _12_body));
                s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(_18_x));
                if ((_7_traitType).is_GeneralTrait) {
                  s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(rTypeParamsDecls, (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("UpcastBox"))).AsType()).Apply1(RAST.Type.create_DynType(_13_fullTraitPath)), RAST.Type.create_TypeApp(_1_genSelfPath, rTypeParams), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("upcast"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.__default.Box(RAST.Type.create_DynType(_13_fullTraitPath))), Std.Wrappers.Option<RAST._IExpr>.create_Some(((((_9_path).AsExpr()).ApplyType(_11_typeArgs)).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_clone"))).Apply1(RAST.__default.self)))))))));
                } else {
                  s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(rTypeParamsDecls, (((RAST.__default.dafny__runtime).MSel((this).Upcast)).AsType()).Apply(Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_DynType(_13_fullTraitPath))), RAST.Type.create_TypeApp(_1_genSelfPath, rTypeParams), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_ImplMemberMacro((((RAST.__default.dafny__runtime).MSel((this).UpcastFnMacro)).AsExpr()).Apply1(RAST.Expr.create_ExprFromType(RAST.Type.create_DynType(_13_fullTraitPath)))))))));
                }
              }
              goto after_match0;
            }
          }
        }
        {
        }
      after_match0: ;
      continue_0: ;
      }
    after_0: ;
      return s;
    }
    public Dafny.ISequence<RAST._IModDecl> GenClass(DAST._IClass c, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> path)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      Dafny.ISequence<DAST._IType> _0_typeParamsSeq;
      Dafny.ISequence<RAST._IType> _1_rTypeParams;
      Dafny.ISequence<RAST._ITypeParamDecl> _2_rTypeParamsDecls;
      Dafny.ISequence<DAST._IType> _out0;
      Dafny.ISequence<RAST._IType> _out1;
      Dafny.ISequence<RAST._ITypeParamDecl> _out2;
      (this).GenTypeParameters((c).dtor_typeParams, out _out0, out _out1, out _out2);
      _0_typeParamsSeq = _out0;
      _1_rTypeParams = _out1;
      _2_rTypeParamsDecls = _out2;
      Dafny.ISequence<Dafny.Rune> _3_constrainedTypeParams;
      _3_constrainedTypeParams = RAST.TypeParamDecl.ToStringMultiple(_2_rTypeParamsDecls, Dafny.Sequence<Dafny.Rune>.Concat(RAST.__default.IND, RAST.__default.IND));
      Dafny.ISequence<RAST._IField> _4_fields;
      _4_fields = Dafny.Sequence<RAST._IField>.FromElements();
      Dafny.ISequence<RAST._IAssignIdentifier> _5_fieldInits;
      _5_fieldInits = Dafny.Sequence<RAST._IAssignIdentifier>.FromElements();
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _6_usedTypeParams;
      _6_usedTypeParams = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      BigInteger _hi0 = new BigInteger(((c).dtor_fields).Count);
      for (BigInteger _7_fieldI = BigInteger.Zero; _7_fieldI < _hi0; _7_fieldI++) {
        DAST._IField _8_field;
        _8_field = ((c).dtor_fields).Select(_7_fieldI);
        RAST._IField _9_rfield;
        RAST._IAssignIdentifier _10_fieldInit;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _11_fieldUsedTypeParams;
        RAST._IField _out3;
        RAST._IAssignIdentifier _out4;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out5;
        (this).GenField(_8_field, out _out3, out _out4, out _out5);
        _9_rfield = _out3;
        _10_fieldInit = _out4;
        _11_fieldUsedTypeParams = _out5;
        _4_fields = Dafny.Sequence<RAST._IField>.Concat(_4_fields, Dafny.Sequence<RAST._IField>.FromElements(_9_rfield));
        _5_fieldInits = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_5_fieldInits, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(_10_fieldInit));
        _6_usedTypeParams = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_6_usedTypeParams, _11_fieldUsedTypeParams);
      }
      BigInteger _hi1 = new BigInteger(((c).dtor_typeParams).Count);
      for (BigInteger _12_typeParamI = BigInteger.Zero; _12_typeParamI < _hi1; _12_typeParamI++) {
        DAST._IType _13_typeArg;
        RAST._ITypeParamDecl _14_typeParam;
        DAST._IType _out6;
        RAST._ITypeParamDecl _out7;
        (this).GenTypeParam(((c).dtor_typeParams).Select(_12_typeParamI), out _out6, out _out7);
        _13_typeArg = _out6;
        _14_typeParam = _out7;
        RAST._IType _15_rTypeArg;
        RAST._IType _out8;
        _out8 = (this).GenType(_13_typeArg, Defs.GenTypeContext.@default());
        _15_rTypeArg = _out8;
        if ((_6_usedTypeParams).Contains((_14_typeParam).dtor_name)) {
          goto continue_0;
        }
        _4_fields = Dafny.Sequence<RAST._IField>.Concat(_4_fields, Dafny.Sequence<RAST._IField>.FromElements(RAST.Field.create(RAST.Visibility.create_PRIV(), RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_phantom_type_param_"), Std.Strings.__default.OfNat(_12_typeParamI)), RAST.Type.create_TypeApp((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("marker"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("PhantomData"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_15_rTypeArg))))));
        _5_fieldInits = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_5_fieldInits, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_phantom_type_param_"), Std.Strings.__default.OfNat(_12_typeParamI)), (((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("marker"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("PhantomData"))).AsExpr())));
      continue_0: ;
      }
    after_0: ;
      Dafny.ISequence<Dafny.Rune> _16_className;
      Defs._IExternAttribute _17_extern;
      Dafny.ISequence<Dafny.Rune> _out9;
      Defs._IExternAttribute _out10;
      (this).GetName((c).dtor_attributes, (c).dtor_name, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("classes"), out _out9, out _out10);
      _16_className = _out9;
      _17_extern = _out10;
      RAST._IStruct _18_struct;
      _18_struct = RAST.Struct.create((c).dtor_docString, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), _16_className, _2_rTypeParamsDecls, RAST.Fields.create_NamedFields(_4_fields));
      s = Dafny.Sequence<RAST._IModDecl>.FromElements();
      if ((_17_extern).is_NoExtern) {
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_StructDecl(_18_struct)));
      }
      Dafny.ISequence<RAST._IImplMember> _19_implBody;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _20_traitBodies;
      Dafny.ISequence<RAST._IImplMember> _out11;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _out12;
      (this).GenClassImplBody((c).dtor_body, false, DAST.Type.create_UserDefined(DAST.ResolvedType.create(path, Dafny.Sequence<DAST._IType>.FromElements(), DAST.ResolvedTypeBase.create_Class(), (c).dtor_attributes, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<DAST._IType>.FromElements())), _0_typeParamsSeq, out _out11, out _out12);
      _19_implBody = _out11;
      _20_traitBodies = _out12;
      if (((_17_extern).is_NoExtern) && (!(_16_className).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_default")))) {
        _19_implBody = Dafny.Sequence<RAST._IImplMember>.Concat(Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Allocates an UNINITIALIZED instance. Only the Dafny compiler should use that."), RAST.__default.NoAttr, RAST.Visibility.create_PUB(), RAST.Fn.create((this).allocate__fn, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(), Std.Wrappers.Option<RAST._IType>.create_Some((this).Object(RAST.__default.SelfOwned)), Std.Wrappers.Option<RAST._IExpr>.create_Some(((((RAST.__default.dafny__runtime).MSel((this).allocate)).AsExpr()).ApplyType1(RAST.__default.SelfOwned)).Apply0())))), _19_implBody);
      }
      RAST._IType _21_selfTypeForImpl = RAST.Type.Default();
      if (((_17_extern).is_NoExtern) || ((_17_extern).is_UnsupportedExtern)) {
        _21_selfTypeForImpl = RAST.Type.create_TIdentifier(_16_className);
      } else if ((_17_extern).is_AdvancedExtern) {
        _21_selfTypeForImpl = (((RAST.__default.crate).MSels((_17_extern).dtor_enclosingModule)).MSel((_17_extern).dtor_overrideName)).AsType();
      } else if ((_17_extern).is_SimpleExtern) {
        _21_selfTypeForImpl = RAST.Type.create_TIdentifier((_17_extern).dtor_overrideName);
      }
      if ((new BigInteger((_19_implBody).Count)).Sign == 1) {
        RAST._IImpl _22_i;
        _22_i = RAST.Impl.create_Impl(_2_rTypeParamsDecls, RAST.Type.create_TypeApp(_21_selfTypeForImpl, _1_rTypeParams), _19_implBody);
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(_22_i)));
      }
      Dafny.ISequence<RAST._IModDecl> _23_testMethods;
      _23_testMethods = Dafny.Sequence<RAST._IModDecl>.FromElements();
      if ((_16_className).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_default"))) {
        BigInteger _hi2 = new BigInteger(((c).dtor_body).Count);
        for (BigInteger _24_i = BigInteger.Zero; _24_i < _hi2; _24_i++) {
          DAST._IMethod _25_m;
          DAST._IMethod _source0 = ((c).dtor_body).Select(_24_i);
          {
            DAST._IMethod _26_m = _source0;
            _25_m = _26_m;
          }
        after_match0: ;
          if (((this).HasAttribute((_25_m).dtor_attributes, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("test"))) && ((new BigInteger(((_25_m).dtor_params).Count)).Sign == 0)) {
            Dafny.ISequence<Dafny.Rune> _27_fnName;
            _27_fnName = Defs.__default.escapeName((_25_m).dtor_name);
            _23_testMethods = Dafny.Sequence<RAST._IModDecl>.Concat(_23_testMethods, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_TopFnDecl(RAST.TopFnDecl.create((_25_m).dtor_docString, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#[test]")), RAST.Visibility.create_PUB(), RAST.Fn.create(_27_fnName, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(), Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(((RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_default"))).FSel(_27_fnName)).Apply(Dafny.Sequence<RAST._IExpr>.FromElements())))))));
          }
        }
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, _23_testMethods);
      }
      RAST._IType _28_genSelfPath;
      RAST._IType _out13;
      _out13 = (this).GenPathType(path);
      _28_genSelfPath = _out13;
      if (!(_16_className).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_default"))) {
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_2_rTypeParamsDecls, (((RAST.__default.dafny__runtime).MSel((this).Upcast)).AsType()).Apply(Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_DynType(RAST.__default.AnyTrait))), RAST.Type.create_TypeApp(_28_genSelfPath, _1_rTypeParams), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_ImplMemberMacro((((RAST.__default.dafny__runtime).MSel((this).UpcastFnMacro)).AsExpr()).Apply1(RAST.Expr.create_ExprFromType(RAST.Type.create_DynType(RAST.__default.AnyTrait)))))))));
      }
      Dafny.ISequence<DAST._IType> _29_superTraitTypes;
      if ((_16_className).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_default"))) {
        _29_superTraitTypes = Dafny.Sequence<DAST._IType>.FromElements();
      } else {
        _29_superTraitTypes = (c).dtor_superTraitTypes;
      }
      Dafny.ISequence<RAST._IModDecl> _30_superTraitImplementations;
      Dafny.ISequence<RAST._IModDecl> _out14;
      _out14 = (this).GenTraitImplementations(path, _1_rTypeParams, _2_rTypeParamsDecls, _29_superTraitTypes, _20_traitBodies, _17_extern, true, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("class"));
      _30_superTraitImplementations = _out14;
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, _30_superTraitImplementations);
      return s;
    }
    public Dafny.ISequence<RAST._IModDecl> GenTrait(DAST._ITrait t, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> containingPath)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      Dafny.ISequence<DAST._IType> _0_typeParamsSeq;
      _0_typeParamsSeq = Dafny.Sequence<DAST._IType>.FromElements();
      Dafny.ISequence<RAST._ITypeParamDecl> _1_rTypeParamsDecls;
      _1_rTypeParamsDecls = Dafny.Sequence<RAST._ITypeParamDecl>.FromElements();
      Dafny.ISequence<RAST._IType> _2_typeParams;
      _2_typeParams = Dafny.Sequence<RAST._IType>.FromElements();
      if ((new BigInteger(((t).dtor_typeParams).Count)).Sign == 1) {
        BigInteger _hi0 = new BigInteger(((t).dtor_typeParams).Count);
        for (BigInteger _3_tpI = BigInteger.Zero; _3_tpI < _hi0; _3_tpI++) {
          DAST._ITypeArgDecl _4_tp;
          _4_tp = ((t).dtor_typeParams).Select(_3_tpI);
          DAST._IType _5_typeArg;
          RAST._ITypeParamDecl _6_typeParamDecl;
          DAST._IType _out0;
          RAST._ITypeParamDecl _out1;
          (this).GenTypeParam(_4_tp, out _out0, out _out1);
          _5_typeArg = _out0;
          _6_typeParamDecl = _out1;
          _0_typeParamsSeq = Dafny.Sequence<DAST._IType>.Concat(_0_typeParamsSeq, Dafny.Sequence<DAST._IType>.FromElements(_5_typeArg));
          _1_rTypeParamsDecls = Dafny.Sequence<RAST._ITypeParamDecl>.Concat(_1_rTypeParamsDecls, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(_6_typeParamDecl));
          RAST._IType _7_typeParam;
          RAST._IType _out2;
          _out2 = (this).GenType(_5_typeArg, Defs.GenTypeContext.@default());
          _7_typeParam = _out2;
          _2_typeParams = Dafny.Sequence<RAST._IType>.Concat(_2_typeParams, Dafny.Sequence<RAST._IType>.FromElements(_7_typeParam));
        }
      }
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _8_fullPath;
      _8_fullPath = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(containingPath, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements((t).dtor_name));
      Dafny.ISequence<Dafny.Rune> _9_name;
      _9_name = Defs.__default.escapeName((t).dtor_name);
      RAST._IType _10_traitFullType;
      _10_traitFullType = (RAST.Type.create_TIdentifier(_9_name)).Apply(_2_typeParams);
      RAST._IExpr _11_traitFullExpr;
      _11_traitFullExpr = (RAST.Expr.create_Identifier(_9_name)).ApplyType(_2_typeParams);
      Dafny.ISequence<RAST._IImplMember> _12_implBody;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _13_implBodyImplementingOtherTraits;
      Dafny.ISequence<RAST._IImplMember> _out3;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _out4;
      (this).GenClassImplBody((t).dtor_body, true, DAST.Type.create_UserDefined(DAST.ResolvedType.create(_8_fullPath, Dafny.Sequence<DAST._IType>.FromElements(), DAST.ResolvedTypeBase.create_Trait((t).dtor_traitType), (t).dtor_attributes, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<DAST._IType>.FromElements())), _0_typeParamsSeq, out _out3, out _out4);
      _12_implBody = _out3;
      _13_implBodyImplementingOtherTraits = _out4;
      if (((t).dtor_traitType).is_GeneralTrait) {
        _12_implBody = Dafny.Sequence<RAST._IImplMember>.Concat(_12_implBody, Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_clone"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.__default.Box(RAST.Type.create_DynType(_10_traitFullType))), Std.Wrappers.Option<RAST._IExpr>.create_None())), RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_fmt_print"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Defs.__default.fmt__print__parameters, Std.Wrappers.Option<RAST._IType>.create_Some(Defs.__default.fmt__print__result), Std.Wrappers.Option<RAST._IExpr>.create_None())), RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_hash"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_U64()), Std.Wrappers.Option<RAST._IExpr>.create_None())), RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_eq"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed, RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("other"), RAST.Type.create_Borrowed(RAST.__default.Box(RAST.Type.create_DynType(_10_traitFullType))))), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Bool()), Std.Wrappers.Option<RAST._IExpr>.create_None())), RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_as_any"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Borrowed(RAST.Type.create_DynType(RAST.__default.AnyTrait))), Std.Wrappers.Option<RAST._IExpr>.create_None()))));
      }
      while ((new BigInteger((_13_implBodyImplementingOtherTraits).Count)).Sign == 1) {
        Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _14_otherTrait;
        foreach (Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _assign_such_that_0 in (_13_implBodyImplementingOtherTraits).Keys.Elements) {
          _14_otherTrait = (Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>)_assign_such_that_0;
          if ((_13_implBodyImplementingOtherTraits).Contains(_14_otherTrait)) {
            goto after__ASSIGN_SUCH_THAT_0;
          }
        }
        throw new System.Exception("assign-such-that search produced no value");
      after__ASSIGN_SUCH_THAT_0: ;
        Dafny.ISequence<RAST._IImplMember> _15_otherMethods;
        _15_otherMethods = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.Select(_13_implBodyImplementingOtherTraits,_14_otherTrait);
        BigInteger _hi1 = new BigInteger((_15_otherMethods).Count);
        for (BigInteger _16_i = BigInteger.Zero; _16_i < _hi1; _16_i++) {
          _12_implBody = Dafny.Sequence<RAST._IImplMember>.Concat(_12_implBody, Dafny.Sequence<RAST._IImplMember>.FromElements((_15_otherMethods).Select(_16_i)));
        }
        _13_implBodyImplementingOtherTraits = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.Subtract(_13_implBodyImplementingOtherTraits, Dafny.Set<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.FromElements(_14_otherTrait));
      }
      Dafny.ISequence<RAST._IType> _17_parents;
      _17_parents = Dafny.Sequence<RAST._IType>.FromElements();
      Dafny.ISequence<RAST._IModDecl> _18_upcastImplemented;
      _18_upcastImplemented = Dafny.Sequence<RAST._IModDecl>.FromElements();
      RAST._IType _19_instantiatedFullType;
      _19_instantiatedFullType = RAST.__default.Box(RAST.Type.create_DynType(_10_traitFullType));
      if ((_19_instantiatedFullType).IsBox()) {
        RAST._IModDecl _20_upcastDynTrait;
        _20_upcastDynTrait = Defs.__default.UpcastDynTraitFor(_1_rTypeParamsDecls, _19_instantiatedFullType, _10_traitFullType, _11_traitFullExpr);
        _18_upcastImplemented = Dafny.Sequence<RAST._IModDecl>.Concat(_18_upcastImplemented, Dafny.Sequence<RAST._IModDecl>.FromElements(_20_upcastDynTrait));
      }
      BigInteger _hi2 = new BigInteger(((t).dtor_parents).Count);
      for (BigInteger _21_i = BigInteger.Zero; _21_i < _hi2; _21_i++) {
        DAST._IType _22_parentTyp;
        _22_parentTyp = ((t).dtor_parents).Select(_21_i);
        RAST._IType _23_parentTpe;
        RAST._IType _out5;
        _out5 = (this).GenType(_22_parentTyp, Defs.GenTypeContext.ForTraitParents());
        _23_parentTpe = _out5;
        Std.Wrappers._IOption<RAST._IExpr> _24_parentTpeExprMaybe;
        _24_parentTpeExprMaybe = (_23_parentTpe).ToExpr();
        RAST._IExpr _25_parentTpeExpr = RAST.Expr.Default();
        if ((_24_parentTpeExprMaybe).is_None) {
          RAST._IExpr _out6;
          _out6 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Cannot convert "), (_23_parentTpe)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" to an expression")), (this).InitEmptyExpr());
          _25_parentTpeExpr = _out6;
        } else {
          _25_parentTpeExpr = (_24_parentTpeExprMaybe).dtor_value;
        }
        _17_parents = Dafny.Sequence<RAST._IType>.Concat(_17_parents, Dafny.Sequence<RAST._IType>.FromElements(_23_parentTpe));
        Dafny.ISequence<Dafny.Rune> _26_upcastTrait;
        if ((_22_parentTyp).IsGeneralTrait()) {
          _26_upcastTrait = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("UpcastBox");
        } else {
          _26_upcastTrait = (this).Upcast;
        }
        _17_parents = Dafny.Sequence<RAST._IType>.Concat(_17_parents, Dafny.Sequence<RAST._IType>.FromElements((((RAST.__default.dafny__runtime).MSel(_26_upcastTrait)).AsType()).Apply1(RAST.Type.create_DynType(_23_parentTpe))));
        if ((_22_parentTyp).IsGeneralTrait()) {
          RAST._IModDecl _27_upcastDynTrait;
          _27_upcastDynTrait = Defs.__default.UpcastDynTraitFor(_1_rTypeParamsDecls, _19_instantiatedFullType, _23_parentTpe, _25_parentTpeExpr);
          _18_upcastImplemented = Dafny.Sequence<RAST._IModDecl>.Concat(_18_upcastImplemented, Dafny.Sequence<RAST._IModDecl>.FromElements(_27_upcastDynTrait));
        }
      }
      Dafny.ISequence<RAST._IModDecl> _28_downcastDefinition;
      _28_downcastDefinition = Dafny.Sequence<RAST._IModDecl>.FromElements();
      if ((new BigInteger(((t).dtor_parents).Count)).Sign == 1) {
        Std.Wrappers._IOption<RAST._IModDecl> _29_downcastDefinitionOpt;
        _29_downcastDefinitionOpt = Defs.__default.DowncastTraitDeclFor(_1_rTypeParamsDecls, _19_instantiatedFullType);
        if ((_29_downcastDefinitionOpt).is_None) {
          RAST._IExpr _30_dummy;
          RAST._IExpr _out7;
          _out7 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not generate downcast definition for "), (_19_instantiatedFullType)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), (this).InitEmptyExpr());
          _30_dummy = _out7;
        } else {
          _28_downcastDefinition = Dafny.Sequence<RAST._IModDecl>.FromElements((_29_downcastDefinitionOpt).dtor_value);
        }
      } else if (((t).dtor_traitType).is_GeneralTrait) {
        _17_parents = Dafny.Sequence<RAST._IType>.Concat(Dafny.Sequence<RAST._IType>.FromElements(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("AnyRef"))).AsType()), _17_parents);
      }
      if ((new BigInteger(((t).dtor_downcastableTraits).Count)).Sign == 1) {
        BigInteger _hi3 = new BigInteger(((t).dtor_downcastableTraits).Count);
        for (BigInteger _31_i = BigInteger.Zero; _31_i < _hi3; _31_i++) {
          RAST._IType _32_downcastableTrait;
          RAST._IType _out8;
          _out8 = (this).GenType(((t).dtor_downcastableTraits).Select(_31_i), Defs.GenTypeContext.ForTraitParents());
          _32_downcastableTrait = _out8;
          Std.Wrappers._IOption<RAST._IType> _33_downcastTraitOpt;
          _33_downcastTraitOpt = (_32_downcastableTrait).ToDowncast();
          if ((_33_downcastTraitOpt).is_Some) {
            _17_parents = Dafny.Sequence<RAST._IType>.Concat(_17_parents, Dafny.Sequence<RAST._IType>.FromElements((_33_downcastTraitOpt).dtor_value));
          } else {
            RAST._IExpr _34_r;
            RAST._IExpr _out9;
            _out9 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Cannot convert "), (_32_downcastableTrait)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" to its downcast version")), (this).InitEmptyExpr());
            _34_r = _out9;
          }
        }
      }
      s = Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_TraitDecl(RAST.Trait.create((t).dtor_docString, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), _1_rTypeParamsDecls, _10_traitFullType, _17_parents, _12_implBody)));
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, _28_downcastDefinition);
      if (((t).dtor_traitType).is_GeneralTrait) {
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1_rTypeParamsDecls, (((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("clone"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Clone"))).AsType(), RAST.__default.Box(RAST.Type.create_DynType(_10_traitFullType)), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("clone"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.__default.SelfOwned), Std.Wrappers.Option<RAST._IExpr>.create_Some(((_11_traitFullExpr).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_clone"))).Apply1(((RAST.__default.self).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply0()))))))), RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1_rTypeParamsDecls, RAST.__default.DafnyPrint, RAST.__default.Box(RAST.Type.create_DynType(_10_traitFullType)), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt_print"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Defs.__default.fmt__print__parameters, Std.Wrappers.Option<RAST._IType>.create_Some(Defs.__default.fmt__print__result), Std.Wrappers.Option<RAST._IExpr>.create_Some(((_11_traitFullExpr).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_fmt_print"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(((RAST.__default.self).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply0(), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_formatter")), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("in_seq"))))))))))));
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1_rTypeParamsDecls, (((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cmp"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("PartialEq"))).AsType(), RAST.__default.Box(RAST.Type.create_DynType(_10_traitFullType)), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("eq"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed, RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("other"), RAST.__default.SelfBorrowed)), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Bool()), Std.Wrappers.Option<RAST._IExpr>.create_Some(((_11_traitFullExpr).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_eq"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(((RAST.__default.self).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply0(), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("other")))))))))), RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1_rTypeParamsDecls, (((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cmp"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Eq"))).AsType(), RAST.__default.Box(RAST.Type.create_DynType(_10_traitFullType)), Dafny.Sequence<RAST._IImplMember>.FromElements()))));
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_1_rTypeParamsDecls, RAST.__default.Hash, RAST.__default.Box(RAST.Type.create_DynType(_10_traitFullType)), Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("hash"), Defs.__default.hash__type__parameters, Defs.__default.hash__parameters, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some((Defs.__default.hash__function).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.__default.Borrow(((_11_traitFullExpr).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_hash"))).Apply1(((RAST.__default.self).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply0())), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_state"))))))))))));
      }
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, _18_upcastImplemented);
      return s;
    }
    public Dafny.ISequence<RAST._IModDecl> GenNewtype(DAST._INewtype c, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> path)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      Dafny.ISequence<DAST._IType> _0_typeParamsSeq;
      Dafny.ISequence<RAST._IType> _1_rTypeParams;
      Dafny.ISequence<RAST._ITypeParamDecl> _2_rTypeParamsDecls;
      Dafny.ISequence<DAST._IType> _out0;
      Dafny.ISequence<RAST._IType> _out1;
      Dafny.ISequence<RAST._ITypeParamDecl> _out2;
      (this).GenTypeParameters((c).dtor_typeParams, out _out0, out _out1, out _out2);
      _0_typeParamsSeq = _out0;
      _1_rTypeParams = _out1;
      _2_rTypeParamsDecls = _out2;
      Dafny.ISequence<Dafny.Rune> _3_constrainedTypeParams;
      _3_constrainedTypeParams = RAST.TypeParamDecl.ToStringMultiple(_2_rTypeParamsDecls, Dafny.Sequence<Dafny.Rune>.Concat(RAST.__default.IND, RAST.__default.IND));
      RAST._IType _4_wrappedType = RAST.Type.Default();
      Std.Wrappers._IOption<RAST._IType> _5_rustType;
      _5_rustType = Defs.__default.NewtypeRangeToUnwrappedBoundedRustType((c).dtor_base, (c).dtor_range);
      if ((_5_rustType).is_Some) {
        _4_wrappedType = (_5_rustType).dtor_value;
      } else {
        RAST._IType _out3;
        _out3 = (this).GenType((c).dtor_base, Defs.GenTypeContext.@default());
        _4_wrappedType = _out3;
      }
      DAST._IType _6_newtypeType;
      _6_newtypeType = DAST.Type.create_UserDefined(DAST.ResolvedType.create(path, _0_typeParamsSeq, DAST.ResolvedTypeBase.create_Newtype((c).dtor_base, (c).dtor_range, false), (c).dtor_attributes, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<DAST._IType>.FromElements()));
      Dafny.ISequence<Dafny.Rune> _7_newtypeName;
      _7_newtypeName = Defs.__default.escapeName((c).dtor_name);
      RAST._IType _8_resultingType;
      _8_resultingType = RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_7_newtypeName), _1_rTypeParams);
      Dafny.ISequence<Dafny.Rune> _9_attributes = Dafny.Sequence<Dafny.Rune>.Empty;
      if (Defs.__default.IsNewtypeCopy((c).dtor_range)) {
        _9_attributes = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#[derive(Clone, PartialEq, Copy)]");
      } else {
        _9_attributes = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#[derive(Clone, PartialEq)]");
      }
      s = Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_StructDecl(RAST.Struct.create((c).dtor_docString, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_9_attributes, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#[repr(transparent)]")), _7_newtypeName, _2_rTypeParamsDecls, RAST.Fields.create_NamelessFields(Dafny.Sequence<RAST._INamelessField>.FromElements(RAST.NamelessField.create(RAST.Visibility.create_PUB(), _4_wrappedType))))));
      RAST._IExpr _10_fnBody = RAST.Expr.Default();
      Std.Wrappers._IOption<DAST._IExpression> _source0 = (c).dtor_witnessExpr;
      {
        if (_source0.is_Some) {
          DAST._IExpression _11_e = _source0.dtor_value;
          {
            DAST._IExpression _12_e;
            _12_e = DAST.Expression.create_Convert(_11_e, (c).dtor_base, _6_newtypeType);
            RAST._IExpr _13_r;
            Defs._IOwnership _14___v5;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _15___v6;
            RAST._IExpr _out4;
            Defs._IOwnership _out5;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out6;
            (this).GenExpr(_12_e, Defs.SelfInfo.create_NoSelf(), Defs.Environment.Empty(), Defs.Ownership.create_OwnershipOwned(), out _out4, out _out5, out _out6);
            _13_r = _out4;
            _14___v5 = _out5;
            _15___v6 = _out6;
            _10_fnBody = _13_r;
          }
          goto after_match0;
        }
      }
      {
        {
          _10_fnBody = (RAST.Expr.create_Identifier(_7_newtypeName)).Apply1(RAST.__default.std__default__Default__default);
        }
      }
    after_match0: ;
      RAST._IImplMember _16_body;
      _16_body = RAST.ImplMember.create_FnDecl(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("An element of "), _7_newtypeName), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("default"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.__default.SelfOwned), Std.Wrappers.Option<RAST._IExpr>.create_Some(_10_fnBody)));
      Std.Wrappers._IOption<DAST._INewtypeConstraint> _source1 = (c).dtor_constraint;
      {
        if (_source1.is_None) {
          goto after_match1;
        }
      }
      {
        DAST._INewtypeConstraint value0 = _source1.dtor_value;
        DAST._IFormal _17_formal = value0.dtor_variable;
        Dafny.ISequence<DAST._IStatement> _18_constraintStmts = value0.dtor_constraintStmts;
        RAST._IExpr _19_rStmts;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _20___v7;
        Defs._IEnvironment _21_newEnv;
        RAST._IExpr _out7;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out8;
        Defs._IEnvironment _out9;
        (this).GenStmts(_18_constraintStmts, Defs.SelfInfo.create_NoSelf(), Defs.Environment.Empty(), false, Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_None(), out _out7, out _out8, out _out9);
        _19_rStmts = _out7;
        _20___v7 = _out8;
        _21_newEnv = _out9;
        Dafny.ISequence<RAST._IFormal> _22_rFormals;
        Dafny.ISequence<RAST._IFormal> _out10;
        _out10 = (this).GenParams(Dafny.Sequence<DAST._IFormal>.FromElements(_17_formal), Dafny.Sequence<DAST._IFormal>.FromElements(_17_formal), false);
        _22_rFormals = _out10;
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_Impl(_2_rTypeParamsDecls, _8_resultingType, Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Constraint check"), RAST.__default.NoAttr, RAST.Visibility.create_PUB(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("is"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), _22_rFormals, Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Bool()), Std.Wrappers.Option<RAST._IExpr>.create_Some(_19_rStmts))))))));
      }
    after_match1: ;
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_2_rTypeParamsDecls, RAST.__default.DefaultTrait, _8_resultingType, Dafny.Sequence<RAST._IImplMember>.FromElements(_16_body)))));
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_2_rTypeParamsDecls, RAST.__default.DafnyPrint, _8_resultingType, Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("For Dafny print statements"), RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt_print"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Defs.__default.fmt__print__parameters, Std.Wrappers.Option<RAST._IType>.create_Some(Defs.__default.fmt__print__result), Std.Wrappers.Option<RAST._IExpr>.create_Some(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyPrint"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt_print"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.__default.Borrow((RAST.__default.self).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0"))), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_formatter")), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("in_seq"))))))))))));
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_ImplFor(_2_rTypeParamsDecls, (((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("ops"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Deref"))).AsType(), _8_resultingType, Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_TypeDeclMember(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Target"), _4_wrappedType), RAST.ImplMember.create_FnDecl(RAST.__default.NoDoc, RAST.__default.NoAttr, RAST.Visibility.create_PRIV(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("deref"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Borrowed(((RAST.Path.create_Self()).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Target"))).AsType())), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.__default.Borrow((RAST.__default.self).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")))))))))));
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_Impl(_2_rTypeParamsDecls, _8_resultingType, Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("SAFETY: The newtype is marked as transparent"), RAST.__default.NoAttr, RAST.Visibility.create_PUB(), RAST.Fn.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_from_ref"), Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("o"), RAST.Type.create_Borrowed(_4_wrappedType))), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Borrowed((RAST.Path.create_Self()).AsType())), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.__default.Unsafe(RAST.Expr.create_Block(((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("mem"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("transmute"))).AsExpr()).Apply1(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("o")))))))))))));
      Dafny.ISequence<RAST._ITypeParamDecl> _23_rTypeParamsDeclsWithHash;
      _23_rTypeParamsDeclsWithHash = RAST.TypeParamDecl.AddConstraintsMultiple(_2_rTypeParamsDecls, Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.Hash));
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(Defs.__default.HashImpl(_23_rTypeParamsDeclsWithHash, _8_resultingType, (Defs.__default.hash__function).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.__default.Borrow((RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0"))), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_state")))))));
      if (((c).dtor_range).HasArithmeticOperations()) {
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(Defs.__default.OpsImpl(new Dafny.Rune('+'), _2_rTypeParamsDecls, _8_resultingType, _7_newtypeName), Defs.__default.OpsImpl(new Dafny.Rune('-'), _2_rTypeParamsDecls, _8_resultingType, _7_newtypeName), Defs.__default.OpsImpl(new Dafny.Rune('*'), _2_rTypeParamsDecls, _8_resultingType, _7_newtypeName), Defs.__default.OpsImpl(new Dafny.Rune('/'), _2_rTypeParamsDecls, _8_resultingType, _7_newtypeName), Defs.__default.PartialOrdImpl(_2_rTypeParamsDecls, _8_resultingType, _7_newtypeName)));
      }
      if (((c).dtor_range).is_Bool) {
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(Defs.__default.UnaryOpsImpl(new Dafny.Rune('!'), _2_rTypeParamsDecls, _8_resultingType, _7_newtypeName)));
      }
      Dafny.ISequence<RAST._IImplMember> _24_implementation;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _25_traitBodies;
      Dafny.ISequence<RAST._IImplMember> _out11;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _out12;
      (this).GenClassImplBody((c).dtor_classItems, false, _6_newtypeType, _0_typeParamsSeq, out _out11, out _out12);
      _24_implementation = _out11;
      _25_traitBodies = _out12;
      if ((new BigInteger((_25_traitBodies).Count)).Sign == 1) {
        (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("No support for trait in newtypes yet"));
      }
      if ((new BigInteger((_24_implementation).Count)).Sign == 1) {
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_ImplDecl(RAST.Impl.create_Impl(_2_rTypeParamsDecls, RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_7_newtypeName), _1_rTypeParams), _24_implementation))));
      }
      return s;
    }
    public Dafny.ISequence<RAST._IModDecl> GenSynonymType(DAST._ISynonymType c)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      Dafny.ISequence<DAST._IType> _0_typeParamsSeq;
      Dafny.ISequence<RAST._IType> _1_rTypeParams;
      Dafny.ISequence<RAST._ITypeParamDecl> _2_rTypeParamsDecls;
      Dafny.ISequence<DAST._IType> _out0;
      Dafny.ISequence<RAST._IType> _out1;
      Dafny.ISequence<RAST._ITypeParamDecl> _out2;
      (this).GenTypeParameters((c).dtor_typeParams, out _out0, out _out1, out _out2);
      _0_typeParamsSeq = _out0;
      _1_rTypeParams = _out1;
      _2_rTypeParamsDecls = _out2;
      Dafny.ISequence<Dafny.Rune> _3_synonymTypeName;
      _3_synonymTypeName = Defs.__default.escapeName((c).dtor_name);
      RAST._IType _4_resultingType;
      RAST._IType _out3;
      _out3 = (this).GenType((c).dtor_base, Defs.GenTypeContext.@default());
      _4_resultingType = _out3;
      s = Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_TypeDecl(RAST.TypeSynonym.create((c).dtor_docString, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), _3_synonymTypeName, _2_rTypeParamsDecls, _4_resultingType)));
      Dafny.ISequence<RAST._ITypeParamDecl> _5_defaultConstrainedTypeParams;
      _5_defaultConstrainedTypeParams = RAST.TypeParamDecl.AddConstraintsMultiple(_2_rTypeParamsDecls, Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.DefaultTrait));
      Std.Wrappers._IOption<DAST._IExpression> _source0 = (c).dtor_witnessExpr;
      {
        if (_source0.is_Some) {
          DAST._IExpression _6_e = _source0.dtor_value;
          {
            RAST._IExpr _7_rStmts;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _8___v8;
            Defs._IEnvironment _9_newEnv;
            RAST._IExpr _out4;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out5;
            Defs._IEnvironment _out6;
            (this).GenStmts((c).dtor_witnessStmts, Defs.SelfInfo.create_NoSelf(), Defs.Environment.Empty(), false, Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_None(), out _out4, out _out5, out _out6);
            _7_rStmts = _out4;
            _8___v8 = _out5;
            _9_newEnv = _out6;
            RAST._IExpr _10_rExpr;
            Defs._IOwnership _11___v9;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _12___v10;
            RAST._IExpr _out7;
            Defs._IOwnership _out8;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out9;
            (this).GenExpr(_6_e, Defs.SelfInfo.create_NoSelf(), _9_newEnv, Defs.Ownership.create_OwnershipOwned(), out _out7, out _out8, out _out9);
            _10_rExpr = _out7;
            _11___v9 = _out8;
            _12___v10 = _out9;
            Dafny.ISequence<Dafny.Rune> _13_constantName;
            _13_constantName = Defs.__default.escapeName(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_init_"), ((c).dtor_name)));
            s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_TopFnDecl(RAST.TopFnDecl.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("An element of "), _3_synonymTypeName), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), RAST.Visibility.create_PUB(), RAST.Fn.create(_13_constantName, _5_defaultConstrainedTypeParams, Dafny.Sequence<RAST._IFormal>.FromElements(), Std.Wrappers.Option<RAST._IType>.create_Some(_4_resultingType), Std.Wrappers.Option<RAST._IExpr>.create_Some((_7_rStmts).Then(_10_rExpr)))))));
          }
          goto after_match0;
        }
      }
      {
      }
    after_match0: ;
      return s;
    }
    public bool TypeIsEq(DAST._IType t, Dafny.ISet<Dafny.ISequence<Dafny.Rune>> typeParametersSupportingEquality)
    {
      DAST._IType _source0 = t;
      {
        if (_source0.is_UserDefined) {
          DAST._IResolvedType resolved0 = _source0.dtor_resolved;
          DAST._IResolvedTypeBase kind0 = resolved0.dtor_kind;
          if (kind0.is_SynonymType) {
            DAST._IType _0_tpe = kind0.dtor_baseType;
            return (this).TypeIsEq(_0_tpe, typeParametersSupportingEquality);
          }
        }
      }
      {
        if (_source0.is_UserDefined) {
          DAST._IResolvedType resolved1 = _source0.dtor_resolved;
          DAST._IResolvedTypeBase kind1 = resolved1.dtor_kind;
          if (kind1.is_Class) {
            return true;
          }
        }
      }
      {
        if (_source0.is_UserDefined) {
          DAST._IResolvedType resolved2 = _source0.dtor_resolved;
          DAST._IResolvedTypeBase kind2 = resolved2.dtor_kind;
          if (kind2.is_Trait) {
            DAST._ITraitType traitType0 = kind2.dtor_traitType;
            if (traitType0.is_GeneralTrait) {
              return false;
            }
          }
        }
      }
      {
        if (_source0.is_UserDefined) {
          DAST._IResolvedType resolved3 = _source0.dtor_resolved;
          DAST._IResolvedTypeBase kind3 = resolved3.dtor_kind;
          if (kind3.is_Trait) {
            DAST._ITraitType traitType1 = kind3.dtor_traitType;
            if (traitType1.is_ObjectTrait) {
              return true;
            }
          }
        }
      }
      {
        if (_source0.is_UserDefined) {
          DAST._IResolvedType resolved4 = _source0.dtor_resolved;
          DAST._IResolvedTypeBase kind4 = resolved4.dtor_kind;
          if (kind4.is_Newtype) {
            DAST._IType _1_tpe = kind4.dtor_baseType;
            return (this).TypeIsEq(_1_tpe, typeParametersSupportingEquality);
          }
        }
      }
      {
        if (_source0.is_UserDefined) {
          DAST._IResolvedType resolved5 = _source0.dtor_resolved;
          Dafny.ISequence<DAST._IType> _2_typeParams = resolved5.dtor_typeArgs;
          DAST._IResolvedTypeBase kind5 = resolved5.dtor_kind;
          if (kind5.is_Datatype) {
            DAST._IEqualitySupport _3_equalitySupport = kind5.dtor_equalitySupport;
            Dafny.ISequence<DAST._ITypeParameterInfo> _4_tps = kind5.dtor_info;
            return (!((_3_equalitySupport).is_Never)) && (((_3_equalitySupport).is_Always) || ((((_3_equalitySupport).is_ConsultTypeArguments) && ((new BigInteger((_4_tps).Count)) == (new BigInteger((_2_typeParams).Count)))) && (Dafny.Helpers.Id<Func<Dafny.ISequence<DAST._ITypeParameterInfo>, Dafny.ISequence<DAST._IType>, Dafny.ISet<Dafny.ISequence<Dafny.Rune>>, bool>>((_5_tps, _6_typeParams, _7_typeParametersSupportingEquality) => Dafny.Helpers.Quantifier<BigInteger>(Dafny.Helpers.IntegerRange(BigInteger.Zero, new BigInteger((_5_tps).Count)), true, (((_forall_var_0) => {
              BigInteger _8_i = (BigInteger)_forall_var_0;
              return !(((_8_i).Sign != -1) && ((_8_i) < (new BigInteger((_5_tps).Count)))) || (!(((_5_tps).Select(_8_i)).dtor_necessaryForEqualitySupportOfSurroundingInductiveDatatype) || ((this).TypeIsEq((_6_typeParams).Select(_8_i), _7_typeParametersSupportingEquality)));
            }))))(_4_tps, _2_typeParams, typeParametersSupportingEquality))));
          }
        }
      }
      {
        if (_source0.is_Tuple) {
          Dafny.ISequence<DAST._IType> _9_ts = _source0.dtor_Tuple_a0;
          return Dafny.Helpers.Id<Func<Dafny.ISequence<DAST._IType>, Dafny.ISet<Dafny.ISequence<Dafny.Rune>>, bool>>((_10_ts, _11_typeParametersSupportingEquality) => Dafny.Helpers.Quantifier<DAST._IType>((_10_ts).UniqueElements, true, (((_forall_var_1) => {
            DAST._IType _12_t = (DAST._IType)_forall_var_1;
            return !((_10_ts).Contains(_12_t)) || ((this).TypeIsEq(_12_t, _11_typeParametersSupportingEquality));
          }))))(_9_ts, typeParametersSupportingEquality);
        }
      }
      {
        if (_source0.is_Array) {
          DAST._IType _13_t = _source0.dtor_element;
          return true;
        }
      }
      {
        if (_source0.is_Seq) {
          DAST._IType _14_t = _source0.dtor_element;
          return (this).TypeIsEq(_14_t, typeParametersSupportingEquality);
        }
      }
      {
        if (_source0.is_Set) {
          DAST._IType _15_t = _source0.dtor_element;
          return true;
        }
      }
      {
        if (_source0.is_Multiset) {
          DAST._IType _16_t = _source0.dtor_element;
          return true;
        }
      }
      {
        if (_source0.is_Map) {
          DAST._IType _17_k = _source0.dtor_key;
          DAST._IType _18_v = _source0.dtor_value;
          return (this).TypeIsEq(_18_v, typeParametersSupportingEquality);
        }
      }
      {
        if (_source0.is_SetBuilder) {
          DAST._IType _19_t = _source0.dtor_element;
          return true;
        }
      }
      {
        if (_source0.is_MapBuilder) {
          DAST._IType _20_k = _source0.dtor_key;
          DAST._IType _21_v = _source0.dtor_value;
          return (this).TypeIsEq(_21_v, typeParametersSupportingEquality);
        }
      }
      {
        if (_source0.is_Arrow) {
          return false;
        }
      }
      {
        if (_source0.is_Primitive) {
          return true;
        }
      }
      {
        if (_source0.is_Passthrough) {
          return true;
        }
      }
      {
        if (_source0.is_TypeArg) {
          Dafny.ISequence<Dafny.Rune> _22_i = _source0.dtor_TypeArg_a0;
          return (typeParametersSupportingEquality).Contains(_22_i);
        }
      }
      {
        return true;
      }
    }
    public Std.Wrappers._IOption<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>> Combine(Dafny.ISequence<Std.Wrappers._IOption<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>> s)
    {
      Std.Wrappers._IOption<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>> _hresult = Std.Wrappers.Option<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>.Default();
      if ((new BigInteger((s).Count)).Sign == 0) {
        _hresult = Std.Wrappers.Option<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>.create_Some(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements());
        return _hresult;
      }
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _0_result;
      _0_result = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      BigInteger _hi0 = new BigInteger((s).Count);
      for (BigInteger _1_i = BigInteger.Zero; _1_i < _hi0; _1_i++) {
        if (((s).Select(_1_i)).is_None) {
          _hresult = Std.Wrappers.Option<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>.create_None();
          return _hresult;
        }
        _0_result = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_0_result, ((s).Select(_1_i)).dtor_value);
      }
      _hresult = Std.Wrappers.Option<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>.create_Some(_0_result);
      return _hresult;
      return _hresult;
    }
    public bool DatatypeIsEq(DAST._IDatatype c, Dafny.ISet<Dafny.ISequence<Dafny.Rune>> typeParametersSupportingEquality)
    {
      return (!((c).dtor_isCo)) && (Dafny.Helpers.Id<Func<DAST._IDatatype, Dafny.ISet<Dafny.ISequence<Dafny.Rune>>, bool>>((_0_c, _1_typeParametersSupportingEquality) => Dafny.Helpers.Quantifier<DAST._IDatatypeCtor>(((_0_c).dtor_ctors).UniqueElements, true, (((_forall_var_0) => {
        DAST._IDatatypeCtor _2_ctor = (DAST._IDatatypeCtor)_forall_var_0;
        return Dafny.Helpers.Quantifier<DAST._IDatatypeDtor>(((_2_ctor).dtor_args).UniqueElements, true, (((_forall_var_1) => {
          DAST._IDatatypeDtor _3_arg = (DAST._IDatatypeDtor)_forall_var_1;
          return !((((_0_c).dtor_ctors).Contains(_2_ctor)) && (((_2_ctor).dtor_args).Contains(_3_arg))) || ((this).TypeIsEq(((_3_arg).dtor_formal).dtor_typ, _1_typeParametersSupportingEquality));
        })));
      }))))(c, typeParametersSupportingEquality));
    }
    public RAST._IExpr write(RAST._IExpr r, bool final)
    {
      RAST._IExpr _0_result = (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("write!"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_formatter")), r));
      if (final) {
        return _0_result;
      } else {
        return RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("?"), _0_result, DAST.Format.UnaryOpFormat.create_NoFormat());
      }
    }
    public RAST._IExpr writeStr(Dafny.ISequence<Dafny.Rune> s, bool final)
    {
      return (this).write(RAST.Expr.create_LiteralString(s, false, false), false);
    }
    public Dafny.ISequence<RAST._IModDecl> GenDatatype(DAST._IDatatype c, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> path)
    {
      Dafny.ISequence<RAST._IModDecl> s = Dafny.Sequence<RAST._IModDecl>.Empty;
      bool _0_isRcWrapped;
      _0_isRcWrapped = Defs.__default.IsRcWrapped((c).dtor_attributes);
      Dafny.ISequence<DAST._IType> _1_typeParamsSeq;
      Dafny.ISequence<RAST._IType> _2_rTypeParams;
      Dafny.ISequence<RAST._ITypeParamDecl> _3_rTypeParamsDecls;
      Dafny.ISequence<DAST._IType> _out0;
      Dafny.ISequence<RAST._IType> _out1;
      Dafny.ISequence<RAST._ITypeParamDecl> _out2;
      (this).GenTypeParameters((c).dtor_typeParams, out _out0, out _out1, out _out2);
      _1_typeParamsSeq = _out0;
      _2_rTypeParams = _out1;
      _3_rTypeParamsDecls = _out2;
      Dafny.ISequence<Dafny.Rune> _4_datatypeName;
      Defs._IExternAttribute _5_extern;
      Dafny.ISequence<Dafny.Rune> _out3;
      Defs._IExternAttribute _out4;
      (this).GetName((c).dtor_attributes, (c).dtor_name, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("datatypes"), out _out3, out _out4);
      _4_datatypeName = _out3;
      _5_extern = _out4;
      Dafny.ISequence<RAST._IEnumCase> _6_ctors;
      _6_ctors = Dafny.Sequence<RAST._IEnumCase>.FromElements();
      Dafny.ISequence<DAST._ITypeParameterInfo> _7_typeParamInfos;
      _7_typeParamInfos = Std.Collections.Seq.__default.Map<DAST._ITypeArgDecl, DAST._ITypeParameterInfo>(((System.Func<DAST._ITypeArgDecl, DAST._ITypeParameterInfo>)((_8_typeParamDecl) => {
        return (_8_typeParamDecl).dtor_info;
      })), (c).dtor_typeParams);
      Dafny.ISequence<RAST._IExpr> _9_singletonConstructors;
      _9_singletonConstructors = Dafny.Sequence<RAST._IExpr>.FromElements();
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _10_usedTypeParams;
      _10_usedTypeParams = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      BigInteger _hi0 = new BigInteger(((c).dtor_ctors).Count);
      for (BigInteger _11_i = BigInteger.Zero; _11_i < _hi0; _11_i++) {
        DAST._IDatatypeCtor _12_ctor;
        _12_ctor = ((c).dtor_ctors).Select(_11_i);
        Dafny.ISequence<RAST._IField> _13_ctorArgs;
        _13_ctorArgs = Dafny.Sequence<RAST._IField>.FromElements();
        bool _14_isNumeric;
        _14_isNumeric = false;
        if ((new BigInteger(((_12_ctor).dtor_args).Count)).Sign == 0) {
          RAST._IExpr _15_instantiation;
          _15_instantiation = RAST.Expr.create_StructBuild((RAST.Expr.create_Identifier(_4_datatypeName)).FSel(Defs.__default.escapeName((_12_ctor).dtor_name)), Dafny.Sequence<RAST._IAssignIdentifier>.FromElements());
          if (_0_isRcWrapped) {
            _15_instantiation = RAST.__default.RcNew(_15_instantiation);
          }
          _9_singletonConstructors = Dafny.Sequence<RAST._IExpr>.Concat(_9_singletonConstructors, Dafny.Sequence<RAST._IExpr>.FromElements(_15_instantiation));
        }
        BigInteger _hi1 = new BigInteger(((_12_ctor).dtor_args).Count);
        for (BigInteger _16_j = BigInteger.Zero; _16_j < _hi1; _16_j++) {
          DAST._IDatatypeDtor _17_dtor;
          _17_dtor = ((_12_ctor).dtor_args).Select(_16_j);
          RAST._IType _18_formalType;
          RAST._IType _out5;
          _out5 = (this).GenType(((_17_dtor).dtor_formal).dtor_typ, Defs.GenTypeContext.@default());
          _18_formalType = _out5;
          _10_usedTypeParams = (this).GatherTypeParamNames(_10_usedTypeParams, _18_formalType);
          Dafny.ISequence<Dafny.Rune> _19_formalName;
          _19_formalName = Defs.__default.escapeVar(((_17_dtor).dtor_formal).dtor_name);
          if (((_16_j).Sign == 0) && ((Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")).Equals(_19_formalName))) {
            _14_isNumeric = true;
          }
          if ((((_16_j).Sign != 0) && (_14_isNumeric)) && (!(Std.Strings.__default.OfNat(_16_j)).Equals(_19_formalName))) {
            (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Formal extern names were supposed to be numeric but got "), _19_formalName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" instead of ")), Std.Strings.__default.OfNat(_16_j)));
            _14_isNumeric = false;
          }
          if ((c).dtor_isCo) {
            _13_ctorArgs = Dafny.Sequence<RAST._IField>.Concat(_13_ctorArgs, Dafny.Sequence<RAST._IField>.FromElements(RAST.Field.create(RAST.Visibility.create_PRIV(), RAST.Formal.create(_19_formalName, RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("LazyFieldWrapper"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_18_formalType))))));
          } else {
            _13_ctorArgs = Dafny.Sequence<RAST._IField>.Concat(_13_ctorArgs, Dafny.Sequence<RAST._IField>.FromElements(RAST.Field.create(RAST.Visibility.create_PRIV(), RAST.Formal.create(_19_formalName, _18_formalType))));
          }
        }
        RAST._IFields _20_namedFields;
        _20_namedFields = RAST.Fields.create_NamedFields(_13_ctorArgs);
        if (_14_isNumeric) {
          _20_namedFields = (_20_namedFields).ToNamelessFields();
        }
        _6_ctors = Dafny.Sequence<RAST._IEnumCase>.Concat(_6_ctors, Dafny.Sequence<RAST._IEnumCase>.FromElements(RAST.EnumCase.create((_12_ctor).dtor_docString, Defs.__default.escapeName((_12_ctor).dtor_name), _20_namedFields)));
      }
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _21_unusedTypeParams;
      _21_unusedTypeParams = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(Dafny.Helpers.Id<Func<Dafny.ISequence<RAST._ITypeParamDecl>, Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>>((_22_rTypeParamsDecls) => ((System.Func<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>)(() => {
        var _coll0 = new System.Collections.Generic.List<Dafny.ISequence<Dafny.Rune>>();
        foreach (RAST._ITypeParamDecl _compr_0 in (_22_rTypeParamsDecls).CloneAsArray()) {
          RAST._ITypeParamDecl _23_tp = (RAST._ITypeParamDecl)_compr_0;
          if ((_22_rTypeParamsDecls).Contains(_23_tp)) {
            _coll0.Add((_23_tp).dtor_name);
          }
        }
        return Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromCollection(_coll0);
      }))())(_3_rTypeParamsDecls), _10_usedTypeParams);
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _24_selfPath;
      _24_selfPath = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements((c).dtor_name);
      Dafny.ISequence<RAST._IImplMember> _25_implBodyRaw;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _26_traitBodies;
      Dafny.ISequence<RAST._IImplMember> _out6;
      Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> _out7;
      (this).GenClassImplBody((c).dtor_body, false, DAST.Type.create_UserDefined(DAST.ResolvedType.create(_24_selfPath, _1_typeParamsSeq, DAST.ResolvedTypeBase.create_Datatype((c).dtor_equalitySupport, _7_typeParamInfos), (c).dtor_attributes, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<DAST._IType>.FromElements())), _1_typeParamsSeq, out _out6, out _out7);
      _25_implBodyRaw = _out6;
      _26_traitBodies = _out7;
      Dafny.ISequence<RAST._IImplMember> _27_implBody;
      _27_implBody = _25_implBodyRaw;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _28_emittedFields;
      _28_emittedFields = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      BigInteger _hi2 = new BigInteger(((c).dtor_ctors).Count);
      for (BigInteger _29_i = BigInteger.Zero; _29_i < _hi2; _29_i++) {
        DAST._IDatatypeCtor _30_ctor;
        _30_ctor = ((c).dtor_ctors).Select(_29_i);
        BigInteger _hi3 = new BigInteger(((_30_ctor).dtor_args).Count);
        for (BigInteger _31_j = BigInteger.Zero; _31_j < _hi3; _31_j++) {
          DAST._IDatatypeDtor _32_dtor;
          _32_dtor = ((_30_ctor).dtor_args).Select(_31_j);
          Dafny.ISequence<Dafny.Rune> _33_callName;
          _33_callName = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.GetOr((_32_dtor).dtor_callName, Defs.__default.escapeVar(((_32_dtor).dtor_formal).dtor_name));
          if (!((_28_emittedFields).Contains(_33_callName))) {
            _28_emittedFields = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_28_emittedFields, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_33_callName));
            RAST._IType _34_formalType;
            RAST._IType _out8;
            _out8 = (this).GenType(((_32_dtor).dtor_formal).dtor_typ, Defs.GenTypeContext.@default());
            _34_formalType = _out8;
            Dafny.ISequence<RAST._IMatchCase> _35_cases;
            _35_cases = Dafny.Sequence<RAST._IMatchCase>.FromElements();
            BigInteger _hi4 = new BigInteger(((c).dtor_ctors).Count);
            for (BigInteger _36_k = BigInteger.Zero; _36_k < _hi4; _36_k++) {
              DAST._IDatatypeCtor _37_ctor2;
              _37_ctor2 = ((c).dtor_ctors).Select(_36_k);
              Dafny.ISequence<Dafny.Rune> _38_pattern;
              _38_pattern = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_4_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), Defs.__default.escapeName((_37_ctor2).dtor_name));
              RAST._IExpr _39_rhs = RAST.Expr.Default();
              Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _40_hasMatchingField;
              _40_hasMatchingField = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None();
              Dafny.ISequence<Dafny.Rune> _41_patternInner;
              _41_patternInner = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
              bool _42_isNumeric;
              _42_isNumeric = false;
              BigInteger _hi5 = new BigInteger(((_37_ctor2).dtor_args).Count);
              for (BigInteger _43_l = BigInteger.Zero; _43_l < _hi5; _43_l++) {
                DAST._IDatatypeDtor _44_dtor2;
                _44_dtor2 = ((_37_ctor2).dtor_args).Select(_43_l);
                Dafny.ISequence<Dafny.Rune> _45_patternName;
                _45_patternName = Defs.__default.escapeVar(((_44_dtor2).dtor_formal).dtor_name);
                if (((_43_l).Sign == 0) && ((_45_patternName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")))) {
                  _42_isNumeric = true;
                }
                if (_42_isNumeric) {
                  _45_patternName = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.GetOr((_44_dtor2).dtor_callName, Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("v"), Std.Strings.__default.OfNat(_43_l)));
                }
                if (object.Equals(((_32_dtor).dtor_formal).dtor_name, ((_44_dtor2).dtor_formal).dtor_name)) {
                  _40_hasMatchingField = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(_45_patternName);
                }
                _41_patternInner = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_41_patternInner, _45_patternName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "));
              }
              if (_42_isNumeric) {
                _38_pattern = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_38_pattern, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")), _41_patternInner), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
              } else {
                _38_pattern = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_38_pattern, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{")), _41_patternInner), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
              }
              if ((_40_hasMatchingField).is_Some) {
                if ((c).dtor_isCo) {
                  _39_rhs = (((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("ops"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Deref"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("deref"))).Apply1(RAST.__default.Borrow((RAST.Expr.create_Identifier((_40_hasMatchingField).dtor_value)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0"))));
                } else {
                  _39_rhs = RAST.Expr.create_Identifier((_40_hasMatchingField).dtor_value);
                }
              } else {
                _39_rhs = Defs.__default.UnreachablePanicIfVerified((this).pointerType, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("field does not exist on this variant"));
              }
              RAST._IMatchCase _46_ctorMatch;
              _46_ctorMatch = RAST.MatchCase.create(_38_pattern, _39_rhs);
              _35_cases = Dafny.Sequence<RAST._IMatchCase>.Concat(_35_cases, Dafny.Sequence<RAST._IMatchCase>.FromElements(_46_ctorMatch));
            }
            if (((new BigInteger(((c).dtor_typeParams).Count)).Sign == 1) && ((new BigInteger((_21_unusedTypeParams).Count)).Sign == 1)) {
              _35_cases = Dafny.Sequence<RAST._IMatchCase>.Concat(_35_cases, Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(_4_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::_PhantomVariant(..)")), Defs.__default.UnreachablePanicIfVerified((this).pointerType, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))));
            }
            RAST._IExpr _47_methodBody;
            _47_methodBody = RAST.Expr.create_Match(RAST.__default.self, _35_cases);
            _27_implBody = Dafny.Sequence<RAST._IImplMember>.Concat(_27_implBody, Dafny.Sequence<RAST._IImplMember>.FromElements(RAST.ImplMember.create_FnDecl((((new BigInteger(((c).dtor_ctors).Count)) == (BigInteger.One)) ? (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Returns a borrow of the field "), _33_callName)) : (Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Gets the field "), _33_callName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" for all enum members which have it")))), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), RAST.Visibility.create_PUB(), RAST.Fn.create(_33_callName, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(), Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.selfBorrowed), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Borrowed(_34_formalType)), Std.Wrappers.Option<RAST._IExpr>.create_Some(_47_methodBody)))));
          }
        }
      }
      Dafny.ISequence<RAST._IType> _48_coerceTypes;
      _48_coerceTypes = Dafny.Sequence<RAST._IType>.FromElements();
      Dafny.ISequence<RAST._ITypeParamDecl> _49_rCoerceTypeParams;
      _49_rCoerceTypeParams = Dafny.Sequence<RAST._ITypeParamDecl>.FromElements();
      Dafny.ISequence<RAST._IFormal> _50_coerceArguments;
      _50_coerceArguments = Dafny.Sequence<RAST._IFormal>.FromElements();
      Dafny.IMap<DAST._IType,DAST._IType> _51_coerceMap;
      _51_coerceMap = Dafny.Map<DAST._IType, DAST._IType>.FromElements();
      Dafny.IMap<RAST._IType,RAST._IType> _52_rCoerceMap;
      _52_rCoerceMap = Dafny.Map<RAST._IType, RAST._IType>.FromElements();
      Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr> _53_coerceMapToArg;
      _53_coerceMapToArg = Dafny.Map<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>.FromElements();
      if ((new BigInteger(((c).dtor_typeParams).Count)).Sign == 1) {
        Dafny.ISequence<RAST._IType> _54_types;
        _54_types = Dafny.Sequence<RAST._IType>.FromElements();
        BigInteger _hi6 = new BigInteger(((c).dtor_typeParams).Count);
        for (BigInteger _55_typeI = BigInteger.Zero; _55_typeI < _hi6; _55_typeI++) {
          DAST._ITypeArgDecl _56_typeParam;
          _56_typeParam = ((c).dtor_typeParams).Select(_55_typeI);
          DAST._IType _57_typeArg;
          RAST._ITypeParamDecl _58_rTypeParamDecl;
          DAST._IType _out9;
          RAST._ITypeParamDecl _out10;
          (this).GenTypeParam(_56_typeParam, out _out9, out _out10);
          _57_typeArg = _out9;
          _58_rTypeParamDecl = _out10;
          RAST._IType _59_rTypeArg;
          RAST._IType _out11;
          _out11 = (this).GenType(_57_typeArg, Defs.GenTypeContext.@default());
          _59_rTypeArg = _out11;
          _54_types = Dafny.Sequence<RAST._IType>.Concat(_54_types, Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_TypeApp((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("marker"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("PhantomData"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_59_rTypeArg))));
          if (((_55_typeI) < (new BigInteger((_7_typeParamInfos).Count))) && ((((_7_typeParamInfos).Select(_55_typeI)).dtor_variance).is_Nonvariant)) {
            _48_coerceTypes = Dafny.Sequence<RAST._IType>.Concat(_48_coerceTypes, Dafny.Sequence<RAST._IType>.FromElements(_59_rTypeArg));
            goto continue_2_0;
          }
          DAST._ITypeArgDecl _60_coerceTypeParam;
          DAST._ITypeArgDecl _61_dt__update__tmp_h0 = _56_typeParam;
          Dafny.ISequence<Dafny.Rune> _62_dt__update_hname_h0 = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_T"), Std.Strings.__default.OfNat(_55_typeI));
          _60_coerceTypeParam = DAST.TypeArgDecl.create(_62_dt__update_hname_h0, (_61_dt__update__tmp_h0).dtor_bounds, (_61_dt__update__tmp_h0).dtor_info);
          DAST._IType _63_coerceTypeArg;
          RAST._ITypeParamDecl _64_rCoerceTypeParamDecl;
          DAST._IType _out12;
          RAST._ITypeParamDecl _out13;
          (this).GenTypeParam(_60_coerceTypeParam, out _out12, out _out13);
          _63_coerceTypeArg = _out12;
          _64_rCoerceTypeParamDecl = _out13;
          _51_coerceMap = Dafny.Map<DAST._IType, DAST._IType>.Merge(_51_coerceMap, Dafny.Map<DAST._IType, DAST._IType>.FromElements(new Dafny.Pair<DAST._IType, DAST._IType>(_57_typeArg, _63_coerceTypeArg)));
          RAST._IType _65_rCoerceType;
          RAST._IType _out14;
          _out14 = (this).GenType(_63_coerceTypeArg, Defs.GenTypeContext.@default());
          _65_rCoerceType = _out14;
          _52_rCoerceMap = Dafny.Map<RAST._IType, RAST._IType>.Merge(_52_rCoerceMap, Dafny.Map<RAST._IType, RAST._IType>.FromElements(new Dafny.Pair<RAST._IType, RAST._IType>(_59_rTypeArg, _65_rCoerceType)));
          _48_coerceTypes = Dafny.Sequence<RAST._IType>.Concat(_48_coerceTypes, Dafny.Sequence<RAST._IType>.FromElements(_65_rCoerceType));
          _49_rCoerceTypeParams = Dafny.Sequence<RAST._ITypeParamDecl>.Concat(_49_rCoerceTypeParams, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(_64_rCoerceTypeParamDecl));
          Dafny.ISequence<Dafny.Rune> _66_coerceFormal;
          _66_coerceFormal = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("f_"), Std.Strings.__default.OfNat(_55_typeI));
          _53_coerceMapToArg = Dafny.Map<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>.Merge(_53_coerceMapToArg, Dafny.Map<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>.FromElements(new Dafny.Pair<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>(_System.Tuple2<RAST._IType, RAST._IType>.create(_59_rTypeArg, _65_rCoerceType), (RAST.Expr.create_Identifier(_66_coerceFormal)).Clone())));
          _50_coerceArguments = Dafny.Sequence<RAST._IFormal>.Concat(_50_coerceArguments, Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.create(_66_coerceFormal, RAST.__default.Rc(RAST.Type.create_IntersectionType(RAST.Type.create_ImplType(RAST.Type.create_FnType(Dafny.Sequence<RAST._IType>.FromElements(_59_rTypeArg), _65_rCoerceType)), RAST.__default.StaticTrait)))));
        continue_2_0: ;
        }
      after_2_0: ;
        if ((new BigInteger((_21_unusedTypeParams).Count)).Sign == 1) {
          _6_ctors = Dafny.Sequence<RAST._IEnumCase>.Concat(_6_ctors, Dafny.Sequence<RAST._IEnumCase>.FromElements(RAST.EnumCase.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_PhantomVariant"), RAST.Fields.create_NamelessFields(Std.Collections.Seq.__default.Map<RAST._IType, RAST._INamelessField>(((System.Func<RAST._IType, RAST._INamelessField>)((_67_tpe) => {
  return RAST.NamelessField.create(RAST.Visibility.create_PRIV(), _67_tpe);
})), _54_types)))));
        }
      }
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _68_typeParametersSupportingEquality;
      _68_typeParametersSupportingEquality = Dafny.Helpers.Id<Func<DAST._IDatatype, Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>>((_69_c) => ((System.Func<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>)(() => {
        var _coll1 = new System.Collections.Generic.List<Dafny.ISequence<Dafny.Rune>>();
        foreach (DAST._ITypeArgDecl _compr_1 in ((_69_c).dtor_typeParams).CloneAsArray()) {
          DAST._ITypeArgDecl _70_tp = (DAST._ITypeArgDecl)_compr_1;
          if ((((_69_c).dtor_typeParams).Contains(_70_tp)) && (((_70_tp).dtor_bounds).Contains(DAST.TypeArgBound.create_SupportsEquality()))) {
            _coll1.Add((_70_tp).dtor_name);
          }
        }
        return Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromCollection(_coll1);
      }))())(c);
      bool _71_cIsAlwaysEq;
      _71_cIsAlwaysEq = (this).DatatypeIsEq(c, _68_typeParametersSupportingEquality);
      RAST._IType _72_datatypeType;
      _72_datatypeType = RAST.Type.create_TypeApp(RAST.Type.create_TIdentifier(_4_datatypeName), _2_rTypeParams);
      s = Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_EnumDecl(RAST.Enum.create((c).dtor_docString, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#[derive(Clone)]")), _4_datatypeName, _3_rTypeParamsDecls, _6_ctors)), RAST.ModDecl.create_ImplDecl(RAST.Impl.create_Impl(_3_rTypeParamsDecls, _72_datatypeType, _27_implBody)));
      if ((new BigInteger(((c).dtor_superTraitTypes).Count)).Sign == 1) {
        RAST._IType _73_fullType;
        if (_0_isRcWrapped) {
          _73_fullType = RAST.__default.Rc(_72_datatypeType);
        } else {
          _73_fullType = _72_datatypeType;
        }
        Std.Wrappers._IOption<RAST._IModDecl> _74_downcastDefinitionOpt;
        _74_downcastDefinitionOpt = Defs.__default.DowncastTraitDeclFor(_3_rTypeParamsDecls, _73_fullType);
        if ((_74_downcastDefinitionOpt).is_None) {
          RAST._IExpr _75_dummy;
          RAST._IExpr _out15;
          _out15 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not generate downcast definition for "), (_73_fullType)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), (this).InitEmptyExpr());
          _75_dummy = _out15;
        } else {
          s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements((_74_downcastDefinitionOpt).dtor_value));
        }
        Std.Wrappers._IOption<RAST._IModDecl> _76_downcastImplementationsOpt;
        _76_downcastImplementationsOpt = Defs.__default.DowncastImplFor(_3_rTypeParamsDecls, _73_fullType);
        if ((_76_downcastImplementationsOpt).is_None) {
          RAST._IExpr _77_dummy;
          RAST._IExpr _out16;
          _out16 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not generate downcast implementation for "), (_73_fullType)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), (this).InitEmptyExpr());
          _77_dummy = _out16;
        } else {
          s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements((_76_downcastImplementationsOpt).dtor_value));
        }
        BigInteger _hi7 = new BigInteger(((c).dtor_superTraitNegativeTypes).Count);
        for (BigInteger _78_i = BigInteger.Zero; _78_i < _hi7; _78_i++) {
          RAST._IType _79_negativeTraitType;
          RAST._IType _out17;
          _out17 = (this).GenType(((c).dtor_superTraitNegativeTypes).Select(_78_i), Defs.GenTypeContext.@default());
          _79_negativeTraitType = _out17;
          Std.Wrappers._IOption<RAST._IModDecl> _80_downcastDefinitionOpt;
          _80_downcastDefinitionOpt = Defs.__default.DowncastNotImplFor(_3_rTypeParamsDecls, _79_negativeTraitType, _73_fullType);
          if ((_80_downcastDefinitionOpt).is_None) {
            RAST._IExpr _81_dummy;
            RAST._IExpr _out18;
            _out18 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not generate negative downcast definition for "), (_73_fullType)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), (this).InitEmptyExpr());
            _81_dummy = _out18;
          } else {
            s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements((_80_downcastDefinitionOpt).dtor_value));
          }
        }
        BigInteger _hi8 = new BigInteger(((c).dtor_superTraitTypes).Count);
        for (BigInteger _82_i = BigInteger.Zero; _82_i < _hi8; _82_i++) {
          DAST._IType _83_c;
          _83_c = ((c).dtor_superTraitTypes).Select(_82_i);
          if ((((_83_c).is_UserDefined) && ((((_83_c).dtor_resolved).dtor_kind).is_Trait)) && ((new BigInteger((((_83_c).dtor_resolved).dtor_extendedTypes).Count)).Sign == 0)) {
            goto continue_3_0;
          }
          RAST._IType _84_cType;
          RAST._IType _out19;
          _out19 = (this).GenType(_83_c, Defs.GenTypeContext.@default());
          _84_cType = _out19;
          bool _85_isImplementing;
          _85_isImplementing = true;
          Std.Wrappers._IOption<RAST._IModDecl> _86_downcastImplementationsOpt;
          _86_downcastImplementationsOpt = Defs.__default.DowncastImplTraitFor(_3_rTypeParamsDecls, _84_cType, _85_isImplementing, _73_fullType);
          if ((_86_downcastImplementationsOpt).is_None) {
            RAST._IExpr _87_dummy;
            RAST._IExpr _out20;
            _out20 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not generate downcast implementation of "), (_84_cType)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" for ")), (_73_fullType)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), (this).InitEmptyExpr());
            _87_dummy = _out20;
          } else {
            s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements((_86_downcastImplementationsOpt).dtor_value));
          }
        continue_3_0: ;
        }
      after_3_0: ;
      }
      Dafny.ISequence<RAST._IMatchCase> _88_printImplBodyCases;
      _88_printImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.FromElements();
      Dafny.ISequence<RAST._IMatchCase> _89_hashImplBodyCases;
      _89_hashImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.FromElements();
      Dafny.ISequence<RAST._IMatchCase> _90_coerceImplBodyCases;
      _90_coerceImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.FromElements();
      Dafny.ISequence<RAST._IMatchCase> _91_partialEqImplBodyCases;
      _91_partialEqImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.FromElements();
      BigInteger _hi9 = new BigInteger(((c).dtor_ctors).Count);
      for (BigInteger _92_i = BigInteger.Zero; _92_i < _hi9; _92_i++) {
        DAST._IDatatypeCtor _93_ctor;
        _93_ctor = ((c).dtor_ctors).Select(_92_i);
        Dafny.ISequence<Dafny.Rune> _94_ctorMatch;
        _94_ctorMatch = Defs.__default.escapeName((_93_ctor).dtor_name);
        Dafny.ISequence<Dafny.Rune> _95_modulePrefix;
        if (((((c).dtor_enclosingModule))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_module"))) {
          _95_modulePrefix = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
        } else {
          _95_modulePrefix = Dafny.Sequence<Dafny.Rune>.Concat((((c).dtor_enclosingModule)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("."));
        }
        Dafny.ISequence<Dafny.Rune> _96_ctorName;
        _96_ctorName = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_95_modulePrefix, ((c).dtor_name)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".")), ((_93_ctor).dtor_name));
        if (((new BigInteger((_96_ctorName).Count)) >= (new BigInteger(13))) && (((_96_ctorName).Subsequence(BigInteger.Zero, new BigInteger(13))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_System.Tuple")))) {
          _96_ctorName = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
        }
        RAST._IExpr _97_printRhs;
        _97_printRhs = (this).writeStr(Dafny.Sequence<Dafny.Rune>.Concat(_96_ctorName, (((_93_ctor).dtor_hasAnyArgs) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))), false);
        RAST._IExpr _98_hashRhs;
        _98_hashRhs = (this).InitEmptyExpr();
        RAST._IExpr _99_partialEqRhs;
        _99_partialEqRhs = RAST.Expr.create_LiteralBool(true);
        Dafny.ISequence<RAST._IAssignIdentifier> _100_coerceRhsArgs;
        _100_coerceRhsArgs = Dafny.Sequence<RAST._IAssignIdentifier>.FromElements();
        bool _101_isNumeric;
        _101_isNumeric = false;
        Dafny.ISequence<Dafny.Rune> _102_ctorMatchInner;
        _102_ctorMatchInner = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
        Dafny.ISequence<Dafny.Rune> _103_ctorMatchInner2;
        _103_ctorMatchInner2 = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
        BigInteger _hi10 = new BigInteger(((_93_ctor).dtor_args).Count);
        for (BigInteger _104_j = BigInteger.Zero; _104_j < _hi10; _104_j++) {
          DAST._IDatatypeDtor _105_dtor;
          _105_dtor = ((_93_ctor).dtor_args).Select(_104_j);
          Dafny.ISequence<Dafny.Rune> _106_patternName;
          _106_patternName = Defs.__default.escapeVar(((_105_dtor).dtor_formal).dtor_name);
          DAST._IType _107_formalType;
          _107_formalType = ((_105_dtor).dtor_formal).dtor_typ;
          if (((_104_j).Sign == 0) && ((_106_patternName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")))) {
            _101_isNumeric = true;
          }
          if (_101_isNumeric) {
            _106_patternName = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.GetOr((_105_dtor).dtor_callName, Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("v"), Std.Strings.__default.OfNat(_104_j)));
          }
          if ((_107_formalType).is_Arrow) {
            _98_hashRhs = (_98_hashRhs).Then(((RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0"))).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("hash"))).Apply1(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_state"))));
          } else {
            _98_hashRhs = (_98_hashRhs).Then((Defs.__default.hash__function).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(_106_patternName), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_state")))));
          }
          _102_ctorMatchInner = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_102_ctorMatchInner, _106_patternName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "));
          _103_ctorMatchInner2 = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_103_ctorMatchInner2, _106_patternName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(": ")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_2_")), _106_patternName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "));
          if ((_107_formalType).is_Arrow) {
            _99_partialEqRhs = (_99_partialEqRhs).And(RAST.Expr.create_LiteralBool(false));
          } else {
            _99_partialEqRhs = (_99_partialEqRhs).And((RAST.Expr.create_Identifier(_106_patternName)).Equals(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_2_"), _106_patternName))));
          }
          if ((_104_j).Sign == 1) {
            _97_printRhs = (_97_printRhs).Then((this).writeStr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", "), false));
          }
          _97_printRhs = (_97_printRhs).Then((((_107_formalType).is_Arrow) ? ((this).writeStr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<function>"), false)) : (RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("?"), ((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyPrint"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fmt_print"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(_106_patternName), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_formatter")), RAST.Expr.create_LiteralBool(false))), DAST.Format.UnaryOpFormat.create_NoFormat()))));
          RAST._IExpr _108_coerceRhsArg = RAST.Expr.Default();
          RAST._IType _109_formalTpe;
          RAST._IType _out21;
          _out21 = (this).GenType(_107_formalType, Defs.GenTypeContext.@default());
          _109_formalTpe = _out21;
          DAST._IType _110_newFormalType;
          _110_newFormalType = (_107_formalType).Replace(_51_coerceMap);
          RAST._IType _111_newFormalTpe;
          _111_newFormalTpe = (_109_formalTpe).ReplaceMap(_52_rCoerceMap);
          Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _112_upcastConverter;
          _112_upcastConverter = (this).UpcastConversionLambda(_107_formalType, _109_formalTpe, _110_newFormalType, _111_newFormalTpe, _53_coerceMapToArg);
          if ((_112_upcastConverter).is_Success) {
            RAST._IExpr _113_coercionFunction;
            _113_coercionFunction = (_112_upcastConverter).dtor_value;
            _108_coerceRhsArg = (_113_coercionFunction).Apply1(RAST.Expr.create_Identifier(_106_patternName));
          } else {
            (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not generate coercion function for contructor "), Std.Strings.__default.OfNat(_104_j)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" of ")), _4_datatypeName));
            _108_coerceRhsArg = (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("todo!"))).Apply1(RAST.Expr.create_LiteralString((this.error).dtor_value, false, false));
          }
          _100_coerceRhsArgs = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_100_coerceRhsArgs, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(_106_patternName, _108_coerceRhsArg)));
        }
        RAST._IExpr _114_coerceRhs;
        _114_coerceRhs = RAST.Expr.create_StructBuild((RAST.Expr.create_Identifier(_4_datatypeName)).FSel(Defs.__default.escapeName((_93_ctor).dtor_name)), _100_coerceRhsArgs);
        Dafny.ISequence<Dafny.Rune> _115_pattern = Dafny.Sequence<Dafny.Rune>.Empty;
        Dafny.ISequence<Dafny.Rune> _116_pattern2 = Dafny.Sequence<Dafny.Rune>.Empty;
        if (_101_isNumeric) {
          _115_pattern = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_94_ctorMatch, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")), _102_ctorMatchInner), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
          _116_pattern2 = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_94_ctorMatch, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("(")), _103_ctorMatchInner2), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"));
        } else {
          _115_pattern = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_94_ctorMatch, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{")), _102_ctorMatchInner), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
          _116_pattern2 = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_94_ctorMatch, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{")), _103_ctorMatchInner2), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("}"));
        }
        if ((_93_ctor).dtor_hasAnyArgs) {
          _97_printRhs = (_97_printRhs).Then((this).writeStr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")"), false));
        }
        _97_printRhs = (_97_printRhs).Then((RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Ok"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Tuple(Dafny.Sequence<RAST._IExpr>.FromElements()))));
        _88_printImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_88_printImplBodyCases, Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_4_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), _115_pattern), RAST.Expr.create_Block(_97_printRhs))));
        _89_hashImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_89_hashImplBodyCases, Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_4_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), _115_pattern), RAST.Expr.create_Block(_98_hashRhs))));
        _91_partialEqImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_91_partialEqImplBodyCases, Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("("), _4_datatypeName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), _115_pattern), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(", ")), _4_datatypeName), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), _116_pattern2), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(")")), RAST.Expr.create_Block(_99_partialEqRhs))));
        _90_coerceImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_90_coerceImplBodyCases, Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(_4_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")), _94_ctorMatch), RAST.Expr.create_Block(_114_coerceRhs))));
      }
      _91_partialEqImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_91_partialEqImplBodyCases, Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"), RAST.Expr.create_Block(RAST.Expr.create_LiteralBool(false)))));
      if (((new BigInteger(((c).dtor_typeParams).Count)).Sign == 1) && ((new BigInteger((_21_unusedTypeParams).Count)).Sign == 1)) {
        Dafny.ISequence<RAST._IMatchCase> _117_extraCases;
        _117_extraCases = Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.Concat(_4_datatypeName, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::_PhantomVariant(..)")), RAST.Expr.create_Block(Defs.__default.UnreachablePanicIfVerified((this).pointerType, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")))));
        _88_printImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_88_printImplBodyCases, _117_extraCases);
        _89_hashImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_89_hashImplBodyCases, _117_extraCases);
        _90_coerceImplBodyCases = Dafny.Sequence<RAST._IMatchCase>.Concat(_90_coerceImplBodyCases, _117_extraCases);
      }
      Dafny.ISequence<RAST._ITypeParamDecl> _118_defaultConstrainedTypeParams;
      _118_defaultConstrainedTypeParams = RAST.TypeParamDecl.AddConstraintsMultiple(_3_rTypeParamsDecls, Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.DefaultTrait));
      RAST._IExpr _119_printImplBody;
      _119_printImplBody = RAST.Expr.create_Match(RAST.__default.self, _88_printImplBodyCases);
      RAST._IExpr _120_hashImplBody;
      _120_hashImplBody = RAST.Expr.create_Match(RAST.__default.self, _89_hashImplBodyCases);
      RAST._IExpr _121_eqImplBody;
      _121_eqImplBody = RAST.Expr.create_Match(RAST.Expr.create_Tuple(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.__default.self, RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("other")))), _91_partialEqImplBodyCases);
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(Defs.__default.DebugImpl(_3_rTypeParamsDecls, _72_datatypeType, _2_rTypeParams), Defs.__default.PrintImpl(_3_rTypeParamsDecls, _72_datatypeType, _2_rTypeParams, _119_printImplBody)));
      if ((new BigInteger((_49_rCoerceTypeParams).Count)).Sign == 1) {
        RAST._IExpr _122_coerceImplBody;
        _122_coerceImplBody = RAST.Expr.create_Match(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this")), _90_coerceImplBodyCases);
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(Defs.__default.CoerceImpl(_3_rTypeParamsDecls, _4_datatypeName, _72_datatypeType, _49_rCoerceTypeParams, _50_coerceArguments, _48_coerceTypes, _122_coerceImplBody)));
      }
      if ((new BigInteger((_9_singletonConstructors).Count)) == (new BigInteger(((c).dtor_ctors).Count))) {
        RAST._IType _123_instantiationType;
        if (_0_isRcWrapped) {
          _123_instantiationType = RAST.__default.Rc(_72_datatypeType);
        } else {
          _123_instantiationType = _72_datatypeType;
        }
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(Defs.__default.SingletonsImpl(_3_rTypeParamsDecls, _72_datatypeType, _123_instantiationType, _9_singletonConstructors)));
      }
      if ((((c).dtor_equalitySupport).is_Always) || (((c).dtor_equalitySupport).is_ConsultTypeArguments)) {
        Dafny.ISequence<RAST._ITypeParamDecl> _124_rTypeParamsDeclsWithEq;
        _124_rTypeParamsDeclsWithEq = _3_rTypeParamsDecls;
        Dafny.ISequence<RAST._ITypeParamDecl> _125_rTypeParamsDeclsWithHash;
        _125_rTypeParamsDeclsWithHash = _3_rTypeParamsDecls;
        if (((c).dtor_equalitySupport).is_ConsultTypeArguments) {
          BigInteger _hi11 = new BigInteger((_3_rTypeParamsDecls).Count);
          for (BigInteger _126_i = BigInteger.Zero; _126_i < _hi11; _126_i++) {
            if (((((c).dtor_typeParams).Select(_126_i)).dtor_info).dtor_necessaryForEqualitySupportOfSurroundingInductiveDatatype) {
              _124_rTypeParamsDeclsWithEq = Dafny.Sequence<RAST._ITypeParamDecl>.Update(_124_rTypeParamsDeclsWithEq, _126_i, ((_124_rTypeParamsDeclsWithEq).Select(_126_i)).AddConstraints(Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.Eq, RAST.__default.Hash)));
              _125_rTypeParamsDeclsWithHash = Dafny.Sequence<RAST._ITypeParamDecl>.Update(_125_rTypeParamsDeclsWithHash, _126_i, ((_125_rTypeParamsDeclsWithHash).Select(_126_i)).AddConstraints(Dafny.Sequence<RAST._IType>.FromElements(RAST.__default.Hash)));
            }
          }
        }
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Defs.__default.EqImpl(_124_rTypeParamsDeclsWithEq, _72_datatypeType, _2_rTypeParams, _121_eqImplBody));
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(Defs.__default.HashImpl(_125_rTypeParamsDeclsWithHash, _72_datatypeType, _120_hashImplBody)));
      }
      if ((new BigInteger(((c).dtor_ctors).Count)).Sign == 1) {
        RAST._IExpr _127_structName;
        _127_structName = (RAST.Expr.create_Identifier(_4_datatypeName)).FSel(Defs.__default.escapeName((((c).dtor_ctors).Select(BigInteger.Zero)).dtor_name));
        Dafny.ISequence<RAST._IAssignIdentifier> _128_structAssignments;
        _128_structAssignments = Dafny.Sequence<RAST._IAssignIdentifier>.FromElements();
        BigInteger _hi12 = new BigInteger(((((c).dtor_ctors).Select(BigInteger.Zero)).dtor_args).Count);
        for (BigInteger _129_i = BigInteger.Zero; _129_i < _hi12; _129_i++) {
          DAST._IDatatypeDtor _130_dtor;
          _130_dtor = ((((c).dtor_ctors).Select(BigInteger.Zero)).dtor_args).Select(_129_i);
          _128_structAssignments = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_128_structAssignments, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(Defs.__default.escapeVar(((_130_dtor).dtor_formal).dtor_name), (((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("default"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Default"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("default"))).Apply0())));
        }
        if ((false) && (_71_cIsAlwaysEq)) {
          s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(Defs.__default.DefaultDatatypeImpl(_3_rTypeParamsDecls, _72_datatypeType, _127_structName, _128_structAssignments)));
        }
        s = Dafny.Sequence<RAST._IModDecl>.Concat(s, Dafny.Sequence<RAST._IModDecl>.FromElements(Defs.__default.AsRefDatatypeImpl(_3_rTypeParamsDecls, _72_datatypeType)));
      }
      Dafny.ISequence<RAST._IModDecl> _131_superTraitImplementations;
      Dafny.ISequence<RAST._IModDecl> _out22;
      _out22 = (this).GenTraitImplementations(path, _2_rTypeParams, _3_rTypeParamsDecls, (c).dtor_superTraitTypes, _26_traitBodies, _5_extern, _71_cIsAlwaysEq, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("datatype"));
      _131_superTraitImplementations = _out22;
      s = Dafny.Sequence<RAST._IModDecl>.Concat(s, _131_superTraitImplementations);
      return s;
    }
    public RAST._IPath GenPath(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> p, bool escape)
    {
      RAST._IPath r = RAST.Path.Default();
      if ((new BigInteger((p).Count)).Sign == 0) {
        r = RAST.Path.create_Self();
        return r;
      } else {
        Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _0_p;
        _0_p = p;
        Dafny.ISequence<Dafny.Rune> _1_name;
        _1_name = (((_0_p).Select(BigInteger.Zero)));
        if (((new BigInteger((_1_name).Count)) >= (new BigInteger(2))) && (((_1_name).Subsequence(BigInteger.Zero, new BigInteger(2))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("::")))) {
          r = RAST.Path.create_Global();
          _0_p = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Update(_0_p, BigInteger.Zero, (_1_name).Drop(new BigInteger(2)));
        } else if (((((_0_p).Select(BigInteger.Zero)))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_System"))) {
          r = RAST.__default.dafny__runtime;
        } else {
          r = (this).thisFile;
        }
        BigInteger _hi0 = new BigInteger((_0_p).Count);
        for (BigInteger _2_i = BigInteger.Zero; _2_i < _hi0; _2_i++) {
          Dafny.ISequence<Dafny.Rune> _3_name;
          _3_name = ((_0_p).Select(_2_i));
          if (escape) {
            _System._ITuple2<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<Dafny.Rune>> _let_tmp_rhs0 = DafnyCompilerRustUtils.__default.DafnyNameToContainingPathAndName(_3_name, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements());
            Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _4_modules = _let_tmp_rhs0.dtor__0;
            Dafny.ISequence<Dafny.Rune> _5_finalName = _let_tmp_rhs0.dtor__1;
            BigInteger _hi1 = new BigInteger((_4_modules).Count);
            for (BigInteger _6_j = BigInteger.Zero; _6_j < _hi1; _6_j++) {
              r = (r).MSel(Defs.__default.escapeName(((_4_modules).Select(_6_j))));
            }
            r = (r).MSel(Defs.__default.escapeName(_5_finalName));
          } else {
            r = (r).MSels(Defs.__default.SplitRustPathElement(Defs.__default.ReplaceDotByDoubleColon((_3_name)), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")));
          }
        }
      }
      return r;
    }
    public RAST._IType GenPathType(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> p)
    {
      RAST._IType t = RAST.Type.Default();
      RAST._IPath _0_p;
      RAST._IPath _out0;
      _out0 = (this).GenPath(p, true);
      _0_p = _out0;
      t = (_0_p).AsType();
      return t;
    }
    public RAST._IExpr GenPathExpr(Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> p, bool escape)
    {
      RAST._IExpr e = RAST.Expr.Default();
      if ((new BigInteger((p).Count)).Sign == 0) {
        e = RAST.__default.self;
        return e;
      }
      RAST._IPath _0_p;
      RAST._IPath _out0;
      _out0 = (this).GenPath(p, escape);
      _0_p = _out0;
      e = (_0_p).AsExpr();
      return e;
    }
    public Dafny.ISequence<RAST._IType> GenTypeArgs(Dafny.ISequence<DAST._IType> args, bool genTypeContext)
    {
      Dafny.ISequence<RAST._IType> s = Dafny.Sequence<RAST._IType>.Empty;
      s = Dafny.Sequence<RAST._IType>.FromElements();
      BigInteger _hi0 = new BigInteger((args).Count);
      for (BigInteger _0_i = BigInteger.Zero; _0_i < _hi0; _0_i++) {
        RAST._IType _1_genTp;
        RAST._IType _out0;
        _out0 = (this).GenType((args).Select(_0_i), genTypeContext);
        _1_genTp = _out0;
        s = Dafny.Sequence<RAST._IType>.Concat(s, Dafny.Sequence<RAST._IType>.FromElements(_1_genTp));
      }
      return s;
    }
    public RAST._IType GenType(DAST._IType c, bool genTypeContext)
    {
      RAST._IType s = RAST.Type.Default();
      DAST._IType _source0 = c;
      {
        if (_source0.is_UserDefined) {
          DAST._IResolvedType _0_resolved = _source0.dtor_resolved;
          {
            RAST._IType _1_t;
            RAST._IType _out0;
            _out0 = (this).GenPathType((_0_resolved).dtor_path);
            _1_t = _out0;
            Dafny.ISequence<RAST._IType> _2_typeArgs;
            Dafny.ISequence<RAST._IType> _out1;
            _out1 = (this).GenTypeArgs((_0_resolved).dtor_typeArgs, false);
            _2_typeArgs = _out1;
            if ((new BigInteger((_2_typeArgs).Count)).Sign == 1) {
              s = RAST.Type.create_TypeApp(_1_t, _2_typeArgs);
            } else {
              s = _1_t;
            }
            DAST._IResolvedTypeBase _source1 = (_0_resolved).dtor_kind;
            {
              if (_source1.is_Class) {
                {
                  s = (this).Object(s);
                }
                goto after_match1;
              }
            }
            {
              if (_source1.is_Datatype) {
                {
                  if (Defs.__default.IsRcWrapped((_0_resolved).dtor_attributes)) {
                    s = RAST.__default.Rc(s);
                  }
                }
                goto after_match1;
              }
            }
            {
              if (_source1.is_Trait) {
                DAST._ITraitType traitType0 = _source1.dtor_traitType;
                if (traitType0.is_GeneralTrait) {
                  {
                    if (!((genTypeContext))) {
                      s = RAST.__default.Box(RAST.Type.create_DynType(s));
                    }
                  }
                  goto after_match1;
                }
              }
            }
            {
              if (_source1.is_Trait) {
                DAST._ITraitType traitType1 = _source1.dtor_traitType;
                if (traitType1.is_ObjectTrait) {
                  {
                    if (((_0_resolved).dtor_path).Equals(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_System"), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("object")))) {
                      s = RAST.__default.AnyTrait;
                    }
                    if (!((genTypeContext))) {
                      s = (this).Object(RAST.Type.create_DynType(s));
                    }
                  }
                  goto after_match1;
                }
              }
            }
            {
              if (_source1.is_SynonymType) {
                DAST._IType _3_base = _source1.dtor_baseType;
                {
                  RAST._IType _4_underlying;
                  RAST._IType _out2;
                  _out2 = (this).GenType(_3_base, Defs.GenTypeContext.@default());
                  _4_underlying = _out2;
                  s = RAST.Type.create_TSynonym(s, _4_underlying);
                }
                goto after_match1;
              }
            }
            {
              DAST._IType _5_base = _source1.dtor_baseType;
              DAST._INewtypeRange _6_range = _source1.dtor_range;
              bool _7_erased = _source1.dtor_erase;
              {
                if (_7_erased) {
                  Std.Wrappers._IOption<RAST._IType> _8_unwrappedType;
                  _8_unwrappedType = Defs.__default.NewtypeRangeToUnwrappedBoundedRustType(_5_base, _6_range);
                  if ((_8_unwrappedType).is_Some) {
                    s = (_8_unwrappedType).dtor_value;
                  } else {
                    RAST._IType _9_unwrappedType;
                    RAST._IType _out3;
                    _out3 = (this).GenType(_5_base, Defs.GenTypeContext.@default());
                    _9_unwrappedType = _out3;
                    s = _9_unwrappedType;
                  }
                } else if (Defs.__default.IsNewtypeCopy(_6_range)) {
                  s = RAST.Type.create_TMetaData(s, true, (_6_range).CanOverflow());
                }
              }
            }
          after_match1: ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Object) {
          {
            s = RAST.__default.AnyTrait;
            if (!((genTypeContext))) {
              s = (this).Object(RAST.Type.create_DynType(s));
            }
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Tuple) {
          Dafny.ISequence<DAST._IType> _10_types = _source0.dtor_Tuple_a0;
          {
            Dafny.ISequence<RAST._IType> _11_args;
            _11_args = Dafny.Sequence<RAST._IType>.FromElements();
            BigInteger _12_i;
            _12_i = BigInteger.Zero;
            while ((_12_i) < (new BigInteger((_10_types).Count))) {
              RAST._IType _13_generated;
              RAST._IType _out4;
              _out4 = (this).GenType((_10_types).Select(_12_i), false);
              _13_generated = _out4;
              _11_args = Dafny.Sequence<RAST._IType>.Concat(_11_args, Dafny.Sequence<RAST._IType>.FromElements(_13_generated));
              _12_i = (_12_i) + (BigInteger.One);
            }
            if ((new BigInteger((_10_types).Count)) <= (RAST.__default.MAX__TUPLE__SIZE)) {
              s = RAST.Type.create_TupleType(_11_args);
            } else {
              s = RAST.__default.SystemTupleType(_11_args);
            }
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Array) {
          DAST._IType _14_element = _source0.dtor_element;
          BigInteger _15_dims = _source0.dtor_dims;
          {
            if ((_15_dims) > (new BigInteger(16))) {
              s = RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<i>Array of dimensions greater than 16</i>"));
            } else {
              RAST._IType _16_elem;
              RAST._IType _out5;
              _out5 = (this).GenType(_14_element, false);
              _16_elem = _out5;
              if ((_15_dims) == (BigInteger.One)) {
                s = RAST.Type.create_Array(_16_elem, Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None());
                s = (this).Object(s);
              } else {
                Dafny.ISequence<Dafny.Rune> _17_n;
                _17_n = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Array"), Std.Strings.__default.OfNat(_15_dims));
                s = (((RAST.__default.dafny__runtime).MSel(_17_n)).AsType()).Apply(Dafny.Sequence<RAST._IType>.FromElements(_16_elem));
                s = (this).Object(s);
              }
            }
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Seq) {
          DAST._IType _18_element = _source0.dtor_element;
          {
            RAST._IType _19_elem;
            RAST._IType _out6;
            _out6 = (this).GenType(_18_element, false);
            _19_elem = _out6;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Sequence"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_19_elem));
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Set) {
          DAST._IType _20_element = _source0.dtor_element;
          {
            RAST._IType _21_elem;
            RAST._IType _out7;
            _out7 = (this).GenType(_20_element, false);
            _21_elem = _out7;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Set"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_21_elem));
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Multiset) {
          DAST._IType _22_element = _source0.dtor_element;
          {
            RAST._IType _23_elem;
            RAST._IType _out8;
            _out8 = (this).GenType(_22_element, false);
            _23_elem = _out8;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Multiset"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_23_elem));
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Map) {
          DAST._IType _24_key = _source0.dtor_key;
          DAST._IType _25_value = _source0.dtor_value;
          {
            RAST._IType _26_keyType;
            RAST._IType _out9;
            _out9 = (this).GenType(_24_key, false);
            _26_keyType = _out9;
            RAST._IType _27_valueType;
            RAST._IType _out10;
            _out10 = (this).GenType(_25_value, genTypeContext);
            _27_valueType = _out10;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Map"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_26_keyType, _27_valueType));
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MapBuilder) {
          DAST._IType _28_key = _source0.dtor_key;
          DAST._IType _29_value = _source0.dtor_value;
          {
            RAST._IType _30_keyType;
            RAST._IType _out11;
            _out11 = (this).GenType(_28_key, false);
            _30_keyType = _out11;
            RAST._IType _31_valueType;
            RAST._IType _out12;
            _out12 = (this).GenType(_29_value, genTypeContext);
            _31_valueType = _out12;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("MapBuilder"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_30_keyType, _31_valueType));
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SetBuilder) {
          DAST._IType _32_elem = _source0.dtor_element;
          {
            RAST._IType _33_elemType;
            RAST._IType _out13;
            _out13 = (this).GenType(_32_elem, false);
            _33_elemType = _out13;
            s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("SetBuilder"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(_33_elemType));
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Arrow) {
          Dafny.ISequence<DAST._IType> _34_args = _source0.dtor_args;
          DAST._IType _35_result = _source0.dtor_result;
          {
            Dafny.ISequence<RAST._IType> _36_argTypes;
            _36_argTypes = Dafny.Sequence<RAST._IType>.FromElements();
            BigInteger _37_i;
            _37_i = BigInteger.Zero;
            while ((_37_i) < (new BigInteger((_34_args).Count))) {
              RAST._IType _38_generated;
              RAST._IType _out14;
              _out14 = (this).GenType((_34_args).Select(_37_i), false);
              _38_generated = _out14;
              _36_argTypes = Dafny.Sequence<RAST._IType>.Concat(_36_argTypes, Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_Borrowed(_38_generated)));
              _37_i = (_37_i) + (BigInteger.One);
            }
            RAST._IType _39_resultType;
            RAST._IType _out15;
            _out15 = (this).GenType(_35_result, Defs.GenTypeContext.@default());
            _39_resultType = _out15;
            s = RAST.__default.Rc(RAST.Type.create_DynType(RAST.Type.create_FnType(_36_argTypes, _39_resultType)));
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_TypeArg) {
          Dafny.ISequence<Dafny.Rune> _h90 = _source0.dtor_TypeArg_a0;
          Dafny.ISequence<Dafny.Rune> _40_name = _h90;
          s = RAST.Type.create_TIdentifier(Defs.__default.escapeName(_40_name));
          goto after_match0;
        }
      }
      {
        if (_source0.is_Primitive) {
          DAST._IPrimitive _41_p = _source0.dtor_Primitive_a0;
          {
            DAST._IPrimitive _source2 = _41_p;
            {
              if (_source2.is_Int) {
                s = ((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyInt"))).AsType();
                goto after_match2;
              }
            }
            {
              if (_source2.is_Real) {
                s = ((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("BigRational"))).AsType();
                goto after_match2;
              }
            }
            {
              if (_source2.is_String) {
                s = RAST.Type.create_TypeApp(((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Sequence"))).AsType(), Dafny.Sequence<RAST._IType>.FromElements(((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsType()));
                goto after_match2;
              }
            }
            {
              if (_source2.is_Native) {
                s = RAST.Type.create_USIZE();
                goto after_match2;
              }
            }
            {
              if (_source2.is_Bool) {
                s = RAST.Type.create_Bool();
                goto after_match2;
              }
            }
            {
              s = ((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsType();
            }
          after_match2: ;
          }
          goto after_match0;
        }
      }
      {
        Dafny.ISequence<Dafny.Rune> _42_v = _source0.dtor_Passthrough_a0;
        s = RAST.__default.RawType(_42_v);
      }
    after_match0: ;
      return s;
    }
    public bool EnclosingIsTrait(DAST._IType tpe) {
      return ((tpe).is_UserDefined) && ((((tpe).dtor_resolved).dtor_kind).is_Trait);
    }
    public void GenClassImplBody(Dafny.ISequence<DAST._IMethod> body, bool forTrait, DAST._IType enclosingType, Dafny.ISequence<DAST._IType> enclosingTypeParams, out Dafny.ISequence<RAST._IImplMember> s, out Dafny.IMap<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>,Dafny.ISequence<RAST._IImplMember>> traitBodies)
    {
      s = Dafny.Sequence<RAST._IImplMember>.Empty;
      traitBodies = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.Empty;
      s = Dafny.Sequence<RAST._IImplMember>.FromElements();
      traitBodies = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.FromElements();
      BigInteger _hi0 = new BigInteger((body).Count);
      for (BigInteger _0_i = BigInteger.Zero; _0_i < _hi0; _0_i++) {
        DAST._IMethod _source0 = (body).Select(_0_i);
        {
          DAST._IMethod _1_m = _source0;
          {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> _source1 = (_1_m).dtor_overridingPath;
            {
              if (_source1.is_Some) {
                Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2_p = _source1.dtor_value;
                {
                  Dafny.ISequence<RAST._IImplMember> _3_existing;
                  _3_existing = Dafny.Sequence<RAST._IImplMember>.FromElements();
                  if ((traitBodies).Contains(_2_p)) {
                    _3_existing = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.Select(traitBodies,_2_p);
                  }
                  if (((new BigInteger(((_1_m).dtor_typeParams).Count)).Sign == 1) && ((this).EnclosingIsTrait(enclosingType))) {
                    (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Error: Rust does not support method with generic type parameters in traits"));
                  }
                  RAST._IImplMember _4_genMethod;
                  RAST._IImplMember _out0;
                  _out0 = (this).GenMethod(_1_m, true, enclosingType, enclosingTypeParams);
                  _4_genMethod = _out0;
                  _3_existing = Dafny.Sequence<RAST._IImplMember>.Concat(_3_existing, Dafny.Sequence<RAST._IImplMember>.FromElements(_4_genMethod));
                  traitBodies = Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.Merge(traitBodies, Dafny.Map<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>.FromElements(new Dafny.Pair<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISequence<RAST._IImplMember>>(_2_p, _3_existing)));
                }
                goto after_match1;
              }
            }
            {
              {
                RAST._IImplMember _5_generated;
                RAST._IImplMember _out1;
                _out1 = (this).GenMethod(_1_m, forTrait, enclosingType, enclosingTypeParams);
                _5_generated = _out1;
                s = Dafny.Sequence<RAST._IImplMember>.Concat(s, Dafny.Sequence<RAST._IImplMember>.FromElements(_5_generated));
              }
            }
          after_match1: ;
          }
        }
      after_match0: ;
      }
    }
    public Dafny.ISequence<RAST._IFormal> GenParams(Dafny.ISequence<DAST._IFormal> @params, Dafny.ISequence<DAST._IFormal> inheritedParams, bool forLambda)
    {
      Dafny.ISequence<RAST._IFormal> s = Dafny.Sequence<RAST._IFormal>.Empty;
      s = Dafny.Sequence<RAST._IFormal>.FromElements();
      BigInteger _hi0 = new BigInteger((@params).Count);
      for (BigInteger _0_i = BigInteger.Zero; _0_i < _hi0; _0_i++) {
        DAST._IFormal _1_param;
        _1_param = (@params).Select(_0_i);
        DAST._IFormal _2_inheritedParam;
        if ((_0_i) < (new BigInteger((inheritedParams).Count))) {
          _2_inheritedParam = (inheritedParams).Select(_0_i);
        } else {
          _2_inheritedParam = _1_param;
        }
        RAST._IType _3_paramType;
        RAST._IType _out0;
        _out0 = (this).GenType((_1_param).dtor_typ, Defs.GenTypeContext.@default());
        _3_paramType = _out0;
        RAST._IType _4_inheritedParamType;
        RAST._IType _out1;
        _out1 = (this).GenType((_2_inheritedParam).dtor_typ, Defs.GenTypeContext.@default());
        _4_inheritedParamType = _out1;
        if (((!((_4_inheritedParamType).CanReadWithoutClone())) || (forLambda)) && (!((_1_param).dtor_attributes).Contains(Defs.__default.AttributeOwned))) {
          _3_paramType = RAST.Type.create_Borrowed(_3_paramType);
        }
        s = Dafny.Sequence<RAST._IFormal>.Concat(s, Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.create(Defs.__default.escapeVar((_1_param).dtor_name), _3_paramType)));
      }
      return s;
    }
    public RAST._IImplMember GenMethod(DAST._IMethod m, bool forTrait, DAST._IType enclosingType, Dafny.ISequence<DAST._IType> enclosingTypeParams)
    {
      RAST._IImplMember s = RAST.ImplMember.Default();
      Dafny.ISequence<RAST._IFormal> _0_params;
      Dafny.ISequence<RAST._IFormal> _out0;
      _out0 = (this).GenParams((m).dtor_params, (m).dtor_inheritedParams, false);
      _0_params = _out0;
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1_paramNames;
      _1_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
      Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> _2_paramTypes;
      _2_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.FromElements();
      BigInteger _hi0 = new BigInteger(((m).dtor_params).Count);
      for (BigInteger _3_paramI = BigInteger.Zero; _3_paramI < _hi0; _3_paramI++) {
        DAST._IFormal _4_dafny__formal;
        _4_dafny__formal = ((m).dtor_params).Select(_3_paramI);
        RAST._IFormal _5_formal;
        _5_formal = (_0_params).Select(_3_paramI);
        Dafny.ISequence<Dafny.Rune> _6_name;
        _6_name = (_5_formal).dtor_name;
        _1_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_1_paramNames, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_6_name));
        _2_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_2_paramTypes, _6_name, (_5_formal).dtor_tpe);
      }
      Dafny.ISequence<Dafny.Rune> _7_fnName;
      _7_fnName = Defs.__default.escapeName((m).dtor_name);
      Defs._ISelfInfo _8_selfIdent;
      _8_selfIdent = Defs.SelfInfo.create_NoSelf();
      if (!((m).dtor_isStatic)) {
        Dafny.ISequence<Dafny.Rune> _9_selfId;
        _9_selfId = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self");
        if ((m).dtor_outVarsAreUninitFieldsToAssign) {
          _9_selfId = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this");
        }
        var _pat_let_tv0 = enclosingTypeParams;
        DAST._IType _10_instanceType;
        DAST._IType _source0 = enclosingType;
        {
          if (_source0.is_UserDefined) {
            DAST._IResolvedType _11_r = _source0.dtor_resolved;
            _10_instanceType = DAST.Type.create_UserDefined(Dafny.Helpers.Let<DAST._IResolvedType, DAST._IResolvedType>(_11_r, _pat_let25_0 => Dafny.Helpers.Let<DAST._IResolvedType, DAST._IResolvedType>(_pat_let25_0, _12_dt__update__tmp_h0 => Dafny.Helpers.Let<Dafny.ISequence<DAST._IType>, DAST._IResolvedType>(_pat_let_tv0, _pat_let26_0 => Dafny.Helpers.Let<Dafny.ISequence<DAST._IType>, DAST._IResolvedType>(_pat_let26_0, _13_dt__update_htypeArgs_h0 => DAST.ResolvedType.create((_12_dt__update__tmp_h0).dtor_path, _13_dt__update_htypeArgs_h0, (_12_dt__update__tmp_h0).dtor_kind, (_12_dt__update__tmp_h0).dtor_attributes, (_12_dt__update__tmp_h0).dtor_properMethods, (_12_dt__update__tmp_h0).dtor_extendedTypes))))));
            goto after_match0;
          }
        }
        {
          _10_instanceType = enclosingType;
        }
      after_match0: ;
        if (forTrait) {
          RAST._IFormal _14_selfFormal;
          _14_selfFormal = RAST.Formal.selfBorrowed;
          _0_params = Dafny.Sequence<RAST._IFormal>.Concat(Dafny.Sequence<RAST._IFormal>.FromElements(_14_selfFormal), _0_params);
        } else {
          RAST._IType _15_tpe;
          RAST._IType _out1;
          _out1 = (this).GenType(_10_instanceType, Defs.GenTypeContext.@default());
          _15_tpe = _out1;
          if ((_9_selfId).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))) {
            if (((this).pointerType).is_RcMut) {
              _15_tpe = RAST.Type.create_Borrowed(_15_tpe);
            }
          } else if ((_9_selfId).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))) {
            if ((_15_tpe).IsObjectOrPointer()) {
              _15_tpe = RAST.__default.SelfBorrowed;
            } else {
              if (((((enclosingType).is_UserDefined) && ((((enclosingType).dtor_resolved).dtor_kind).is_Newtype)) && (Defs.__default.IsNewtypeCopy((((enclosingType).dtor_resolved).dtor_kind).dtor_range))) && (!(forTrait))) {
                _15_tpe = RAST.Type.create_TMetaData(RAST.__default.SelfOwned, true, ((((enclosingType).dtor_resolved).dtor_kind).dtor_range).CanOverflow());
              } else {
                _15_tpe = RAST.__default.SelfBorrowed;
              }
            }
          }
          RAST._IFormal _16_formal;
          _16_formal = RAST.Formal.create(_9_selfId, _15_tpe);
          _0_params = Dafny.Sequence<RAST._IFormal>.Concat(Dafny.Sequence<RAST._IFormal>.FromElements(_16_formal), _0_params);
          Dafny.ISequence<Dafny.Rune> _17_name;
          _17_name = (_16_formal).dtor_name;
          _1_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_17_name), _1_paramNames);
          _2_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_2_paramTypes, _17_name, (_16_formal).dtor_tpe);
        }
        _8_selfIdent = Defs.SelfInfo.create_ThisTyped(_9_selfId, _10_instanceType);
      }
      Dafny.ISequence<RAST._IType> _18_retTypeArgs;
      _18_retTypeArgs = Dafny.Sequence<RAST._IType>.FromElements();
      BigInteger _19_typeI;
      _19_typeI = BigInteger.Zero;
      while ((_19_typeI) < (new BigInteger(((m).dtor_outTypes).Count))) {
        RAST._IType _20_typeExpr;
        RAST._IType _out2;
        _out2 = (this).GenType(((m).dtor_outTypes).Select(_19_typeI), Defs.GenTypeContext.@default());
        _20_typeExpr = _out2;
        _18_retTypeArgs = Dafny.Sequence<RAST._IType>.Concat(_18_retTypeArgs, Dafny.Sequence<RAST._IType>.FromElements(_20_typeExpr));
        _19_typeI = (_19_typeI) + (BigInteger.One);
      }
      RAST._IVisibility _21_visibility;
      if (forTrait) {
        _21_visibility = RAST.Visibility.create_PRIV();
      } else {
        _21_visibility = RAST.Visibility.create_PUB();
      }
      Dafny.ISequence<DAST._ITypeArgDecl> _22_typeParamsFiltered;
      _22_typeParamsFiltered = Dafny.Sequence<DAST._ITypeArgDecl>.FromElements();
      BigInteger _hi1 = new BigInteger(((m).dtor_typeParams).Count);
      for (BigInteger _23_typeParamI = BigInteger.Zero; _23_typeParamI < _hi1; _23_typeParamI++) {
        DAST._ITypeArgDecl _24_typeParam;
        _24_typeParam = ((m).dtor_typeParams).Select(_23_typeParamI);
        if (!((enclosingTypeParams).Contains(DAST.Type.create_TypeArg((_24_typeParam).dtor_name)))) {
          _22_typeParamsFiltered = Dafny.Sequence<DAST._ITypeArgDecl>.Concat(_22_typeParamsFiltered, Dafny.Sequence<DAST._ITypeArgDecl>.FromElements(_24_typeParam));
        }
      }
      Dafny.ISequence<RAST._ITypeParamDecl> _25_typeParams;
      _25_typeParams = Dafny.Sequence<RAST._ITypeParamDecl>.FromElements();
      if ((new BigInteger((_22_typeParamsFiltered).Count)).Sign == 1) {
        BigInteger _hi2 = new BigInteger((_22_typeParamsFiltered).Count);
        for (BigInteger _26_i = BigInteger.Zero; _26_i < _hi2; _26_i++) {
          DAST._IType _27_typeArg;
          RAST._ITypeParamDecl _28_rTypeParamDecl;
          DAST._IType _out3;
          RAST._ITypeParamDecl _out4;
          (this).GenTypeParam((_22_typeParamsFiltered).Select(_26_i), out _out3, out _out4);
          _27_typeArg = _out3;
          _28_rTypeParamDecl = _out4;
          RAST._ITypeParamDecl _29_dt__update__tmp_h1 = _28_rTypeParamDecl;
          Dafny.ISequence<RAST._IType> _30_dt__update_hconstraints_h0 = (_28_rTypeParamDecl).dtor_constraints;
          _28_rTypeParamDecl = RAST.TypeParamDecl.create((_29_dt__update__tmp_h1).dtor_name, _30_dt__update_hconstraints_h0);
          _25_typeParams = Dafny.Sequence<RAST._ITypeParamDecl>.Concat(_25_typeParams, Dafny.Sequence<RAST._ITypeParamDecl>.FromElements(_28_rTypeParamDecl));
        }
      }
      Std.Wrappers._IOption<RAST._IExpr> _31_fBody = Std.Wrappers.Option<RAST._IExpr>.Default();
      Defs._IEnvironment _32_env = Defs.Environment.Default();
      RAST._IExpr _33_preBody;
      _33_preBody = (this).InitEmptyExpr();
      Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _34_preAssignNames;
      _34_preAssignNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
      Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> _35_preAssignTypes;
      _35_preAssignTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.FromElements();
      if ((m).dtor_hasBody) {
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> _36_earlyReturn;
        _36_earlyReturn = Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_None();
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> _source1 = (m).dtor_outVars;
        {
          if (_source1.is_Some) {
            Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _37_outVars = _source1.dtor_value;
            {
              if ((m).dtor_outVarsAreUninitFieldsToAssign) {
                _36_earlyReturn = Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_Some(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements());
                BigInteger _hi3 = new BigInteger((_37_outVars).Count);
                for (BigInteger _38_outI = BigInteger.Zero; _38_outI < _hi3; _38_outI++) {
                  Dafny.ISequence<Dafny.Rune> _39_outVar;
                  _39_outVar = (_37_outVars).Select(_38_outI);
                  Dafny.ISequence<Dafny.Rune> _40_outName;
                  _40_outName = Defs.__default.escapeVar(_39_outVar);
                  Dafny.ISequence<Dafny.Rune> _41_tracker__name;
                  _41_tracker__name = Defs.__default.AddAssignedPrefix(_40_outName);
                  _34_preAssignNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_34_preAssignNames, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_41_tracker__name));
                  _35_preAssignTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_35_preAssignTypes, _41_tracker__name, RAST.Type.create_Bool());
                  _33_preBody = (_33_preBody).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _41_tracker__name, Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_Bool()), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_LiteralBool(false))));
                }
              } else {
                Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _42_tupleArgs;
                _42_tupleArgs = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
                bool _43_isTailRecursive;
                _43_isTailRecursive = ((new BigInteger(((m).dtor_body).Count)) == (BigInteger.One)) && ((((m).dtor_body).Select(BigInteger.Zero)).is_TailRecursive);
                BigInteger _hi4 = new BigInteger((_37_outVars).Count);
                for (BigInteger _44_outI = BigInteger.Zero; _44_outI < _hi4; _44_outI++) {
                  Dafny.ISequence<Dafny.Rune> _45_outVar;
                  _45_outVar = (_37_outVars).Select(_44_outI);
                  DAST._IType _46_outTyp;
                  _46_outTyp = ((m).dtor_outTypes).Select(_44_outI);
                  RAST._IType _47_outType;
                  RAST._IType _out5;
                  _out5 = (this).GenType(_46_outTyp, Defs.GenTypeContext.@default());
                  _47_outType = _out5;
                  Dafny.ISequence<Dafny.Rune> _48_outName;
                  _48_outName = Defs.__default.escapeVar(_45_outVar);
                  if (!(_43_isTailRecursive)) {
                    _1_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_1_paramNames, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_48_outName));
                    RAST._IType _49_outMaybeType;
                    if ((_47_outType).CanReadWithoutClone()) {
                      _49_outMaybeType = _47_outType;
                    } else {
                      _49_outMaybeType = RAST.__default.MaybePlaceboType(_47_outType);
                    }
                    _2_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_2_paramTypes, _48_outName, _49_outMaybeType);
                  }
                  _42_tupleArgs = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_42_tupleArgs, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_48_outName));
                }
                _36_earlyReturn = Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_Some(_42_tupleArgs);
              }
            }
            goto after_match1;
          }
        }
        {
        }
      after_match1: ;
        _32_env = Defs.Environment.create(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_34_preAssignNames, _1_paramNames), Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Merge(_35_preAssignTypes, _2_paramTypes), Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements());
        RAST._IExpr _50_body;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _51___v50;
        Defs._IEnvironment _52___v51;
        RAST._IExpr _out6;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out7;
        Defs._IEnvironment _out8;
        (this).GenStmts((m).dtor_body, _8_selfIdent, _32_env, true, _36_earlyReturn, out _out6, out _out7, out _out8);
        _50_body = _out6;
        _51___v50 = _out7;
        _52___v51 = _out8;
        _31_fBody = Std.Wrappers.Option<RAST._IExpr>.create_Some((_33_preBody).Then(_50_body));
      } else {
        _32_env = Defs.Environment.create(_1_paramNames, _2_paramTypes, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements());
        _31_fBody = Std.Wrappers.Option<RAST._IExpr>.create_None();
      }
      s = RAST.ImplMember.create_FnDecl((m).dtor_docString, RAST.__default.NoAttr, _21_visibility, RAST.Fn.create(_7_fnName, _25_typeParams, _0_params, Std.Wrappers.Option<RAST._IType>.create_Some((((new BigInteger((_18_retTypeArgs).Count)) == (BigInteger.One)) ? ((_18_retTypeArgs).Select(BigInteger.Zero)) : (RAST.Type.create_TupleType(_18_retTypeArgs)))), _31_fBody));
      return s;
    }
    public void GenStmts(Dafny.ISequence<DAST._IStatement> stmts, Defs._ISelfInfo selfIdent, Defs._IEnvironment env, bool isLast, Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> earlyReturn, out RAST._IExpr generated, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents, out Defs._IEnvironment newEnv)
    {
      generated = RAST.Expr.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      newEnv = Defs.Environment.Default();
      generated = (this).InitEmptyExpr();
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _0_declarations;
      _0_declarations = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      BigInteger _1_i;
      _1_i = BigInteger.Zero;
      newEnv = env;
      while ((_1_i) < (new BigInteger((stmts).Count))) {
        DAST._IStatement _2_stmt;
        _2_stmt = (stmts).Select(_1_i);
        if (((((((_2_stmt).is_DeclareVar) && (((_2_stmt).dtor_maybeValue).is_None)) && (((_1_i) + (BigInteger.One)) < (new BigInteger((stmts).Count)))) && (((stmts).Select((_1_i) + (BigInteger.One))).is_Assign)) && ((((stmts).Select((_1_i) + (BigInteger.One))).dtor_lhs).is_Ident)) && (object.Equals((((stmts).Select((_1_i) + (BigInteger.One))).dtor_lhs).dtor_ident, (_2_stmt).dtor_name))) {
          Dafny.ISequence<Dafny.Rune> _3_name;
          _3_name = (_2_stmt).dtor_name;
          DAST._IType _4_typ;
          _4_typ = (_2_stmt).dtor_typ;
          RAST._IExpr _5_stmtExpr;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _6_recIdents;
          Defs._IEnvironment _7_newEnv2;
          RAST._IExpr _out0;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out1;
          Defs._IEnvironment _out2;
          (this).GenDeclareVarAssign(_3_name, _4_typ, ((stmts).Select((_1_i) + (BigInteger.One))).dtor_value, selfIdent, newEnv, out _out0, out _out1, out _out2);
          _5_stmtExpr = _out0;
          _6_recIdents = _out1;
          _7_newEnv2 = _out2;
          newEnv = _7_newEnv2;
          readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_6_recIdents, _0_declarations));
          _0_declarations = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_0_declarations, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(Defs.__default.escapeVar(_3_name)));
          generated = (generated).Then(_5_stmtExpr);
          _1_i = (_1_i) + (new BigInteger(2));
        } else {
          DAST._IStatement _source0 = _2_stmt;
          {
            if (_source0.is_DeclareVar) {
              Dafny.ISequence<Dafny.Rune> _8_name = _source0.dtor_name;
              DAST._IType _9_optType = _source0.dtor_typ;
              Std.Wrappers._IOption<DAST._IExpression> maybeValue0 = _source0.dtor_maybeValue;
              if (maybeValue0.is_None) {
                Defs._IAssignmentStatus _10_laterAssignmentStatus;
                _10_laterAssignmentStatus = Defs.__default.DetectAssignmentStatus((stmts).Drop((_1_i) + (BigInteger.One)), _8_name);
                newEnv = (newEnv).AddAssignmentStatus(Defs.__default.escapeVar(_8_name), _10_laterAssignmentStatus);
                goto after_match0;
              }
            }
          }
          {
            if (_source0.is_DeclareVar) {
              Dafny.ISequence<Dafny.Rune> _11_name = _source0.dtor_name;
              DAST._IType _12_optType = _source0.dtor_typ;
              Std.Wrappers._IOption<DAST._IExpression> maybeValue1 = _source0.dtor_maybeValue;
              if (maybeValue1.is_Some) {
                DAST._IExpression value0 = maybeValue1.dtor_value;
                if (value0.is_InitializationValue) {
                  DAST._IType _13_typ = value0.dtor_typ;
                  RAST._IType _14_tpe;
                  RAST._IType _out3;
                  _out3 = (this).GenType(_13_typ, Defs.GenTypeContext.@default());
                  _14_tpe = _out3;
                  Dafny.ISequence<Dafny.Rune> _15_varName;
                  _15_varName = Defs.__default.escapeVar(_11_name);
                  Defs._IAssignmentStatus _16_laterAssignmentStatus;
                  _16_laterAssignmentStatus = Defs.__default.DetectAssignmentStatus((stmts).Drop((_1_i) + (BigInteger.One)), _11_name);
                  newEnv = (newEnv).AddAssignmentStatus(_15_varName, _16_laterAssignmentStatus);
                  goto after_match0;
                }
              }
            }
          }
          {
          }
        after_match0: ;
          RAST._IExpr _17_stmtExpr;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _18_recIdents;
          Defs._IEnvironment _19_newEnv2;
          RAST._IExpr _out4;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out5;
          Defs._IEnvironment _out6;
          (this).GenStmt(_2_stmt, selfIdent, newEnv, (isLast) && ((_1_i) == ((new BigInteger((stmts).Count)) - (BigInteger.One))), earlyReturn, out _out4, out _out5, out _out6);
          _17_stmtExpr = _out4;
          _18_recIdents = _out5;
          _19_newEnv2 = _out6;
          newEnv = _19_newEnv2;
          DAST._IStatement _source1 = _2_stmt;
          {
            if (_source1.is_DeclareVar) {
              Dafny.ISequence<Dafny.Rune> _20_name = _source1.dtor_name;
              {
                _0_declarations = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_0_declarations, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(Defs.__default.escapeVar(_20_name)));
              }
              goto after_match1;
            }
          }
          {
          }
        after_match1: ;
          readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_18_recIdents, _0_declarations));
          generated = (generated).Then(_17_stmtExpr);
          if ((_17_stmtExpr).is_Return) {
            goto after_0;
          }
          _1_i = (_1_i) + (BigInteger.One);
        }
      continue_0: ;
      }
    after_0: ;
    }
    public void GenDeclareVarAssign(Dafny.ISequence<Dafny.Rune> name, DAST._IType typ, DAST._IExpression rhs, Defs._ISelfInfo selfIdent, Defs._IEnvironment env, out RAST._IExpr generated, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents, out Defs._IEnvironment newEnv)
    {
      generated = RAST.Expr.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      newEnv = Defs.Environment.Default();
      RAST._IType _0_tpe;
      RAST._IType _out0;
      _out0 = (this).GenType(typ, Defs.GenTypeContext.@default());
      _0_tpe = _out0;
      Dafny.ISequence<Dafny.Rune> _1_varName;
      _1_varName = Defs.__default.escapeVar(name);
      RAST._IExpr _2_exprRhs = RAST.Expr.Default();
      if (((rhs).is_InitializationValue) && ((_0_tpe).IsObjectOrPointer())) {
        readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
        if ((rhs).is_NewUninitArray) {
          _0_tpe = (_0_tpe).TypeAtInitialization();
        } else {
          _0_tpe = _0_tpe;
        }
        _2_exprRhs = (_0_tpe).ToNullExpr();
      } else {
        RAST._IExpr _3_expr = RAST.Expr.Default();
        Defs._IOwnership _4_exprOwnership = Defs.Ownership.Default();
        RAST._IExpr _out1;
        Defs._IOwnership _out2;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out3;
        (this).GenExpr(rhs, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out1, out _out2, out _out3);
        _3_expr = _out1;
        _4_exprOwnership = _out2;
        readIdents = _out3;
        if ((rhs).is_NewUninitArray) {
          _0_tpe = (_0_tpe).TypeAtInitialization();
        } else {
          _0_tpe = _0_tpe;
        }
        _2_exprRhs = _3_expr;
      }
      generated = RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _1_varName, Std.Wrappers.Option<RAST._IType>.create_Some(_0_tpe), Std.Wrappers.Option<RAST._IExpr>.create_Some(_2_exprRhs));
      newEnv = (env).AddAssigned(_1_varName, _0_tpe);
    }
    public void GenAssignLhs(DAST._IAssignLhs lhs, RAST._IExpr rhs, Defs._ISelfInfo selfIdent, Defs._IEnvironment env, out RAST._IExpr generated, out bool needsIIFE, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents, out Defs._IEnvironment newEnv)
    {
      generated = RAST.Expr.Default();
      needsIIFE = false;
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      newEnv = Defs.Environment.Default();
      newEnv = env;
      DAST._IAssignLhs _source0 = lhs;
      {
        if (_source0.is_Ident) {
          Dafny.ISequence<Dafny.Rune> _0_id = _source0.dtor_ident;
          {
            Dafny.ISequence<Dafny.Rune> _1_idRust;
            _1_idRust = Defs.__default.escapeVar(_0_id);
            if (((env).IsBorrowed(_1_idRust)) || ((env).IsBorrowedMut(_1_idRust))) {
              generated = RAST.__default.AssignVar(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"), _1_idRust), rhs);
            } else {
              generated = RAST.__default.AssignVar(_1_idRust, rhs);
            }
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_1_idRust);
            needsIIFE = false;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Select) {
          DAST._IExpression _2_on = _source0.dtor_expr;
          Dafny.ISequence<Dafny.Rune> _3_field = _source0.dtor_field;
          bool _4_isConstant = _source0.dtor_isConstant;
          {
            Dafny.ISequence<Dafny.Rune> _5_fieldName;
            _5_fieldName = Defs.__default.escapeVar(_3_field);
            RAST._IExpr _6_onExpr;
            Defs._IOwnership _7_onOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _8_recIdents;
            RAST._IExpr _out0;
            Defs._IOwnership _out1;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out2;
            (this).GenExpr(_2_on, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out0, out _out1, out _out2);
            _6_onExpr = _out0;
            _7_onOwned = _out1;
            _8_recIdents = _out2;
            RAST._IExpr _source1 = _6_onExpr;
            {
              bool disjunctiveMatch0 = false;
              if (_source1.is_Call) {
                RAST._IExpr obj0 = _source1.dtor_obj;
                if (obj0.is_Select) {
                  RAST._IExpr obj1 = obj0.dtor_obj;
                  if (obj1.is_Identifier) {
                    Dafny.ISequence<Dafny.Rune> name0 = obj1.dtor_name;
                    if (object.Equals(name0, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))) {
                      Dafny.ISequence<Dafny.Rune> name1 = obj0.dtor_name;
                      if (object.Equals(name1, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("clone"))) {
                        disjunctiveMatch0 = true;
                      }
                    }
                  }
                }
              }
              if (_source1.is_Identifier) {
                Dafny.ISequence<Dafny.Rune> name2 = _source1.dtor_name;
                if (object.Equals(name2, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))) {
                  disjunctiveMatch0 = true;
                }
              }
              if (_source1.is_UnaryOp) {
                Dafny.ISequence<Dafny.Rune> op10 = _source1.dtor_op1;
                if (object.Equals(op10, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"))) {
                  RAST._IExpr underlying0 = _source1.dtor_underlying;
                  if (underlying0.is_Identifier) {
                    Dafny.ISequence<Dafny.Rune> name3 = underlying0.dtor_name;
                    if (object.Equals(name3, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))) {
                      disjunctiveMatch0 = true;
                    }
                  }
                }
              }
              if (disjunctiveMatch0) {
                Dafny.ISequence<Dafny.Rune> _9_isAssignedVar;
                _9_isAssignedVar = Defs.__default.AddAssignedPrefix(_5_fieldName);
                if (((newEnv).dtor_names).Contains(_9_isAssignedVar)) {
                  Dafny.ISequence<Dafny.Rune> _10_update__field__uninit;
                  if (_4_isConstant) {
                    _10_update__field__uninit = (this).update__field__uninit__macro;
                  } else {
                    _10_update__field__uninit = (this).update__field__mut__uninit__macro;
                  }
                  generated = (((RAST.__default.dafny__runtime).MSel(_10_update__field__uninit)).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements((this).thisInConstructor, RAST.Expr.create_Identifier(_5_fieldName), RAST.Expr.create_Identifier(_9_isAssignedVar), rhs));
                  newEnv = (newEnv).RemoveAssigned(_9_isAssignedVar);
                } else {
                  generated = ((this).modify__mutable__field__macro).Apply(Dafny.Sequence<RAST._IExpr>.FromElements((((this).read__macro).Apply1((this).thisInConstructor)).Sel(_5_fieldName), rhs));
                }
                goto after_match1;
              }
            }
            {
              if (!object.Equals(_6_onExpr, RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self")))) {
                _6_onExpr = ((this).read__macro).Apply1(_6_onExpr);
              }
              generated = ((this).modify__mutable__field__macro).Apply(Dafny.Sequence<RAST._IExpr>.FromElements((_6_onExpr).Sel(_5_fieldName), rhs));
            }
          after_match1: ;
            readIdents = _8_recIdents;
            needsIIFE = false;
          }
          goto after_match0;
        }
      }
      {
        DAST._IExpression _11_on = _source0.dtor_expr;
        Dafny.ISequence<DAST._IExpression> _12_indices = _source0.dtor_indices;
        {
          RAST._IExpr _13_onExpr;
          Defs._IOwnership _14_onOwned;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _15_recIdents;
          RAST._IExpr _out3;
          Defs._IOwnership _out4;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out5;
          (this).GenExpr(_11_on, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out3, out _out4, out _out5);
          _13_onExpr = _out3;
          _14_onOwned = _out4;
          _15_recIdents = _out5;
          readIdents = _15_recIdents;
          _13_onExpr = ((this).modify__macro).Apply1(_13_onExpr);
          RAST._IExpr _16_r;
          _16_r = (this).InitEmptyExpr();
          Dafny.ISequence<RAST._IExpr> _17_indicesExpr;
          _17_indicesExpr = Dafny.Sequence<RAST._IExpr>.FromElements();
          BigInteger _hi0 = new BigInteger((_12_indices).Count);
          for (BigInteger _18_i = BigInteger.Zero; _18_i < _hi0; _18_i++) {
            RAST._IExpr _19_idx;
            Defs._IOwnership _20___v59;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _21_recIdentsIdx;
            RAST._IExpr _out6;
            Defs._IOwnership _out7;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out8;
            (this).GenExpr((_12_indices).Select(_18_i), selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out6, out _out7, out _out8);
            _19_idx = _out6;
            _20___v59 = _out7;
            _21_recIdentsIdx = _out8;
            Dafny.ISequence<Dafny.Rune> _22_varName;
            _22_varName = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("__idx"), Std.Strings.__default.OfNat(_18_i));
            _17_indicesExpr = Dafny.Sequence<RAST._IExpr>.Concat(_17_indicesExpr, Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(_22_varName)));
            _16_r = (_16_r).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), _22_varName, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.__default.IntoUsize(_19_idx))));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _21_recIdentsIdx);
          }
          if ((new BigInteger((_12_indices).Count)) > (BigInteger.One)) {
            _13_onExpr = (_13_onExpr).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("data"));
          }
          generated = (_16_r).Then(RAST.Expr.create_Assign(Std.Wrappers.Option<RAST._IAssignLhs>.create_Some(RAST.AssignLhs.create_Index(_13_onExpr, _17_indicesExpr)), rhs));
          needsIIFE = true;
        }
      }
    after_match0: ;
    }
    public RAST._IExpr FromGeneralBorrowToSelfBorrow(RAST._IExpr onExpr, Defs._IOwnership onExprOwnership, Defs._IEnvironment env)
    {
      if (((onExpr).is_Identifier) && ((env).NeedsAsRefForBorrow((onExpr).dtor_name))) {
        return ((onExpr).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply0();
      } else if (((onExprOwnership).is_OwnershipBorrowed) && (!object.Equals(onExpr, RAST.__default.self))) {
        return (((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("convert"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("AsRef"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply1(onExpr);
      } else {
        return onExpr;
      }
    }
    public void GenOwnedCallPart(DAST._IExpression @on, Defs._ISelfInfo selfIdent, DAST._ICallName name, Dafny.ISequence<DAST._IType> typeArgs, Dafny.ISequence<DAST._IExpression> args, Defs._IEnvironment env, out RAST._IExpr r, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      Dafny.ISequence<RAST._IExpr> _0_argExprs;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _1_recIdents;
      Dafny.ISequence<RAST._IType> _2_typeExprs;
      Std.Wrappers._IOption<DAST._IResolvedType> _3_fullNameQualifier;
      Dafny.ISequence<RAST._IExpr> _out0;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out1;
      Dafny.ISequence<RAST._IType> _out2;
      Std.Wrappers._IOption<DAST._IResolvedType> _out3;
      (this).GenArgs(selfIdent, name, typeArgs, args, env, out _out0, out _out1, out _out2, out _out3);
      _0_argExprs = _out0;
      _1_recIdents = _out1;
      _2_typeExprs = _out2;
      _3_fullNameQualifier = _out3;
      readIdents = _1_recIdents;
      if ((((((selfIdent).IsSelf()) && ((name).is_CallName)) && (((name).dtor_receiverArg).is_Some)) && ((new BigInteger((_0_argExprs).Count)).Sign == 1)) && (RAST.__default.IsBorrowUpcastBox((_0_argExprs).Select(BigInteger.Zero)))) {
        _0_argExprs = Dafny.Sequence<RAST._IExpr>.Concat(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))), (_0_argExprs).Drop(BigInteger.One));
      }
      Std.Wrappers._IOption<DAST._IResolvedType> _source0 = _3_fullNameQualifier;
      {
        if (_source0.is_Some) {
          DAST._IResolvedType value0 = _source0.dtor_value;
          Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _4_path = value0.dtor_path;
          Dafny.ISequence<DAST._IType> _5_onTypeArgs = value0.dtor_typeArgs;
          DAST._IResolvedTypeBase _6_base = value0.dtor_kind;
          RAST._IExpr _7_fullPath;
          RAST._IExpr _out4;
          _out4 = (this).GenPathExpr(_4_path, true);
          _7_fullPath = _out4;
          Dafny.ISequence<RAST._IType> _8_onTypeExprs;
          Dafny.ISequence<RAST._IType> _out5;
          _out5 = (this).GenTypeArgs(_5_onTypeArgs, Defs.GenTypeContext.@default());
          _8_onTypeExprs = _out5;
          RAST._IExpr _9_onExpr = RAST.Expr.Default();
          Defs._IOwnership _10_recOwnership = Defs.Ownership.Default();
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _11_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
          if ((((_6_base).is_Trait) && (((_6_base).dtor_traitType).is_ObjectTrait)) || ((_6_base).is_Class)) {
            RAST._IExpr _out6;
            Defs._IOwnership _out7;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out8;
            (this).GenExpr(@on, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out6, out _out7, out _out8);
            _9_onExpr = _out6;
            _10_recOwnership = _out7;
            _11_recIdents = _out8;
            _9_onExpr = ((this).read__macro).Apply1(_9_onExpr);
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _11_recIdents);
          } else if (((_6_base).is_Trait) && (((_6_base).dtor_traitType).is_GeneralTrait)) {
            if ((@on).IsThisUpcast()) {
              _9_onExpr = RAST.__default.self;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self")));
            } else {
              RAST._IExpr _out9;
              Defs._IOwnership _out10;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out11;
              (this).GenExpr(@on, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out9, out _out10, out _out11);
              _9_onExpr = _out9;
              _10_recOwnership = _out10;
              _11_recIdents = _out11;
              _9_onExpr = (this).FromGeneralBorrowToSelfBorrow(_9_onExpr, _10_recOwnership, env);
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _11_recIdents);
            }
          } else if (((_6_base).is_Newtype) && (Defs.__default.IsNewtypeCopy((_6_base).dtor_range))) {
            RAST._IExpr _out12;
            Defs._IOwnership _out13;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out14;
            (this).GenExpr(@on, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out12, out _out13, out _out14);
            _9_onExpr = _out12;
            _10_recOwnership = _out13;
            _11_recIdents = _out14;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _11_recIdents);
          } else {
            RAST._IExpr _out15;
            Defs._IOwnership _out16;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out17;
            (this).GenExpr(@on, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out15, out _out16, out _out17);
            _9_onExpr = _out15;
            _10_recOwnership = _out16;
            _11_recIdents = _out17;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _11_recIdents);
          }
          r = ((((_7_fullPath).ApplyType(_8_onTypeExprs)).FSel(Defs.__default.escapeName((name).dtor_name))).ApplyType(_2_typeExprs)).Apply(Dafny.Sequence<RAST._IExpr>.Concat(Dafny.Sequence<RAST._IExpr>.FromElements(_9_onExpr), _0_argExprs));
          goto after_match0;
        }
      }
      {
        RAST._IExpr _12_onExpr = RAST.Expr.Default();
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _13_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
        Defs._IOwnership _14_dummy = Defs.Ownership.Default();
        if ((@on).IsThisUpcast()) {
          _12_onExpr = RAST.__default.self;
          _13_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"));
        } else {
          RAST._IExpr _out18;
          Defs._IOwnership _out19;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out20;
          (this).GenExpr(@on, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out18, out _out19, out _out20);
          _12_onExpr = _out18;
          _14_dummy = _out19;
          _13_recIdents = _out20;
        }
        readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _13_recIdents);
        Dafny.ISequence<Dafny.Rune> _15_renderedName;
        _15_renderedName = (this).GetMethodName(@on, name);
        DAST._IExpression _source1 = @on;
        {
          bool disjunctiveMatch0 = false;
          if (_source1.is_Companion) {
            disjunctiveMatch0 = true;
          }
          if (_source1.is_ExternCompanion) {
            disjunctiveMatch0 = true;
          }
          if (disjunctiveMatch0) {
            {
              _12_onExpr = (_12_onExpr).FSel(_15_renderedName);
            }
            goto after_match1;
          }
        }
        {
          {
            if (!object.Equals(_12_onExpr, RAST.__default.self)) {
              DAST._ICallName _source2 = name;
              {
                if (_source2.is_CallName) {
                  Std.Wrappers._IOption<DAST._IType> onType0 = _source2.dtor_onType;
                  if (onType0.is_Some) {
                    DAST._IType _16_tpe = onType0.dtor_value;
                    RAST._IType _17_typ;
                    RAST._IType _out21;
                    _out21 = (this).GenType(_16_tpe, Defs.GenTypeContext.@default());
                    _17_typ = _out21;
                    if (((_17_typ).IsObjectOrPointer()) && (!object.Equals(_12_onExpr, RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))))) {
                      _12_onExpr = ((this).read__macro).Apply1(_12_onExpr);
                    }
                    goto after_match2;
                  }
                }
              }
              {
              }
            after_match2: ;
            }
            _12_onExpr = (_12_onExpr).Sel(_15_renderedName);
          }
        }
      after_match1: ;
        r = ((_12_onExpr).ApplyType(_2_typeExprs)).Apply(_0_argExprs);
      }
    after_match0: ;
    }
    public void GenStmt(DAST._IStatement stmt, Defs._ISelfInfo selfIdent, Defs._IEnvironment env, bool isLast, Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> earlyReturn, out RAST._IExpr generated, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents, out Defs._IEnvironment newEnv)
    {
      generated = RAST.Expr.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      newEnv = Defs.Environment.Default();
      DAST._IStatement _source0 = stmt;
      {
        if (_source0.is_ConstructorNewSeparator) {
          Dafny.ISequence<DAST._IField> _0_fields = _source0.dtor_fields;
          {
            generated = (this).InitEmptyExpr();
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            newEnv = env;
            BigInteger _hi0 = new BigInteger((_0_fields).Count);
            for (BigInteger _1_i = BigInteger.Zero; _1_i < _hi0; _1_i++) {
              DAST._IField _2_field;
              _2_field = (_0_fields).Select(_1_i);
              Dafny.ISequence<Dafny.Rune> _3_fieldName;
              _3_fieldName = Defs.__default.escapeVar(((_2_field).dtor_formal).dtor_name);
              RAST._IType _4_fieldTyp;
              RAST._IType _out0;
              _out0 = (this).GenType(((_2_field).dtor_formal).dtor_typ, Defs.GenTypeContext.@default());
              _4_fieldTyp = _out0;
              Dafny.ISequence<Dafny.Rune> _5_isAssignedVar;
              _5_isAssignedVar = Defs.__default.AddAssignedPrefix(_3_fieldName);
              if (((newEnv).dtor_names).Contains(_5_isAssignedVar)) {
                RAST._IExpr _6_rhs;
                Defs._IOwnership _7___v73;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _8___v74;
                RAST._IExpr _out1;
                Defs._IOwnership _out2;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out3;
                (this).GenExpr(DAST.Expression.create_InitializationValue(((_2_field).dtor_formal).dtor_typ), selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out1, out _out2, out _out3);
                _6_rhs = _out1;
                _7___v73 = _out2;
                _8___v74 = _out3;
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_5_isAssignedVar));
                Dafny.ISequence<Dafny.Rune> _9_update__if__uninit;
                if ((_2_field).dtor_isConstant) {
                  _9_update__if__uninit = (this).update__field__if__uninit__macro;
                } else {
                  _9_update__if__uninit = (this).update__field__mut__if__uninit__macro;
                }
                generated = (generated).Then((((RAST.__default.dafny__runtime).MSel(_9_update__if__uninit)).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this")), RAST.Expr.create_Identifier(_3_fieldName), RAST.Expr.create_Identifier(_5_isAssignedVar), _6_rhs)));
                newEnv = (newEnv).RemoveAssigned(_5_isAssignedVar);
              }
            }
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_DeclareVar) {
          Dafny.ISequence<Dafny.Rune> _10_name = _source0.dtor_name;
          DAST._IType _11_typ = _source0.dtor_typ;
          Std.Wrappers._IOption<DAST._IExpression> maybeValue0 = _source0.dtor_maybeValue;
          if (maybeValue0.is_Some) {
            DAST._IExpression _12_expression = maybeValue0.dtor_value;
            {
              RAST._IType _13_tpe;
              RAST._IType _out4;
              _out4 = (this).GenType(_11_typ, Defs.GenTypeContext.@default());
              _13_tpe = _out4;
              Dafny.ISequence<Dafny.Rune> _14_varName;
              _14_varName = Defs.__default.escapeVar(_10_name);
              bool _15_hasCopySemantics;
              _15_hasCopySemantics = (_13_tpe).CanReadWithoutClone();
              if (((_12_expression).is_InitializationValue) && (!(_15_hasCopySemantics))) {
                if ((env).IsAssignmentStatusKnown(_14_varName)) {
                  generated = RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _14_varName, Std.Wrappers.Option<RAST._IType>.create_Some(_13_tpe), Std.Wrappers.Option<RAST._IExpr>.create_None());
                } else {
                  generated = RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _14_varName, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(((((RAST.__default.MaybePlaceboPath).AsExpr()).ApplyType1(_13_tpe)).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply0()));
                  _13_tpe = RAST.__default.MaybePlaceboType(_13_tpe);
                }
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
                newEnv = (env).AddAssigned(_14_varName, _13_tpe);
              } else {
                RAST._IExpr _out5;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out6;
                Defs._IEnvironment _out7;
                (this).GenDeclareVarAssign(_10_name, _11_typ, _12_expression, selfIdent, env, out _out5, out _out6, out _out7);
                generated = _out5;
                readIdents = _out6;
                newEnv = _out7;
                return ;
              }
            }
            goto after_match0;
          }
        }
      }
      {
        if (_source0.is_DeclareVar) {
          Dafny.ISequence<Dafny.Rune> _16_name = _source0.dtor_name;
          DAST._IType _17_typ = _source0.dtor_typ;
          Std.Wrappers._IOption<DAST._IExpression> maybeValue1 = _source0.dtor_maybeValue;
          if (maybeValue1.is_None) {
            {
              Dafny.ISequence<Dafny.Rune> _18_varName;
              _18_varName = Defs.__default.escapeVar(_16_name);
              if ((env).IsAssignmentStatusKnown(_18_varName)) {
                RAST._IType _19_tpe;
                RAST._IType _out8;
                _out8 = (this).GenType(_17_typ, Defs.GenTypeContext.@default());
                _19_tpe = _out8;
                generated = RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _18_varName, Std.Wrappers.Option<RAST._IType>.create_Some(_19_tpe), Std.Wrappers.Option<RAST._IExpr>.create_None());
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
                newEnv = (env).AddAssigned(_18_varName, _19_tpe);
              } else {
                DAST._IStatement _20_newStmt;
                _20_newStmt = DAST.Statement.create_DeclareVar(_16_name, _17_typ, Std.Wrappers.Option<DAST._IExpression>.create_Some(DAST.Expression.create_InitializationValue(_17_typ)));
                RAST._IExpr _out9;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out10;
                Defs._IEnvironment _out11;
                (this).GenStmt(_20_newStmt, selfIdent, env, isLast, earlyReturn, out _out9, out _out10, out _out11);
                generated = _out9;
                readIdents = _out10;
                newEnv = _out11;
              }
            }
            goto after_match0;
          }
        }
      }
      {
        if (_source0.is_Assign) {
          DAST._IAssignLhs _21_lhs = _source0.dtor_lhs;
          DAST._IExpression _22_expression = _source0.dtor_value;
          {
            RAST._IExpr _23_exprGen;
            Defs._IOwnership _24___v75;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _25_exprIdents;
            RAST._IExpr _out12;
            Defs._IOwnership _out13;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out14;
            (this).GenExpr(_22_expression, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out12, out _out13, out _out14);
            _23_exprGen = _out12;
            _24___v75 = _out13;
            _25_exprIdents = _out14;
            if ((_21_lhs).is_Ident) {
              Dafny.ISequence<Dafny.Rune> _26_rustId;
              _26_rustId = Defs.__default.escapeVar((_21_lhs).dtor_ident);
              Std.Wrappers._IOption<RAST._IType> _27_tpe;
              _27_tpe = (env).GetType(_26_rustId);
              if (((_27_tpe).is_Some) && ((((_27_tpe).dtor_value).ExtractMaybePlacebo()).is_Some)) {
                _23_exprGen = RAST.__default.MaybePlacebo(_23_exprGen);
              }
            }
            if (((_21_lhs).is_Index) && (((_21_lhs).dtor_expr).is_Ident)) {
              Dafny.ISequence<Dafny.Rune> _28_rustId;
              _28_rustId = Defs.__default.escapeVar(((_21_lhs).dtor_expr).dtor_name);
              Std.Wrappers._IOption<RAST._IType> _29_tpe;
              _29_tpe = (env).GetType(_28_rustId);
              if (((_29_tpe).is_Some) && ((((_29_tpe).dtor_value).ExtractMaybeUninitArrayElement()).is_Some)) {
                _23_exprGen = RAST.__default.MaybeUninitNew(_23_exprGen);
              }
            }
            RAST._IExpr _30_lhsGen;
            bool _31_needsIIFE;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _32_recIdents;
            Defs._IEnvironment _33_resEnv;
            RAST._IExpr _out15;
            bool _out16;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out17;
            Defs._IEnvironment _out18;
            (this).GenAssignLhs(_21_lhs, _23_exprGen, selfIdent, env, out _out15, out _out16, out _out17, out _out18);
            _30_lhsGen = _out15;
            _31_needsIIFE = _out16;
            _32_recIdents = _out17;
            _33_resEnv = _out18;
            generated = _30_lhsGen;
            newEnv = _33_resEnv;
            if (_31_needsIIFE) {
              generated = RAST.Expr.create_Block(generated);
            }
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_32_recIdents, _25_exprIdents);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_If) {
          DAST._IExpression _34_cond = _source0.dtor_cond;
          Dafny.ISequence<DAST._IStatement> _35_thnDafny = _source0.dtor_thn;
          Dafny.ISequence<DAST._IStatement> _36_elsDafny = _source0.dtor_els;
          {
            RAST._IExpr _37_cond;
            Defs._IOwnership _38___v76;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _39_recIdents;
            RAST._IExpr _out19;
            Defs._IOwnership _out20;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out21;
            (this).GenExpr(_34_cond, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out19, out _out20, out _out21);
            _37_cond = _out19;
            _38___v76 = _out20;
            _39_recIdents = _out21;
            Dafny.ISequence<Dafny.Rune> _40_condString;
            _40_condString = (_37_cond)._ToString(Defs.__default.IND);
            readIdents = _39_recIdents;
            RAST._IExpr _41_thn;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _42_thnIdents;
            Defs._IEnvironment _43_thnEnv;
            RAST._IExpr _out22;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out23;
            Defs._IEnvironment _out24;
            (this).GenStmts(_35_thnDafny, selfIdent, env, isLast, earlyReturn, out _out22, out _out23, out _out24);
            _41_thn = _out22;
            _42_thnIdents = _out23;
            _43_thnEnv = _out24;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _42_thnIdents);
            RAST._IExpr _44_els;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _45_elsIdents;
            Defs._IEnvironment _46_elsEnv;
            RAST._IExpr _out25;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out26;
            Defs._IEnvironment _out27;
            (this).GenStmts(_36_elsDafny, selfIdent, env, isLast, earlyReturn, out _out25, out _out26, out _out27);
            _44_els = _out25;
            _45_elsIdents = _out26;
            _46_elsEnv = _out27;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _45_elsIdents);
            newEnv = env;
            generated = RAST.Expr.create_IfExpr(_37_cond, _41_thn, _44_els);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Labeled) {
          Dafny.ISequence<Dafny.Rune> _47_lbl = _source0.dtor_lbl;
          Dafny.ISequence<DAST._IStatement> _48_body = _source0.dtor_body;
          {
            RAST._IExpr _49_body;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _50_bodyIdents;
            Defs._IEnvironment _51_env2;
            RAST._IExpr _out28;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out29;
            Defs._IEnvironment _out30;
            (this).GenStmts(_48_body, selfIdent, env, isLast, earlyReturn, out _out28, out _out29, out _out30);
            _49_body = _out28;
            _50_bodyIdents = _out29;
            _51_env2 = _out30;
            readIdents = _50_bodyIdents;
            generated = RAST.Expr.create_Labelled(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("label_"), _47_lbl), RAST.Expr.create_Loop(Std.Wrappers.Option<RAST._IExpr>.create_None(), RAST.Expr.create_StmtExpr(_49_body, RAST.Expr.create_Break(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None()))));
            newEnv = env;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_While) {
          DAST._IExpression _52_cond = _source0.dtor_cond;
          Dafny.ISequence<DAST._IStatement> _53_body = _source0.dtor_body;
          {
            RAST._IExpr _54_cond;
            Defs._IOwnership _55___v77;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _56_recIdents;
            RAST._IExpr _out31;
            Defs._IOwnership _out32;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out33;
            (this).GenExpr(_52_cond, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out31, out _out32, out _out33);
            _54_cond = _out31;
            _55___v77 = _out32;
            _56_recIdents = _out33;
            readIdents = _56_recIdents;
            RAST._IExpr _57_bodyExpr;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _58_bodyIdents;
            Defs._IEnvironment _59_bodyEnv;
            RAST._IExpr _out34;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out35;
            Defs._IEnvironment _out36;
            (this).GenStmts(_53_body, selfIdent, env, false, earlyReturn, out _out34, out _out35, out _out36);
            _57_bodyExpr = _out34;
            _58_bodyIdents = _out35;
            _59_bodyEnv = _out36;
            newEnv = env;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _58_bodyIdents);
            generated = RAST.Expr.create_Loop(Std.Wrappers.Option<RAST._IExpr>.create_Some(_54_cond), _57_bodyExpr);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Foreach) {
          Dafny.ISequence<Dafny.Rune> _60_boundName = _source0.dtor_boundName;
          DAST._IType _61_boundType = _source0.dtor_boundType;
          DAST._IExpression _62_overExpr = _source0.dtor_over;
          Dafny.ISequence<DAST._IStatement> _63_body = _source0.dtor_body;
          {
            RAST._IExpr _64_over;
            Defs._IOwnership _65___v78;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _66_recIdents;
            RAST._IExpr _out37;
            Defs._IOwnership _out38;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out39;
            (this).GenExpr(_62_overExpr, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out37, out _out38, out _out39);
            _64_over = _out37;
            _65___v78 = _out38;
            _66_recIdents = _out39;
            if (((_62_overExpr).is_MapBoundedPool) || ((_62_overExpr).is_SetBoundedPool)) {
              _64_over = ((_64_over).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cloned"))).Apply0();
            }
            RAST._IType _67_boundTpe;
            RAST._IType _out40;
            _out40 = (this).GenType(_61_boundType, Defs.GenTypeContext.@default());
            _67_boundTpe = _out40;
            readIdents = _66_recIdents;
            Dafny.ISequence<Dafny.Rune> _68_boundRName;
            _68_boundRName = Defs.__default.escapeVar(_60_boundName);
            RAST._IExpr _69_bodyExpr;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _70_bodyIdents;
            Defs._IEnvironment _71_bodyEnv;
            RAST._IExpr _out41;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out42;
            Defs._IEnvironment _out43;
            (this).GenStmts(_63_body, selfIdent, (env).AddAssigned(_68_boundRName, _67_boundTpe), false, earlyReturn, out _out41, out _out42, out _out43);
            _69_bodyExpr = _out41;
            _70_bodyIdents = _out42;
            _71_bodyEnv = _out43;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _70_bodyIdents), Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_68_boundRName));
            newEnv = env;
            generated = RAST.Expr.create_For(_68_boundRName, _64_over, _69_bodyExpr);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Break) {
          Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _72_toLabel = _source0.dtor_toLabel;
          {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _source1 = _72_toLabel;
            {
              if (_source1.is_Some) {
                Dafny.ISequence<Dafny.Rune> _73_lbl = _source1.dtor_value;
                {
                  generated = RAST.Expr.create_Break(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("label_"), _73_lbl)));
                }
                goto after_match1;
              }
            }
            {
              {
                generated = RAST.Expr.create_Break(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None());
              }
            }
          after_match1: ;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            newEnv = env;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_TailRecursive) {
          Dafny.ISequence<DAST._IStatement> _74_body = _source0.dtor_body;
          {
            generated = (this).InitEmptyExpr();
            Defs._IEnvironment _75_oldEnv;
            _75_oldEnv = env;
            if (!object.Equals(selfIdent, Defs.SelfInfo.create_NoSelf())) {
              RAST._IExpr _76_selfClone;
              Defs._IOwnership _77___v79;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _78___v80;
              RAST._IExpr _out44;
              Defs._IOwnership _out45;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out46;
              (this).GenIdent((selfIdent).dtor_rSelfName, selfIdent, Defs.Environment.Empty(), Defs.Ownership.create_OwnershipOwned(), out _out44, out _out45, out _out46);
              _76_selfClone = _out44;
              _77___v79 = _out45;
              _78___v80 = _out46;
              generated = (generated).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_this"), Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(_76_selfClone)));
              if (((_75_oldEnv).dtor_names).Contains((selfIdent).dtor_rSelfName)) {
                _75_oldEnv = (_75_oldEnv).RemoveAssigned((selfIdent).dtor_rSelfName);
              }
            }
            RAST._IExpr _79_loopBegin;
            _79_loopBegin = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
            newEnv = env;
            BigInteger _hi1 = new BigInteger(((_75_oldEnv).dtor_names).Count);
            for (BigInteger _80_paramI = BigInteger.Zero; _80_paramI < _hi1; _80_paramI++) {
              Dafny.ISequence<Dafny.Rune> _81_param;
              _81_param = ((_75_oldEnv).dtor_names).Select(_80_paramI);
              if ((_81_param).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_accumulator"))) {
                goto continue_4_0;
              }
              RAST._IExpr _82_paramInit;
              Defs._IOwnership _83___v81;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _84___v82;
              RAST._IExpr _out47;
              Defs._IOwnership _out48;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out49;
              (this).GenIdent(_81_param, selfIdent, _75_oldEnv, Defs.Ownership.create_OwnershipOwned(), out _out47, out _out48, out _out49);
              _82_paramInit = _out47;
              _83___v81 = _out48;
              _84___v82 = _out49;
              Dafny.ISequence<Dafny.Rune> _85_recVar;
              _85_recVar = Dafny.Sequence<Dafny.Rune>.Concat(Defs.__default.TailRecursionPrefix, Std.Strings.__default.OfNat(_80_paramI));
              generated = (generated).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _85_recVar, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(_82_paramInit)));
              if (((_75_oldEnv).dtor_types).Contains(_81_param)) {
                RAST._IType _86_declaredType;
                _86_declaredType = (Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Select((_75_oldEnv).dtor_types,_81_param)).ToOwned();
                newEnv = (newEnv).AddAssigned(_81_param, _86_declaredType);
                newEnv = (newEnv).AddAssigned(_85_recVar, _86_declaredType);
              }
              _79_loopBegin = (_79_loopBegin).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), _81_param, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_Identifier(_85_recVar))));
            continue_4_0: ;
            }
          after_4_0: ;
            RAST._IExpr _87_bodyExpr;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _88_bodyIdents;
            Defs._IEnvironment _89_bodyEnv;
            RAST._IExpr _out50;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out51;
            Defs._IEnvironment _out52;
            (this).GenStmts(_74_body, ((!object.Equals(selfIdent, Defs.SelfInfo.create_NoSelf())) ? (Defs.SelfInfo.create_ThisTyped(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_this"), (selfIdent).dtor_dafnyType)) : (Defs.SelfInfo.create_NoSelf())), newEnv, false, earlyReturn, out _out50, out _out51, out _out52);
            _87_bodyExpr = _out50;
            _88_bodyIdents = _out51;
            _89_bodyEnv = _out52;
            readIdents = _88_bodyIdents;
            generated = (generated).Then(RAST.Expr.create_Labelled(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("TAIL_CALL_START"), RAST.Expr.create_Loop(Std.Wrappers.Option<RAST._IExpr>.create_None(), (_79_loopBegin).Then(_87_bodyExpr))));
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_JumpTailCallStart) {
          {
            generated = RAST.Expr.create_Continue(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("TAIL_CALL_START")));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            newEnv = env;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Call) {
          DAST._IExpression _90_on = _source0.dtor_on;
          DAST._ICallName _91_name = _source0.dtor_callName;
          Dafny.ISequence<DAST._IType> _92_typeArgs = _source0.dtor_typeArgs;
          Dafny.ISequence<DAST._IExpression> _93_args = _source0.dtor_args;
          Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> _94_maybeOutVars = _source0.dtor_outs;
          {
            RAST._IExpr _out53;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out54;
            (this).GenOwnedCallPart(_90_on, selfIdent, _91_name, _92_typeArgs, _93_args, env, out _out53, out _out54);
            generated = _out53;
            readIdents = _out54;
            newEnv = env;
            if (((_94_maybeOutVars).is_Some) && ((new BigInteger(((_94_maybeOutVars).dtor_value).Count)) == (BigInteger.One))) {
              Dafny.ISequence<Dafny.Rune> _95_outVar;
              _95_outVar = Defs.__default.escapeVar(((_94_maybeOutVars).dtor_value).Select(BigInteger.Zero));
              if ((env).IsMaybePlacebo(_95_outVar)) {
                generated = RAST.__default.MaybePlacebo(generated);
              }
              generated = RAST.__default.AssignVar(_95_outVar, generated);
            } else if (((_94_maybeOutVars).is_None) || ((new BigInteger(((_94_maybeOutVars).dtor_value).Count)).Sign == 0)) {
            } else {
              Dafny.ISequence<Dafny.Rune> _96_tmpVar;
              _96_tmpVar = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_x");
              RAST._IExpr _97_tmpId;
              _97_tmpId = RAST.Expr.create_Identifier(_96_tmpVar);
              generated = RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), _96_tmpVar, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(generated));
              Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _98_outVars;
              _98_outVars = (_94_maybeOutVars).dtor_value;
              BigInteger _hi2 = new BigInteger((_98_outVars).Count);
              for (BigInteger _99_outI = BigInteger.Zero; _99_outI < _hi2; _99_outI++) {
                Dafny.ISequence<Dafny.Rune> _100_outVar;
                _100_outVar = Defs.__default.escapeVar((_98_outVars).Select(_99_outI));
                RAST._IExpr _101_rhs;
                _101_rhs = (_97_tmpId).Sel(Std.Strings.__default.OfNat(_99_outI));
                if ((env).IsMaybePlacebo(_100_outVar)) {
                  _101_rhs = RAST.__default.MaybePlacebo(_101_rhs);
                }
                generated = (generated).Then(RAST.__default.AssignVar(_100_outVar, _101_rhs));
              }
            }
            newEnv = env;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Return) {
          DAST._IExpression _102_exprDafny = _source0.dtor_expr;
          {
            RAST._IExpr _103_expr;
            Defs._IOwnership _104___v83;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _105_recIdents;
            RAST._IExpr _out55;
            Defs._IOwnership _out56;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out57;
            (this).GenExpr(_102_exprDafny, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out55, out _out56, out _out57);
            _103_expr = _out55;
            _104___v83 = _out56;
            _105_recIdents = _out57;
            readIdents = _105_recIdents;
            if (isLast) {
              generated = _103_expr;
            } else {
              generated = RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_Some(_103_expr));
            }
            newEnv = env;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_EarlyReturn) {
          {
            Std.Wrappers._IOption<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>> _source2 = earlyReturn;
            {
              if (_source2.is_None) {
                generated = RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_None());
                goto after_match2;
              }
            }
            {
              Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _106_rustIdents = _source2.dtor_value;
              Dafny.ISequence<RAST._IExpr> _107_tupleArgs;
              _107_tupleArgs = Dafny.Sequence<RAST._IExpr>.FromElements();
              BigInteger _hi3 = new BigInteger((_106_rustIdents).Count);
              for (BigInteger _108_i = BigInteger.Zero; _108_i < _hi3; _108_i++) {
                RAST._IExpr _109_rIdent;
                Defs._IOwnership _110___v84;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _111___v85;
                RAST._IExpr _out58;
                Defs._IOwnership _out59;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out60;
                (this).GenIdent((_106_rustIdents).Select(_108_i), selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out58, out _out59, out _out60);
                _109_rIdent = _out58;
                _110___v84 = _out59;
                _111___v85 = _out60;
                _107_tupleArgs = Dafny.Sequence<RAST._IExpr>.Concat(_107_tupleArgs, Dafny.Sequence<RAST._IExpr>.FromElements(_109_rIdent));
              }
              if ((new BigInteger((_107_tupleArgs).Count)) == (BigInteger.One)) {
                generated = RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_Some((_107_tupleArgs).Select(BigInteger.Zero)));
              } else {
                generated = RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_Tuple(_107_tupleArgs)));
              }
            }
          after_match2: ;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            newEnv = env;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Halt) {
          {
            generated = (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("panic!"))).Apply1(RAST.Expr.create_LiteralString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Halt"), false, false));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            newEnv = env;
          }
          goto after_match0;
        }
      }
      {
        DAST._IExpression _112_e = _source0.dtor_Print_a0;
        {
          RAST._IExpr _113_printedExpr;
          Defs._IOwnership _114_recOwnership;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _115_recIdents;
          RAST._IExpr _out61;
          Defs._IOwnership _out62;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out63;
          (this).GenExpr(_112_e, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out61, out _out62, out _out63);
          _113_printedExpr = _out61;
          _114_recOwnership = _out62;
          _115_recIdents = _out63;
          generated = (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("print!"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_LiteralString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{}"), false, false), (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyPrintWrapper"))).AsExpr()).Apply1(_113_printedExpr)));
          readIdents = _115_recIdents;
          newEnv = env;
        }
      }
    after_match0: ;
    }
    public void FromOwned(RAST._IExpr r, Defs._IOwnership expectedOwnership, out RAST._IExpr @out, out Defs._IOwnership resultingOwnership)
    {
      @out = RAST.Expr.Default();
      resultingOwnership = Defs.Ownership.Default();
      if ((object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipOwned())) || (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipAutoBorrowed()))) {
        @out = r;
        resultingOwnership = Defs.Ownership.create_OwnershipOwned();
      } else if (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipBorrowed())) {
        @out = RAST.__default.Borrow(r);
        resultingOwnership = Defs.Ownership.create_OwnershipBorrowed();
      } else {
        RAST._IExpr _out0;
        _out0 = (this).Error(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Conversion from Borrowed or BorrowedMut to BorrowedMut"), r);
        @out = _out0;
        @out = ((this).modify__macro).Apply1(@out);
        resultingOwnership = Defs.Ownership.create_OwnershipBorrowedMut();
      }
    }
    public void FromOwnership(RAST._IExpr r, Defs._IOwnership ownership, Defs._IOwnership expectedOwnership, out RAST._IExpr @out, out Defs._IOwnership resultingOwnership)
    {
      @out = RAST.Expr.Default();
      resultingOwnership = Defs.Ownership.Default();
      if (object.Equals(ownership, expectedOwnership)) {
        @out = r;
        resultingOwnership = expectedOwnership;
        return ;
      }
      if (object.Equals(ownership, Defs.Ownership.create_OwnershipOwned())) {
        RAST._IExpr _out0;
        Defs._IOwnership _out1;
        (this).FromOwned(r, expectedOwnership, out _out0, out _out1);
        @out = _out0;
        resultingOwnership = _out1;
        return ;
      } else if ((object.Equals(ownership, Defs.Ownership.create_OwnershipBorrowed())) || (object.Equals(ownership, Defs.Ownership.create_OwnershipBorrowedMut()))) {
        if (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipOwned())) {
          resultingOwnership = Defs.Ownership.create_OwnershipOwned();
          @out = (r).Clone();
        } else if ((object.Equals(expectedOwnership, ownership)) || (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipAutoBorrowed()))) {
          resultingOwnership = ownership;
          @out = r;
        } else if ((object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipBorrowed())) && (object.Equals(ownership, Defs.Ownership.create_OwnershipBorrowedMut()))) {
          resultingOwnership = Defs.Ownership.create_OwnershipBorrowed();
          @out = r;
        } else {
          resultingOwnership = Defs.Ownership.create_OwnershipBorrowedMut();
          @out = RAST.__default.BorrowMut(r);
        }
        return ;
      } else {
      }
    }
    public void GenExprLiteral(DAST._IExpression e, Defs._ISelfInfo selfIdent, Defs._IEnvironment env, Defs._IOwnership expectedOwnership, out RAST._IExpr r, out Defs._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = Defs.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _source0 = e;
      {
        if (_source0.is_Literal) {
          DAST._ILiteral _h670 = _source0.dtor_Literal_a0;
          if (_h670.is_BoolLiteral) {
            bool _0_b = _h670.dtor_BoolLiteral_a0;
            {
              RAST._IExpr _out0;
              Defs._IOwnership _out1;
              (this).FromOwned(RAST.Expr.create_LiteralBool(_0_b), expectedOwnership, out _out0, out _out1);
              r = _out0;
              resultingOwnership = _out1;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match0;
          }
        }
      }
      {
        if (_source0.is_Literal) {
          DAST._ILiteral _h671 = _source0.dtor_Literal_a0;
          if (_h671.is_IntLiteral) {
            Dafny.ISequence<Dafny.Rune> _1_i = _h671.dtor_IntLiteral_a0;
            DAST._IType _2_t = _h671.dtor_IntLiteral_a1;
            {
              DAST._IType _source1 = _2_t;
              {
                if (_source1.is_Primitive) {
                  DAST._IPrimitive _h70 = _source1.dtor_Primitive_a0;
                  if (_h70.is_Int) {
                    {
                      if ((new BigInteger((_1_i).Count)) <= (new BigInteger(4))) {
                        r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1(RAST.Expr.create_LiteralInt(_1_i));
                      } else {
                        r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1(RAST.Expr.create_LiteralString(_1_i, true, false));
                      }
                    }
                    goto after_match1;
                  }
                }
              }
              {
                DAST._IType _3_o = _source1;
                {
                  RAST._IType _4_genType;
                  RAST._IType _out2;
                  _out2 = (this).GenType(_3_o, Defs.GenTypeContext.@default());
                  _4_genType = _out2;
                  r = RAST.Expr.create_TypeAscription(RAST.Expr.create_LiteralInt(_1_i), _4_genType);
                }
              }
            after_match1: ;
              RAST._IExpr _out3;
              Defs._IOwnership _out4;
              (this).FromOwned(r, expectedOwnership, out _out3, out _out4);
              r = _out3;
              resultingOwnership = _out4;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match0;
          }
        }
      }
      {
        if (_source0.is_Literal) {
          DAST._ILiteral _h672 = _source0.dtor_Literal_a0;
          if (_h672.is_DecLiteral) {
            Dafny.ISequence<Dafny.Rune> _5_n = _h672.dtor_DecLiteral_a0;
            Dafny.ISequence<Dafny.Rune> _6_d = _h672.dtor_DecLiteral_a1;
            DAST._IType _7_t = _h672.dtor_DecLiteral_a2;
            {
              DAST._IType _source2 = _7_t;
              {
                if (_source2.is_Primitive) {
                  DAST._IPrimitive _h71 = _source2.dtor_Primitive_a0;
                  if (_h71.is_Real) {
                    {
                      r = ((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("BigRational"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(((((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("BigInt"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("parse_bytes"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_LiteralString(_5_n, true, false), RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("10"))))).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unwrap"))).Apply0(), ((((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("BigInt"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("parse_bytes"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_LiteralString(_6_d, true, false), RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("10"))))).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unwrap"))).Apply0()));
                    }
                    goto after_match2;
                  }
                }
              }
              {
                DAST._IType _8_o = _source2;
                {
                  RAST._IType _9_genType;
                  RAST._IType _out5;
                  _out5 = (this).GenType(_8_o, Defs.GenTypeContext.@default());
                  _9_genType = _out5;
                  r = RAST.Expr.create_TypeAscription(RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/"), (RAST.Expr.create_LiteralInt(_5_n)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")), (RAST.Expr.create_LiteralInt(_6_d)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")), DAST.Format.BinaryOpFormat.create_NoFormat()), _9_genType);
                }
              }
            after_match2: ;
              RAST._IExpr _out6;
              Defs._IOwnership _out7;
              (this).FromOwned(r, expectedOwnership, out _out6, out _out7);
              r = _out6;
              resultingOwnership = _out7;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match0;
          }
        }
      }
      {
        if (_source0.is_Literal) {
          DAST._ILiteral _h673 = _source0.dtor_Literal_a0;
          if (_h673.is_StringLiteral) {
            Dafny.ISequence<Dafny.Rune> _10_l = _h673.dtor_StringLiteral_a0;
            bool _11_verbatim = _h673.dtor_verbatim;
            {
              r = (((RAST.__default.dafny__runtime).MSel((this).string__of)).AsExpr()).Apply1(RAST.Expr.create_LiteralString(_10_l, false, _11_verbatim));
              RAST._IExpr _out8;
              Defs._IOwnership _out9;
              (this).FromOwned(r, expectedOwnership, out _out8, out _out9);
              r = _out8;
              resultingOwnership = _out9;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match0;
          }
        }
      }
      {
        if (_source0.is_Literal) {
          DAST._ILiteral _h674 = _source0.dtor_Literal_a0;
          if (_h674.is_CharLiteralUTF16) {
            BigInteger _12_c = _h674.dtor_CharLiteralUTF16_a0;
            {
              r = RAST.Expr.create_LiteralInt(Std.Strings.__default.OfNat(_12_c));
              r = RAST.Expr.create_TypeAscription(r, RAST.Type.create_U16());
              r = (((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsExpr()).Apply1(r);
              RAST._IExpr _out10;
              Defs._IOwnership _out11;
              (this).FromOwned(r, expectedOwnership, out _out10, out _out11);
              r = _out10;
              resultingOwnership = _out11;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match0;
          }
        }
      }
      {
        if (_source0.is_Literal) {
          DAST._ILiteral _h675 = _source0.dtor_Literal_a0;
          if (_h675.is_CharLiteral) {
            Dafny.Rune _13_c = _h675.dtor_CharLiteral_a0;
            {
              r = RAST.Expr.create_LiteralInt(Std.Strings.__default.OfNat(new BigInteger((_13_c).Value)));
              if (!(((this).charType).is_UTF32)) {
                r = RAST.Expr.create_TypeAscription(r, RAST.Type.create_U16());
              } else {
                r = (((((((RAST.__default.@global).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("std"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("primitive"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("char"))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_u32"))).Apply1(r)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unwrap"))).Apply0();
              }
              r = (((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsExpr()).Apply1(r);
              RAST._IExpr _out12;
              Defs._IOwnership _out13;
              (this).FromOwned(r, expectedOwnership, out _out12, out _out13);
              r = _out12;
              resultingOwnership = _out13;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              return ;
            }
            goto after_match0;
          }
        }
      }
      {
        DAST._ILiteral _h676 = _source0.dtor_Literal_a0;
        DAST._IType _14_tpe = _h676.dtor_Null_a0;
        {
          RAST._IType _15_tpeGen;
          RAST._IType _out14;
          _out14 = (this).GenType(_14_tpe, Defs.GenTypeContext.@default());
          _15_tpeGen = _out14;
          if (((this).pointerType).is_Raw) {
            r = ((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Ptr"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("null"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
          } else {
            r = RAST.Expr.create_TypeAscription((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Object"))).AsExpr()).Apply1(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("None"))), _15_tpeGen);
          }
          RAST._IExpr _out15;
          Defs._IOwnership _out16;
          (this).FromOwned(r, expectedOwnership, out _out15, out _out16);
          r = _out15;
          resultingOwnership = _out16;
          readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
          return ;
        }
      }
    after_match0: ;
    }
    public RAST._IExpr ToPrimitive(RAST._IExpr r, DAST._IType typ, DAST._IType primitiveType, Defs._IEnvironment env)
    {
      RAST._IExpr @out = RAST.Expr.Default();
      @out = r;
      if (!object.Equals(typ, primitiveType)) {
        Defs._IOwnership _0_dummy = Defs.Ownership.Default();
        RAST._IExpr _out0;
        Defs._IOwnership _out1;
        (this).GenExprConvertTo(r, Defs.Ownership.create_OwnershipOwned(), typ, primitiveType, env, Defs.Ownership.create_OwnershipOwned(), out _out0, out _out1);
        @out = _out0;
        _0_dummy = _out1;
      }
      return @out;
    }
    public RAST._IExpr ToBool(RAST._IExpr r, DAST._IType typ, Defs._IEnvironment env)
    {
      RAST._IExpr @out = RAST.Expr.Default();
      RAST._IExpr _out0;
      _out0 = (this).ToPrimitive(r, typ, DAST.Type.create_Primitive(DAST.Primitive.create_Bool()), env);
      @out = _out0;
      return @out;
    }
    public RAST._IExpr ToInt(RAST._IExpr r, DAST._IType typ, Defs._IEnvironment env)
    {
      RAST._IExpr @out = RAST.Expr.Default();
      RAST._IExpr _out0;
      _out0 = (this).ToPrimitive(r, typ, DAST.Type.create_Primitive(DAST.Primitive.create_Int()), env);
      @out = _out0;
      return @out;
    }
    public RAST._IExpr FromPrimitive(RAST._IExpr r, DAST._IType primitiveType, DAST._IType typ, Defs._IEnvironment env)
    {
      RAST._IExpr @out = RAST.Expr.Default();
      @out = r;
      if (!object.Equals(typ, primitiveType)) {
        Defs._IOwnership _0_dummy = Defs.Ownership.Default();
        RAST._IExpr _out0;
        Defs._IOwnership _out1;
        (this).GenExprConvertTo(r, Defs.Ownership.create_OwnershipOwned(), primitiveType, typ, env, Defs.Ownership.create_OwnershipOwned(), out _out0, out _out1);
        @out = _out0;
        _0_dummy = _out1;
      }
      return @out;
    }
    public RAST._IExpr FromBool(RAST._IExpr r, DAST._IType typ, Defs._IEnvironment env)
    {
      RAST._IExpr @out = RAST.Expr.Default();
      RAST._IExpr _out0;
      _out0 = (this).FromPrimitive(r, DAST.Type.create_Primitive(DAST.Primitive.create_Bool()), typ, env);
      @out = _out0;
      return @out;
    }
    public RAST._IExpr FromInt(RAST._IExpr r, DAST._IType typ, Defs._IEnvironment env)
    {
      RAST._IExpr @out = RAST.Expr.Default();
      RAST._IExpr _out0;
      _out0 = (this).FromPrimitive(r, DAST.Type.create_Primitive(DAST.Primitive.create_Int()), typ, env);
      @out = _out0;
      return @out;
    }
    public void GenExprBinary(DAST._IExpression e, Defs._ISelfInfo selfIdent, Defs._IEnvironment env, Defs._IOwnership expectedOwnership, out RAST._IExpr r, out Defs._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = Defs.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _let_tmp_rhs0 = e;
      DAST._ITypedBinOp _let_tmp_rhs1 = _let_tmp_rhs0.dtor_op;
      DAST._IBinOp _0_op = _let_tmp_rhs1.dtor_op;
      DAST._IType _1_lType = _let_tmp_rhs1.dtor_leftType;
      DAST._IType _2_rType = _let_tmp_rhs1.dtor_rightType;
      DAST._IType _3_resType = _let_tmp_rhs1.dtor_resultType;
      DAST._IExpression _4_lExpr = _let_tmp_rhs0.dtor_left;
      DAST._IExpression _5_rExpr = _let_tmp_rhs0.dtor_right;
      DAST.Format._IBinaryOpFormat _6_format = _let_tmp_rhs0.dtor_format2;
      bool _7_becomesLeftCallsRight;
      _7_becomesLeftCallsRight = Defs.__default.BecomesLeftCallsRight(_0_op);
      bool _8_becomesRightCallsLeft;
      _8_becomesRightCallsLeft = Defs.__default.BecomesRightCallsLeft(_0_op);
      Defs._IOwnership _9_expectedLeftOwnership;
      if (_7_becomesLeftCallsRight) {
        _9_expectedLeftOwnership = Defs.Ownership.create_OwnershipAutoBorrowed();
      } else if (_8_becomesRightCallsLeft) {
        _9_expectedLeftOwnership = Defs.Ownership.create_OwnershipBorrowed();
      } else {
        _9_expectedLeftOwnership = Defs.Ownership.create_OwnershipOwned();
      }
      Defs._IOwnership _10_expectedRightOwnership;
      if (_7_becomesLeftCallsRight) {
        _10_expectedRightOwnership = Defs.Ownership.create_OwnershipBorrowed();
      } else if (_8_becomesRightCallsLeft) {
        _10_expectedRightOwnership = Defs.Ownership.create_OwnershipAutoBorrowed();
      } else {
        _10_expectedRightOwnership = Defs.Ownership.create_OwnershipOwned();
      }
      RAST._IExpr _11_left;
      Defs._IOwnership _12___v86;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _13_recIdentsL;
      RAST._IExpr _out0;
      Defs._IOwnership _out1;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out2;
      (this).GenExpr(_4_lExpr, selfIdent, env, _9_expectedLeftOwnership, out _out0, out _out1, out _out2);
      _11_left = _out0;
      _12___v86 = _out1;
      _13_recIdentsL = _out2;
      RAST._IExpr _14_right;
      Defs._IOwnership _15___v87;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _16_recIdentsR;
      RAST._IExpr _out3;
      Defs._IOwnership _out4;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out5;
      (this).GenExpr(_5_rExpr, selfIdent, env, _10_expectedRightOwnership, out _out3, out _out4, out _out5);
      _14_right = _out3;
      _15___v87 = _out4;
      _16_recIdentsR = _out5;
      DAST._IBinOp _source0 = _0_op;
      {
        if (_source0.is_In) {
          {
            r = ((_14_right).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("contains"))).Apply1(_11_left);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SeqProperPrefix) {
          r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), _11_left, _14_right, _6_format);
          goto after_match0;
        }
      }
      {
        if (_source0.is_SeqPrefix) {
          r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<="), _11_left, _14_right, _6_format);
          goto after_match0;
        }
      }
      {
        if (_source0.is_SetMerge) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("merge"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SetSubtraction) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("subtract"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SetIntersection) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("intersect"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Subset) {
          {
            r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<="), _11_left, _14_right, _6_format);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_ProperSubset) {
          {
            r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), _11_left, _14_right, _6_format);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SetDisjoint) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("disjoint"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MapMerge) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("merge"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MapSubtraction) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("subtract"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MultisetMerge) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("merge"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MultisetSubtraction) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("subtract"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MultisetIntersection) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("intersect"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Submultiset) {
          {
            r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<="), _11_left, _14_right, _6_format);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_ProperSubmultiset) {
          {
            r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), _11_left, _14_right, _6_format);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MultisetDisjoint) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("disjoint"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Concat) {
          {
            r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("concat"))).Apply1(_14_right);
          }
          goto after_match0;
        }
      }
      {
        {
          if ((Defs.__default.OpTable).Contains(_0_op)) {
            if (Defs.__default.IsBooleanOperator(_0_op)) {
              RAST._IExpr _out6;
              _out6 = (this).ToBool(_11_left, _1_lType, env);
              _11_left = _out6;
              RAST._IExpr _out7;
              _out7 = (this).ToBool(_14_right, _2_rType, env);
              _14_right = _out7;
            }
            r = RAST.Expr.create_BinaryOp(Dafny.Map<DAST._IBinOp, Dafny.ISequence<Dafny.Rune>>.Select(Defs.__default.OpTable,_0_op), _11_left, _14_right, _6_format);
            if (Defs.__default.IsBooleanOperator(_0_op)) {
              RAST._IExpr _out8;
              _out8 = (this).FromBool(r, _3_resType, env);
              r = _out8;
            }
          } else {
            if (Defs.__default.IsComplexArithmetic(_0_op)) {
              RAST._IExpr _out9;
              _out9 = (this).ToInt(_11_left, _1_lType, env);
              _11_left = _out9;
              RAST._IExpr _out10;
              _out10 = (this).ToInt(_14_right, _2_rType, env);
              _14_right = _out10;
            }
            DAST._IBinOp _source1 = _0_op;
            {
              if (_source1.is_Eq) {
                bool _17_referential = _source1.dtor_referential;
                {
                  if (_17_referential) {
                    if (((this).pointerType).is_Raw) {
                      RAST._IExpr _out11;
                      _out11 = (this).Error(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Raw pointer comparison not properly implemented yet"), (this).InitEmptyExpr());
                      r = _out11;
                    } else {
                      r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="), _11_left, _14_right, DAST.Format.BinaryOpFormat.create_NoFormat());
                    }
                  } else {
                    if (((_5_rExpr).is_SeqValue) && ((new BigInteger(((_5_rExpr).dtor_elements).Count)).Sign == 0)) {
                      r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="), ((((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("to_array"))).Apply0()).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("len"))).Apply0(), RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")), DAST.Format.BinaryOpFormat.create_NoFormat());
                    } else if (((_4_lExpr).is_SeqValue) && ((new BigInteger(((_4_lExpr).dtor_elements).Count)).Sign == 0)) {
                      r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="), RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")), ((((_14_right).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("to_array"))).Apply0()).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("len"))).Apply0(), DAST.Format.BinaryOpFormat.create_NoFormat());
                    } else {
                      r = RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="), _11_left, _14_right, DAST.Format.BinaryOpFormat.create_NoFormat());
                      if (!object.Equals(_3_resType, DAST.Type.create_Primitive(DAST.Primitive.create_Bool()))) {
                        RAST._IExpr _out12;
                        Defs._IOwnership _out13;
                        (this).GenExprConvertTo(r, Defs.Ownership.create_OwnershipOwned(), DAST.Type.create_Primitive(DAST.Primitive.create_Bool()), _3_resType, env, Defs.Ownership.create_OwnershipOwned(), out _out12, out _out13);
                        r = _out12;
                        resultingOwnership = _out13;
                      }
                    }
                  }
                }
                goto after_match1;
              }
            }
            {
              if (_source1.is_Div) {
                bool overflow0 = _source1.dtor_overflow;
                if ((overflow0) == (true)) {
                  r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("wrapping_div"))).Apply1(_14_right);
                  goto after_match1;
                }
              }
            }
            {
              if (_source1.is_Plus) {
                bool overflow1 = _source1.dtor_overflow;
                if ((overflow1) == (true)) {
                  r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("wrapping_add"))).Apply1(_14_right);
                  goto after_match1;
                }
              }
            }
            {
              if (_source1.is_Times) {
                bool overflow2 = _source1.dtor_overflow;
                if ((overflow2) == (true)) {
                  r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("wrapping_mul"))).Apply1(_14_right);
                  goto after_match1;
                }
              }
            }
            {
              if (_source1.is_Minus) {
                bool overflow3 = _source1.dtor_overflow;
                if ((overflow3) == (true)) {
                  r = ((_11_left).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("wrapping_sub"))).Apply1(_14_right);
                  goto after_match1;
                }
              }
            }
            {
              if (_source1.is_EuclidianDiv) {
                {
                  r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("euclidian_division"))).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_11_left, _14_right));
                }
                goto after_match1;
              }
            }
            {
              if (_source1.is_EuclidianMod) {
                {
                  r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("euclidian_modulo"))).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_11_left, _14_right));
                }
                goto after_match1;
              }
            }
            {
              Dafny.ISequence<Dafny.Rune> _18_op = _source1.dtor_Passthrough_a0;
              {
                r = RAST.Expr.create_BinaryOp(_18_op, _11_left, _14_right, _6_format);
              }
            }
          after_match1: ;
            if (Defs.__default.IsComplexArithmetic(_0_op)) {
              RAST._IExpr _out14;
              _out14 = (this).FromInt(r, _3_resType, env);
              r = _out14;
            }
          }
        }
      }
    after_match0: ;
      RAST._IExpr _out15;
      Defs._IOwnership _out16;
      (this).FromOwned(r, expectedOwnership, out _out15, out _out16);
      r = _out15;
      resultingOwnership = _out16;
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_13_recIdentsL, _16_recIdentsR);
      return ;
    }
    public RAST._IExpr UnwrapNewtype(RAST._IExpr expr, Defs._IOwnership exprOwnership, DAST._IType fromTpe)
    {
      RAST._IExpr r = RAST.Expr.Default();
      r = expr;
      if (!((((fromTpe).dtor_resolved).dtor_kind).dtor_erase)) {
        r = (r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0"));
        if (object.Equals(exprOwnership, Defs.Ownership.create_OwnershipBorrowed())) {
          r = RAST.__default.Borrow(r);
        }
      }
      return r;
    }
    public RAST._IExpr WrapWithNewtype(RAST._IExpr expr, Defs._IOwnership exprOwnership, DAST._IType toTpe)
    {
      RAST._IExpr r = RAST.Expr.Default();
      r = expr;
      DAST._IResolvedTypeBase _0_toKind;
      _0_toKind = ((toTpe).dtor_resolved).dtor_kind;
      if (!((_0_toKind).dtor_erase)) {
        RAST._IExpr _1_fullPath;
        RAST._IExpr _out0;
        _out0 = (this).GenPathExpr(((toTpe).dtor_resolved).dtor_path, true);
        _1_fullPath = _out0;
        if (object.Equals(exprOwnership, Defs.Ownership.create_OwnershipOwned())) {
          r = (_1_fullPath).Apply1(r);
        } else {
          r = ((_1_fullPath).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_from_ref"))).Apply1(r);
        }
      }
      return r;
    }
    public void GenExprConvertTo(RAST._IExpr expr, Defs._IOwnership exprOwnership, DAST._IType fromTpe, DAST._IType toTpe, Defs._IEnvironment env, Defs._IOwnership expectedOwnership, out RAST._IExpr r, out Defs._IOwnership resultingOwnership)
    {
      r = RAST.Expr.Default();
      resultingOwnership = Defs.Ownership.Default();
      r = expr;
      DAST._IType _0_fromTpe;
      _0_fromTpe = (fromTpe).RemoveSynonyms();
      DAST._IType _1_toTpe;
      _1_toTpe = (toTpe).RemoveSynonyms();
      if (object.Equals(_0_fromTpe, _1_toTpe)) {
        RAST._IExpr _out0;
        Defs._IOwnership _out1;
        (this).FromOwnership(r, exprOwnership, expectedOwnership, out _out0, out _out1);
        r = _out0;
        resultingOwnership = _out1;
        return ;
      }
      if (Defs.__default.NeedsUnwrappingConversion(_0_fromTpe)) {
        RAST._IExpr _out2;
        _out2 = (this).UnwrapNewtype(r, exprOwnership, _0_fromTpe);
        r = _out2;
        RAST._IExpr _out3;
        Defs._IOwnership _out4;
        (this).GenExprConvertTo(r, exprOwnership, (((_0_fromTpe).dtor_resolved).dtor_kind).dtor_baseType, _1_toTpe, env, expectedOwnership, out _out3, out _out4);
        r = _out3;
        resultingOwnership = _out4;
        return ;
      }
      if (Defs.__default.NeedsUnwrappingConversion(_1_toTpe)) {
        DAST._IResolvedTypeBase _2_toKind;
        _2_toKind = ((_1_toTpe).dtor_resolved).dtor_kind;
        RAST._IExpr _out5;
        Defs._IOwnership _out6;
        (this).GenExprConvertTo(r, exprOwnership, _0_fromTpe, (_2_toKind).dtor_baseType, env, expectedOwnership, out _out5, out _out6);
        r = _out5;
        resultingOwnership = _out6;
        RAST._IExpr _out7;
        _out7 = (this).WrapWithNewtype(r, resultingOwnership, _1_toTpe);
        r = _out7;
        RAST._IExpr _out8;
        Defs._IOwnership _out9;
        (this).FromOwnership(r, resultingOwnership, expectedOwnership, out _out8, out _out9);
        r = _out8;
        resultingOwnership = _out9;
        return ;
      }
      Std.Wrappers._IOption<RAST._IType> _3_unwrappedFromType;
      _3_unwrappedFromType = Defs.__default.GetUnwrappedBoundedRustType(_0_fromTpe);
      Std.Wrappers._IOption<RAST._IType> _4_unwrappedToType;
      _4_unwrappedToType = Defs.__default.GetUnwrappedBoundedRustType(_1_toTpe);
      if ((_4_unwrappedToType).is_Some) {
        RAST._IType _5_boundedToType;
        _5_boundedToType = (_4_unwrappedToType).dtor_value;
        if ((_3_unwrappedFromType).is_Some) {
          RAST._IExpr _out10;
          _out10 = (this).UnwrapNewtype(r, exprOwnership, _0_fromTpe);
          r = _out10;
          Defs._IOwnership _6_inOwnership;
          _6_inOwnership = exprOwnership;
          if (!object.Equals((_3_unwrappedFromType).dtor_value, (_4_unwrappedToType).dtor_value)) {
            RAST._IType _7_asType;
            _7_asType = _5_boundedToType;
            if (object.Equals(_6_inOwnership, Defs.Ownership.create_OwnershipBorrowed())) {
              RAST._IExpr _source0 = r;
              {
                if (_source0.is_UnaryOp) {
                  Dafny.ISequence<Dafny.Rune> op10 = _source0.dtor_op1;
                  if (object.Equals(op10, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"))) {
                    RAST._IExpr _8_underlying = _source0.dtor_underlying;
                    r = _8_underlying;
                    goto after_match0;
                  }
                }
              }
              {
                r = (r).Clone();
              }
            after_match0: ;
            }
            r = RAST.Expr.create_TypeAscription(r, _7_asType);
            _6_inOwnership = Defs.Ownership.create_OwnershipOwned();
          }
          RAST._IExpr _out11;
          _out11 = (this).WrapWithNewtype(r, Defs.Ownership.create_OwnershipOwned(), _1_toTpe);
          r = _out11;
          RAST._IExpr _out12;
          Defs._IOwnership _out13;
          (this).FromOwnership(r, _6_inOwnership, expectedOwnership, out _out12, out _out13);
          r = _out12;
          resultingOwnership = _out13;
          return ;
        }
        if ((_0_fromTpe).IsPrimitiveInt()) {
          if (object.Equals(exprOwnership, Defs.Ownership.create_OwnershipBorrowed())) {
            r = (r).Clone();
          }
          r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("truncate!"))).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(r, RAST.Expr.create_ExprFromType(_5_boundedToType)));
          RAST._IExpr _out14;
          _out14 = (this).WrapWithNewtype(r, Defs.Ownership.create_OwnershipOwned(), _1_toTpe);
          r = _out14;
          RAST._IExpr _out15;
          Defs._IOwnership _out16;
          (this).FromOwned(r, expectedOwnership, out _out15, out _out16);
          r = _out15;
          resultingOwnership = _out16;
          return ;
        }
        if (object.Equals(_0_fromTpe, DAST.Type.create_Primitive(DAST.Primitive.create_Char()))) {
          r = RAST.Expr.create_TypeAscription((r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")), _5_boundedToType);
          RAST._IExpr _out17;
          _out17 = (this).WrapWithNewtype(r, Defs.Ownership.create_OwnershipOwned(), _1_toTpe);
          r = _out17;
          RAST._IExpr _out18;
          Defs._IOwnership _out19;
          (this).FromOwned(r, expectedOwnership, out _out18, out _out19);
          r = _out18;
          resultingOwnership = _out19;
          return ;
        }
        RAST._IType _9_fromTpeRust;
        RAST._IType _out20;
        _out20 = (this).GenType(_0_fromTpe, Defs.GenTypeContext.@default());
        _9_fromTpeRust = _out20;
        RAST._IExpr _out21;
        _out21 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("No conversion available from "), (_9_fromTpeRust)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" to ")), (_5_boundedToType)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), (this).InitEmptyExpr());
        r = _out21;
        RAST._IExpr _out22;
        Defs._IOwnership _out23;
        (this).FromOwned(r, expectedOwnership, out _out22, out _out23);
        r = _out22;
        resultingOwnership = _out23;
        return ;
      }
      if ((_3_unwrappedFromType).is_Some) {
        if (!((((_0_fromTpe).dtor_resolved).dtor_kind).dtor_erase)) {
          r = (r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0"));
        }
        if (object.Equals(_1_toTpe, DAST.Type.create_Primitive(DAST.Primitive.create_Char()))) {
          RAST._IExpr _out24;
          Defs._IOwnership _out25;
          (this).FromOwnership((((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsExpr()).Apply1(RAST.Expr.create_TypeAscription(r, (this).DafnyCharUnderlying)), exprOwnership, expectedOwnership, out _out24, out _out25);
          r = _out24;
          resultingOwnership = _out25;
          return ;
        }
        if ((_1_toTpe).IsPrimitiveInt()) {
          RAST._IExpr _out26;
          Defs._IOwnership _out27;
          (this).FromOwnership(r, exprOwnership, Defs.Ownership.create_OwnershipOwned(), out _out26, out _out27);
          r = _out26;
          resultingOwnership = _out27;
          r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1(r);
          RAST._IExpr _out28;
          Defs._IOwnership _out29;
          (this).FromOwned(r, expectedOwnership, out _out28, out _out29);
          r = _out28;
          resultingOwnership = _out29;
          return ;
        }
        RAST._IType _10_toTpeRust;
        RAST._IType _out30;
        _out30 = (this).GenType(_1_toTpe, Defs.GenTypeContext.@default());
        _10_toTpeRust = _out30;
        RAST._IExpr _out31;
        _out31 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("No conversion available from "), ((_3_unwrappedFromType).dtor_value)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" to ")), (_10_toTpeRust)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), (this).InitEmptyExpr());
        r = _out31;
        RAST._IExpr _out32;
        Defs._IOwnership _out33;
        (this).FromOwned(r, expectedOwnership, out _out32, out _out33);
        r = _out32;
        resultingOwnership = _out33;
        return ;
      }
      _System._ITuple2<DAST._IType, DAST._IType> _source1 = _System.Tuple2<DAST._IType, DAST._IType>.create(_0_fromTpe, _1_toTpe);
      {
        DAST._IType _00 = _source1.dtor__0;
        if (_00.is_Primitive) {
          DAST._IPrimitive _h70 = _00.dtor_Primitive_a0;
          if (_h70.is_Int) {
            DAST._IType _10 = _source1.dtor__1;
            if (_10.is_Primitive) {
              DAST._IPrimitive _h71 = _10.dtor_Primitive_a0;
              if (_h71.is_Real) {
                {
                  r = RAST.__default.RcNew(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("BigRational"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_integer"))).Apply1(r));
                  RAST._IExpr _out34;
                  Defs._IOwnership _out35;
                  (this).FromOwned(r, expectedOwnership, out _out34, out _out35);
                  r = _out34;
                  resultingOwnership = _out35;
                }
                goto after_match1;
              }
            }
          }
        }
      }
      {
        DAST._IType _01 = _source1.dtor__0;
        if (_01.is_Primitive) {
          DAST._IPrimitive _h72 = _01.dtor_Primitive_a0;
          if (_h72.is_Real) {
            DAST._IType _11 = _source1.dtor__1;
            if (_11.is_Primitive) {
              DAST._IPrimitive _h73 = _11.dtor_Primitive_a0;
              if (_h73.is_Int) {
                {
                  r = (((RAST.__default.dafny__runtime).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_rational_to_int"))).Apply1(r);
                  RAST._IExpr _out36;
                  Defs._IOwnership _out37;
                  (this).FromOwned(r, expectedOwnership, out _out36, out _out37);
                  r = _out36;
                  resultingOwnership = _out37;
                }
                goto after_match1;
              }
            }
          }
        }
      }
      {
        DAST._IType _02 = _source1.dtor__0;
        if (_02.is_Primitive) {
          DAST._IPrimitive _h74 = _02.dtor_Primitive_a0;
          if (_h74.is_Int) {
            DAST._IType _12 = _source1.dtor__1;
            if (_12.is_Primitive) {
              DAST._IPrimitive _h75 = _12.dtor_Primitive_a0;
              if (_h75.is_Char) {
                {
                  RAST._IType _11_rhsType;
                  RAST._IType _out38;
                  _out38 = (this).GenType(_1_toTpe, Defs.GenTypeContext.@default());
                  _11_rhsType = _out38;
                  RAST._IType _12_uType;
                  if (((this).charType).is_UTF32) {
                    _12_uType = RAST.Type.create_U32();
                  } else {
                    _12_uType = RAST.Type.create_U16();
                  }
                  if (!object.Equals(exprOwnership, Defs.Ownership.create_OwnershipOwned())) {
                    r = (r).Clone();
                  }
                  r = ((((RAST.Expr.create_TraitCast(_12_uType, ((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("NumCast"))).AsType())).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from"))).Apply1(r)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unwrap"))).Apply0();
                  if (((this).charType).is_UTF32) {
                    r = ((((RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("char"))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_u32"))).Apply1(r)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unwrap"))).Apply0();
                  }
                  r = (((RAST.__default.dafny__runtime).MSel((this).DafnyChar)).AsExpr()).Apply1(r);
                  RAST._IExpr _out39;
                  Defs._IOwnership _out40;
                  (this).FromOwned(r, expectedOwnership, out _out39, out _out40);
                  r = _out39;
                  resultingOwnership = _out40;
                }
                goto after_match1;
              }
            }
          }
        }
      }
      {
        DAST._IType _03 = _source1.dtor__0;
        if (_03.is_Primitive) {
          DAST._IPrimitive _h76 = _03.dtor_Primitive_a0;
          if (_h76.is_Char) {
            DAST._IType _13 = _source1.dtor__1;
            if (_13.is_Primitive) {
              DAST._IPrimitive _h77 = _13.dtor_Primitive_a0;
              if (_h77.is_Int) {
                {
                  RAST._IType _13_rhsType;
                  RAST._IType _out41;
                  _out41 = (this).GenType(_0_fromTpe, Defs.GenTypeContext.@default());
                  _13_rhsType = _out41;
                  r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1((r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("0")));
                  RAST._IExpr _out42;
                  Defs._IOwnership _out43;
                  (this).FromOwned(r, expectedOwnership, out _out42, out _out43);
                  r = _out42;
                  resultingOwnership = _out43;
                }
                goto after_match1;
              }
            }
          }
        }
      }
      {
        {
          RAST._IExpr _out44;
          Defs._IOwnership _out45;
          (this).GenExprConvertOther(expr, exprOwnership, _0_fromTpe, _1_toTpe, env, expectedOwnership, out _out44, out _out45);
          r = _out44;
          resultingOwnership = _out45;
        }
      }
    after_match1: ;
    }
    public bool IsBuiltinCollection(DAST._IType typ) {
      return ((((typ).is_Seq) || ((typ).is_Set)) || ((typ).is_Map)) || ((typ).is_Multiset);
    }
    public DAST._IType GetBuiltinCollectionElement(DAST._IType typ) {
      if ((typ).is_Map) {
        return (typ).dtor_value;
      } else {
        return (typ).dtor_element;
      }
    }
    public bool SameTypesButDifferentTypeParameters(DAST._IType fromType, RAST._IType fromTpe, DAST._IType toType, RAST._IType toTpe)
    {
      return (((((((fromTpe).is_TypeApp) && ((toTpe).is_TypeApp)) && (object.Equals((fromTpe).dtor_baseName, (toTpe).dtor_baseName))) && ((fromType).is_UserDefined)) && ((toType).is_UserDefined)) && ((this).IsSameResolvedTypeAnyArgs((fromType).dtor_resolved, (toType).dtor_resolved))) && ((((new BigInteger((((fromType).dtor_resolved).dtor_typeArgs).Count)) == (new BigInteger((((toType).dtor_resolved).dtor_typeArgs).Count))) && ((new BigInteger((((toType).dtor_resolved).dtor_typeArgs).Count)) == (new BigInteger(((fromTpe).dtor_arguments).Count)))) && ((new BigInteger(((fromTpe).dtor_arguments).Count)) == (new BigInteger(((toTpe).dtor_arguments).Count))));
    }
    public Std.Wrappers._IResult<Dafny.ISequence<__T>, __E> SeqResultToResultSeq<__T, __E>(Dafny.ISequence<Std.Wrappers._IResult<__T, __E>> xs) {
      if ((new BigInteger((xs).Count)).Sign == 0) {
        return Std.Wrappers.Result<Dafny.ISequence<__T>, __E>.create_Success(Dafny.Sequence<__T>.FromElements());
      } else {
        Std.Wrappers._IResult<__T, __E> _0_valueOrError0 = (xs).Select(BigInteger.Zero);
        if ((_0_valueOrError0).IsFailure()) {
          return (_0_valueOrError0).PropagateFailure<Dafny.ISequence<__T>>();
        } else {
          __T _1_head = (_0_valueOrError0).Extract();
          Std.Wrappers._IResult<Dafny.ISequence<__T>, __E> _2_valueOrError1 = (this).SeqResultToResultSeq<__T, __E>((xs).Drop(BigInteger.One));
          if ((_2_valueOrError1).IsFailure()) {
            return (_2_valueOrError1).PropagateFailure<Dafny.ISequence<__T>>();
          } else {
            Dafny.ISequence<__T> _3_tail = (_2_valueOrError1).Extract();
            return Std.Wrappers.Result<Dafny.ISequence<__T>, __E>.create_Success(Dafny.Sequence<__T>.Concat(Dafny.Sequence<__T>.FromElements(_1_head), _3_tail));
          }
        }
      }
    }
    public Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> UpcastConversionLambda(DAST._IType fromType, RAST._IType fromTpe, DAST._IType toType, RAST._IType toTpe, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr> typeParams)
    {
      var _pat_let_tv0 = fromType;
      var _pat_let_tv1 = fromTpe;
      var _pat_let_tv2 = toType;
      var _pat_let_tv3 = toTpe;
      var _pat_let_tv4 = typeParams;
      if (object.Equals(fromTpe, toTpe)) {
        return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("upcast_id"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(fromTpe))).Apply0());
      } else if ((toTpe).IsBox()) {
        RAST._IType _0_toTpeUnderlying = (toTpe).BoxUnderlying();
        if (!((_0_toTpeUnderlying).is_DynType)) {
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Failure(_System.Tuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>.create(fromType, fromTpe, toType, toTpe, typeParams));
        } else if ((fromTpe).IsBox()) {
          RAST._IType _1_fromTpeUnderlying = (fromTpe).BoxUnderlying();
          if (!((_1_fromTpeUnderlying).is_DynType)) {
            return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Failure(_System.Tuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>.create(fromType, fromTpe, toType, toTpe, typeParams));
          } else {
            return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("upcast_box_box"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_1_fromTpeUnderlying, _0_toTpeUnderlying))).Apply0());
          }
        } else {
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("upcast_box"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(fromTpe, _0_toTpeUnderlying))).Apply0());
        }
      } else if (((fromTpe).IsObjectOrPointer()) && ((toTpe).IsObjectOrPointer())) {
        RAST._IType _2_toTpeUnderlying = (toTpe).ObjectOrPointerUnderlying();
        if (!((_2_toTpeUnderlying).is_DynType)) {
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Failure(_System.Tuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>.create(fromType, fromTpe, toType, toTpe, typeParams));
        } else {
          RAST._IType _3_fromTpeUnderlying = (fromTpe).ObjectOrPointerUnderlying();
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(((((RAST.__default.dafny__runtime).MSel((this).upcast)).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_3_fromTpeUnderlying, _2_toTpeUnderlying))).Apply0());
        }
      } else if ((typeParams).Contains(_System.Tuple2<RAST._IType, RAST._IType>.create(fromTpe, toTpe))) {
        return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(Dafny.Map<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>.Select(typeParams,_System.Tuple2<RAST._IType, RAST._IType>.create(fromTpe, toTpe)));
      } else if (((fromTpe).IsRc()) && ((toTpe).IsRc())) {
        Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _4_valueOrError0 = (this).UpcastConversionLambda(fromType, (fromTpe).RcUnderlying(), toType, (toTpe).RcUnderlying(), typeParams);
        if ((_4_valueOrError0).IsFailure()) {
          return (_4_valueOrError0).PropagateFailure<RAST._IExpr>();
        } else {
          RAST._IExpr _5_lambda = (_4_valueOrError0).Extract();
          if ((fromType).is_Arrow) {
            return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(_5_lambda);
          } else {
            return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("rc_coerce"))).AsExpr()).Apply1(_5_lambda));
          }
        }
      } else if ((this).SameTypesButDifferentTypeParameters(fromType, fromTpe, toType, toTpe)) {
        Dafny.ISequence<BigInteger> _6_indices = ((((fromType).is_UserDefined) && ((((fromType).dtor_resolved).dtor_kind).is_Datatype)) ? (Std.Collections.Seq.__default.Filter<BigInteger>(Dafny.Helpers.Id<Func<RAST._IType, DAST._IType, Func<BigInteger, bool>>>((_7_fromTpe, _8_fromType) => ((System.Func<BigInteger, bool>)((_9_i) => {
          return ((((_9_i).Sign != -1) && ((_9_i) < (new BigInteger(((_7_fromTpe).dtor_arguments).Count)))) ? (!(((_9_i).Sign != -1) && ((_9_i) < (new BigInteger(((((_8_fromType).dtor_resolved).dtor_kind).dtor_info).Count)))) || (!(((((((_8_fromType).dtor_resolved).dtor_kind).dtor_info).Select(_9_i)).dtor_variance).is_Nonvariant))) : (false));
        })))(fromTpe, fromType), ((System.Func<Dafny.ISequence<BigInteger>>) (() => {
          BigInteger dim15 = new BigInteger(((fromTpe).dtor_arguments).Count);
          var arr15 = new BigInteger[Dafny.Helpers.ToIntChecked(dim15, "array size exceeds memory limit")];
          for (int i15 = 0; i15 < dim15; i15++) {
            var _10_i = (BigInteger) i15;
            arr15[(int)(_10_i)] = _10_i;
          }
          return Dafny.Sequence<BigInteger>.FromArray(arr15);
        }))())) : (((System.Func<Dafny.ISequence<BigInteger>>) (() => {
          BigInteger dim16 = new BigInteger(((fromTpe).dtor_arguments).Count);
          var arr16 = new BigInteger[Dafny.Helpers.ToIntChecked(dim16, "array size exceeds memory limit")];
          for (int i16 = 0; i16 < dim16; i16++) {
            var _11_i = (BigInteger) i16;
            arr16[(int)(_11_i)] = _11_i;
          }
          return Dafny.Sequence<BigInteger>.FromArray(arr16);
        }))()));
        Std.Wrappers._IResult<Dafny.ISequence<RAST._IExpr>, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _12_valueOrError1 = (this).SeqResultToResultSeq<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>(((System.Func<Dafny.ISequence<Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>>>) (() => {
          BigInteger dim17 = new BigInteger((_6_indices).Count);
          var arr17 = new Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>[Dafny.Helpers.ToIntChecked(dim17, "array size exceeds memory limit")];
          for (int i17 = 0; i17 < dim17; i17++) {
            var _13_j = (BigInteger) i17;
            arr17[(int)(_13_j)] = Dafny.Helpers.Let<BigInteger, Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>>((_6_indices).Select(_13_j), _pat_let27_0 => Dafny.Helpers.Let<BigInteger, Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>>(_pat_let27_0, _14_i => (this).UpcastConversionLambda((((_pat_let_tv0).dtor_resolved).dtor_typeArgs).Select(_14_i), ((_pat_let_tv1).dtor_arguments).Select(_14_i), (((_pat_let_tv2).dtor_resolved).dtor_typeArgs).Select(_14_i), ((_pat_let_tv3).dtor_arguments).Select(_14_i), _pat_let_tv4)));
          }
          return Dafny.Sequence<Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>>.FromArray(arr17);
        }))());
        if ((_12_valueOrError1).IsFailure()) {
          return (_12_valueOrError1).PropagateFailure<RAST._IExpr>();
        } else {
          Dafny.ISequence<RAST._IExpr> _15_lambdas = (_12_valueOrError1).Extract();
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success((((RAST.Expr.create_ExprFromType((fromTpe).dtor_baseName)).ApplyType(((System.Func<Dafny.ISequence<RAST._IType>>) (() => {
  BigInteger dim18 = new BigInteger(((fromTpe).dtor_arguments).Count);
  var arr18 = new RAST._IType[Dafny.Helpers.ToIntChecked(dim18, "array size exceeds memory limit")];
  for (int i18 = 0; i18 < dim18; i18++) {
    var _16_i = (BigInteger) i18;
    arr18[(int)(_16_i)] = ((fromTpe).dtor_arguments).Select(_16_i);
  }
  return Dafny.Sequence<RAST._IType>.FromArray(arr18);
}))())).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("coerce"))).Apply(_15_lambdas));
        }
      } else if (((((fromTpe).IsBuiltinCollection()) && ((toTpe).IsBuiltinCollection())) && ((this).IsBuiltinCollection(fromType))) && ((this).IsBuiltinCollection(toType))) {
        RAST._IType _17_newFromTpe = (fromTpe).GetBuiltinCollectionElement();
        RAST._IType _18_newToTpe = (toTpe).GetBuiltinCollectionElement();
        DAST._IType _19_newFromType = (this).GetBuiltinCollectionElement(fromType);
        DAST._IType _20_newToType = (this).GetBuiltinCollectionElement(toType);
        Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _21_valueOrError2 = (this).UpcastConversionLambda(_19_newFromType, _17_newFromTpe, _20_newToType, _18_newToTpe, typeParams);
        if ((_21_valueOrError2).IsFailure()) {
          return (_21_valueOrError2).PropagateFailure<RAST._IExpr>();
        } else {
          RAST._IExpr _22_coerceArg = (_21_valueOrError2).Extract();
          RAST._IPath _23_collectionType = (RAST.__default.dafny__runtime).MSel(((((fromTpe).Expand()).dtor_baseName).dtor_path).dtor_name);
          RAST._IExpr _24_baseType = (((((((fromTpe).Expand()).dtor_baseName).dtor_path).dtor_name).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Map"))) ? (((_23_collectionType).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements((((fromTpe).Expand()).dtor_arguments).Select(BigInteger.Zero), _17_newFromTpe))) : (((_23_collectionType).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_17_newFromTpe))));
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(((_24_baseType).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("coerce"))).Apply1(_22_coerceArg));
        }
      } else if ((((((((((fromTpe).is_DynType) && (((fromTpe).dtor_underlying).is_FnType)) && ((toTpe).is_DynType)) && (((toTpe).dtor_underlying).is_FnType)) && ((((fromTpe).dtor_underlying).dtor_arguments).Equals(((toTpe).dtor_underlying).dtor_arguments))) && ((fromType).is_Arrow)) && ((toType).is_Arrow)) && ((new BigInteger((((fromTpe).dtor_underlying).dtor_arguments).Count)) == (BigInteger.One))) && (((((fromTpe).dtor_underlying).dtor_arguments).Select(BigInteger.Zero)).is_Borrowed)) {
        Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _25_valueOrError3 = (this).UpcastConversionLambda((fromType).dtor_result, ((fromTpe).dtor_underlying).dtor_returnType, (toType).dtor_result, ((toTpe).dtor_underlying).dtor_returnType, typeParams);
        if ((_25_valueOrError3).IsFailure()) {
          return (_25_valueOrError3).PropagateFailure<RAST._IExpr>();
        } else {
          RAST._IExpr _26_lambda = (_25_valueOrError3).Extract();
          return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Success(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("fn1_coerce"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(((((fromTpe).dtor_underlying).dtor_arguments).Select(BigInteger.Zero)).dtor_underlying, ((fromTpe).dtor_underlying).dtor_returnType, ((toTpe).dtor_underlying).dtor_returnType))).Apply1(_26_lambda));
        }
      } else {
        return Std.Wrappers.Result<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>>.create_Failure(_System.Tuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>.create(fromType, fromTpe, toType, toTpe, typeParams));
      }
    }
    public bool IsDowncastConversion(RAST._IType fromTpe, RAST._IType toTpe)
    {
      if (((fromTpe).IsObjectOrPointer()) && ((toTpe).IsObjectOrPointer())) {
        return (((fromTpe).ObjectOrPointerUnderlying()).is_DynType) && (!(((toTpe).ObjectOrPointerUnderlying()).is_DynType));
      } else {
        return false;
      }
    }
    public RAST._IExpr BorrowedToOwned(RAST._IExpr expr, Defs._IEnvironment env)
    {
      RAST._IExpr _source0 = expr;
      {
        if (_source0.is_UnaryOp) {
          Dafny.ISequence<Dafny.Rune> op10 = _source0.dtor_op1;
          if (object.Equals(op10, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"))) {
            RAST._IExpr _0_underlying = _source0.dtor_underlying;
            if (((_0_underlying).is_Identifier) && ((env).CanReadWithoutClone((_0_underlying).dtor_name))) {
              return _0_underlying;
            } else {
              return (_0_underlying).Clone();
            }
          }
        }
      }
      {
        return (expr).Clone();
      }
    }
    public void GenExprConvertOther(RAST._IExpr expr, Defs._IOwnership exprOwnership, DAST._IType fromTyp, DAST._IType toTyp, Defs._IEnvironment env, Defs._IOwnership expectedOwnership, out RAST._IExpr r, out Defs._IOwnership resultingOwnership)
    {
      r = RAST.Expr.Default();
      resultingOwnership = Defs.Ownership.Default();
      r = expr;
      RAST._IType _0_fromTpeGen;
      RAST._IType _out0;
      _out0 = (this).GenType(fromTyp, Defs.GenTypeContext.@default());
      _0_fromTpeGen = _out0;
      RAST._IType _1_toTpeGen;
      RAST._IType _out1;
      _out1 = (this).GenType(toTyp, Defs.GenTypeContext.@default());
      _1_toTpeGen = _out1;
      bool _2_isDatatype;
      _2_isDatatype = (toTyp).IsDatatype();
      bool _3_isGeneralTrait;
      _3_isGeneralTrait = (!(_2_isDatatype)) && ((toTyp).IsGeneralTrait());
      if ((_2_isDatatype) || (_3_isGeneralTrait)) {
        bool _4_isDowncast;
        _4_isDowncast = (toTyp).Extends(fromTyp);
        if (_4_isDowncast) {
          DAST._IType _5_underlyingType;
          if (_2_isDatatype) {
            _5_underlyingType = (toTyp).GetDatatypeType();
          } else {
            _5_underlyingType = (toTyp).GetGeneralTraitType();
          }
          RAST._IType _6_toTpeRaw;
          RAST._IType _out2;
          _out2 = (this).GenType(_5_underlyingType, Defs.GenTypeContext.@default());
          _6_toTpeRaw = _out2;
          Std.Wrappers._IOption<RAST._IExpr> _7_toTpeRawDowncastOpt;
          _7_toTpeRawDowncastOpt = (_6_toTpeRaw).ToDowncastExpr();
          if ((_7_toTpeRawDowncastOpt).is_Some) {
            RAST._IExpr _8_newExpr;
            _8_newExpr = expr;
            if ((exprOwnership).is_OwnershipOwned) {
              RAST._IExpr _source0 = _8_newExpr;
              {
                if (_source0.is_Call) {
                  RAST._IExpr obj0 = _source0.dtor_obj;
                  if (obj0.is_Select) {
                    RAST._IExpr obj1 = obj0.dtor_obj;
                    if (obj1.is_Identifier) {
                      Dafny.ISequence<Dafny.Rune> _9_name = obj1.dtor_name;
                      Dafny.ISequence<Dafny.Rune> name0 = obj0.dtor_name;
                      if (object.Equals(name0, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("clone"))) {
                        Dafny.ISequence<RAST._IExpr> _10_arguments = _source0.dtor_arguments;
                        if ((new BigInteger((_10_arguments).Count)).Sign == 0) {
                          _8_newExpr = RAST.Expr.create_Identifier(_9_name);
                          if (!((env).IsBorrowed(_9_name))) {
                            _8_newExpr = RAST.__default.Borrow(_8_newExpr);
                          }
                        } else {
                          RAST._IExpr _out3;
                          Defs._IOwnership _out4;
                          (this).FromOwnership(_8_newExpr, Defs.Ownership.create_OwnershipOwned(), Defs.Ownership.create_OwnershipBorrowed(), out _out3, out _out4);
                          _8_newExpr = _out3;
                          resultingOwnership = _out4;
                        }
                        goto after_match0;
                      }
                    }
                  }
                }
              }
              {
                {
                  RAST._IExpr _out5;
                  Defs._IOwnership _out6;
                  (this).FromOwnership(_8_newExpr, Defs.Ownership.create_OwnershipOwned(), Defs.Ownership.create_OwnershipBorrowed(), out _out5, out _out6);
                  _8_newExpr = _out5;
                  resultingOwnership = _out6;
                }
              }
            after_match0: ;
            }
            _8_newExpr = (this).FromGeneralBorrowToSelfBorrow(_8_newExpr, Defs.Ownership.create_OwnershipBorrowed(), env);
            if (_2_isDatatype) {
              _8_newExpr = ((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("AnyRef"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_any_ref"))).Apply1(_8_newExpr);
            }
            r = (((_7_toTpeRawDowncastOpt).dtor_value).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_as"))).Apply1(_8_newExpr);
            RAST._IExpr _out7;
            Defs._IOwnership _out8;
            (this).FromOwnership(r, Defs.Ownership.create_OwnershipOwned(), expectedOwnership, out _out7, out _out8);
            r = _out7;
            resultingOwnership = _out8;
            return ;
          } else {
            RAST._IExpr _out9;
            _out9 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not convert "), (_6_toTpeRaw)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" to a Downcast trait")), (this).InitEmptyExpr());
            r = _out9;
            RAST._IExpr _out10;
            Defs._IOwnership _out11;
            (this).FromOwned(r, expectedOwnership, out _out10, out _out11);
            r = _out10;
            resultingOwnership = _out11;
            return ;
          }
        }
      } else {
        RAST._IExpr _out12;
        _out12 = (this).Error(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Source and/or target types of type test is/are not Object, Ptr, General trait or Datatype"), (this).InitEmptyExpr());
        r = _out12;
        RAST._IExpr _out13;
        Defs._IOwnership _out14;
        (this).FromOwned(r, expectedOwnership, out _out13, out _out14);
        r = _out13;
        resultingOwnership = _out14;
        return ;
      }
      Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _11_upcastConverter;
      _11_upcastConverter = (this).UpcastConversionLambda(fromTyp, _0_fromTpeGen, toTyp, _1_toTpeGen, Dafny.Map<_System._ITuple2<RAST._IType, RAST._IType>, RAST._IExpr>.FromElements());
      if ((_11_upcastConverter).is_Success) {
        RAST._IExpr _12_conversionLambda;
        _12_conversionLambda = (_11_upcastConverter).dtor_value;
        if (object.Equals(exprOwnership, Defs.Ownership.create_OwnershipBorrowed())) {
          if (((fromTyp).IsGeneralTrait()) && (object.Equals(r, RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))))) {
            RAST._IType _13_traitType;
            RAST._IType _out15;
            _out15 = (this).GenType(fromTyp, Defs.GenTypeContext.ForTraitParents());
            _13_traitType = _out15;
            Std.Wrappers._IOption<RAST._IExpr> _14_traitExpr;
            _14_traitExpr = (_13_traitType).ToExpr();
            if ((_14_traitExpr).is_None) {
              RAST._IExpr _out16;
              _out16 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not convert "), (_13_traitType)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" to an expression")), (this).InitEmptyExpr());
              r = _out16;
            } else {
              r = (((_14_traitExpr).dtor_value).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_clone"))).Apply1(r);
            }
          } else {
            r = (this).BorrowedToOwned(r, env);
          }
        }
        r = (_12_conversionLambda).Apply1(r);
        RAST._IExpr _out17;
        Defs._IOwnership _out18;
        (this).FromOwnership(r, Defs.Ownership.create_OwnershipOwned(), expectedOwnership, out _out17, out _out18);
        r = _out17;
        resultingOwnership = _out18;
      } else if ((this).IsDowncastConversion(_0_fromTpeGen, _1_toTpeGen)) {
        _1_toTpeGen = (_1_toTpeGen).ObjectOrPointerUnderlying();
        r = (((RAST.__default.dafny__runtime).MSel((this).downcast)).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(r, RAST.Expr.create_ExprFromType(_1_toTpeGen)));
        RAST._IExpr _out19;
        Defs._IOwnership _out20;
        (this).FromOwnership(r, Defs.Ownership.create_OwnershipOwned(), expectedOwnership, out _out19, out _out20);
        r = _out19;
        resultingOwnership = _out20;
      } else {
        Std.Wrappers._IResult<RAST._IExpr, _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>>> _let_tmp_rhs0 = _11_upcastConverter;
        _System._ITuple5<DAST._IType, RAST._IType, DAST._IType, RAST._IType, Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr>> _let_tmp_rhs1 = _let_tmp_rhs0.dtor_error;
        DAST._IType _15_fromType = _let_tmp_rhs1.dtor__0;
        RAST._IType _16_fromTpeGen = _let_tmp_rhs1.dtor__1;
        DAST._IType _17_toType = _let_tmp_rhs1.dtor__2;
        RAST._IType _18_toTpeGen = _let_tmp_rhs1.dtor__3;
        Dafny.IMap<_System._ITuple2<RAST._IType, RAST._IType>,RAST._IExpr> _19_m = _let_tmp_rhs1.dtor__4;
        RAST._IExpr _out21;
        _out21 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<i>Coercion from "), (_16_fromTpeGen)._ToString(Defs.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" to ")), (_18_toTpeGen)._ToString(Defs.__default.IND)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("</i> not yet implemented")), r);
        r = _out21;
        RAST._IExpr _out22;
        Defs._IOwnership _out23;
        (this).FromOwned(r, expectedOwnership, out _out22, out _out23);
        r = _out22;
        resultingOwnership = _out23;
      }
    }
    public void GenExprConvert(DAST._IExpression e, Defs._ISelfInfo selfIdent, Defs._IEnvironment env, Defs._IOwnership expectedOwnership, out RAST._IExpr r, out Defs._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = Defs.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _let_tmp_rhs0 = e;
      DAST._IExpression _0_expr = _let_tmp_rhs0.dtor_value;
      DAST._IType _1_fromTpe = _let_tmp_rhs0.dtor_from;
      DAST._IType _2_toTpe = _let_tmp_rhs0.dtor_typ;
      Defs._IOwnership _3_argumentOwnership;
      _3_argumentOwnership = expectedOwnership;
      if (object.Equals(_3_argumentOwnership, Defs.Ownership.create_OwnershipAutoBorrowed())) {
        _3_argumentOwnership = Defs.Ownership.create_OwnershipBorrowed();
      }
      RAST._IExpr _4_recursiveGen;
      Defs._IOwnership _5_recOwned;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _6_recIdents;
      RAST._IExpr _out0;
      Defs._IOwnership _out1;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out2;
      (this).GenExpr(_0_expr, selfIdent, env, _3_argumentOwnership, out _out0, out _out1, out _out2);
      _4_recursiveGen = _out0;
      _5_recOwned = _out1;
      _6_recIdents = _out2;
      r = _4_recursiveGen;
      readIdents = _6_recIdents;
      RAST._IExpr _out3;
      Defs._IOwnership _out4;
      (this).GenExprConvertTo(r, _5_recOwned, _1_fromTpe, _2_toTpe, env, expectedOwnership, out _out3, out _out4);
      r = _out3;
      resultingOwnership = _out4;
      return ;
    }
    public void GenIdent(Dafny.ISequence<Dafny.Rune> rName, Defs._ISelfInfo selfIdent, Defs._IEnvironment env, Defs._IOwnership expectedOwnership, out RAST._IExpr r, out Defs._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = Defs.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      r = RAST.Expr.create_Identifier(rName);
      Std.Wrappers._IOption<RAST._IType> _0_tpe;
      _0_tpe = (env).GetType(rName);
      Std.Wrappers._IOption<RAST._IType> _1_placeboOpt;
      if ((_0_tpe).is_Some) {
        _1_placeboOpt = ((_0_tpe).dtor_value).ExtractMaybePlacebo();
      } else {
        _1_placeboOpt = Std.Wrappers.Option<RAST._IType>.create_None();
      }
      bool _2_currentlyBorrowed;
      _2_currentlyBorrowed = (env).IsBorrowed(rName);
      bool _3_noNeedOfClone;
      _3_noNeedOfClone = (env).CanReadWithoutClone(rName);
      bool _4_isSelf;
      _4_isSelf = (((selfIdent).is_ThisTyped) && ((selfIdent).IsSelf())) && (((selfIdent).dtor_rSelfName).Equals(rName));
      if ((_1_placeboOpt).is_Some) {
        r = ((r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("read"))).Apply0();
        _2_currentlyBorrowed = false;
        _3_noNeedOfClone = true;
        _0_tpe = Std.Wrappers.Option<RAST._IType>.create_Some((_1_placeboOpt).dtor_value);
      }
      if (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipAutoBorrowed())) {
        if (_2_currentlyBorrowed) {
          resultingOwnership = Defs.Ownership.create_OwnershipBorrowed();
        } else {
          resultingOwnership = Defs.Ownership.create_OwnershipOwned();
        }
      } else if (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipBorrowedMut())) {
        if ((rName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))) {
          resultingOwnership = Defs.Ownership.create_OwnershipBorrowedMut();
        } else {
          if (((_0_tpe).is_Some) && (((_0_tpe).dtor_value).IsObjectOrPointer())) {
            r = ((this).modify__macro).Apply1(r);
          } else {
            r = RAST.__default.BorrowMut(r);
          }
        }
        resultingOwnership = Defs.Ownership.create_OwnershipBorrowedMut();
      } else if (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipOwned())) {
        bool _5_needsObjectFromRef;
        _5_needsObjectFromRef = (_4_isSelf) && ((selfIdent).IsClassOrObjectTrait());
        bool _6_needsRcWrapping;
        _6_needsRcWrapping = (_4_isSelf) && ((selfIdent).IsRcWrappedDatatype());
        if (_5_needsObjectFromRef) {
          r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Object"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_TIdentifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"))))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_ref"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(r));
        } else if (_6_needsRcWrapping) {
          r = RAST.__default.RcNew((r).Clone());
        } else {
          if (!(_3_noNeedOfClone)) {
            bool _7_needUnderscoreClone;
            _7_needUnderscoreClone = (_4_isSelf) && ((selfIdent).IsGeneralTrait());
            if (_7_needUnderscoreClone) {
              RAST._IType _8_traitType;
              RAST._IType _out0;
              _out0 = (this).GenType((selfIdent).dtor_dafnyType, Defs.GenTypeContext.ForTraitParents());
              _8_traitType = _out0;
              Std.Wrappers._IOption<RAST._IExpr> _9_traitExpr;
              _9_traitExpr = (_8_traitType).ToExpr();
              if ((_9_traitExpr).is_None) {
                RAST._IExpr _out1;
                _out1 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not convert "), (_8_traitType)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" to an expression")), (this).InitEmptyExpr());
                r = _out1;
              } else {
                r = (((_9_traitExpr).dtor_value).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_clone"))).Apply1(r);
              }
            } else {
              r = (r).Clone();
            }
          }
        }
        resultingOwnership = Defs.Ownership.create_OwnershipOwned();
      } else if (_2_currentlyBorrowed) {
        resultingOwnership = Defs.Ownership.create_OwnershipBorrowed();
      } else {
        bool _10_selfIsGeneralTrait;
        _10_selfIsGeneralTrait = (_4_isSelf) && (((System.Func<bool>)(() => {
          DAST._IType _source0 = (selfIdent).dtor_dafnyType;
          {
            if (_source0.is_UserDefined) {
              DAST._IResolvedType resolved0 = _source0.dtor_resolved;
              DAST._IResolvedTypeBase _11_base = resolved0.dtor_kind;
              Dafny.ISequence<DAST._IAttribute> _12_attributes = resolved0.dtor_attributes;
              return ((_11_base).is_Trait) && (((_11_base).dtor_traitType).is_GeneralTrait);
            }
          }
          {
            return false;
          }
        }))());
        if (!(rName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"))) {
          if (((_0_tpe).is_Some) && (((_0_tpe).dtor_value).IsPointer())) {
            r = ((this).read__macro).Apply1(r);
          } else {
            r = RAST.__default.Borrow(r);
          }
        }
        resultingOwnership = Defs.Ownership.create_OwnershipBorrowed();
      }
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(rName);
      return ;
    }
    public bool HasExternAttributeRenamingModule(Dafny.ISequence<DAST._IAttribute> attributes) {
      return Dafny.Helpers.Id<Func<Dafny.ISequence<DAST._IAttribute>, bool>>((_0_attributes) => Dafny.Helpers.Quantifier<DAST._IAttribute>((_0_attributes).UniqueElements, false, (((_exists_var_0) => {
        DAST._IAttribute _1_attribute = (DAST._IAttribute)_exists_var_0;
        return ((_0_attributes).Contains(_1_attribute)) && ((((_1_attribute).dtor_name).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("extern"))) && ((new BigInteger(((_1_attribute).dtor_args).Count)) == (new BigInteger(2))));
      }))))(attributes);
    }
    public void GenArgs(Defs._ISelfInfo selfIdent, DAST._ICallName name, Dafny.ISequence<DAST._IType> typeArgs, Dafny.ISequence<DAST._IExpression> args, Defs._IEnvironment env, out Dafny.ISequence<RAST._IExpr> argExprs, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents, out Dafny.ISequence<RAST._IType> typeExprs, out Std.Wrappers._IOption<DAST._IResolvedType> fullNameQualifier)
    {
      argExprs = Dafny.Sequence<RAST._IExpr>.Empty;
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      typeExprs = Dafny.Sequence<RAST._IType>.Empty;
      fullNameQualifier = Std.Wrappers.Option<DAST._IResolvedType>.Default();
      argExprs = Dafny.Sequence<RAST._IExpr>.FromElements();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
      Dafny.ISequence<DAST._IFormal> _0_borrowSignature = Dafny.Sequence<DAST._IFormal>.Empty;
      if ((name).is_CallName) {
        if ((((name).dtor_receiverArg).is_Some) && ((name).dtor_receiverAsArgument)) {
          _0_borrowSignature = Dafny.Sequence<DAST._IFormal>.Concat(Dafny.Sequence<DAST._IFormal>.FromElements(((name).dtor_receiverArg).dtor_value), ((name).dtor_signature).dtor_inheritedParams);
        } else {
          _0_borrowSignature = ((name).dtor_signature).dtor_inheritedParams;
        }
      } else {
        _0_borrowSignature = Dafny.Sequence<DAST._IFormal>.FromElements();
      }
      BigInteger _hi0 = new BigInteger((args).Count);
      for (BigInteger _1_i = BigInteger.Zero; _1_i < _hi0; _1_i++) {
        Defs._IOwnership _2_argOwnership;
        _2_argOwnership = Defs.Ownership.create_OwnershipBorrowed();
        if ((_1_i) < (new BigInteger((_0_borrowSignature).Count))) {
          RAST._IType _3_tpe;
          RAST._IType _out0;
          _out0 = (this).GenType(((_0_borrowSignature).Select(_1_i)).dtor_typ, Defs.GenTypeContext.@default());
          _3_tpe = _out0;
          if ((_3_tpe).CanReadWithoutClone()) {
            _2_argOwnership = Defs.Ownership.create_OwnershipOwned();
          }
        }
        RAST._IExpr _4_argExpr;
        Defs._IOwnership _5___v100;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _6_argIdents;
        RAST._IExpr _out1;
        Defs._IOwnership _out2;
        Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out3;
        (this).GenExpr((args).Select(_1_i), selfIdent, env, _2_argOwnership, out _out1, out _out2, out _out3);
        _4_argExpr = _out1;
        _5___v100 = _out2;
        _6_argIdents = _out3;
        argExprs = Dafny.Sequence<RAST._IExpr>.Concat(argExprs, Dafny.Sequence<RAST._IExpr>.FromElements(_4_argExpr));
        readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _6_argIdents);
      }
      typeExprs = Dafny.Sequence<RAST._IType>.FromElements();
      BigInteger _hi1 = new BigInteger((typeArgs).Count);
      for (BigInteger _7_typeI = BigInteger.Zero; _7_typeI < _hi1; _7_typeI++) {
        RAST._IType _8_typeExpr;
        RAST._IType _out4;
        _out4 = (this).GenType((typeArgs).Select(_7_typeI), Defs.GenTypeContext.@default());
        _8_typeExpr = _out4;
        typeExprs = Dafny.Sequence<RAST._IType>.Concat(typeExprs, Dafny.Sequence<RAST._IType>.FromElements(_8_typeExpr));
      }
      DAST._ICallName _source0 = name;
      {
        if (_source0.is_CallName) {
          Dafny.ISequence<Dafny.Rune> _9_nameIdent = _source0.dtor_name;
          Std.Wrappers._IOption<DAST._IType> onType0 = _source0.dtor_onType;
          if (onType0.is_Some) {
            DAST._IType value0 = onType0.dtor_value;
            if (value0.is_UserDefined) {
              DAST._IResolvedType _10_resolvedType = value0.dtor_resolved;
              if (((((_10_resolvedType).dtor_kind).is_Trait) || ((Defs.__default.builtin__trait__preferred__methods).Contains((_9_nameIdent)))) || (Dafny.Helpers.Id<Func<DAST._IResolvedType, Dafny.ISequence<Dafny.Rune>, bool>>((_11_resolvedType, _12_nameIdent) => Dafny.Helpers.Quantifier<Dafny.ISequence<Dafny.Rune>>(Dafny.Helpers.SingleValue<Dafny.ISequence<Dafny.Rune>>(_12_nameIdent), true, (((_forall_var_0) => {
                Dafny.ISequence<Dafny.Rune> _13_m = (Dafny.ISequence<Dafny.Rune>)_forall_var_0;
                return !(((_11_resolvedType).dtor_properMethods).Contains(_13_m)) || (!object.Equals(_13_m, _12_nameIdent));
              }))))(_10_resolvedType, _9_nameIdent))) {
                fullNameQualifier = Std.Wrappers.Option<DAST._IResolvedType>.create_Some(Std.Wrappers.Option<DAST._IResolvedType>.GetOr(Defs.__default.TraitTypeContainingMethod(_10_resolvedType, (_9_nameIdent)), _10_resolvedType));
              } else {
                fullNameQualifier = Std.Wrappers.Option<DAST._IResolvedType>.create_None();
              }
              goto after_match0;
            }
          }
        }
      }
      {
        fullNameQualifier = Std.Wrappers.Option<DAST._IResolvedType>.create_None();
      }
    after_match0: ;
      if ((((((fullNameQualifier).is_Some) && ((selfIdent).is_ThisTyped)) && (((selfIdent).dtor_dafnyType).is_UserDefined)) && ((this).IsSameResolvedType(((selfIdent).dtor_dafnyType).dtor_resolved, (fullNameQualifier).dtor_value))) && (!((this).HasExternAttributeRenamingModule(((fullNameQualifier).dtor_value).dtor_attributes)))) {
        fullNameQualifier = Std.Wrappers.Option<DAST._IResolvedType>.create_None();
      }
    }
    public Dafny.ISequence<Dafny.Rune> GetMethodName(DAST._IExpression @on, DAST._ICallName name)
    {
      DAST._ICallName _source0 = name;
      {
        if (_source0.is_CallName) {
          Dafny.ISequence<Dafny.Rune> _0_ident = _source0.dtor_name;
          if ((@on).is_ExternCompanion) {
            return (_0_ident);
          } else {
            return Defs.__default.escapeName(_0_ident);
          }
        }
      }
      {
        bool disjunctiveMatch0 = false;
        if (_source0.is_MapBuilderAdd) {
          disjunctiveMatch0 = true;
        }
        if (_source0.is_SetBuilderAdd) {
          disjunctiveMatch0 = true;
        }
        if (disjunctiveMatch0) {
          return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("add");
        }
      }
      {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("build");
      }
    }
    public void GenExpr(DAST._IExpression e, Defs._ISelfInfo selfIdent, Defs._IEnvironment env, Defs._IOwnership expectedOwnership, out RAST._IExpr r, out Defs._IOwnership resultingOwnership, out Dafny.ISet<Dafny.ISequence<Dafny.Rune>> readIdents)
    {
      r = RAST.Expr.Default();
      resultingOwnership = Defs.Ownership.Default();
      readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
      DAST._IExpression _source0 = e;
      {
        if (_source0.is_Literal) {
          RAST._IExpr _out0;
          Defs._IOwnership _out1;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out2;
          (this).GenExprLiteral(e, selfIdent, env, expectedOwnership, out _out0, out _out1, out _out2);
          r = _out0;
          resultingOwnership = _out1;
          readIdents = _out2;
          goto after_match0;
        }
      }
      {
        if (_source0.is_Ident) {
          Dafny.ISequence<Dafny.Rune> _0_name = _source0.dtor_name;
          {
            RAST._IExpr _out3;
            Defs._IOwnership _out4;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out5;
            (this).GenIdent(Defs.__default.escapeVar(_0_name), selfIdent, env, expectedOwnership, out _out3, out _out4, out _out5);
            r = _out3;
            resultingOwnership = _out4;
            readIdents = _out5;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_ExternCompanion) {
          Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _1_path = _source0.dtor_ExternCompanion_a0;
          {
            RAST._IExpr _out6;
            _out6 = (this).GenPathExpr(_1_path, false);
            r = _out6;
            if (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipBorrowed())) {
              resultingOwnership = Defs.Ownership.create_OwnershipBorrowed();
            } else if (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipOwned())) {
              resultingOwnership = Defs.Ownership.create_OwnershipOwned();
            } else {
              RAST._IExpr _out7;
              Defs._IOwnership _out8;
              (this).FromOwned(r, expectedOwnership, out _out7, out _out8);
              r = _out7;
              resultingOwnership = _out8;
            }
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Companion) {
          Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _2_path = _source0.dtor_Companion_a0;
          Dafny.ISequence<DAST._IType> _3_typeArgs = _source0.dtor_typeArgs;
          {
            RAST._IExpr _out9;
            _out9 = (this).GenPathExpr(_2_path, true);
            r = _out9;
            if ((new BigInteger((_3_typeArgs).Count)).Sign == 1) {
              Dafny.ISequence<RAST._IType> _4_typeExprs;
              _4_typeExprs = Dafny.Sequence<RAST._IType>.FromElements();
              BigInteger _hi0 = new BigInteger((_3_typeArgs).Count);
              for (BigInteger _5_i = BigInteger.Zero; _5_i < _hi0; _5_i++) {
                RAST._IType _6_typeExpr;
                RAST._IType _out10;
                _out10 = (this).GenType((_3_typeArgs).Select(_5_i), Defs.GenTypeContext.@default());
                _6_typeExpr = _out10;
                _4_typeExprs = Dafny.Sequence<RAST._IType>.Concat(_4_typeExprs, Dafny.Sequence<RAST._IType>.FromElements(_6_typeExpr));
              }
              r = (r).ApplyType(_4_typeExprs);
            }
            if (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipBorrowed())) {
              resultingOwnership = Defs.Ownership.create_OwnershipBorrowed();
            } else if (object.Equals(expectedOwnership, Defs.Ownership.create_OwnershipOwned())) {
              resultingOwnership = Defs.Ownership.create_OwnershipOwned();
            } else {
              RAST._IExpr _out11;
              Defs._IOwnership _out12;
              (this).FromOwned(r, expectedOwnership, out _out11, out _out12);
              r = _out11;
              resultingOwnership = _out12;
            }
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_InitializationValue) {
          DAST._IType _7_typ = _source0.dtor_typ;
          {
            RAST._IType _8_typExpr;
            RAST._IType _out13;
            _out13 = (this).GenType(_7_typ, Defs.GenTypeContext.@default());
            _8_typExpr = _out13;
            r = ((RAST.Expr.create_TraitCast(_8_typExpr, RAST.__default.DefaultTrait)).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("default"))).Apply0();
            RAST._IExpr _out14;
            Defs._IOwnership _out15;
            (this).FromOwned(r, expectedOwnership, out _out14, out _out15);
            r = _out14;
            resultingOwnership = _out15;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Tuple) {
          Dafny.ISequence<DAST._IExpression> _9_values = _source0.dtor_Tuple_a0;
          {
            Dafny.ISequence<RAST._IExpr> _10_exprs;
            _10_exprs = Dafny.Sequence<RAST._IExpr>.FromElements();
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            BigInteger _hi1 = new BigInteger((_9_values).Count);
            for (BigInteger _11_i = BigInteger.Zero; _11_i < _hi1; _11_i++) {
              RAST._IExpr _12_recursiveGen;
              Defs._IOwnership _13___v110;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _14_recIdents;
              RAST._IExpr _out16;
              Defs._IOwnership _out17;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out18;
              (this).GenExpr((_9_values).Select(_11_i), selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out16, out _out17, out _out18);
              _12_recursiveGen = _out16;
              _13___v110 = _out17;
              _14_recIdents = _out18;
              _10_exprs = Dafny.Sequence<RAST._IExpr>.Concat(_10_exprs, Dafny.Sequence<RAST._IExpr>.FromElements(_12_recursiveGen));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _14_recIdents);
            }
            if ((new BigInteger((_9_values).Count)) <= (RAST.__default.MAX__TUPLE__SIZE)) {
              r = RAST.Expr.create_Tuple(_10_exprs);
            } else {
              r = RAST.__default.SystemTuple(_10_exprs);
            }
            RAST._IExpr _out19;
            Defs._IOwnership _out20;
            (this).FromOwned(r, expectedOwnership, out _out19, out _out20);
            r = _out19;
            resultingOwnership = _out20;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_New) {
          Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _15_path = _source0.dtor_path;
          Dafny.ISequence<DAST._IType> _16_typeArgs = _source0.dtor_typeArgs;
          Dafny.ISequence<DAST._IExpression> _17_args = _source0.dtor_args;
          {
            RAST._IExpr _out21;
            _out21 = (this).GenPathExpr(_15_path, true);
            r = _out21;
            if ((new BigInteger((_16_typeArgs).Count)).Sign == 1) {
              Dafny.ISequence<RAST._IType> _18_typeExprs;
              _18_typeExprs = Dafny.Sequence<RAST._IType>.FromElements();
              BigInteger _hi2 = new BigInteger((_16_typeArgs).Count);
              for (BigInteger _19_i = BigInteger.Zero; _19_i < _hi2; _19_i++) {
                RAST._IType _20_typeExpr;
                RAST._IType _out22;
                _out22 = (this).GenType((_16_typeArgs).Select(_19_i), Defs.GenTypeContext.@default());
                _20_typeExpr = _out22;
                _18_typeExprs = Dafny.Sequence<RAST._IType>.Concat(_18_typeExprs, Dafny.Sequence<RAST._IType>.FromElements(_20_typeExpr));
              }
              r = (r).ApplyType(_18_typeExprs);
            }
            r = (r).FSel((this).allocate__fn);
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            Dafny.ISequence<RAST._IExpr> _21_arguments;
            _21_arguments = Dafny.Sequence<RAST._IExpr>.FromElements();
            BigInteger _hi3 = new BigInteger((_17_args).Count);
            for (BigInteger _22_i = BigInteger.Zero; _22_i < _hi3; _22_i++) {
              RAST._IExpr _23_recursiveGen;
              Defs._IOwnership _24___v111;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _25_recIdents;
              RAST._IExpr _out23;
              Defs._IOwnership _out24;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out25;
              (this).GenExpr((_17_args).Select(_22_i), selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out23, out _out24, out _out25);
              _23_recursiveGen = _out23;
              _24___v111 = _out24;
              _25_recIdents = _out25;
              _21_arguments = Dafny.Sequence<RAST._IExpr>.Concat(_21_arguments, Dafny.Sequence<RAST._IExpr>.FromElements(_23_recursiveGen));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _25_recIdents);
            }
            r = (r).Apply(_21_arguments);
            RAST._IExpr _out26;
            Defs._IOwnership _out27;
            (this).FromOwned(r, expectedOwnership, out _out26, out _out27);
            r = _out26;
            resultingOwnership = _out27;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_NewUninitArray) {
          Dafny.ISequence<DAST._IExpression> _26_dims = _source0.dtor_dims;
          DAST._IType _27_typ = _source0.dtor_typ;
          {
            if ((new BigInteger(16)) < (new BigInteger((_26_dims).Count))) {
              RAST._IExpr _out28;
              _out28 = (this).Error(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Unsupported: Creation of arrays of more than 16 dimensions"), (this).InitEmptyExpr());
              r = _out28;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            } else {
              r = (this).InitEmptyExpr();
              RAST._IType _28_typeGen;
              RAST._IType _out29;
              _out29 = (this).GenType(_27_typ, Defs.GenTypeContext.@default());
              _28_typeGen = _out29;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              Dafny.ISequence<RAST._IExpr> _29_dimExprs;
              _29_dimExprs = Dafny.Sequence<RAST._IExpr>.FromElements();
              BigInteger _hi4 = new BigInteger((_26_dims).Count);
              for (BigInteger _30_i = BigInteger.Zero; _30_i < _hi4; _30_i++) {
                RAST._IExpr _31_recursiveGen;
                Defs._IOwnership _32___v112;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _33_recIdents;
                RAST._IExpr _out30;
                Defs._IOwnership _out31;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out32;
                (this).GenExpr((_26_dims).Select(_30_i), selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out30, out _out31, out _out32);
                _31_recursiveGen = _out30;
                _32___v112 = _out31;
                _33_recIdents = _out32;
                _29_dimExprs = Dafny.Sequence<RAST._IExpr>.Concat(_29_dimExprs, Dafny.Sequence<RAST._IExpr>.FromElements(RAST.__default.IntoUsize(_31_recursiveGen)));
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _33_recIdents);
              }
              if ((new BigInteger((_26_dims).Count)) > (BigInteger.One)) {
                Dafny.ISequence<Dafny.Rune> _34_class__name;
                _34_class__name = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Array"), Std.Strings.__default.OfNat(new BigInteger((_26_dims).Count)));
                r = (((((RAST.__default.dafny__runtime).MSel(_34_class__name)).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_28_typeGen))).FSel((this).placebos__usize)).Apply(_29_dimExprs);
              } else {
                r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("array"))).AsExpr()).FSel((this).placebos__usize)).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_28_typeGen))).Apply(_29_dimExprs);
              }
            }
            RAST._IExpr _out33;
            Defs._IOwnership _out34;
            (this).FromOwned(r, expectedOwnership, out _out33, out _out34);
            r = _out33;
            resultingOwnership = _out34;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_ArrayIndexToInt) {
          DAST._IExpression _35_underlying = _source0.dtor_value;
          {
            RAST._IExpr _36_recursiveGen;
            Defs._IOwnership _37___v113;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _38_recIdents;
            RAST._IExpr _out35;
            Defs._IOwnership _out36;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out37;
            (this).GenExpr(_35_underlying, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out35, out _out36, out _out37);
            _36_recursiveGen = _out35;
            _37___v113 = _out36;
            _38_recIdents = _out37;
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1(_36_recursiveGen);
            readIdents = _38_recIdents;
            RAST._IExpr _out38;
            Defs._IOwnership _out39;
            (this).FromOwned(r, expectedOwnership, out _out38, out _out39);
            r = _out38;
            resultingOwnership = _out39;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_FinalizeNewArray) {
          DAST._IExpression _39_underlying = _source0.dtor_value;
          DAST._IType _40_typ = _source0.dtor_typ;
          {
            RAST._IType _41_tpe;
            RAST._IType _out40;
            _out40 = (this).GenType(_40_typ, Defs.GenTypeContext.@default());
            _41_tpe = _out40;
            RAST._IExpr _42_recursiveGen;
            Defs._IOwnership _43___v114;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _44_recIdents;
            RAST._IExpr _out41;
            Defs._IOwnership _out42;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out43;
            (this).GenExpr(_39_underlying, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out41, out _out42, out _out43);
            _42_recursiveGen = _out41;
            _43___v114 = _out42;
            _44_recIdents = _out43;
            readIdents = _44_recIdents;
            if ((_41_tpe).IsObjectOrPointer()) {
              RAST._IType _45_t;
              _45_t = (_41_tpe).ObjectOrPointerUnderlying();
              if ((_45_t).is_Array) {
                r = ((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("array"))).AsExpr()).FSel((this).array__construct)).Apply1(_42_recursiveGen);
              } else if ((_45_t).IsMultiArray()) {
                Dafny.ISequence<Dafny.Rune> _46_c;
                _46_c = (_45_t).MultiArrayClass();
                r = ((((RAST.__default.dafny__runtime).MSel(_46_c)).AsExpr()).FSel((this).array__construct)).Apply1(_42_recursiveGen);
              } else {
                RAST._IExpr _out44;
                _out44 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Finalize New Array with a pointer or object type to something that is not an array or a multi array: "), (_41_tpe)._ToString(Defs.__default.IND)), (this).InitEmptyExpr());
                r = _out44;
              }
            } else {
              RAST._IExpr _out45;
              _out45 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Finalize New Array with a type that is not a pointer or an object: "), (_41_tpe)._ToString(Defs.__default.IND)), (this).InitEmptyExpr());
              r = _out45;
            }
            RAST._IExpr _out46;
            Defs._IOwnership _out47;
            (this).FromOwned(r, expectedOwnership, out _out46, out _out47);
            r = _out46;
            resultingOwnership = _out47;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_DatatypeValue) {
          DAST._IResolvedType _47_datatypeType = _source0.dtor_datatypeType;
          Dafny.ISequence<DAST._IType> _48_typeArgs = _source0.dtor_typeArgs;
          Dafny.ISequence<Dafny.Rune> _49_variant = _source0.dtor_variant;
          bool _50_isCo = _source0.dtor_isCo;
          Dafny.ISequence<_System._ITuple2<Dafny.ISequence<Dafny.Rune>, DAST._IExpression>> _51_values = _source0.dtor_contents;
          {
            RAST._IExpr _out48;
            _out48 = (this).GenPathExpr((_47_datatypeType).dtor_path, true);
            r = _out48;
            Dafny.ISequence<RAST._IType> _52_genTypeArgs;
            _52_genTypeArgs = Dafny.Sequence<RAST._IType>.FromElements();
            BigInteger _hi5 = new BigInteger((_48_typeArgs).Count);
            for (BigInteger _53_i = BigInteger.Zero; _53_i < _hi5; _53_i++) {
              RAST._IType _54_typeExpr;
              RAST._IType _out49;
              _out49 = (this).GenType((_48_typeArgs).Select(_53_i), Defs.GenTypeContext.@default());
              _54_typeExpr = _out49;
              _52_genTypeArgs = Dafny.Sequence<RAST._IType>.Concat(_52_genTypeArgs, Dafny.Sequence<RAST._IType>.FromElements(_54_typeExpr));
            }
            if ((new BigInteger((_48_typeArgs).Count)).Sign == 1) {
              r = (r).ApplyType(_52_genTypeArgs);
            }
            r = (r).FSel(Defs.__default.escapeName(_49_variant));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            Dafny.ISequence<RAST._IAssignIdentifier> _55_assignments;
            _55_assignments = Dafny.Sequence<RAST._IAssignIdentifier>.FromElements();
            BigInteger _hi6 = new BigInteger((_51_values).Count);
            for (BigInteger _56_i = BigInteger.Zero; _56_i < _hi6; _56_i++) {
              _System._ITuple2<Dafny.ISequence<Dafny.Rune>, DAST._IExpression> _let_tmp_rhs0 = (_51_values).Select(_56_i);
              Dafny.ISequence<Dafny.Rune> _57_name = _let_tmp_rhs0.dtor__0;
              DAST._IExpression _58_value = _let_tmp_rhs0.dtor__1;
              if (_50_isCo) {
                RAST._IExpr _59_recursiveGen;
                Defs._IOwnership _60___v115;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _61_recIdents;
                RAST._IExpr _out50;
                Defs._IOwnership _out51;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out52;
                (this).GenExpr(_58_value, selfIdent, Defs.Environment.Empty(), Defs.Ownership.create_OwnershipOwned(), out _out50, out _out51, out _out52);
                _59_recursiveGen = _out50;
                _60___v115 = _out51;
                _61_recIdents = _out52;
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _61_recIdents);
                RAST._IExpr _62_allReadCloned;
                _62_allReadCloned = (this).InitEmptyExpr();
                while (!(_61_recIdents).Equals(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements())) {
                  Dafny.ISequence<Dafny.Rune> _63_next;
                  foreach (Dafny.ISequence<Dafny.Rune> _assign_such_that_0 in (_61_recIdents).Elements) {
                    _63_next = (Dafny.ISequence<Dafny.Rune>)_assign_such_that_0;
                    if ((_61_recIdents).Contains(_63_next)) {
                      goto after__ASSIGN_SUCH_THAT_0;
                    }
                  }
                  throw new System.Exception("assign-such-that search produced no value");
                after__ASSIGN_SUCH_THAT_0: ;
                  _62_allReadCloned = (_62_allReadCloned).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), _63_next, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some((RAST.Expr.create_Identifier(_63_next)).Clone())));
                  _61_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_61_recIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_63_next));
                }
                RAST._IExpr _64_wasAssigned;
                _64_wasAssigned = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("LazyFieldWrapper"))).AsExpr()).Apply1(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Lazy"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply1((((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("boxed"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Box"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply1((_62_allReadCloned).Then(RAST.Expr.create_Lambda(Dafny.Sequence<RAST._IFormal>.FromElements(), Std.Wrappers.Option<RAST._IType>.create_None(), _59_recursiveGen)))));
                _55_assignments = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_55_assignments, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(Defs.__default.escapeVar(_57_name), _64_wasAssigned)));
              } else {
                RAST._IExpr _65_recursiveGen;
                Defs._IOwnership _66___v116;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _67_recIdents;
                RAST._IExpr _out53;
                Defs._IOwnership _out54;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out55;
                (this).GenExpr(_58_value, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out53, out _out54, out _out55);
                _65_recursiveGen = _out53;
                _66___v116 = _out54;
                _67_recIdents = _out55;
                _55_assignments = Dafny.Sequence<RAST._IAssignIdentifier>.Concat(_55_assignments, Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(Defs.__default.escapeVar(_57_name), _65_recursiveGen)));
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _67_recIdents);
              }
            }
            r = RAST.Expr.create_StructBuild(r, _55_assignments);
            if (Defs.__default.IsRcWrapped((_47_datatypeType).dtor_attributes)) {
              r = RAST.__default.RcNew(r);
            }
            RAST._IExpr _out56;
            Defs._IOwnership _out57;
            (this).FromOwned(r, expectedOwnership, out _out56, out _out57);
            r = _out56;
            resultingOwnership = _out57;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Convert) {
          {
            RAST._IExpr _out58;
            Defs._IOwnership _out59;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out60;
            (this).GenExprConvert(e, selfIdent, env, expectedOwnership, out _out58, out _out59, out _out60);
            r = _out58;
            resultingOwnership = _out59;
            readIdents = _out60;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SeqConstruct) {
          DAST._IExpression _68_length = _source0.dtor_length;
          DAST._IExpression _69_expr = _source0.dtor_elem;
          {
            RAST._IExpr _70_recursiveGen;
            Defs._IOwnership _71___v120;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _72_recIdents;
            RAST._IExpr _out61;
            Defs._IOwnership _out62;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out63;
            (this).GenExpr(_69_expr, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out61, out _out62, out _out63);
            _70_recursiveGen = _out61;
            _71___v120 = _out62;
            _72_recIdents = _out63;
            RAST._IExpr _73_lengthGen;
            Defs._IOwnership _74___v121;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _75_lengthIdents;
            RAST._IExpr _out64;
            Defs._IOwnership _out65;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out66;
            (this).GenExpr(_68_length, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out64, out _out65, out _out66);
            _73_lengthGen = _out64;
            _74___v121 = _out65;
            _75_lengthIdents = _out66;
            r = RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_initializer"), Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(_70_recursiveGen));
            RAST._IExpr _76_range;
            _76_range = ((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("integer_range"))).AsExpr();
            _76_range = (_76_range).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Zero"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("zero"))).Apply0(), _73_lengthGen));
            _76_range = (_76_range).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("map"));
            RAST._IExpr _77_rangeMap;
            _77_rangeMap = RAST.Expr.create_Lambda(Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.ImplicitlyTyped(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i"))), Std.Wrappers.Option<RAST._IType>.create_None(), (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_initializer"))).Apply1(RAST.__default.Borrow(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("i")))));
            _76_range = (_76_range).Apply1(_77_rangeMap);
            _76_range = (((_76_range).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("collect"))).ApplyType(Dafny.Sequence<RAST._IType>.FromElements((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Sequence"))).AsType()).Apply(Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_TIdentifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"))))))).Apply0();
            r = RAST.Expr.create_Block((r).Then(_76_range));
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_72_recIdents, _75_lengthIdents);
            RAST._IExpr _out67;
            Defs._IOwnership _out68;
            (this).FromOwned(r, expectedOwnership, out _out67, out _out68);
            r = _out67;
            resultingOwnership = _out68;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SeqValue) {
          Dafny.ISequence<DAST._IExpression> _78_exprs = _source0.dtor_elements;
          DAST._IType _79_typ = _source0.dtor_typ;
          {
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            RAST._IType _80_genTpe;
            RAST._IType _out69;
            _out69 = (this).GenType(_79_typ, Defs.GenTypeContext.@default());
            _80_genTpe = _out69;
            BigInteger _81_i;
            _81_i = BigInteger.Zero;
            Dafny.ISequence<RAST._IExpr> _82_args;
            _82_args = Dafny.Sequence<RAST._IExpr>.FromElements();
            while ((_81_i) < (new BigInteger((_78_exprs).Count))) {
              RAST._IExpr _83_recursiveGen;
              Defs._IOwnership _84___v122;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _85_recIdents;
              RAST._IExpr _out70;
              Defs._IOwnership _out71;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out72;
              (this).GenExpr((_78_exprs).Select(_81_i), selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out70, out _out71, out _out72);
              _83_recursiveGen = _out70;
              _84___v122 = _out71;
              _85_recIdents = _out72;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _85_recIdents);
              _82_args = Dafny.Sequence<RAST._IExpr>.Concat(_82_args, Dafny.Sequence<RAST._IExpr>.FromElements(_83_recursiveGen));
              _81_i = (_81_i) + (BigInteger.One);
            }
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("seq!"))).AsExpr()).Apply(_82_args);
            if ((new BigInteger((_82_args).Count)).Sign == 0) {
              r = RAST.Expr.create_TypeAscription(r, (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Sequence"))).AsType()).Apply1(_80_genTpe));
            }
            RAST._IExpr _out73;
            Defs._IOwnership _out74;
            (this).FromOwned(r, expectedOwnership, out _out73, out _out74);
            r = _out73;
            resultingOwnership = _out74;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SetValue) {
          Dafny.ISequence<DAST._IExpression> _86_exprs = _source0.dtor_elements;
          {
            Dafny.ISequence<RAST._IExpr> _87_generatedValues;
            _87_generatedValues = Dafny.Sequence<RAST._IExpr>.FromElements();
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            BigInteger _88_i;
            _88_i = BigInteger.Zero;
            while ((_88_i) < (new BigInteger((_86_exprs).Count))) {
              RAST._IExpr _89_recursiveGen;
              Defs._IOwnership _90___v123;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _91_recIdents;
              RAST._IExpr _out75;
              Defs._IOwnership _out76;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out77;
              (this).GenExpr((_86_exprs).Select(_88_i), selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out75, out _out76, out _out77);
              _89_recursiveGen = _out75;
              _90___v123 = _out76;
              _91_recIdents = _out77;
              _87_generatedValues = Dafny.Sequence<RAST._IExpr>.Concat(_87_generatedValues, Dafny.Sequence<RAST._IExpr>.FromElements(_89_recursiveGen));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _91_recIdents);
              _88_i = (_88_i) + (BigInteger.One);
            }
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("set!"))).AsExpr()).Apply(_87_generatedValues);
            RAST._IExpr _out78;
            Defs._IOwnership _out79;
            (this).FromOwned(r, expectedOwnership, out _out78, out _out79);
            r = _out78;
            resultingOwnership = _out79;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MultisetValue) {
          Dafny.ISequence<DAST._IExpression> _92_exprs = _source0.dtor_elements;
          {
            Dafny.ISequence<RAST._IExpr> _93_generatedValues;
            _93_generatedValues = Dafny.Sequence<RAST._IExpr>.FromElements();
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            BigInteger _94_i;
            _94_i = BigInteger.Zero;
            while ((_94_i) < (new BigInteger((_92_exprs).Count))) {
              RAST._IExpr _95_recursiveGen;
              Defs._IOwnership _96___v124;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _97_recIdents;
              RAST._IExpr _out80;
              Defs._IOwnership _out81;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out82;
              (this).GenExpr((_92_exprs).Select(_94_i), selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out80, out _out81, out _out82);
              _95_recursiveGen = _out80;
              _96___v124 = _out81;
              _97_recIdents = _out82;
              _93_generatedValues = Dafny.Sequence<RAST._IExpr>.Concat(_93_generatedValues, Dafny.Sequence<RAST._IExpr>.FromElements(_95_recursiveGen));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _97_recIdents);
              _94_i = (_94_i) + (BigInteger.One);
            }
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("multiset!"))).AsExpr()).Apply(_93_generatedValues);
            RAST._IExpr _out83;
            Defs._IOwnership _out84;
            (this).FromOwned(r, expectedOwnership, out _out83, out _out84);
            r = _out83;
            resultingOwnership = _out84;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_ToMultiset) {
          DAST._IExpression _98_expr = _source0.dtor_ToMultiset_a0;
          {
            RAST._IExpr _99_recursiveGen;
            Defs._IOwnership _100___v125;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _101_recIdents;
            RAST._IExpr _out85;
            Defs._IOwnership _out86;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out87;
            (this).GenExpr(_98_expr, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out85, out _out86, out _out87);
            _99_recursiveGen = _out85;
            _100___v125 = _out86;
            _101_recIdents = _out87;
            r = ((_99_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_dafny_multiset"))).Apply0();
            readIdents = _101_recIdents;
            RAST._IExpr _out88;
            Defs._IOwnership _out89;
            (this).FromOwned(r, expectedOwnership, out _out88, out _out89);
            r = _out88;
            resultingOwnership = _out89;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MapValue) {
          Dafny.ISequence<_System._ITuple2<DAST._IExpression, DAST._IExpression>> _102_mapElems = _source0.dtor_mapElems;
          {
            Dafny.ISequence<_System._ITuple2<RAST._IExpr, RAST._IExpr>> _103_generatedValues;
            _103_generatedValues = Dafny.Sequence<_System._ITuple2<RAST._IExpr, RAST._IExpr>>.FromElements();
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            BigInteger _104_i;
            _104_i = BigInteger.Zero;
            while ((_104_i) < (new BigInteger((_102_mapElems).Count))) {
              RAST._IExpr _105_recursiveGenKey;
              Defs._IOwnership _106___v126;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _107_recIdentsKey;
              RAST._IExpr _out90;
              Defs._IOwnership _out91;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out92;
              (this).GenExpr(((_102_mapElems).Select(_104_i)).dtor__0, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out90, out _out91, out _out92);
              _105_recursiveGenKey = _out90;
              _106___v126 = _out91;
              _107_recIdentsKey = _out92;
              RAST._IExpr _108_recursiveGenValue;
              Defs._IOwnership _109___v127;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _110_recIdentsValue;
              RAST._IExpr _out93;
              Defs._IOwnership _out94;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out95;
              (this).GenExpr(((_102_mapElems).Select(_104_i)).dtor__1, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out93, out _out94, out _out95);
              _108_recursiveGenValue = _out93;
              _109___v127 = _out94;
              _110_recIdentsValue = _out95;
              _103_generatedValues = Dafny.Sequence<_System._ITuple2<RAST._IExpr, RAST._IExpr>>.Concat(_103_generatedValues, Dafny.Sequence<_System._ITuple2<RAST._IExpr, RAST._IExpr>>.FromElements(_System.Tuple2<RAST._IExpr, RAST._IExpr>.create(_105_recursiveGenKey, _108_recursiveGenValue)));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _107_recIdentsKey), _110_recIdentsValue);
              _104_i = (_104_i) + (BigInteger.One);
            }
            _104_i = BigInteger.Zero;
            Dafny.ISequence<RAST._IExpr> _111_arguments;
            _111_arguments = Dafny.Sequence<RAST._IExpr>.FromElements();
            while ((_104_i) < (new BigInteger((_103_generatedValues).Count))) {
              RAST._IExpr _112_genKey;
              _112_genKey = ((_103_generatedValues).Select(_104_i)).dtor__0;
              RAST._IExpr _113_genValue;
              _113_genValue = ((_103_generatedValues).Select(_104_i)).dtor__1;
              _111_arguments = Dafny.Sequence<RAST._IExpr>.Concat(_111_arguments, Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=>"), _112_genKey, _113_genValue, DAST.Format.BinaryOpFormat.create_NoFormat())));
              _104_i = (_104_i) + (BigInteger.One);
            }
            r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("map!"))).AsExpr()).Apply(_111_arguments);
            RAST._IExpr _out96;
            Defs._IOwnership _out97;
            (this).FromOwned(r, expectedOwnership, out _out96, out _out97);
            r = _out96;
            resultingOwnership = _out97;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SeqUpdate) {
          DAST._IExpression _114_expr = _source0.dtor_expr;
          DAST._IExpression _115_index = _source0.dtor_indexExpr;
          DAST._IExpression _116_value = _source0.dtor_value;
          {
            RAST._IExpr _117_exprR;
            Defs._IOwnership _118___v128;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _119_exprIdents;
            RAST._IExpr _out98;
            Defs._IOwnership _out99;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out100;
            (this).GenExpr(_114_expr, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out98, out _out99, out _out100);
            _117_exprR = _out98;
            _118___v128 = _out99;
            _119_exprIdents = _out100;
            RAST._IExpr _120_indexR;
            Defs._IOwnership _121_indexOwnership;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _122_indexIdents;
            RAST._IExpr _out101;
            Defs._IOwnership _out102;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out103;
            (this).GenExpr(_115_index, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out101, out _out102, out _out103);
            _120_indexR = _out101;
            _121_indexOwnership = _out102;
            _122_indexIdents = _out103;
            RAST._IExpr _123_valueR;
            Defs._IOwnership _124_valueOwnership;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _125_valueIdents;
            RAST._IExpr _out104;
            Defs._IOwnership _out105;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out106;
            (this).GenExpr(_116_value, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out104, out _out105, out _out106);
            _123_valueR = _out104;
            _124_valueOwnership = _out105;
            _125_valueIdents = _out106;
            r = ((_117_exprR).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_index"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_120_indexR, _123_valueR));
            RAST._IExpr _out107;
            Defs._IOwnership _out108;
            (this).FromOwned(r, expectedOwnership, out _out107, out _out108);
            r = _out107;
            resultingOwnership = _out108;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_119_exprIdents, _122_indexIdents), _125_valueIdents);
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MapUpdate) {
          DAST._IExpression _126_expr = _source0.dtor_expr;
          DAST._IExpression _127_index = _source0.dtor_indexExpr;
          DAST._IExpression _128_value = _source0.dtor_value;
          {
            RAST._IExpr _129_exprR;
            Defs._IOwnership _130___v129;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _131_exprIdents;
            RAST._IExpr _out109;
            Defs._IOwnership _out110;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out111;
            (this).GenExpr(_126_expr, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out109, out _out110, out _out111);
            _129_exprR = _out109;
            _130___v129 = _out110;
            _131_exprIdents = _out111;
            RAST._IExpr _132_indexR;
            Defs._IOwnership _133_indexOwnership;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _134_indexIdents;
            RAST._IExpr _out112;
            Defs._IOwnership _out113;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out114;
            (this).GenExpr(_127_index, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out112, out _out113, out _out114);
            _132_indexR = _out112;
            _133_indexOwnership = _out113;
            _134_indexIdents = _out114;
            RAST._IExpr _135_valueR;
            Defs._IOwnership _136_valueOwnership;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _137_valueIdents;
            RAST._IExpr _out115;
            Defs._IOwnership _out116;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out117;
            (this).GenExpr(_128_value, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out115, out _out116, out _out117);
            _135_valueR = _out115;
            _136_valueOwnership = _out116;
            _137_valueIdents = _out117;
            r = ((_129_exprR).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_index"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_132_indexR, _135_valueR));
            RAST._IExpr _out118;
            Defs._IOwnership _out119;
            (this).FromOwned(r, expectedOwnership, out _out118, out _out119);
            r = _out118;
            resultingOwnership = _out119;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_131_exprIdents, _134_indexIdents), _137_valueIdents);
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_This) {
          {
            Defs._ISelfInfo _source1 = selfIdent;
            {
              if (_source1.is_ThisTyped) {
                Dafny.ISequence<Dafny.Rune> _138_id = _source1.dtor_rSelfName;
                DAST._IType _139_dafnyType = _source1.dtor_dafnyType;
                {
                  RAST._IExpr _out120;
                  Defs._IOwnership _out121;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out122;
                  (this).GenIdent(_138_id, selfIdent, env, expectedOwnership, out _out120, out _out121, out _out122);
                  r = _out120;
                  resultingOwnership = _out121;
                  readIdents = _out122;
                }
                goto after_match1;
              }
            }
            {
              Defs._ISelfInfo _140_None = _source1;
              {
                RAST._IExpr _out123;
                _out123 = (this).Error(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this outside of a method"), (this).InitEmptyExpr());
                r = _out123;
                RAST._IExpr _out124;
                Defs._IOwnership _out125;
                (this).FromOwned(r, expectedOwnership, out _out124, out _out125);
                r = _out124;
                resultingOwnership = _out125;
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              }
            }
          after_match1: ;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Ite) {
          DAST._IExpression _141_cond = _source0.dtor_cond;
          DAST._IExpression _142_t = _source0.dtor_thn;
          DAST._IExpression _143_f = _source0.dtor_els;
          {
            RAST._IExpr _144_cond;
            Defs._IOwnership _145___v130;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _146_recIdentsCond;
            RAST._IExpr _out126;
            Defs._IOwnership _out127;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out128;
            (this).GenExpr(_141_cond, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out126, out _out127, out _out128);
            _144_cond = _out126;
            _145___v130 = _out127;
            _146_recIdentsCond = _out128;
            RAST._IExpr _147_fExpr;
            Defs._IOwnership _148_fOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _149_recIdentsF;
            RAST._IExpr _out129;
            Defs._IOwnership _out130;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out131;
            (this).GenExpr(_143_f, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out129, out _out130, out _out131);
            _147_fExpr = _out129;
            _148_fOwned = _out130;
            _149_recIdentsF = _out131;
            RAST._IExpr _150_tExpr;
            Defs._IOwnership _151___v131;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _152_recIdentsT;
            RAST._IExpr _out132;
            Defs._IOwnership _out133;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out134;
            (this).GenExpr(_142_t, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out132, out _out133, out _out134);
            _150_tExpr = _out132;
            _151___v131 = _out133;
            _152_recIdentsT = _out134;
            r = RAST.Expr.create_IfExpr(_144_cond, _150_tExpr, _147_fExpr);
            RAST._IExpr _out135;
            Defs._IOwnership _out136;
            (this).FromOwnership(r, Defs.Ownership.create_OwnershipOwned(), expectedOwnership, out _out135, out _out136);
            r = _out135;
            resultingOwnership = _out136;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_146_recIdentsCond, _152_recIdentsT), _149_recIdentsF);
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_UnOp) {
          DAST._IUnaryOp unOp0 = _source0.dtor_unOp;
          if (unOp0.is_Not) {
            DAST._IExpression _153_e = _source0.dtor_expr;
            DAST.Format._IUnaryOpFormat _154_format = _source0.dtor_format1;
            {
              RAST._IExpr _155_recursiveGen;
              Defs._IOwnership _156___v132;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _157_recIdents;
              RAST._IExpr _out137;
              Defs._IOwnership _out138;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out139;
              (this).GenExpr(_153_e, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out137, out _out138, out _out139);
              _155_recursiveGen = _out137;
              _156___v132 = _out138;
              _157_recIdents = _out139;
              r = RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"), _155_recursiveGen, _154_format);
              RAST._IExpr _out140;
              Defs._IOwnership _out141;
              (this).FromOwned(r, expectedOwnership, out _out140, out _out141);
              r = _out140;
              resultingOwnership = _out141;
              readIdents = _157_recIdents;
              return ;
            }
            goto after_match0;
          }
        }
      }
      {
        if (_source0.is_UnOp) {
          DAST._IUnaryOp unOp1 = _source0.dtor_unOp;
          if (unOp1.is_BitwiseNot) {
            DAST._IExpression _158_e = _source0.dtor_expr;
            DAST.Format._IUnaryOpFormat _159_format = _source0.dtor_format1;
            {
              RAST._IExpr _160_recursiveGen;
              Defs._IOwnership _161___v133;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _162_recIdents;
              RAST._IExpr _out142;
              Defs._IOwnership _out143;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out144;
              (this).GenExpr(_158_e, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out142, out _out143, out _out144);
              _160_recursiveGen = _out142;
              _161___v133 = _out143;
              _162_recIdents = _out144;
              r = RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"), _160_recursiveGen, _159_format);
              RAST._IExpr _out145;
              Defs._IOwnership _out146;
              (this).FromOwned(r, expectedOwnership, out _out145, out _out146);
              r = _out145;
              resultingOwnership = _out146;
              readIdents = _162_recIdents;
              return ;
            }
            goto after_match0;
          }
        }
      }
      {
        if (_source0.is_UnOp) {
          DAST._IUnaryOp unOp2 = _source0.dtor_unOp;
          if (unOp2.is_Cardinality) {
            DAST._IExpression _163_e = _source0.dtor_expr;
            DAST.Format._IUnaryOpFormat _164_format = _source0.dtor_format1;
            {
              RAST._IExpr _165_recursiveGen;
              Defs._IOwnership _166_recOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _167_recIdents;
              RAST._IExpr _out147;
              Defs._IOwnership _out148;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out149;
              (this).GenExpr(_163_e, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out147, out _out148, out _out149);
              _165_recursiveGen = _out147;
              _166_recOwned = _out148;
              _167_recIdents = _out149;
              r = ((_165_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cardinality"))).Apply0();
              RAST._IExpr _out150;
              Defs._IOwnership _out151;
              (this).FromOwned(r, expectedOwnership, out _out150, out _out151);
              r = _out150;
              resultingOwnership = _out151;
              readIdents = _167_recIdents;
              return ;
            }
            goto after_match0;
          }
        }
      }
      {
        if (_source0.is_BinOp) {
          RAST._IExpr _out152;
          Defs._IOwnership _out153;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out154;
          (this).GenExprBinary(e, selfIdent, env, expectedOwnership, out _out152, out _out153, out _out154);
          r = _out152;
          resultingOwnership = _out153;
          readIdents = _out154;
          goto after_match0;
        }
      }
      {
        if (_source0.is_ArrayLen) {
          DAST._IExpression _168_expr = _source0.dtor_expr;
          DAST._IType _169_exprType = _source0.dtor_exprType;
          BigInteger _170_dim = _source0.dtor_dim;
          bool _171_native = _source0.dtor_native;
          {
            RAST._IExpr _172_recursiveGen;
            Defs._IOwnership _173___v138;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _174_recIdents;
            RAST._IExpr _out155;
            Defs._IOwnership _out156;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out157;
            (this).GenExpr(_168_expr, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out155, out _out156, out _out157);
            _172_recursiveGen = _out155;
            _173___v138 = _out156;
            _174_recIdents = _out157;
            RAST._IType _175_arrayType;
            RAST._IType _out158;
            _out158 = (this).GenType(_169_exprType, Defs.GenTypeContext.@default());
            _175_arrayType = _out158;
            if (!((_175_arrayType).IsObjectOrPointer())) {
              RAST._IExpr _out159;
              _out159 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Array length of something not an array but "), (_175_arrayType)._ToString(Defs.__default.IND)), (this).InitEmptyExpr());
              r = _out159;
            } else {
              RAST._IType _176_underlying;
              _176_underlying = (_175_arrayType).ObjectOrPointerUnderlying();
              if (((_170_dim).Sign == 0) && ((_176_underlying).is_Array)) {
                r = ((((this).read__macro).Apply1(_172_recursiveGen)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("len"))).Apply0();
              } else {
                if ((_170_dim).Sign == 0) {
                  r = (((((this).read__macro).Apply1(_172_recursiveGen)).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("data"))).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("len"))).Apply0();
                } else {
                  r = ((((this).read__macro).Apply1(_172_recursiveGen)).Sel(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("length"), Std.Strings.__default.OfNat(_170_dim)), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_usize")))).Apply0();
                }
              }
              if (!(_171_native)) {
                r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("int!"))).AsExpr()).Apply1(r);
              }
            }
            RAST._IExpr _out160;
            Defs._IOwnership _out161;
            (this).FromOwned(r, expectedOwnership, out _out160, out _out161);
            r = _out160;
            resultingOwnership = _out161;
            readIdents = _174_recIdents;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MapKeys) {
          DAST._IExpression _177_expr = _source0.dtor_expr;
          {
            RAST._IExpr _178_recursiveGen;
            Defs._IOwnership _179___v139;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _180_recIdents;
            RAST._IExpr _out162;
            Defs._IOwnership _out163;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out164;
            (this).GenExpr(_177_expr, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out162, out _out163, out _out164);
            _178_recursiveGen = _out162;
            _179___v139 = _out163;
            _180_recIdents = _out164;
            readIdents = _180_recIdents;
            r = ((_178_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("keys"))).Apply0();
            RAST._IExpr _out165;
            Defs._IOwnership _out166;
            (this).FromOwned(r, expectedOwnership, out _out165, out _out166);
            r = _out165;
            resultingOwnership = _out166;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MapValues) {
          DAST._IExpression _181_expr = _source0.dtor_expr;
          {
            RAST._IExpr _182_recursiveGen;
            Defs._IOwnership _183___v140;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _184_recIdents;
            RAST._IExpr _out167;
            Defs._IOwnership _out168;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out169;
            (this).GenExpr(_181_expr, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out167, out _out168, out _out169);
            _182_recursiveGen = _out167;
            _183___v140 = _out168;
            _184_recIdents = _out169;
            readIdents = _184_recIdents;
            r = ((_182_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("values"))).Apply0();
            RAST._IExpr _out170;
            Defs._IOwnership _out171;
            (this).FromOwned(r, expectedOwnership, out _out170, out _out171);
            r = _out170;
            resultingOwnership = _out171;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MapItems) {
          DAST._IExpression _185_expr = _source0.dtor_expr;
          {
            RAST._IExpr _186_recursiveGen;
            Defs._IOwnership _187___v141;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _188_recIdents;
            RAST._IExpr _out172;
            Defs._IOwnership _out173;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out174;
            (this).GenExpr(_185_expr, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out172, out _out173, out _out174);
            _186_recursiveGen = _out172;
            _187___v141 = _out173;
            _188_recIdents = _out174;
            readIdents = _188_recIdents;
            r = ((_186_recursiveGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("items"))).Apply0();
            RAST._IExpr _out175;
            Defs._IOwnership _out176;
            (this).FromOwned(r, expectedOwnership, out _out175, out _out176);
            r = _out175;
            resultingOwnership = _out176;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SelectFn) {
          DAST._IExpression _189_on = _source0.dtor_expr;
          Dafny.ISequence<Dafny.Rune> _190_field = _source0.dtor_field;
          bool _191_isDatatype = _source0.dtor_onDatatype;
          bool _192_isStatic = _source0.dtor_isStatic;
          bool _193_isConstant = _source0.dtor_isConstant;
          Dafny.ISequence<DAST._IType> _194_arguments = _source0.dtor_arguments;
          {
            RAST._IExpr _195_onExpr;
            Defs._IOwnership _196_onOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _197_recIdents;
            RAST._IExpr _out177;
            Defs._IOwnership _out178;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out179;
            (this).GenExpr(_189_on, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out177, out _out178, out _out179);
            _195_onExpr = _out177;
            _196_onOwned = _out178;
            _197_recIdents = _out179;
            Dafny.ISequence<Dafny.Rune> _198_onString;
            _198_onString = (_195_onExpr)._ToString(Defs.__default.IND);
            Defs._IEnvironment _199_lEnv;
            _199_lEnv = env;
            Dafny.ISequence<_System._ITuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType>> _200_args;
            _200_args = Dafny.Sequence<_System._ITuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType>>.FromElements();
            Dafny.ISequence<RAST._IFormal> _201_parameters;
            _201_parameters = Dafny.Sequence<RAST._IFormal>.FromElements();
            BigInteger _hi7 = new BigInteger((_194_arguments).Count);
            for (BigInteger _202_i = BigInteger.Zero; _202_i < _hi7; _202_i++) {
              RAST._IType _203_ty;
              RAST._IType _out180;
              _out180 = (this).GenType((_194_arguments).Select(_202_i), Defs.GenTypeContext.@default());
              _203_ty = _out180;
              RAST._IType _204_bTy;
              _204_bTy = RAST.Type.create_Borrowed(_203_ty);
              Dafny.ISequence<Dafny.Rune> _205_name;
              _205_name = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"), Std.Strings.__default.OfInt(_202_i));
              _199_lEnv = (_199_lEnv).AddAssigned(_205_name, _204_bTy);
              _201_parameters = Dafny.Sequence<RAST._IFormal>.Concat(_201_parameters, Dafny.Sequence<RAST._IFormal>.FromElements(RAST.Formal.create(_205_name, _204_bTy)));
              _200_args = Dafny.Sequence<_System._ITuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType>>.Concat(_200_args, Dafny.Sequence<_System._ITuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType>>.FromElements(_System.Tuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType>.create(_205_name, _203_ty)));
            }
            RAST._IExpr _206_body;
            if (_192_isStatic) {
              _206_body = (_195_onExpr).FSel(Defs.__default.escapeVar(_190_field));
            } else {
              _206_body = (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("callTarget"))).Sel(Defs.__default.escapeVar(_190_field));
            }
            if (_193_isConstant) {
              _206_body = (_206_body).Apply0();
            }
            Dafny.ISequence<RAST._IExpr> _207_onExprArgs;
            _207_onExprArgs = Dafny.Sequence<RAST._IExpr>.FromElements();
            BigInteger _hi8 = new BigInteger((_200_args).Count);
            for (BigInteger _208_i = BigInteger.Zero; _208_i < _hi8; _208_i++) {
              _System._ITuple2<Dafny.ISequence<Dafny.Rune>, RAST._IType> _let_tmp_rhs1 = (_200_args).Select(_208_i);
              Dafny.ISequence<Dafny.Rune> _209_name = _let_tmp_rhs1.dtor__0;
              RAST._IType _210_ty = _let_tmp_rhs1.dtor__1;
              RAST._IExpr _211_rIdent;
              Defs._IOwnership _212___v142;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _213___v143;
              RAST._IExpr _out181;
              Defs._IOwnership _out182;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out183;
              (this).GenIdent(_209_name, selfIdent, _199_lEnv, (((!(_193_isConstant)) && ((_210_ty).CanReadWithoutClone())) ? (Defs.Ownership.create_OwnershipOwned()) : (Defs.Ownership.create_OwnershipBorrowed())), out _out181, out _out182, out _out183);
              _211_rIdent = _out181;
              _212___v142 = _out182;
              _213___v143 = _out183;
              _207_onExprArgs = Dafny.Sequence<RAST._IExpr>.Concat(_207_onExprArgs, Dafny.Sequence<RAST._IExpr>.FromElements(_211_rIdent));
            }
            _206_body = (_206_body).Apply(_207_onExprArgs);
            r = RAST.Expr.create_Lambda(_201_parameters, Std.Wrappers.Option<RAST._IType>.create_None(), _206_body);
            if (_192_isStatic) {
            } else {
              RAST._IExpr _214_target;
              if (object.Equals(_196_onOwned, Defs.Ownership.create_OwnershipOwned())) {
                _214_target = _195_onExpr;
              } else {
                _214_target = (_195_onExpr).Clone();
              }
              r = RAST.Expr.create_Block((RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("callTarget"), Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(_214_target))).Then(r));
            }
            Dafny.ISequence<RAST._IType> _215_typeShapeArgs;
            _215_typeShapeArgs = Dafny.Sequence<RAST._IType>.FromElements();
            BigInteger _hi9 = new BigInteger((_194_arguments).Count);
            for (BigInteger _216_i = BigInteger.Zero; _216_i < _hi9; _216_i++) {
              _215_typeShapeArgs = Dafny.Sequence<RAST._IType>.Concat(_215_typeShapeArgs, Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_Borrowed(RAST.Type.create_TIdentifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_")))));
            }
            RAST._IType _217_typeShape;
            _217_typeShape = RAST.Type.create_DynType(RAST.Type.create_FnType(_215_typeShapeArgs, RAST.Type.create_TIdentifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"))));
            r = RAST.Expr.create_TypeAscription((RAST.__default.std__rc__Rc__new).Apply1(r), ((RAST.__default.std__rc__Rc).AsType()).Apply(Dafny.Sequence<RAST._IType>.FromElements(_217_typeShape)));
            RAST._IExpr _out184;
            Defs._IOwnership _out185;
            (this).FromOwned(r, expectedOwnership, out _out184, out _out185);
            r = _out184;
            resultingOwnership = _out185;
            readIdents = _197_recIdents;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Select) {
          DAST._IExpression _218_on = _source0.dtor_expr;
          Dafny.ISequence<Dafny.Rune> _219_field = _source0.dtor_field;
          DAST._IFieldMutability _220_fieldMutability = _source0.dtor_fieldMutability;
          DAST._ISelectContext _221_selectContext = _source0.dtor_selectContext;
          DAST._IType _222_fieldType = _source0.dtor_isfieldType;
          {
            if (((_218_on).is_Companion) || ((_218_on).is_ExternCompanion)) {
              RAST._IExpr _223_onExpr;
              Defs._IOwnership _224_onOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _225_recIdents;
              RAST._IExpr _out186;
              Defs._IOwnership _out187;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out188;
              (this).GenExpr(_218_on, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out186, out _out187, out _out188);
              _223_onExpr = _out186;
              _224_onOwned = _out187;
              _225_recIdents = _out188;
              r = ((_223_onExpr).FSel(Defs.__default.escapeVar(_219_field))).Apply0();
              RAST._IExpr _out189;
              Defs._IOwnership _out190;
              (this).FromOwned(r, expectedOwnership, out _out189, out _out190);
              r = _out189;
              resultingOwnership = _out190;
              readIdents = _225_recIdents;
              return ;
            } else if ((_221_selectContext).is_SelectContextDatatype) {
              RAST._IExpr _226_onExpr;
              Defs._IOwnership _227_onOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _228_recIdents;
              RAST._IExpr _out191;
              Defs._IOwnership _out192;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out193;
              (this).GenExpr(_218_on, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out191, out _out192, out _out193);
              _226_onExpr = _out191;
              _227_onOwned = _out192;
              _228_recIdents = _out193;
              r = ((_226_onExpr).Sel(Defs.__default.escapeVar(_219_field))).Apply0();
              Defs._IOwnership _229_originalMutability;
              _229_originalMutability = Defs.Ownership.create_OwnershipOwned();
              DAST._IFieldMutability _source2 = _220_fieldMutability;
              {
                if (_source2.is_ConstantField) {
                  goto after_match2;
                }
              }
              {
                if (_source2.is_InternalClassConstantFieldOrDatatypeDestructor) {
                  _229_originalMutability = Defs.Ownership.create_OwnershipBorrowed();
                  goto after_match2;
                }
              }
              {
                RAST._IExpr _out194;
                _out194 = (this).Error(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("datatypes don't have mutable fields"), (this).InitEmptyExpr());
                r = _out194;
              }
            after_match2: ;
              RAST._IType _230_typ;
              RAST._IType _out195;
              _out195 = (this).GenType(_222_fieldType, Defs.GenTypeContext.@default());
              _230_typ = _out195;
              RAST._IExpr _out196;
              Defs._IOwnership _out197;
              (this).FromOwnership(r, _229_originalMutability, expectedOwnership, out _out196, out _out197);
              r = _out196;
              resultingOwnership = _out197;
              readIdents = _228_recIdents;
            } else if ((_221_selectContext).is_SelectContextGeneralTrait) {
              Defs._IOwnership _231_onOwned = Defs.Ownership.Default();
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _232_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Empty;
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
              if ((_218_on).IsThisUpcast()) {
                r = RAST.__default.self;
                _232_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"));
              } else {
                RAST._IExpr _out198;
                Defs._IOwnership _out199;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out200;
                (this).GenExpr(_218_on, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out198, out _out199, out _out200);
                r = _out198;
                _231_onOwned = _out199;
                _232_recIdents = _out200;
                if (!object.Equals(r, RAST.__default.self)) {
                  r = (((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("convert"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("AsRef"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply1(r);
                }
              }
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _232_recIdents);
              r = ((r).Sel(Defs.__default.escapeVar(_219_field))).Apply0();
              RAST._IExpr _out201;
              Defs._IOwnership _out202;
              (this).FromOwned(r, expectedOwnership, out _out201, out _out202);
              r = _out201;
              resultingOwnership = _out202;
              readIdents = _232_recIdents;
            } else {
              RAST._IExpr _233_onExpr;
              Defs._IOwnership _234_onOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _235_recIdents;
              RAST._IExpr _out203;
              Defs._IOwnership _out204;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out205;
              (this).GenExpr(_218_on, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out203, out _out204, out _out205);
              _233_onExpr = _out203;
              _234_onOwned = _out204;
              _235_recIdents = _out205;
              r = _233_onExpr;
              if (!object.Equals(_233_onExpr, RAST.__default.self)) {
                RAST._IExpr _source3 = _233_onExpr;
                {
                  if (_source3.is_UnaryOp) {
                    Dafny.ISequence<Dafny.Rune> op10 = _source3.dtor_op1;
                    if (object.Equals(op10, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"))) {
                      RAST._IExpr underlying0 = _source3.dtor_underlying;
                      if (underlying0.is_Identifier) {
                        Dafny.ISequence<Dafny.Rune> name0 = underlying0.dtor_name;
                        if (object.Equals(name0, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))) {
                          r = RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"));
                          goto after_match3;
                        }
                      }
                    }
                  }
                }
                {
                }
              after_match3: ;
                if (((this).pointerType).is_RcMut) {
                  r = (r).Clone();
                }
                r = ((this).read__macro).Apply1(r);
              }
              r = (r).Sel(Defs.__default.escapeVar(_219_field));
              DAST._IFieldMutability _source4 = _220_fieldMutability;
              {
                if (_source4.is_ConstantField) {
                  r = (r).Apply0();
                  r = (r).Clone();
                  goto after_match4;
                }
              }
              {
                if (_source4.is_InternalClassConstantFieldOrDatatypeDestructor) {
                  r = (r).Clone();
                  goto after_match4;
                }
              }
              {
                r = ((this).read__mutable__field__macro).Apply1(r);
              }
            after_match4: ;
              RAST._IExpr _out206;
              Defs._IOwnership _out207;
              (this).FromOwned(r, expectedOwnership, out _out206, out _out207);
              r = _out206;
              resultingOwnership = _out207;
              readIdents = _235_recIdents;
            }
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Index) {
          DAST._IExpression _236_on = _source0.dtor_expr;
          DAST._ICollKind _237_collKind = _source0.dtor_collKind;
          Dafny.ISequence<DAST._IExpression> _238_indices = _source0.dtor_indices;
          {
            RAST._IExpr _239_onExpr;
            Defs._IOwnership _240_onOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _241_recIdents;
            RAST._IExpr _out208;
            Defs._IOwnership _out209;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out210;
            (this).GenExpr(_236_on, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out208, out _out209, out _out210);
            _239_onExpr = _out208;
            _240_onOwned = _out209;
            _241_recIdents = _out210;
            readIdents = _241_recIdents;
            r = _239_onExpr;
            bool _242_hadArray;
            _242_hadArray = false;
            if (object.Equals(_237_collKind, DAST.CollKind.create_Array())) {
              r = ((this).read__macro).Apply1(r);
              _242_hadArray = true;
              if ((new BigInteger((_238_indices).Count)) > (BigInteger.One)) {
                r = (r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("data"));
              }
            }
            BigInteger _hi10 = new BigInteger((_238_indices).Count);
            for (BigInteger _243_i = BigInteger.Zero; _243_i < _hi10; _243_i++) {
              if (object.Equals(_237_collKind, DAST.CollKind.create_Array())) {
                RAST._IExpr _244_idx;
                Defs._IOwnership _245_idxOwned;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _246_recIdentsIdx;
                RAST._IExpr _out211;
                Defs._IOwnership _out212;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out213;
                (this).GenExpr((_238_indices).Select(_243_i), selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out211, out _out212, out _out213);
                _244_idx = _out211;
                _245_idxOwned = _out212;
                _246_recIdentsIdx = _out213;
                _244_idx = RAST.__default.IntoUsize(_244_idx);
                r = RAST.Expr.create_SelectIndex(r, _244_idx);
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _246_recIdentsIdx);
              } else {
                RAST._IExpr _247_idx;
                Defs._IOwnership _248_idxOwned;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _249_recIdentsIdx;
                RAST._IExpr _out214;
                Defs._IOwnership _out215;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out216;
                (this).GenExpr((_238_indices).Select(_243_i), selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out214, out _out215, out _out216);
                _247_idx = _out214;
                _248_idxOwned = _out215;
                _249_recIdentsIdx = _out216;
                r = ((r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("get"))).Apply1(_247_idx);
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _249_recIdentsIdx);
              }
            }
            if (_242_hadArray) {
              r = (r).Clone();
            }
            RAST._IExpr _out217;
            Defs._IOwnership _out218;
            (this).FromOwned(r, expectedOwnership, out _out217, out _out218);
            r = _out217;
            resultingOwnership = _out218;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_IndexRange) {
          DAST._IExpression _250_on = _source0.dtor_expr;
          bool _251_isArray = _source0.dtor_isArray;
          Std.Wrappers._IOption<DAST._IExpression> _252_low = _source0.dtor_low;
          Std.Wrappers._IOption<DAST._IExpression> _253_high = _source0.dtor_high;
          {
            Defs._IOwnership _254_onExpectedOwnership;
            if (_251_isArray) {
              if (((this).pointerType).is_Raw) {
                _254_onExpectedOwnership = Defs.Ownership.create_OwnershipOwned();
              } else {
                _254_onExpectedOwnership = Defs.Ownership.create_OwnershipBorrowed();
              }
            } else {
              _254_onExpectedOwnership = Defs.Ownership.create_OwnershipAutoBorrowed();
            }
            RAST._IExpr _255_onExpr;
            Defs._IOwnership _256_onOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _257_recIdents;
            RAST._IExpr _out219;
            Defs._IOwnership _out220;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out221;
            (this).GenExpr(_250_on, selfIdent, env, _254_onExpectedOwnership, out _out219, out _out220, out _out221);
            _255_onExpr = _out219;
            _256_onOwned = _out220;
            _257_recIdents = _out221;
            readIdents = _257_recIdents;
            Dafny.ISequence<Dafny.Rune> _258_methodName;
            if ((_252_low).is_Some) {
              if ((_253_high).is_Some) {
                _258_methodName = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("slice");
              } else {
                _258_methodName = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("drop");
              }
            } else if ((_253_high).is_Some) {
              _258_methodName = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("take");
            } else {
              _258_methodName = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
            }
            Dafny.ISequence<RAST._IExpr> _259_arguments;
            _259_arguments = Dafny.Sequence<RAST._IExpr>.FromElements();
            Std.Wrappers._IOption<DAST._IExpression> _source5 = _252_low;
            {
              if (_source5.is_Some) {
                DAST._IExpression _260_l = _source5.dtor_value;
                {
                  RAST._IExpr _261_lExpr;
                  Defs._IOwnership _262___v146;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _263_recIdentsL;
                  RAST._IExpr _out222;
                  Defs._IOwnership _out223;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out224;
                  (this).GenExpr(_260_l, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out222, out _out223, out _out224);
                  _261_lExpr = _out222;
                  _262___v146 = _out223;
                  _263_recIdentsL = _out224;
                  _259_arguments = Dafny.Sequence<RAST._IExpr>.Concat(_259_arguments, Dafny.Sequence<RAST._IExpr>.FromElements(_261_lExpr));
                  readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _263_recIdentsL);
                }
                goto after_match5;
              }
            }
            {
            }
          after_match5: ;
            Std.Wrappers._IOption<DAST._IExpression> _source6 = _253_high;
            {
              if (_source6.is_Some) {
                DAST._IExpression _264_h = _source6.dtor_value;
                {
                  RAST._IExpr _265_hExpr;
                  Defs._IOwnership _266___v147;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _267_recIdentsH;
                  RAST._IExpr _out225;
                  Defs._IOwnership _out226;
                  Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out227;
                  (this).GenExpr(_264_h, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out225, out _out226, out _out227);
                  _265_hExpr = _out225;
                  _266___v147 = _out226;
                  _267_recIdentsH = _out227;
                  _259_arguments = Dafny.Sequence<RAST._IExpr>.Concat(_259_arguments, Dafny.Sequence<RAST._IExpr>.FromElements(_265_hExpr));
                  readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _267_recIdentsH);
                }
                goto after_match6;
              }
            }
            {
            }
          after_match6: ;
            r = _255_onExpr;
            if (_251_isArray) {
              if (!(_258_methodName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) {
                _258_methodName = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"), _258_methodName);
              }
              Dafny.ISequence<Dafny.Rune> _268_object__suffix;
              if (((this).pointerType).is_Raw) {
                _268_object__suffix = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("");
              } else {
                _268_object__suffix = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_object");
              }
              r = ((RAST.__default.dafny__runtime__Sequence).FSel(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from_array"), _258_methodName), _268_object__suffix))).Apply(Dafny.Sequence<RAST._IExpr>.Concat(Dafny.Sequence<RAST._IExpr>.FromElements(_255_onExpr), _259_arguments));
            } else {
              if (!(_258_methodName).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))) {
                r = ((r).Sel(_258_methodName)).Apply(_259_arguments);
              } else {
                r = (r).Clone();
              }
            }
            RAST._IExpr _out228;
            Defs._IOwnership _out229;
            (this).FromOwned(r, expectedOwnership, out _out228, out _out229);
            r = _out228;
            resultingOwnership = _out229;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_TupleSelect) {
          DAST._IExpression _269_on = _source0.dtor_expr;
          BigInteger _270_idx = _source0.dtor_index;
          DAST._IType _271_fieldType = _source0.dtor_fieldType;
          {
            RAST._IExpr _272_onExpr;
            Defs._IOwnership _273_onOwnership;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _274_recIdents;
            RAST._IExpr _out230;
            Defs._IOwnership _out231;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out232;
            (this).GenExpr(_269_on, selfIdent, env, Defs.Ownership.create_OwnershipAutoBorrowed(), out _out230, out _out231, out _out232);
            _272_onExpr = _out230;
            _273_onOwnership = _out231;
            _274_recIdents = _out232;
            Dafny.ISequence<Dafny.Rune> _275_selName;
            _275_selName = Std.Strings.__default.OfNat(_270_idx);
            DAST._IType _source7 = _271_fieldType;
            {
              if (_source7.is_Tuple) {
                Dafny.ISequence<DAST._IType> _276_tps = _source7.dtor_Tuple_a0;
                if (((_271_fieldType).is_Tuple) && ((new BigInteger((_276_tps).Count)) > (RAST.__default.MAX__TUPLE__SIZE))) {
                  _275_selName = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"), _275_selName);
                }
                goto after_match7;
              }
            }
            {
            }
          after_match7: ;
            r = ((_272_onExpr).Sel(_275_selName)).Clone();
            RAST._IExpr _out233;
            Defs._IOwnership _out234;
            (this).FromOwnership(r, Defs.Ownership.create_OwnershipOwned(), expectedOwnership, out _out233, out _out234);
            r = _out233;
            resultingOwnership = _out234;
            readIdents = _274_recIdents;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Call) {
          DAST._IExpression _277_on = _source0.dtor_on;
          DAST._ICallName _278_name = _source0.dtor_callName;
          Dafny.ISequence<DAST._IType> _279_typeArgs = _source0.dtor_typeArgs;
          Dafny.ISequence<DAST._IExpression> _280_args = _source0.dtor_args;
          {
            RAST._IExpr _out235;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out236;
            (this).GenOwnedCallPart(_277_on, selfIdent, _278_name, _279_typeArgs, _280_args, env, out _out235, out _out236);
            r = _out235;
            readIdents = _out236;
            RAST._IExpr _out237;
            Defs._IOwnership _out238;
            (this).FromOwned(r, expectedOwnership, out _out237, out _out238);
            r = _out237;
            resultingOwnership = _out238;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Lambda) {
          Dafny.ISequence<DAST._IFormal> _281_paramsDafny = _source0.dtor_params;
          DAST._IType _282_retType = _source0.dtor_retType;
          Dafny.ISequence<DAST._IStatement> _283_body = _source0.dtor_body;
          {
            Dafny.ISequence<RAST._IFormal> _284_params;
            Dafny.ISequence<RAST._IFormal> _out239;
            _out239 = (this).GenParams(_281_paramsDafny, _281_paramsDafny, true);
            _284_params = _out239;
            Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _285_paramNames;
            _285_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
            Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> _286_paramTypesMap;
            _286_paramTypesMap = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.FromElements();
            BigInteger _hi11 = new BigInteger((_284_params).Count);
            for (BigInteger _287_i = BigInteger.Zero; _287_i < _hi11; _287_i++) {
              Dafny.ISequence<Dafny.Rune> _288_name;
              _288_name = ((_284_params).Select(_287_i)).dtor_name;
              _285_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_285_paramNames, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_288_name));
              _286_paramTypesMap = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_286_paramTypesMap, _288_name, ((_284_params).Select(_287_i)).dtor_tpe);
            }
            Defs._IEnvironment _289_subEnv;
            _289_subEnv = ((env).ToOwned()).merge(Defs.Environment.create(_285_paramNames, _286_paramTypesMap, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements()));
            RAST._IExpr _290_recursiveGen;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _291_recIdents;
            Defs._IEnvironment _292___v149;
            RAST._IExpr _out240;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out241;
            Defs._IEnvironment _out242;
            (this).GenStmts(_283_body, ((!object.Equals(selfIdent, Defs.SelfInfo.create_NoSelf())) ? (Defs.SelfInfo.create_ThisTyped(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_this"), (selfIdent).dtor_dafnyType)) : (Defs.SelfInfo.create_NoSelf())), _289_subEnv, true, Std.Wrappers.Option<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>>.create_None(), out _out240, out _out241, out _out242);
            _290_recursiveGen = _out240;
            _291_recIdents = _out241;
            _292___v149 = _out242;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            _291_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_291_recIdents, Dafny.Helpers.Id<Func<Dafny.ISequence<Dafny.ISequence<Dafny.Rune>>, Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>>((_293_paramNames) => ((System.Func<Dafny.ISet<Dafny.ISequence<Dafny.Rune>>>)(() => {
              var _coll0 = new System.Collections.Generic.List<Dafny.ISequence<Dafny.Rune>>();
              foreach (Dafny.ISequence<Dafny.Rune> _compr_0 in (_293_paramNames).CloneAsArray()) {
                Dafny.ISequence<Dafny.Rune> _294_name = (Dafny.ISequence<Dafny.Rune>)_compr_0;
                if ((_293_paramNames).Contains(_294_name)) {
                  _coll0.Add(_294_name);
                }
              }
              return Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromCollection(_coll0);
            }))())(_285_paramNames));
            RAST._IExpr _295_allReadCloned;
            _295_allReadCloned = (this).InitEmptyExpr();
            while (!(_291_recIdents).Equals(Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements())) {
              Dafny.ISequence<Dafny.Rune> _296_next;
              foreach (Dafny.ISequence<Dafny.Rune> _assign_such_that_1 in (_291_recIdents).Elements) {
                _296_next = (Dafny.ISequence<Dafny.Rune>)_assign_such_that_1;
                if ((_291_recIdents).Contains(_296_next)) {
                  goto after__ASSIGN_SUCH_THAT_1;
                }
              }
              throw new System.Exception("assign-such-that search produced no value");
            after__ASSIGN_SUCH_THAT_1: ;
              if ((!object.Equals(selfIdent, Defs.SelfInfo.create_NoSelf())) && ((_296_next).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_this")))) {
                RAST._IExpr _297_selfCloned;
                Defs._IOwnership _298___v150;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _299___v151;
                RAST._IExpr _out243;
                Defs._IOwnership _out244;
                Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out245;
                (this).GenIdent(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("self"), selfIdent, Defs.Environment.Empty(), Defs.Ownership.create_OwnershipOwned(), out _out243, out _out244, out _out245);
                _297_selfCloned = _out243;
                _298___v150 = _out244;
                _299___v151 = _out245;
                _295_allReadCloned = (_295_allReadCloned).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_this"), Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(_297_selfCloned)));
              } else if (!((_285_paramNames).Contains(_296_next))) {
                RAST._IExpr _300_copy;
                _300_copy = (RAST.Expr.create_Identifier(_296_next)).Clone();
                _295_allReadCloned = (_295_allReadCloned).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), _296_next, Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(_300_copy)));
                readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_296_next));
              }
              _291_recIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_291_recIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_296_next));
            }
            RAST._IType _301_retTypeGen;
            RAST._IType _out246;
            _out246 = (this).GenType(_282_retType, Defs.GenTypeContext.@default());
            _301_retTypeGen = _out246;
            r = RAST.Expr.create_Block((_295_allReadCloned).Then(RAST.__default.RcNew(RAST.Expr.create_Lambda(_284_params, Std.Wrappers.Option<RAST._IType>.create_Some(_301_retTypeGen), RAST.Expr.create_Block(_290_recursiveGen)))));
            RAST._IExpr _out247;
            Defs._IOwnership _out248;
            (this).FromOwned(r, expectedOwnership, out _out247, out _out248);
            r = _out247;
            resultingOwnership = _out248;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_BetaRedex) {
          Dafny.ISequence<_System._ITuple2<DAST._IFormal, DAST._IExpression>> _302_values = _source0.dtor_values;
          DAST._IType _303_retType = _source0.dtor_retType;
          DAST._IExpression _304_expr = _source0.dtor_expr;
          {
            Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _305_paramNames;
            _305_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements();
            Dafny.ISequence<RAST._IFormal> _306_paramFormals;
            Dafny.ISequence<RAST._IFormal> _out249;
            _out249 = (this).GenParams(Std.Collections.Seq.__default.Map<_System._ITuple2<DAST._IFormal, DAST._IExpression>, DAST._IFormal>(((System.Func<_System._ITuple2<DAST._IFormal, DAST._IExpression>, DAST._IFormal>)((_307_value) => {
              return (_307_value).dtor__0;
            })), _302_values), Std.Collections.Seq.__default.Map<_System._ITuple2<DAST._IFormal, DAST._IExpression>, DAST._IFormal>(((System.Func<_System._ITuple2<DAST._IFormal, DAST._IExpression>, DAST._IFormal>)((_307_value) => {
              return (_307_value).dtor__0;
            })), _302_values), false);
            _306_paramFormals = _out249;
            Dafny.IMap<Dafny.ISequence<Dafny.Rune>,RAST._IType> _308_paramTypes;
            _308_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.FromElements();
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _309_paramNamesSet;
            _309_paramNamesSet = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            BigInteger _hi12 = new BigInteger((_302_values).Count);
            for (BigInteger _310_i = BigInteger.Zero; _310_i < _hi12; _310_i++) {
              Dafny.ISequence<Dafny.Rune> _311_name;
              _311_name = (((_302_values).Select(_310_i)).dtor__0).dtor_name;
              Dafny.ISequence<Dafny.Rune> _312_rName;
              _312_rName = Defs.__default.escapeVar(_311_name);
              _305_paramNames = Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_305_paramNames, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_312_rName));
              _308_paramTypes = Dafny.Map<Dafny.ISequence<Dafny.Rune>, RAST._IType>.Update(_308_paramTypes, _312_rName, ((_306_paramFormals).Select(_310_i)).dtor_tpe);
              _309_paramNamesSet = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_309_paramNamesSet, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_312_rName));
            }
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            r = (this).InitEmptyExpr();
            BigInteger _hi13 = new BigInteger((_302_values).Count);
            for (BigInteger _313_i = BigInteger.Zero; _313_i < _hi13; _313_i++) {
              RAST._IType _314_typeGen;
              RAST._IType _out250;
              _out250 = (this).GenType((((_302_values).Select(_313_i)).dtor__0).dtor_typ, Defs.GenTypeContext.@default());
              _314_typeGen = _out250;
              RAST._IExpr _315_valueGen;
              Defs._IOwnership _316___v152;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _317_recIdents;
              RAST._IExpr _out251;
              Defs._IOwnership _out252;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out253;
              (this).GenExpr(((_302_values).Select(_313_i)).dtor__1, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out251, out _out252, out _out253);
              _315_valueGen = _out251;
              _316___v152 = _out252;
              _317_recIdents = _out253;
              r = (r).Then(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), Defs.__default.escapeVar((((_302_values).Select(_313_i)).dtor__0).dtor_name), Std.Wrappers.Option<RAST._IType>.create_Some(_314_typeGen), Std.Wrappers.Option<RAST._IExpr>.create_Some(_315_valueGen)));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _317_recIdents);
            }
            Defs._IEnvironment _318_newEnv;
            _318_newEnv = Defs.Environment.create(_305_paramNames, _308_paramTypes, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements());
            RAST._IExpr _319_recGen;
            Defs._IOwnership _320_recOwned;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _321_recIdents;
            RAST._IExpr _out254;
            Defs._IOwnership _out255;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out256;
            (this).GenExpr(_304_expr, selfIdent, _318_newEnv, expectedOwnership, out _out254, out _out255, out _out256);
            _319_recGen = _out254;
            _320_recOwned = _out255;
            _321_recIdents = _out256;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_321_recIdents, _309_paramNamesSet);
            r = RAST.Expr.create_Block((r).Then(_319_recGen));
            RAST._IExpr _out257;
            Defs._IOwnership _out258;
            (this).FromOwnership(r, _320_recOwned, expectedOwnership, out _out257, out _out258);
            r = _out257;
            resultingOwnership = _out258;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_IIFE) {
          Dafny.ISequence<Dafny.Rune> _322_name = _source0.dtor_ident;
          DAST._IType _323_tpe = _source0.dtor_typ;
          DAST._IExpression _324_value = _source0.dtor_value;
          DAST._IExpression _325_iifeBody = _source0.dtor_iifeBody;
          {
            RAST._IExpr _326_valueGen;
            Defs._IOwnership _327___v153;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _328_recIdents;
            RAST._IExpr _out259;
            Defs._IOwnership _out260;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out261;
            (this).GenExpr(_324_value, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out259, out _out260, out _out261);
            _326_valueGen = _out259;
            _327___v153 = _out260;
            _328_recIdents = _out261;
            readIdents = _328_recIdents;
            RAST._IType _329_valueTypeGen;
            RAST._IType _out262;
            _out262 = (this).GenType(_323_tpe, Defs.GenTypeContext.@default());
            _329_valueTypeGen = _out262;
            Dafny.ISequence<Dafny.Rune> _330_iifeVar;
            _330_iifeVar = Defs.__default.escapeVar(_322_name);
            RAST._IExpr _331_bodyGen;
            Defs._IOwnership _332___v154;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _333_bodyIdents;
            RAST._IExpr _out263;
            Defs._IOwnership _out264;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out265;
            (this).GenExpr(_325_iifeBody, selfIdent, (env).AddAssigned(_330_iifeVar, _329_valueTypeGen), Defs.Ownership.create_OwnershipOwned(), out _out263, out _out264, out _out265);
            _331_bodyGen = _out263;
            _332___v154 = _out264;
            _333_bodyIdents = _out265;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Difference(_333_bodyIdents, Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements(_330_iifeVar)));
            r = RAST.Expr.create_Block((RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), _330_iifeVar, Std.Wrappers.Option<RAST._IType>.create_Some(_329_valueTypeGen), Std.Wrappers.Option<RAST._IExpr>.create_Some(_326_valueGen))).Then(_331_bodyGen));
            RAST._IExpr _out266;
            Defs._IOwnership _out267;
            (this).FromOwned(r, expectedOwnership, out _out266, out _out267);
            r = _out266;
            resultingOwnership = _out267;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Apply) {
          DAST._IExpression _334_func = _source0.dtor_expr;
          Dafny.ISequence<DAST._IExpression> _335_args = _source0.dtor_args;
          {
            RAST._IExpr _336_funcExpr;
            Defs._IOwnership _337___v155;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _338_recIdents;
            RAST._IExpr _out268;
            Defs._IOwnership _out269;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out270;
            (this).GenExpr(_334_func, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out268, out _out269, out _out270);
            _336_funcExpr = _out268;
            _337___v155 = _out269;
            _338_recIdents = _out270;
            readIdents = _338_recIdents;
            Dafny.ISequence<RAST._IExpr> _339_rArgs;
            _339_rArgs = Dafny.Sequence<RAST._IExpr>.FromElements();
            BigInteger _hi14 = new BigInteger((_335_args).Count);
            for (BigInteger _340_i = BigInteger.Zero; _340_i < _hi14; _340_i++) {
              RAST._IExpr _341_argExpr;
              Defs._IOwnership _342_argOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _343_argIdents;
              RAST._IExpr _out271;
              Defs._IOwnership _out272;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out273;
              (this).GenExpr((_335_args).Select(_340_i), selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out271, out _out272, out _out273);
              _341_argExpr = _out271;
              _342_argOwned = _out272;
              _343_argIdents = _out273;
              _339_rArgs = Dafny.Sequence<RAST._IExpr>.Concat(_339_rArgs, Dafny.Sequence<RAST._IExpr>.FromElements(_341_argExpr));
              readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(readIdents, _343_argIdents);
            }
            r = (_336_funcExpr).Apply(_339_rArgs);
            RAST._IExpr _out274;
            Defs._IOwnership _out275;
            (this).FromOwned(r, expectedOwnership, out _out274, out _out275);
            r = _out274;
            resultingOwnership = _out275;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_TypeTest) {
          DAST._IExpression _344_on = _source0.dtor_on;
          Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> _345_dType = _source0.dtor_dType;
          Dafny.ISequence<Dafny.Rune> _346_variant = _source0.dtor_variant;
          {
            RAST._IExpr _347_exprGen;
            Defs._IOwnership _348___v156;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _349_recIdents;
            RAST._IExpr _out276;
            Defs._IOwnership _out277;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out278;
            (this).GenExpr(_344_on, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out276, out _out277, out _out278);
            _347_exprGen = _out276;
            _348___v156 = _out277;
            _349_recIdents = _out278;
            RAST._IExpr _350_variantExprPath;
            RAST._IExpr _out279;
            _out279 = (this).GenPathExpr(Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.Concat(_345_dType, Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(_346_variant)), true);
            _350_variantExprPath = _out279;
            r = (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("matches!"))).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(((_347_exprGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply0(), RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("{ .. }"), _350_variantExprPath, DAST.Format.UnaryOpFormat.create_NoFormat())));
            RAST._IExpr _out280;
            Defs._IOwnership _out281;
            (this).FromOwned(r, expectedOwnership, out _out280, out _out281);
            r = _out280;
            resultingOwnership = _out281;
            readIdents = _349_recIdents;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_Is) {
          DAST._IExpression _351_expr = _source0.dtor_expr;
          DAST._IType _352_fromTyp = _source0.dtor_fromType;
          DAST._IType _353_toTyp = _source0.dtor_toType;
          {
            RAST._IType _354_fromTpe;
            RAST._IType _out282;
            _out282 = (this).GenType(_352_fromTyp, Defs.GenTypeContext.@default());
            _354_fromTpe = _out282;
            RAST._IType _355_toTpe;
            RAST._IType _out283;
            _out283 = (this).GenType(_353_toTyp, Defs.GenTypeContext.@default());
            _355_toTpe = _out283;
            if (((_354_fromTpe).IsObjectOrPointer()) && ((_355_toTpe).IsObjectOrPointer())) {
              RAST._IExpr _356_expr;
              Defs._IOwnership _357_recOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _358_recIdents;
              RAST._IExpr _out284;
              Defs._IOwnership _out285;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out286;
              (this).GenExpr(_351_expr, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out284, out _out285, out _out286);
              _356_expr = _out284;
              _357_recOwned = _out285;
              _358_recIdents = _out286;
              r = (((_356_expr).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("is_instance_of"))).ApplyType(Dafny.Sequence<RAST._IType>.FromElements((_355_toTpe).ObjectOrPointerUnderlying()))).Apply0();
              RAST._IExpr _out287;
              Defs._IOwnership _out288;
              (this).FromOwnership(r, _357_recOwned, expectedOwnership, out _out287, out _out288);
              r = _out287;
              resultingOwnership = _out288;
              readIdents = _358_recIdents;
            } else {
              RAST._IExpr _359_expr;
              Defs._IOwnership _360_recOwned;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _361_recIdents;
              RAST._IExpr _out289;
              Defs._IOwnership _out290;
              Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out291;
              (this).GenExpr(_351_expr, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out289, out _out290, out _out291);
              _359_expr = _out289;
              _360_recOwned = _out290;
              _361_recIdents = _out291;
              bool _362_isDatatype;
              _362_isDatatype = (_353_toTyp).IsDatatype();
              bool _363_isGeneralTrait;
              _363_isGeneralTrait = (!(_362_isDatatype)) && ((_353_toTyp).IsGeneralTrait());
              if ((_362_isDatatype) || (_363_isGeneralTrait)) {
                bool _364_isDowncast;
                _364_isDowncast = (_353_toTyp).Extends(_352_fromTyp);
                if (_364_isDowncast) {
                  DAST._IType _365_underlyingType;
                  if (_362_isDatatype) {
                    _365_underlyingType = (_353_toTyp).GetDatatypeType();
                  } else {
                    _365_underlyingType = (_353_toTyp).GetGeneralTraitType();
                  }
                  RAST._IType _366_toTpeRaw;
                  RAST._IType _out292;
                  _out292 = (this).GenType(_365_underlyingType, Defs.GenTypeContext.@default());
                  _366_toTpeRaw = _out292;
                  Std.Wrappers._IOption<RAST._IExpr> _367_toTpeRawDowncastOpt;
                  _367_toTpeRawDowncastOpt = (_366_toTpeRaw).ToDowncastExpr();
                  if ((_367_toTpeRawDowncastOpt).is_Some) {
                    _359_expr = (this).FromGeneralBorrowToSelfBorrow(_359_expr, Defs.Ownership.create_OwnershipBorrowed(), env);
                    if (_362_isDatatype) {
                      _359_expr = ((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("AnyRef"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_any_ref"))).Apply1(_359_expr);
                    }
                    r = (((_367_toTpeRawDowncastOpt).dtor_value).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_is"))).Apply1(_359_expr);
                    _360_recOwned = Defs.Ownership.create_OwnershipOwned();
                  } else {
                    RAST._IExpr _out293;
                    _out293 = (this).Error(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Could not convert "), (_366_toTpeRaw)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(" to a Downcast trait")), (this).InitEmptyExpr());
                    r = _out293;
                  }
                } else {
                  RAST._IExpr _out294;
                  _out294 = (this).Error(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Needs support for upcasting general traits"), (this).InitEmptyExpr());
                  r = _out294;
                }
              } else {
                RAST._IExpr _out295;
                _out295 = (this).Error(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Source and/or target types of type test is/are not Object, Ptr, General trait or Datatype"), (this).InitEmptyExpr());
                r = _out295;
              }
              RAST._IExpr _out296;
              Defs._IOwnership _out297;
              (this).FromOwnership(r, _360_recOwned, expectedOwnership, out _out296, out _out297);
              r = _out296;
              resultingOwnership = _out297;
              readIdents = _361_recIdents;
            }
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_BoolBoundedPool) {
          {
            r = RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("[false, true]"));
            RAST._IExpr _out298;
            Defs._IOwnership _out299;
            (this).FromOwned(r, expectedOwnership, out _out298, out _out299);
            r = _out298;
            resultingOwnership = _out299;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SetBoundedPool) {
          DAST._IExpression _368_of = _source0.dtor_of;
          {
            RAST._IExpr _369_exprGen;
            Defs._IOwnership _370___v157;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _371_recIdents;
            RAST._IExpr _out300;
            Defs._IOwnership _out301;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out302;
            (this).GenExpr(_368_of, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out300, out _out301, out _out302);
            _369_exprGen = _out300;
            _370___v157 = _out301;
            _371_recIdents = _out302;
            r = ((_369_exprGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("iter"))).Apply0();
            RAST._IExpr _out303;
            Defs._IOwnership _out304;
            (this).FromOwned(r, expectedOwnership, out _out303, out _out304);
            r = _out303;
            resultingOwnership = _out304;
            readIdents = _371_recIdents;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SeqBoundedPool) {
          DAST._IExpression _372_of = _source0.dtor_of;
          bool _373_includeDuplicates = _source0.dtor_includeDuplicates;
          {
            RAST._IExpr _374_exprGen;
            Defs._IOwnership _375___v158;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _376_recIdents;
            RAST._IExpr _out305;
            Defs._IOwnership _out306;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out307;
            (this).GenExpr(_372_of, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out305, out _out306, out _out307);
            _374_exprGen = _out305;
            _375___v158 = _out306;
            _376_recIdents = _out307;
            r = ((_374_exprGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("iter"))).Apply0();
            if (!(_373_includeDuplicates)) {
              r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("itertools"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Itertools"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unique"))).Apply1(r);
            }
            RAST._IExpr _out308;
            Defs._IOwnership _out309;
            (this).FromOwned(r, expectedOwnership, out _out308, out _out309);
            r = _out308;
            resultingOwnership = _out309;
            readIdents = _376_recIdents;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MultisetBoundedPool) {
          DAST._IExpression _377_of = _source0.dtor_of;
          bool _378_includeDuplicates = _source0.dtor_includeDuplicates;
          {
            RAST._IExpr _379_exprGen;
            Defs._IOwnership _380___v159;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _381_recIdents;
            RAST._IExpr _out310;
            Defs._IOwnership _out311;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out312;
            (this).GenExpr(_377_of, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out310, out _out311, out _out312);
            _379_exprGen = _out310;
            _380___v159 = _out311;
            _381_recIdents = _out312;
            r = ((_379_exprGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("iter"))).Apply0();
            if (!(_378_includeDuplicates)) {
              r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("itertools"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Itertools"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unique"))).Apply1(r);
            }
            RAST._IExpr _out313;
            Defs._IOwnership _out314;
            (this).FromOwned(r, expectedOwnership, out _out313, out _out314);
            r = _out313;
            resultingOwnership = _out314;
            readIdents = _381_recIdents;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MapBoundedPool) {
          DAST._IExpression _382_of = _source0.dtor_of;
          {
            RAST._IExpr _383_exprGen;
            Defs._IOwnership _384___v160;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _385_recIdents;
            RAST._IExpr _out315;
            Defs._IOwnership _out316;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out317;
            (this).GenExpr(_382_of, selfIdent, env, Defs.Ownership.create_OwnershipBorrowed(), out _out315, out _out316, out _out317);
            _383_exprGen = _out315;
            _384___v160 = _out316;
            _385_recIdents = _out317;
            r = ((((_383_exprGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("keys"))).Apply0()).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("iter"))).Apply0();
            readIdents = _385_recIdents;
            RAST._IExpr _out318;
            Defs._IOwnership _out319;
            (this).FromOwned(r, expectedOwnership, out _out318, out _out319);
            r = _out318;
            resultingOwnership = _out319;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_ExactBoundedPool) {
          DAST._IExpression _386_of = _source0.dtor_of;
          {
            RAST._IExpr _387_exprGen;
            Defs._IOwnership _388___v161;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _389_recIdents;
            RAST._IExpr _out320;
            Defs._IOwnership _out321;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out322;
            (this).GenExpr(_386_of, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out320, out _out321, out _out322);
            _387_exprGen = _out320;
            _388___v161 = _out321;
            _389_recIdents = _out322;
            r = ((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("iter"))).AsExpr()).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("once"))).Apply1(_387_exprGen);
            readIdents = _389_recIdents;
            RAST._IExpr _out323;
            Defs._IOwnership _out324;
            (this).FromOwned(r, expectedOwnership, out _out323, out _out324);
            r = _out323;
            resultingOwnership = _out324;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_IntRange) {
          DAST._IType _390_typ = _source0.dtor_elemType;
          DAST._IExpression _391_lo = _source0.dtor_lo;
          DAST._IExpression _392_hi = _source0.dtor_hi;
          bool _393_up = _source0.dtor_up;
          {
            RAST._IExpr _394_lo;
            Defs._IOwnership _395___v162;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _396_recIdentsLo;
            RAST._IExpr _out325;
            Defs._IOwnership _out326;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out327;
            (this).GenExpr(_391_lo, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out325, out _out326, out _out327);
            _394_lo = _out325;
            _395___v162 = _out326;
            _396_recIdentsLo = _out327;
            RAST._IExpr _397_hi;
            Defs._IOwnership _398___v163;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _399_recIdentsHi;
            RAST._IExpr _out328;
            Defs._IOwnership _out329;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out330;
            (this).GenExpr(_392_hi, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out328, out _out329, out _out330);
            _397_hi = _out328;
            _398___v163 = _out329;
            _399_recIdentsHi = _out330;
            if (_393_up) {
              r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("integer_range"))).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_394_lo, _397_hi));
            } else {
              r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("integer_range_down"))).AsExpr()).Apply(Dafny.Sequence<RAST._IExpr>.FromElements(_397_hi, _394_lo));
            }
            if (!((_390_typ).is_Primitive)) {
              RAST._IType _400_tpe;
              RAST._IType _out331;
              _out331 = (this).GenType(_390_typ, Defs.GenTypeContext.@default());
              _400_tpe = _out331;
              r = ((r).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("map"))).Apply1((((((RAST.__default.std).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("convert"))).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Into"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_400_tpe))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("into")));
            }
            RAST._IExpr _out332;
            Defs._IOwnership _out333;
            (this).FromOwned(r, expectedOwnership, out _out332, out _out333);
            r = _out332;
            resultingOwnership = _out333;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_396_recIdentsLo, _399_recIdentsHi);
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_UnboundedIntRange) {
          DAST._IExpression _401_start = _source0.dtor_start;
          bool _402_up = _source0.dtor_up;
          {
            RAST._IExpr _403_start;
            Defs._IOwnership _404___v164;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _405_recIdentStart;
            RAST._IExpr _out334;
            Defs._IOwnership _out335;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out336;
            (this).GenExpr(_401_start, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out334, out _out335, out _out336);
            _403_start = _out334;
            _404___v164 = _out335;
            _405_recIdentStart = _out336;
            if (_402_up) {
              r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("integer_range_unbounded"))).AsExpr()).Apply1(_403_start);
            } else {
              r = (((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("integer_range_down_unbounded"))).AsExpr()).Apply1(_403_start);
            }
            RAST._IExpr _out337;
            Defs._IOwnership _out338;
            (this).FromOwned(r, expectedOwnership, out _out337, out _out338);
            r = _out337;
            resultingOwnership = _out338;
            readIdents = _405_recIdentStart;
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_MapBuilder) {
          DAST._IType _406_keyType = _source0.dtor_keyType;
          DAST._IType _407_valueType = _source0.dtor_valueType;
          {
            RAST._IType _408_kType;
            RAST._IType _out339;
            _out339 = (this).GenType(_406_keyType, Defs.GenTypeContext.@default());
            _408_kType = _out339;
            RAST._IType _409_vType;
            RAST._IType _out340;
            _out340 = (this).GenType(_407_valueType, Defs.GenTypeContext.@default());
            _409_vType = _out340;
            r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("MapBuilder"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_408_kType, _409_vType))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply0();
            RAST._IExpr _out341;
            Defs._IOwnership _out342;
            (this).FromOwned(r, expectedOwnership, out _out341, out _out342);
            r = _out341;
            resultingOwnership = _out342;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            return ;
          }
          goto after_match0;
        }
      }
      {
        if (_source0.is_SetBuilder) {
          DAST._IType _410_elemType = _source0.dtor_elemType;
          {
            RAST._IType _411_eType;
            RAST._IType _out343;
            _out343 = (this).GenType(_410_elemType, Defs.GenTypeContext.@default());
            _411_eType = _out343;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
            r = (((((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("SetBuilder"))).AsExpr()).ApplyType(Dafny.Sequence<RAST._IType>.FromElements(_411_eType))).FSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("new"))).Apply0();
            RAST._IExpr _out344;
            Defs._IOwnership _out345;
            (this).FromOwned(r, expectedOwnership, out _out344, out _out345);
            r = _out344;
            resultingOwnership = _out345;
            return ;
          }
          goto after_match0;
        }
      }
      {
        DAST._IType _412_elemType = _source0.dtor_elemType;
        DAST._IExpression _413_collection = _source0.dtor_collection;
        bool _414_is__forall = _source0.dtor_is__forall;
        DAST._IExpression _415_lambda = _source0.dtor_lambda;
        {
          RAST._IType _416_tpe;
          RAST._IType _out346;
          _out346 = (this).GenType(_412_elemType, Defs.GenTypeContext.@default());
          _416_tpe = _out346;
          RAST._IExpr _417_collectionGen;
          Defs._IOwnership _418___v165;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _419_recIdents;
          RAST._IExpr _out347;
          Defs._IOwnership _out348;
          Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out349;
          (this).GenExpr(_413_collection, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out347, out _out348, out _out349);
          _417_collectionGen = _out347;
          _418___v165 = _out348;
          _419_recIdents = _out349;
          Dafny.ISequence<DAST._IAttribute> _420_extraAttributes;
          _420_extraAttributes = Dafny.Sequence<DAST._IAttribute>.FromElements();
          if ((((((_413_collection).is_IntRange) || ((_413_collection).is_UnboundedIntRange)) || ((_413_collection).is_SeqBoundedPool)) || ((_413_collection).is_ExactBoundedPool)) || ((_413_collection).is_MultisetBoundedPool)) {
            _420_extraAttributes = Dafny.Sequence<DAST._IAttribute>.FromElements(Defs.__default.AttributeOwned);
          }
          if ((_415_lambda).is_Lambda) {
            Dafny.ISequence<DAST._IFormal> _421_formals;
            _421_formals = (_415_lambda).dtor_params;
            Dafny.ISequence<DAST._IFormal> _422_newFormals;
            _422_newFormals = Dafny.Sequence<DAST._IFormal>.FromElements();
            BigInteger _hi15 = new BigInteger((_421_formals).Count);
            for (BigInteger _423_i = BigInteger.Zero; _423_i < _hi15; _423_i++) {
              var _pat_let_tv0 = _420_extraAttributes;
              var _pat_let_tv1 = _421_formals;
              _422_newFormals = Dafny.Sequence<DAST._IFormal>.Concat(_422_newFormals, Dafny.Sequence<DAST._IFormal>.FromElements(Dafny.Helpers.Let<DAST._IFormal, DAST._IFormal>((_421_formals).Select(_423_i), _pat_let28_0 => Dafny.Helpers.Let<DAST._IFormal, DAST._IFormal>(_pat_let28_0, _424_dt__update__tmp_h0 => Dafny.Helpers.Let<Dafny.ISequence<DAST._IAttribute>, DAST._IFormal>(Dafny.Sequence<DAST._IAttribute>.Concat(_pat_let_tv0, ((_pat_let_tv1).Select(_423_i)).dtor_attributes), _pat_let29_0 => Dafny.Helpers.Let<Dafny.ISequence<DAST._IAttribute>, DAST._IFormal>(_pat_let29_0, _425_dt__update_hattributes_h0 => DAST.Formal.create((_424_dt__update__tmp_h0).dtor_name, (_424_dt__update__tmp_h0).dtor_typ, _425_dt__update_hattributes_h0)))))));
            }
            DAST._IExpression _426_newLambda;
            DAST._IExpression _427_dt__update__tmp_h1 = _415_lambda;
            Dafny.ISequence<DAST._IFormal> _428_dt__update_hparams_h0 = _422_newFormals;
            _426_newLambda = DAST.Expression.create_Lambda(_428_dt__update_hparams_h0, (_427_dt__update__tmp_h1).dtor_retType, (_427_dt__update__tmp_h1).dtor_body);
            RAST._IExpr _429_lambdaGen;
            Defs._IOwnership _430___v166;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _431_recLambdaIdents;
            RAST._IExpr _out350;
            Defs._IOwnership _out351;
            Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out352;
            (this).GenExpr(_426_newLambda, selfIdent, env, Defs.Ownership.create_OwnershipOwned(), out _out350, out _out351, out _out352);
            _429_lambdaGen = _out350;
            _430___v166 = _out351;
            _431_recLambdaIdents = _out352;
            Dafny.ISequence<Dafny.Rune> _432_fn;
            if (_414_is__forall) {
              _432_fn = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("all");
            } else {
              _432_fn = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("any");
            }
            r = ((_417_collectionGen).Sel(_432_fn)).Apply1(((_429_lambdaGen).Sel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("as_ref"))).Apply0());
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.Union(_419_recIdents, _431_recLambdaIdents);
          } else {
            RAST._IExpr _out353;
            _out353 = (this).Error(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Quantifier without an inline lambda"), (this).InitEmptyExpr());
            r = _out353;
            readIdents = Dafny.Set<Dafny.ISequence<Dafny.Rune>>.FromElements();
          }
          RAST._IExpr _out354;
          Defs._IOwnership _out355;
          (this).FromOwned(r, expectedOwnership, out _out354, out _out355);
          r = _out354;
          resultingOwnership = _out355;
        }
      }
    after_match0: ;
    }
    public RAST._IExpr InitEmptyExpr() {
      return RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""));
    }
    public RAST._IExpr Error(Dafny.ISequence<Dafny.Rune> message, RAST._IExpr defaultExpr)
    {
      RAST._IExpr r = RAST.Expr.Default();
      if ((this.error).is_None) {
        (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(message);
      }
      r = RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/*"), message), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*/")), defaultExpr, DAST.Format.UnaryOpFormat.create_NoFormat());
      return r;
    }
    public Dafny.ISequence<Dafny.Rune> Compile(Dafny.ISequence<DAST._IModule> p, Dafny.ISequence<Dafny.ISequence<Dafny.Rune>> externalFiles)
    {
      Dafny.ISequence<Dafny.Rune> s = Dafny.Sequence<Dafny.Rune>.Empty;
      s = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#![allow(warnings, unconditional_panic)]\n");
      s = Dafny.Sequence<Dafny.Rune>.Concat(s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("#![allow(nonstandard_style)]\n"));
      Dafny.ISequence<RAST._IModDecl> _0_externUseDecls;
      _0_externUseDecls = Dafny.Sequence<RAST._IModDecl>.FromElements();
      BigInteger _hi0 = new BigInteger((externalFiles).Count);
      for (BigInteger _1_i = BigInteger.Zero; _1_i < _hi0; _1_i++) {
        Dafny.ISequence<Dafny.Rune> _2_externalFile;
        _2_externalFile = (externalFiles).Select(_1_i);
        Dafny.ISequence<Dafny.Rune> _3_externalMod;
        _3_externalMod = _2_externalFile;
        if (((new BigInteger((_2_externalFile).Count)) > (new BigInteger(3))) && (((_2_externalFile).Drop((new BigInteger((_2_externalFile).Count)) - (new BigInteger(3)))).Equals(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".rs")))) {
          _3_externalMod = (_2_externalFile).Subsequence(BigInteger.Zero, (new BigInteger((_2_externalFile).Count)) - (new BigInteger(3)));
        } else {
          (this).error = Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Unrecognized external file "), _2_externalFile), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(". External file must be *.rs files")));
        }
        if (((this).rootType).is_RootCrate) {
          RAST._IMod _4_externMod;
          _4_externMod = RAST.Mod.create_ExternMod(_3_externalMod);
          s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(s, (_4_externMod)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"));
        }
        _0_externUseDecls = Dafny.Sequence<RAST._IModDecl>.Concat(_0_externUseDecls, Dafny.Sequence<RAST._IModDecl>.FromElements(RAST.ModDecl.create_UseDecl(RAST.Use.create(RAST.Visibility.create_PUB(), ((RAST.__default.crate).MSel(_3_externalMod)).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"))))));
      }
      if (!(_0_externUseDecls).Equals(Dafny.Sequence<RAST._IModDecl>.FromElements())) {
        s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(s, (RAST.Mod.create_Mod(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Flattens all imported externs so that they can be accessed from this module"), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements(), Defs.__default.DAFNY__EXTERN__MODULE, _0_externUseDecls))._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"));
      }
      DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _5_allModules;
      _5_allModules = DafnyCompilerRustUtils.SeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule>.Empty();
      BigInteger _hi1 = new BigInteger((p).Count);
      for (BigInteger _6_i = BigInteger.Zero; _6_i < _hi1; _6_i++) {
        DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _7_m;
        DafnyCompilerRustUtils._ISeqMap<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule> _out0;
        _out0 = (this).GenModule((p).Select(_6_i), Dafny.Sequence<Dafny.ISequence<Dafny.Rune>>.FromElements());
        _7_m = _out0;
        _5_allModules = DafnyCompilerRustUtils.GatheringModule.MergeSeqMap(_5_allModules, _7_m);
      }
      BigInteger _hi2 = new BigInteger(((_5_allModules).dtor_keys).Count);
      for (BigInteger _8_i = BigInteger.Zero; _8_i < _hi2; _8_i++) {
        if (!((_5_allModules).dtor_values).Contains(((_5_allModules).dtor_keys).Select(_8_i))) {
          goto continue_0;
        }
        RAST._IMod _9_m;
        _9_m = (Dafny.Map<Dafny.ISequence<Dafny.Rune>, DafnyCompilerRustUtils._IGatheringModule>.Select((_5_allModules).dtor_values,((_5_allModules).dtor_keys).Select(_8_i))).ToRust();
        BigInteger _hi3 = new BigInteger((this.optimizations).Count);
        for (BigInteger _10_j = BigInteger.Zero; _10_j < _hi3; _10_j++) {
          _9_m = Dafny.Helpers.Id<Func<RAST._IMod, RAST._IMod>>((this.optimizations).Select(_10_j))(_9_m);
        }
        s = Dafny.Sequence<Dafny.Rune>.Concat(s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\n"));
        s = Dafny.Sequence<Dafny.Rune>.Concat(s, (_9_m)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")));
      continue_0: ;
      }
    after_0: ;
      return s;
    }
    public Dafny.ISequence<Dafny.Rune> EmitCallToMain(DAST._IExpression companion, Dafny.ISequence<Dafny.Rune> mainMethodName, bool hasArgs)
    {
      Dafny.ISequence<Dafny.Rune> s = Dafny.Sequence<Dafny.Rune>.Empty;
      s = Dafny.Sequence<Dafny.Rune>.UnicodeFromString("\nfn main() {");
      if (hasArgs) {
        s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(s, Dafny.Sequence<Dafny.Rune>.UnicodeFromString(@"
  let args: Vec<String> = ::std::env::args().collect();
  let dafny_args =
    ::dafny_runtime::dafny_runtime_conversions::vec_to_dafny_sequence(
    &args, |s| 
  ::dafny_runtime::dafny_runtime_conversions::")), (this).conversions), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(@"::string_to_dafny_string(s));
  "));
      }
      RAST._IExpr _0_call;
      Defs._IOwnership _1___v167;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _2___v168;
      RAST._IExpr _out0;
      Defs._IOwnership _out1;
      Dafny.ISet<Dafny.ISequence<Dafny.Rune>> _out2;
      (this).GenExpr(companion, Defs.SelfInfo.create_NoSelf(), Defs.Environment.Empty(), Defs.Ownership.create_OwnershipOwned(), out _out0, out _out1, out _out2);
      _0_call = _out0;
      _1___v167 = _out1;
      _2___v168 = _out2;
      _0_call = (_0_call).FSel(mainMethodName);
      if (hasArgs) {
        _0_call = (_0_call).Apply1(RAST.__default.Borrow(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_args"))));
      } else {
        _0_call = (_0_call).Apply(Dafny.Sequence<RAST._IExpr>.FromElements());
      }
      s = Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.Concat(s, (_0_call)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))), Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";\n}"));
      return s;
    }
    public Defs._IRootType _rootType {get; set;}
    public Defs._IRootType rootType { get {
      return this._rootType;
    } }
    public RAST._IPath thisFile { get {
      if (((this).rootType).is_RootCrate) {
        return RAST.__default.crate;
      } else {
        return (RAST.__default.crate).MSel(((this).rootType).dtor_moduleName);
      }
    } }
    public Defs._ICharType _charType {get; set;}
    public Defs._ICharType charType { get {
      return this._charType;
    } }
    public Dafny.ISequence<Dafny.Rune> DafnyChar { get {
      if (((this).charType).is_UTF32) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyChar");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyCharUTF16");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> conversions { get {
      if (((this).charType).is_UTF32) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unicode_chars_true");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("unicode_chars_false");
      }
    } }
    public RAST._IType DafnyCharUnderlying { get {
      if (((this).charType).is_UTF32) {
        return RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("char"));
      } else {
        return RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("u16"));
      }
    } }
    public Dafny.ISequence<Dafny.Rune> string__of { get {
      if (((this).charType).is_UTF32) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("string_of");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("string_utf16_of");
      }
    } }
    public Defs._IPointerType _pointerType {get; set;}
    public Defs._IPointerType pointerType { get {
      return this._pointerType;
    } }
    public Dafny.ISequence<Dafny.Rune> allocate { get {
      if (((this).pointerType).is_Raw) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("allocate");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("allocate_object");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> allocate__fn { get {
      return Dafny.Sequence<Dafny.Rune>.Concat(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("_"), (this).allocate);
    } }
    public Dafny.ISequence<Dafny.Rune> update__field__uninit__macro { get {
      if (((this).pointerType).is_Raw) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_uninit!");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_uninit_object!");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> update__field__mut__uninit__macro { get {
      if (((this).pointerType).is_Raw) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_mut_uninit!");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_mut_uninit_object!");
      }
    } }
    public RAST._IExpr thisInConstructor { get {
      if (((this).pointerType).is_Raw) {
        return RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"));
      } else {
        return (RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("this"))).Clone();
      }
    } }
    public Dafny.ISequence<Dafny.Rune> array__construct { get {
      if (((this).pointerType).is_Raw) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("construct");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("construct_object");
      }
    } }
    public RAST._IExpr modify__macro { get {
      return ((RAST.__default.dafny__runtime).MSel(((((this).pointerType).is_Raw) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("modify!")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("md!"))))).AsExpr();
    } }
    public RAST._IExpr read__macro { get {
      return ((RAST.__default.dafny__runtime).MSel(((((this).pointerType).is_Raw) ? (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("read!")) : (Dafny.Sequence<Dafny.Rune>.UnicodeFromString("rd!"))))).AsExpr();
    } }
    public Dafny.ISequence<Dafny.Rune> placebos__usize { get {
      if (((this).pointerType).is_Raw) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("placebos_usize");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("placebos_usize_object");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> update__field__if__uninit__macro { get {
      if (((this).pointerType).is_Raw) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_if_uninit!");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_if_uninit_object!");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> update__field__mut__if__uninit__macro { get {
      if (((this).pointerType).is_Raw) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_mut_if_uninit!");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("update_field_mut_if_uninit_object!");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> Upcast { get {
      if (((this).pointerType).is_Raw) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Upcast");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("UpcastObject");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> UpcastFnMacro { get {
      return Dafny.Sequence<Dafny.Rune>.Concat((this).Upcast, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("Fn!"));
    } }
    public Dafny.ISequence<Dafny.Rune> upcast { get {
      if (((this).pointerType).is_Raw) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("upcast");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("upcast_object");
      }
    } }
    public Dafny.ISequence<Dafny.Rune> downcast { get {
      if (((this).pointerType).is_Raw) {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cast!");
      } else {
        return Dafny.Sequence<Dafny.Rune>.UnicodeFromString("cast_object!");
      }
    } }
    public RAST._IExpr read__mutable__field__macro { get {
      return ((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("read_field!"))).AsExpr();
    } }
    public RAST._IExpr modify__mutable__field__macro { get {
      return ((RAST.__default.dafny__runtime).MSel(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("modify_field!"))).AsExpr();
    } }
  }
} // end of namespace DCOMP