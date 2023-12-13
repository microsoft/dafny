// Package Std_Strings_HexConversion
// Dafny module Std_Strings_HexConversion compiled into Go

package Std_Strings_HexConversion

import (
  _dafny "dafny"
  os "os"
  _System "System_"
  Std_Wrappers "Std_Wrappers"
  Std_Concurrent "Std_Concurrent"
  Std_FileIOInternalExterns "Std_FileIOInternalExterns"
  Std_BoundedInts "Std_BoundedInts"
  Std_Base64 "Std_Base64"
  Std_Relations "Std_Relations"
  Std_Math "Std_Math"
  Std_Collections_Seq "Std_Collections_Seq"
  Std_Collections_Array "Std_Collections_Array"
  Std_Collections_Imap "Std_Collections_Imap"
  Std_Functions "Std_Functions"
  Std_Collections_Iset "Std_Collections_Iset"
  Std_Collections_Map "Std_Collections_Map"
  Std_Collections_Set "Std_Collections_Set"
  Std_Collections "Std_Collections"
  Std_DynamicArray "Std_DynamicArray"
  Std_FileIO "Std_FileIO"
  Std_Arithmetic_GeneralInternals "Std_Arithmetic_GeneralInternals"
  Std_Arithmetic_MulInternalsNonlinear "Std_Arithmetic_MulInternalsNonlinear"
  Std_Arithmetic_MulInternals "Std_Arithmetic_MulInternals"
  Std_Arithmetic_Mul "Std_Arithmetic_Mul"
  Std_Arithmetic_ModInternalsNonlinear "Std_Arithmetic_ModInternalsNonlinear"
  Std_Arithmetic_DivInternalsNonlinear "Std_Arithmetic_DivInternalsNonlinear"
  Std_Arithmetic_ModInternals "Std_Arithmetic_ModInternals"
  Std_Arithmetic_DivInternals "Std_Arithmetic_DivInternals"
  Std_Arithmetic_DivMod "Std_Arithmetic_DivMod"
  Std_Arithmetic_Power "Std_Arithmetic_Power"
  Std_Arithmetic_Logarithm "Std_Arithmetic_Logarithm"
  Std_Arithmetic_Power2 "Std_Arithmetic_Power2"
  Std_Arithmetic "Std_Arithmetic"
)
var _ _dafny.Dummy__
var _ = os.Args
var _ _System.Dummy__
var _ Std_Wrappers.Dummy__
var _ Std_Concurrent.Dummy__
var _ = Std_FileIOInternalExterns.INTERNAL__ReadBytesFromFile
var _ Std_BoundedInts.Dummy__
var _ Std_Base64.Dummy__
var _ Std_Relations.Dummy__
var _ Std_Math.Dummy__
var _ Std_Collections_Seq.Dummy__
var _ Std_Collections_Array.Dummy__
var _ Std_Collections_Imap.Dummy__
var _ Std_Functions.Dummy__
var _ Std_Collections_Iset.Dummy__
var _ Std_Collections_Map.Dummy__
var _ Std_Collections_Set.Dummy__
var _ Std_Collections.Dummy__
var _ Std_DynamicArray.Dummy__
var _ Std_FileIO.Dummy__
var _ Std_Arithmetic_GeneralInternals.Dummy__
var _ Std_Arithmetic_MulInternalsNonlinear.Dummy__
var _ Std_Arithmetic_MulInternals.Dummy__
var _ Std_Arithmetic_Mul.Dummy__
var _ Std_Arithmetic_ModInternalsNonlinear.Dummy__
var _ Std_Arithmetic_DivInternalsNonlinear.Dummy__
var _ Std_Arithmetic_ModInternals.Dummy__
var _ Std_Arithmetic_DivInternals.Dummy__
var _ Std_Arithmetic_DivMod.Dummy__
var _ Std_Arithmetic_Power.Dummy__
var _ Std_Arithmetic_Logarithm.Dummy__
var _ Std_Arithmetic_Power2.Dummy__
var _ Std_Arithmetic.Dummy__

type Dummy__ struct{}


// Definition of class Default__
type Default__ struct {
  dummy byte
}

func New_Default___() *Default__ {
  _this := Default__{}

  return &_this
}

type CompanionStruct_Default___ struct {
}
var Companion_Default___ = CompanionStruct_Default___ {
}

func (_this *Default__) Equals(other *Default__) bool {
  return _this == other
}

func (_this *Default__) EqualsGeneric(x interface{}) bool {
  other, ok := x.(*Default__)
  return ok && _this.Equals(other)
}

func (*Default__) String() string {
  return "Std_Strings_HexConversion.Default__"
}
func (_this *Default__) ParentTraits_() []*_dafny.TraitID {
  return [](*_dafny.TraitID){};
}
var _ _dafny.TraitOffspring = &Default__{}

func (_static *CompanionStruct_Default___) BASE() _dafny.Int {
  return Companion_Default___.Base()
}
func (_static *CompanionStruct_Default___) IsDigitChar(c _dafny.CodePoint) bool {
  return (Companion_Default___.CharToDigit()).Contains(c)
}
func (_static *CompanionStruct_Default___) OfDigits(digits _dafny.Sequence) _dafny.Sequence {
  var _134___accumulator _dafny.Sequence = _dafny.SeqOf()
  _ = _134___accumulator
  goto TAIL_CALL_START
  TAIL_CALL_START:
  if (_dafny.Companion_Sequence_.Equal(digits, _dafny.SeqOf())) {
    return _dafny.Companion_Sequence_.Concatenate(_dafny.SeqOf(), _134___accumulator)
  } else {
    _134___accumulator = _dafny.Companion_Sequence_.Concatenate(_dafny.SeqOf((Companion_Default___.Chars()).Select(((digits).Select(0).(_dafny.Int)).Uint32()).(_dafny.CodePoint)), _134___accumulator)
    var _in46 _dafny.Sequence = (digits).Drop(1)
    _ = _in46
    digits = _in46
    goto TAIL_CALL_START
  }
}
func (_static *CompanionStruct_Default___) OfNat(n _dafny.Int) _dafny.Sequence {
  if ((n).Sign() == 0) {
    return _dafny.SeqOf((Companion_Default___.Chars()).Select(0).(_dafny.CodePoint))
  } else {
    return Companion_Default___.OfDigits(Companion_Default___.FromNat(n))
  }
}
func (_static *CompanionStruct_Default___) IsNumberStr(str _dafny.Sequence, minus _dafny.CodePoint) bool {
  return !(!_dafny.Companion_Sequence_.Equal(str, _dafny.SeqOf())) || (((((str).Select(0).(_dafny.CodePoint)) == (minus)) || ((Companion_Default___.CharToDigit()).Contains((str).Select(0).(_dafny.CodePoint)))) && (_dafny.Quantifier(((str).Drop(1)).UniqueElements(), true, func (_forall_var_1 _dafny.CodePoint) bool {
    var _135_c _dafny.CodePoint
    _135_c = interface{}(_forall_var_1).(_dafny.CodePoint)
    return !(_dafny.Companion_Sequence_.Contains((str).Drop(1), _135_c)) || (Companion_Default___.IsDigitChar(_135_c))
  })))
}
func (_static *CompanionStruct_Default___) OfInt(n _dafny.Int, minus _dafny.CodePoint) _dafny.Sequence {
  if ((n).Sign() != -1) {
    return Companion_Default___.OfNat(n)
  } else {
    return _dafny.Companion_Sequence_.Concatenate(_dafny.SeqOf(minus), Companion_Default___.OfNat((_dafny.Zero).Minus(n)))
  }
}
func (_static *CompanionStruct_Default___) ToNat(str _dafny.Sequence) _dafny.Int {
  if (_dafny.Companion_Sequence_.Equal(str, _dafny.SeqOf())) {
    return _dafny.Zero
  } else {
    var _136_c _dafny.CodePoint = (str).Select(((_dafny.IntOfUint32((str).Cardinality())).Minus(_dafny.One)).Uint32()).(_dafny.CodePoint)
    _ = _136_c
    return ((Companion_Default___.ToNat((str).Take(((_dafny.IntOfUint32((str).Cardinality())).Minus(_dafny.One)).Uint32()))).Times(Companion_Default___.Base())).Plus((Companion_Default___.CharToDigit()).Get(_136_c).(_dafny.Int))
  }
}
func (_static *CompanionStruct_Default___) ToInt(str _dafny.Sequence, minus _dafny.CodePoint) _dafny.Int {
  if (_dafny.Companion_Sequence_.IsPrefixOf(_dafny.SeqOf(minus), str)) {
    return (_dafny.Zero).Minus((Companion_Default___.ToNat((str).Drop(1))))
  } else {
    return Companion_Default___.ToNat(str)
  }
}
func (_static *CompanionStruct_Default___) ToNatRight(xs _dafny.Sequence) _dafny.Int {
  if ((_dafny.IntOfUint32((xs).Cardinality())).Sign() == 0) {
    return _dafny.Zero
  } else {
    return ((Companion_Default___.ToNatRight(Std_Collections_Seq.Companion_Default___.DropFirst(xs))).Times(Companion_Default___.BASE())).Plus(Std_Collections_Seq.Companion_Default___.First(xs).(_dafny.Int))
  }
}
func (_static *CompanionStruct_Default___) ToNatLeft(xs _dafny.Sequence) _dafny.Int {
  var _137___accumulator _dafny.Int = _dafny.Zero
  _ = _137___accumulator
  goto TAIL_CALL_START
  TAIL_CALL_START:
  if ((_dafny.IntOfUint32((xs).Cardinality())).Sign() == 0) {
    return (_dafny.Zero).Plus(_137___accumulator)
  } else {
    _137___accumulator = ((Std_Collections_Seq.Companion_Default___.Last(xs).(_dafny.Int)).Times(Std_Arithmetic_Power.Companion_Default___.Pow(Companion_Default___.BASE(), (_dafny.IntOfUint32((xs).Cardinality())).Minus(_dafny.One)))).Plus(_137___accumulator)
    var _in47 _dafny.Sequence = Std_Collections_Seq.Companion_Default___.DropLast(xs)
    _ = _in47
    xs = _in47
    goto TAIL_CALL_START
  }
}
func (_static *CompanionStruct_Default___) FromNat(n _dafny.Int) _dafny.Sequence {
  var _138___accumulator _dafny.Sequence = _dafny.SeqOf()
  _ = _138___accumulator
  goto TAIL_CALL_START
  TAIL_CALL_START:
  if ((n).Sign() == 0) {
    return _dafny.Companion_Sequence_.Concatenate(_138___accumulator, _dafny.SeqOf())
  } else {
    _138___accumulator = _dafny.Companion_Sequence_.Concatenate(_138___accumulator, _dafny.SeqOf((n).Modulo(Companion_Default___.BASE())))
    var _in48 _dafny.Int = (n).DivBy(Companion_Default___.BASE())
    _ = _in48
    n = _in48
    goto TAIL_CALL_START
  }
}
func (_static *CompanionStruct_Default___) SeqExtend(xs _dafny.Sequence, n _dafny.Int) _dafny.Sequence {
  goto TAIL_CALL_START
  TAIL_CALL_START:
  if ((_dafny.IntOfUint32((xs).Cardinality())).Cmp(n) >= 0) {
    return xs
  } else {
    var _in49 _dafny.Sequence = _dafny.Companion_Sequence_.Concatenate(xs, _dafny.SeqOf(_dafny.Zero))
    _ = _in49
    var _in50 _dafny.Int = n
    _ = _in50
    xs = _in49
    n = _in50
    goto TAIL_CALL_START
  }
}
func (_static *CompanionStruct_Default___) SeqExtendMultiple(xs _dafny.Sequence, n _dafny.Int) _dafny.Sequence {
  var _139_newLen _dafny.Int = ((_dafny.IntOfUint32((xs).Cardinality())).Plus(n)).Minus((_dafny.IntOfUint32((xs).Cardinality())).Modulo(n))
  _ = _139_newLen
  return Companion_Default___.SeqExtend(xs, _139_newLen)
}
func (_static *CompanionStruct_Default___) FromNatWithLen(n _dafny.Int, len_ _dafny.Int) _dafny.Sequence {
  return Companion_Default___.SeqExtend(Companion_Default___.FromNat(n), len_)
}
func (_static *CompanionStruct_Default___) SeqZero(len_ _dafny.Int) _dafny.Sequence {
  var _140_xs _dafny.Sequence = Companion_Default___.FromNatWithLen(_dafny.Zero, len_)
  _ = _140_xs
  return _140_xs
}
func (_static *CompanionStruct_Default___) SeqAdd(xs _dafny.Sequence, ys _dafny.Sequence) _dafny.Tuple {
  if ((_dafny.IntOfUint32((xs).Cardinality())).Sign() == 0) {
    return _dafny.TupleOf(_dafny.SeqOf(), _dafny.Zero)
  } else {
    var _let_tmp_rhs1 _dafny.Tuple = Companion_Default___.SeqAdd(Std_Collections_Seq.Companion_Default___.DropLast(xs), Std_Collections_Seq.Companion_Default___.DropLast(ys))
    _ = _let_tmp_rhs1
    var _141_zs_k _dafny.Sequence = (*(_let_tmp_rhs1).IndexInt(0)).(_dafny.Sequence)
    _ = _141_zs_k
    var _142_cin _dafny.Int = (*(_let_tmp_rhs1).IndexInt(1)).(_dafny.Int)
    _ = _142_cin
    var _143_sum _dafny.Int = ((Std_Collections_Seq.Companion_Default___.Last(xs).(_dafny.Int)).Plus(Std_Collections_Seq.Companion_Default___.Last(ys).(_dafny.Int))).Plus(_142_cin)
    _ = _143_sum
    var _let_tmp_rhs2 _dafny.Tuple = (func () _dafny.Tuple { if (_143_sum).Cmp(Companion_Default___.BASE()) < 0 { return _dafny.TupleOf(_143_sum, _dafny.Zero) }; return _dafny.TupleOf((_143_sum).Minus(Companion_Default___.BASE()), _dafny.One) })() 
    _ = _let_tmp_rhs2
    var _144_sum__out _dafny.Int = (*(_let_tmp_rhs2).IndexInt(0)).(_dafny.Int)
    _ = _144_sum__out
    var _145_cout _dafny.Int = (*(_let_tmp_rhs2).IndexInt(1)).(_dafny.Int)
    _ = _145_cout
    return _dafny.TupleOf(_dafny.Companion_Sequence_.Concatenate(_141_zs_k, _dafny.SeqOf(_144_sum__out)), _145_cout)
  }
}
func (_static *CompanionStruct_Default___) SeqSub(xs _dafny.Sequence, ys _dafny.Sequence) _dafny.Tuple {
  if ((_dafny.IntOfUint32((xs).Cardinality())).Sign() == 0) {
    return _dafny.TupleOf(_dafny.SeqOf(), _dafny.Zero)
  } else {
    var _let_tmp_rhs3 _dafny.Tuple = Companion_Default___.SeqSub(Std_Collections_Seq.Companion_Default___.DropLast(xs), Std_Collections_Seq.Companion_Default___.DropLast(ys))
    _ = _let_tmp_rhs3
    var _146_zs _dafny.Sequence = (*(_let_tmp_rhs3).IndexInt(0)).(_dafny.Sequence)
    _ = _146_zs
    var _147_cin _dafny.Int = (*(_let_tmp_rhs3).IndexInt(1)).(_dafny.Int)
    _ = _147_cin
    var _let_tmp_rhs4 _dafny.Tuple = (func () _dafny.Tuple { if (Std_Collections_Seq.Companion_Default___.Last(xs).(_dafny.Int)).Cmp((Std_Collections_Seq.Companion_Default___.Last(ys).(_dafny.Int)).Plus(_147_cin)) >= 0 { return _dafny.TupleOf(((Std_Collections_Seq.Companion_Default___.Last(xs).(_dafny.Int)).Minus(Std_Collections_Seq.Companion_Default___.Last(ys).(_dafny.Int))).Minus(_147_cin), _dafny.Zero) }; return _dafny.TupleOf((((Companion_Default___.BASE()).Plus(Std_Collections_Seq.Companion_Default___.Last(xs).(_dafny.Int))).Minus(Std_Collections_Seq.Companion_Default___.Last(ys).(_dafny.Int))).Minus(_147_cin), _dafny.One) })() 
    _ = _let_tmp_rhs4
    var _148_diff__out _dafny.Int = (*(_let_tmp_rhs4).IndexInt(0)).(_dafny.Int)
    _ = _148_diff__out
    var _149_cout _dafny.Int = (*(_let_tmp_rhs4).IndexInt(1)).(_dafny.Int)
    _ = _149_cout
    return _dafny.TupleOf(_dafny.Companion_Sequence_.Concatenate(_146_zs, _dafny.SeqOf(_148_diff__out)), _149_cout)
  }
}
func (_static *CompanionStruct_Default___) HEX__DIGITS() _dafny.Sequence {
  return _dafny.UnicodeSeqOfUtf8Bytes("0123456789ABCDEF")
}
func (_static *CompanionStruct_Default___) Chars() _dafny.Sequence {
  return Companion_Default___.HEX__DIGITS()
}
func (_static *CompanionStruct_Default___) Base() _dafny.Int {
  return _dafny.IntOfUint32((Companion_Default___.Chars()).Cardinality())
}
func (_static *CompanionStruct_Default___) CharToDigit() _dafny.Map {
  return _dafny.NewMapBuilder().ToMap().UpdateUnsafe(_dafny.CodePoint('0'), _dafny.Zero).UpdateUnsafe(_dafny.CodePoint('1'), _dafny.One).UpdateUnsafe(_dafny.CodePoint('2'), _dafny.IntOfInt64(2)).UpdateUnsafe(_dafny.CodePoint('3'), _dafny.IntOfInt64(3)).UpdateUnsafe(_dafny.CodePoint('4'), _dafny.IntOfInt64(4)).UpdateUnsafe(_dafny.CodePoint('5'), _dafny.IntOfInt64(5)).UpdateUnsafe(_dafny.CodePoint('6'), _dafny.IntOfInt64(6)).UpdateUnsafe(_dafny.CodePoint('7'), _dafny.IntOfInt64(7)).UpdateUnsafe(_dafny.CodePoint('8'), _dafny.IntOfInt64(8)).UpdateUnsafe(_dafny.CodePoint('9'), _dafny.IntOfInt64(9)).UpdateUnsafe(_dafny.CodePoint('a'), _dafny.IntOfInt64(10)).UpdateUnsafe(_dafny.CodePoint('b'), _dafny.IntOfInt64(11)).UpdateUnsafe(_dafny.CodePoint('c'), _dafny.IntOfInt64(12)).UpdateUnsafe(_dafny.CodePoint('d'), _dafny.IntOfInt64(13)).UpdateUnsafe(_dafny.CodePoint('e'), _dafny.IntOfInt64(14)).UpdateUnsafe(_dafny.CodePoint('f'), _dafny.IntOfInt64(15)).UpdateUnsafe(_dafny.CodePoint('A'), _dafny.IntOfInt64(10)).UpdateUnsafe(_dafny.CodePoint('B'), _dafny.IntOfInt64(11)).UpdateUnsafe(_dafny.CodePoint('C'), _dafny.IntOfInt64(12)).UpdateUnsafe(_dafny.CodePoint('D'), _dafny.IntOfInt64(13)).UpdateUnsafe(_dafny.CodePoint('E'), _dafny.IntOfInt64(14)).UpdateUnsafe(_dafny.CodePoint('F'), _dafny.IntOfInt64(15))
}
// End of class Default__

// Definition of class CharSeq
type CharSeq struct {
}

func New_CharSeq_() *CharSeq {
  _this := CharSeq{}

  return &_this
}

type CompanionStruct_CharSeq_ struct {
}
var Companion_CharSeq_ = CompanionStruct_CharSeq_ {
}

func (*CharSeq) String() string {
  return "Std_Strings_HexConversion.CharSeq"
}
// End of class CharSeq

func Type_CharSeq_() _dafny.TypeDescriptor {
  return type_CharSeq_{}
}

type type_CharSeq_ struct {
}

func (_this type_CharSeq_) Default() interface{} {
  return _dafny.EmptySeq
}

func (_this type_CharSeq_) String() string {
  return "Std_Strings_HexConversion.CharSeq"
}

// Definition of class Digit
type Digit struct {
}

func New_Digit_() *Digit {
  _this := Digit{}

  return &_this
}

type CompanionStruct_Digit_ struct {
}
var Companion_Digit_ = CompanionStruct_Digit_ {
}

func (*Digit) String() string {
  return "Std_Strings_HexConversion.Digit"
}
// End of class Digit

func Type_Digit_() _dafny.TypeDescriptor {
  return type_Digit_{}
}

type type_Digit_ struct {
}

func (_this type_Digit_) Default() interface{} {
  return _dafny.Zero
}

func (_this type_Digit_) String() string {
  return "Std_Strings_HexConversion.Digit"
}
