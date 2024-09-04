#![allow(warnings, unconditional_panic)]
#![allow(nonstandard_style)]

pub mod _System {
  pub use crate::DafnyType;
  pub use ::std::fmt::Debug;
  pub use crate::DafnyPrint;
  pub use ::std::cmp::Eq;
  pub use ::std::hash::Hash;
  pub use ::std::default::Default;
  pub use ::std::convert::AsRef;

  pub type nat = crate::DafnyInt;

  #[derive(PartialEq, Clone)]
  pub enum Tuple2<T0: crate::DafnyType, T1: crate::DafnyType> {
    _T2 {
      _0: T0,
      _1: T1
    }
  }

  impl<T0: DafnyType, T1: DafnyType> Tuple2<T0, T1> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple2::_T2{_0, _1, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple2::_T2{_0, _1, } => _1,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType> Debug
    for Tuple2<T0, T1> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType> DafnyPrint
    for Tuple2<T0, T1> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple2::_T2{_0, _1, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType> Tuple2<T0, T1> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple2<T0, T1>) -> Tuple2<r#__T0, r#__T1>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple2<r#__T0, r#__T1>{
          match this {
            Tuple2::_T2{_0, _1, } => {
              Tuple2::_T2 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq> Eq
    for Tuple2<T0, T1> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash> Hash
    for Tuple2<T0, T1> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple2::_T2{_0, _1, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default> Default
    for Tuple2<T0, T1> {
    fn default() -> Tuple2<T0, T1> {
      Tuple2::_T2 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType> AsRef<Tuple2<T0, T1>>
    for &Tuple2<T0, T1> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple0 {
    _T0 {}
  }

  impl Tuple0 {}

  impl Debug
    for Tuple0 {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl DafnyPrint
    for Tuple0 {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple0::_T0{} => {
          write!(_formatter, "")?;
          Ok(())
        },
      }
    }
  }

  impl Eq
    for Tuple0 {}

  impl Hash
    for Tuple0 {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple0::_T0{} => {
          
        },
      }
    }
  }

  impl Default
    for Tuple0 {
    fn default() -> Tuple0 {
      Tuple0::_T0 {}
    }
  }

  impl AsRef<Tuple0>
    for &Tuple0 {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple1<T0: crate::DafnyType> {
    _T1 {
      _0: T0
    }
  }

  impl<T0: DafnyType> Tuple1<T0> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple1::_T1{_0, } => _0,
      }
    }
  }

  impl<T0: DafnyType> Debug
    for Tuple1<T0> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType> DafnyPrint
    for Tuple1<T0> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple1::_T1{_0, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType> Tuple1<T0> {
    pub fn coerce<r#__T0: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple1<T0>) -> Tuple1<r#__T0>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple1<r#__T0>{
          match this {
            Tuple1::_T1{_0, } => {
              Tuple1::_T1 {
                _0: f_0.clone()(_0)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq> Eq
    for Tuple1<T0> {}

  impl<T0: DafnyType + Hash> Hash
    for Tuple1<T0> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple1::_T1{_0, } => {
          ::std::hash::Hash::hash(_0, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default> Default
    for Tuple1<T0> {
    fn default() -> Tuple1<T0> {
      Tuple1::_T1 {
        _0: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType> AsRef<Tuple1<T0>>
    for &Tuple1<T0> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple3<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType> {
    _T3 {
      _0: T0,
      _1: T1,
      _2: T2
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType> Tuple3<T0, T1, T2> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple3::_T3{_0, _1, _2, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple3::_T3{_0, _1, _2, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple3::_T3{_0, _1, _2, } => _2,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType> Debug
    for Tuple3<T0, T1, T2> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType> DafnyPrint
    for Tuple3<T0, T1, T2> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple3::_T3{_0, _1, _2, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType> Tuple3<T0, T1, T2> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple3<T0, T1, T2>) -> Tuple3<r#__T0, r#__T1, r#__T2>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple3<r#__T0, r#__T1, r#__T2>{
          match this {
            Tuple3::_T3{_0, _1, _2, } => {
              Tuple3::_T3 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq> Eq
    for Tuple3<T0, T1, T2> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash> Hash
    for Tuple3<T0, T1, T2> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple3::_T3{_0, _1, _2, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default> Default
    for Tuple3<T0, T1, T2> {
    fn default() -> Tuple3<T0, T1, T2> {
      Tuple3::_T3 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType> AsRef<Tuple3<T0, T1, T2>>
    for &Tuple3<T0, T1, T2> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple4<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType> {
    _T4 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType> Tuple4<T0, T1, T2, T3> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple4::_T4{_0, _1, _2, _3, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple4::_T4{_0, _1, _2, _3, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple4::_T4{_0, _1, _2, _3, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple4::_T4{_0, _1, _2, _3, } => _3,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType> Debug
    for Tuple4<T0, T1, T2, T3> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType> DafnyPrint
    for Tuple4<T0, T1, T2, T3> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple4::_T4{_0, _1, _2, _3, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType> Tuple4<T0, T1, T2, T3> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple4<T0, T1, T2, T3>) -> Tuple4<r#__T0, r#__T1, r#__T2, r#__T3>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple4<r#__T0, r#__T1, r#__T2, r#__T3>{
          match this {
            Tuple4::_T4{_0, _1, _2, _3, } => {
              Tuple4::_T4 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq> Eq
    for Tuple4<T0, T1, T2, T3> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash> Hash
    for Tuple4<T0, T1, T2, T3> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple4::_T4{_0, _1, _2, _3, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default> Default
    for Tuple4<T0, T1, T2, T3> {
    fn default() -> Tuple4<T0, T1, T2, T3> {
      Tuple4::_T4 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType> AsRef<Tuple4<T0, T1, T2, T3>>
    for &Tuple4<T0, T1, T2, T3> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple5<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType> {
    _T5 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType> Tuple5<T0, T1, T2, T3, T4> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple5::_T5{_0, _1, _2, _3, _4, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple5::_T5{_0, _1, _2, _3, _4, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple5::_T5{_0, _1, _2, _3, _4, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple5::_T5{_0, _1, _2, _3, _4, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple5::_T5{_0, _1, _2, _3, _4, } => _4,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType> Debug
    for Tuple5<T0, T1, T2, T3, T4> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType> DafnyPrint
    for Tuple5<T0, T1, T2, T3, T4> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple5::_T5{_0, _1, _2, _3, _4, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType> Tuple5<T0, T1, T2, T3, T4> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple5<T0, T1, T2, T3, T4>) -> Tuple5<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple5<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4>{
          match this {
            Tuple5::_T5{_0, _1, _2, _3, _4, } => {
              Tuple5::_T5 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq> Eq
    for Tuple5<T0, T1, T2, T3, T4> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash> Hash
    for Tuple5<T0, T1, T2, T3, T4> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple5::_T5{_0, _1, _2, _3, _4, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default> Default
    for Tuple5<T0, T1, T2, T3, T4> {
    fn default() -> Tuple5<T0, T1, T2, T3, T4> {
      Tuple5::_T5 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType> AsRef<Tuple5<T0, T1, T2, T3, T4>>
    for &Tuple5<T0, T1, T2, T3, T4> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple6<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType> {
    _T6 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType> Tuple6<T0, T1, T2, T3, T4, T5> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple6::_T6{_0, _1, _2, _3, _4, _5, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple6::_T6{_0, _1, _2, _3, _4, _5, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple6::_T6{_0, _1, _2, _3, _4, _5, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple6::_T6{_0, _1, _2, _3, _4, _5, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple6::_T6{_0, _1, _2, _3, _4, _5, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple6::_T6{_0, _1, _2, _3, _4, _5, } => _5,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType> Debug
    for Tuple6<T0, T1, T2, T3, T4, T5> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType> DafnyPrint
    for Tuple6<T0, T1, T2, T3, T4, T5> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple6::_T6{_0, _1, _2, _3, _4, _5, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType> Tuple6<T0, T1, T2, T3, T4, T5> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple6<T0, T1, T2, T3, T4, T5>) -> Tuple6<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple6<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5>{
          match this {
            Tuple6::_T6{_0, _1, _2, _3, _4, _5, } => {
              Tuple6::_T6 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq> Eq
    for Tuple6<T0, T1, T2, T3, T4, T5> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash> Hash
    for Tuple6<T0, T1, T2, T3, T4, T5> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple6::_T6{_0, _1, _2, _3, _4, _5, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default> Default
    for Tuple6<T0, T1, T2, T3, T4, T5> {
    fn default() -> Tuple6<T0, T1, T2, T3, T4, T5> {
      Tuple6::_T6 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType> AsRef<Tuple6<T0, T1, T2, T3, T4, T5>>
    for &Tuple6<T0, T1, T2, T3, T4, T5> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple7<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType> {
    _T7 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType> Tuple7<T0, T1, T2, T3, T4, T5, T6> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple7::_T7{_0, _1, _2, _3, _4, _5, _6, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple7::_T7{_0, _1, _2, _3, _4, _5, _6, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple7::_T7{_0, _1, _2, _3, _4, _5, _6, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple7::_T7{_0, _1, _2, _3, _4, _5, _6, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple7::_T7{_0, _1, _2, _3, _4, _5, _6, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple7::_T7{_0, _1, _2, _3, _4, _5, _6, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple7::_T7{_0, _1, _2, _3, _4, _5, _6, } => _6,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType> Debug
    for Tuple7<T0, T1, T2, T3, T4, T5, T6> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType> DafnyPrint
    for Tuple7<T0, T1, T2, T3, T4, T5, T6> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple7::_T7{_0, _1, _2, _3, _4, _5, _6, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType> Tuple7<T0, T1, T2, T3, T4, T5, T6> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple7<T0, T1, T2, T3, T4, T5, T6>) -> Tuple7<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple7<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6>{
          match this {
            Tuple7::_T7{_0, _1, _2, _3, _4, _5, _6, } => {
              Tuple7::_T7 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq> Eq
    for Tuple7<T0, T1, T2, T3, T4, T5, T6> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash> Hash
    for Tuple7<T0, T1, T2, T3, T4, T5, T6> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple7::_T7{_0, _1, _2, _3, _4, _5, _6, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default> Default
    for Tuple7<T0, T1, T2, T3, T4, T5, T6> {
    fn default() -> Tuple7<T0, T1, T2, T3, T4, T5, T6> {
      Tuple7::_T7 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType> AsRef<Tuple7<T0, T1, T2, T3, T4, T5, T6>>
    for &Tuple7<T0, T1, T2, T3, T4, T5, T6> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple8<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType> {
    _T8 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType> Tuple8<T0, T1, T2, T3, T4, T5, T6, T7> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => _7,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType> Debug
    for Tuple8<T0, T1, T2, T3, T4, T5, T6, T7> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType> DafnyPrint
    for Tuple8<T0, T1, T2, T3, T4, T5, T6, T7> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType> Tuple8<T0, T1, T2, T3, T4, T5, T6, T7> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple8<T0, T1, T2, T3, T4, T5, T6, T7>) -> Tuple8<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple8<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7>{
          match this {
            Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => {
              Tuple8::_T8 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq> Eq
    for Tuple8<T0, T1, T2, T3, T4, T5, T6, T7> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash> Hash
    for Tuple8<T0, T1, T2, T3, T4, T5, T6, T7> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple8::_T8{_0, _1, _2, _3, _4, _5, _6, _7, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default> Default
    for Tuple8<T0, T1, T2, T3, T4, T5, T6, T7> {
    fn default() -> Tuple8<T0, T1, T2, T3, T4, T5, T6, T7> {
      Tuple8::_T8 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType> AsRef<Tuple8<T0, T1, T2, T3, T4, T5, T6, T7>>
    for &Tuple8<T0, T1, T2, T3, T4, T5, T6, T7> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple9<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType> {
    _T9 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType> Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => _8,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType> Debug
    for Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType> DafnyPrint
    for Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType> Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8>) -> Tuple9<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple9<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8>{
          match this {
            Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => {
              Tuple9::_T9 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq> Eq
    for Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash> Hash
    for Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple9::_T9{_0, _1, _2, _3, _4, _5, _6, _7, _8, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default> Default
    for Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> {
    fn default() -> Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> {
      Tuple9::_T9 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType> AsRef<Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8>>
    for &Tuple9<T0, T1, T2, T3, T4, T5, T6, T7, T8> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple10<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType> {
    _T10 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType> Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => _9,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType> Debug
    for Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType> DafnyPrint
    for Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType> Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>) -> Tuple10<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple10<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9>{
          match this {
            Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => {
              Tuple10::_T10 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq> Eq
    for Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash> Hash
    for Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple10::_T10{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default> Default
    for Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> {
    fn default() -> Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> {
      Tuple10::_T10 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType> AsRef<Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>>
    for &Tuple10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple11<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType, T10: crate::DafnyType> {
    _T11 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9,
      _10: T10
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType> Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _9,
      }
    }
    pub fn _10(&self) -> &T10 {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => _10,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType> Debug
    for Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType> DafnyPrint
    for Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_10, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType> Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType, r#__T10: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>, f_10: ::std::rc::Rc<impl ::std::ops::Fn(T10) -> r#__T10 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>) -> Tuple11<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple11<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10>{
          match this {
            Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => {
              Tuple11::_T11 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9),
                _10: f_10.clone()(_10)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq, T10: DafnyType + Eq> Eq
    for Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash, T10: DafnyType + Hash> Hash
    for Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple11::_T11{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state);
          ::std::hash::Hash::hash(_10, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default, T10: DafnyType + Default> Default
    for Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
    fn default() -> Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
      Tuple11::_T11 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default(),
        _10: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType> AsRef<Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
    for &Tuple11<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple12<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType, T10: crate::DafnyType, T11: crate::DafnyType> {
    _T12 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9,
      _10: T10,
      _11: T11
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType> Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _9,
      }
    }
    pub fn _10(&self) -> &T10 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _10,
      }
    }
    pub fn _11(&self) -> &T11 {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => _11,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType> Debug
    for Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType> DafnyPrint
    for Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_10, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_11, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType> Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType, r#__T10: crate::DafnyType, r#__T11: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>, f_10: ::std::rc::Rc<impl ::std::ops::Fn(T10) -> r#__T10 + 'static>, f_11: ::std::rc::Rc<impl ::std::ops::Fn(T11) -> r#__T11 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>) -> Tuple12<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple12<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11>{
          match this {
            Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => {
              Tuple12::_T12 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9),
                _10: f_10.clone()(_10),
                _11: f_11.clone()(_11)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq, T10: DafnyType + Eq, T11: DafnyType + Eq> Eq
    for Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash, T10: DafnyType + Hash, T11: DafnyType + Hash> Hash
    for Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple12::_T12{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state);
          ::std::hash::Hash::hash(_10, _state);
          ::std::hash::Hash::hash(_11, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default, T10: DafnyType + Default, T11: DafnyType + Default> Default
    for Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
    fn default() -> Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
      Tuple12::_T12 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default(),
        _10: ::std::default::Default::default(),
        _11: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType> AsRef<Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
    for &Tuple12<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple13<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType, T10: crate::DafnyType, T11: crate::DafnyType, T12: crate::DafnyType> {
    _T13 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9,
      _10: T10,
      _11: T11,
      _12: T12
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType> Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _9,
      }
    }
    pub fn _10(&self) -> &T10 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _10,
      }
    }
    pub fn _11(&self) -> &T11 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _11,
      }
    }
    pub fn _12(&self) -> &T12 {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => _12,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType> Debug
    for Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType> DafnyPrint
    for Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_10, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_11, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_12, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType> Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType, r#__T10: crate::DafnyType, r#__T11: crate::DafnyType, r#__T12: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>, f_10: ::std::rc::Rc<impl ::std::ops::Fn(T10) -> r#__T10 + 'static>, f_11: ::std::rc::Rc<impl ::std::ops::Fn(T11) -> r#__T11 + 'static>, f_12: ::std::rc::Rc<impl ::std::ops::Fn(T12) -> r#__T12 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>) -> Tuple13<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple13<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12>{
          match this {
            Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => {
              Tuple13::_T13 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9),
                _10: f_10.clone()(_10),
                _11: f_11.clone()(_11),
                _12: f_12.clone()(_12)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq, T10: DafnyType + Eq, T11: DafnyType + Eq, T12: DafnyType + Eq> Eq
    for Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash, T10: DafnyType + Hash, T11: DafnyType + Hash, T12: DafnyType + Hash> Hash
    for Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple13::_T13{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state);
          ::std::hash::Hash::hash(_10, _state);
          ::std::hash::Hash::hash(_11, _state);
          ::std::hash::Hash::hash(_12, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default, T10: DafnyType + Default, T11: DafnyType + Default, T12: DafnyType + Default> Default
    for Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
    fn default() -> Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
      Tuple13::_T13 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default(),
        _10: ::std::default::Default::default(),
        _11: ::std::default::Default::default(),
        _12: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType> AsRef<Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
    for &Tuple13<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple14<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType, T10: crate::DafnyType, T11: crate::DafnyType, T12: crate::DafnyType, T13: crate::DafnyType> {
    _T14 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9,
      _10: T10,
      _11: T11,
      _12: T12,
      _13: T13
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType> Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _9,
      }
    }
    pub fn _10(&self) -> &T10 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _10,
      }
    }
    pub fn _11(&self) -> &T11 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _11,
      }
    }
    pub fn _12(&self) -> &T12 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _12,
      }
    }
    pub fn _13(&self) -> &T13 {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => _13,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType> Debug
    for Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType> DafnyPrint
    for Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_10, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_11, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_12, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_13, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType> Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType, r#__T10: crate::DafnyType, r#__T11: crate::DafnyType, r#__T12: crate::DafnyType, r#__T13: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>, f_10: ::std::rc::Rc<impl ::std::ops::Fn(T10) -> r#__T10 + 'static>, f_11: ::std::rc::Rc<impl ::std::ops::Fn(T11) -> r#__T11 + 'static>, f_12: ::std::rc::Rc<impl ::std::ops::Fn(T12) -> r#__T12 + 'static>, f_13: ::std::rc::Rc<impl ::std::ops::Fn(T13) -> r#__T13 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>) -> Tuple14<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple14<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13>{
          match this {
            Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => {
              Tuple14::_T14 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9),
                _10: f_10.clone()(_10),
                _11: f_11.clone()(_11),
                _12: f_12.clone()(_12),
                _13: f_13.clone()(_13)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq, T10: DafnyType + Eq, T11: DafnyType + Eq, T12: DafnyType + Eq, T13: DafnyType + Eq> Eq
    for Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash, T10: DafnyType + Hash, T11: DafnyType + Hash, T12: DafnyType + Hash, T13: DafnyType + Hash> Hash
    for Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple14::_T14{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state);
          ::std::hash::Hash::hash(_10, _state);
          ::std::hash::Hash::hash(_11, _state);
          ::std::hash::Hash::hash(_12, _state);
          ::std::hash::Hash::hash(_13, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default, T10: DafnyType + Default, T11: DafnyType + Default, T12: DafnyType + Default, T13: DafnyType + Default> Default
    for Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
    fn default() -> Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
      Tuple14::_T14 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default(),
        _10: ::std::default::Default::default(),
        _11: ::std::default::Default::default(),
        _12: ::std::default::Default::default(),
        _13: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType> AsRef<Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
    for &Tuple14<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple15<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType, T10: crate::DafnyType, T11: crate::DafnyType, T12: crate::DafnyType, T13: crate::DafnyType, T14: crate::DafnyType> {
    _T15 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9,
      _10: T10,
      _11: T11,
      _12: T12,
      _13: T13,
      _14: T14
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType> Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _9,
      }
    }
    pub fn _10(&self) -> &T10 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _10,
      }
    }
    pub fn _11(&self) -> &T11 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _11,
      }
    }
    pub fn _12(&self) -> &T12 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _12,
      }
    }
    pub fn _13(&self) -> &T13 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _13,
      }
    }
    pub fn _14(&self) -> &T14 {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => _14,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType> Debug
    for Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType> DafnyPrint
    for Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_10, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_11, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_12, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_13, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_14, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType> Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType, r#__T10: crate::DafnyType, r#__T11: crate::DafnyType, r#__T12: crate::DafnyType, r#__T13: crate::DafnyType, r#__T14: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>, f_10: ::std::rc::Rc<impl ::std::ops::Fn(T10) -> r#__T10 + 'static>, f_11: ::std::rc::Rc<impl ::std::ops::Fn(T11) -> r#__T11 + 'static>, f_12: ::std::rc::Rc<impl ::std::ops::Fn(T12) -> r#__T12 + 'static>, f_13: ::std::rc::Rc<impl ::std::ops::Fn(T13) -> r#__T13 + 'static>, f_14: ::std::rc::Rc<impl ::std::ops::Fn(T14) -> r#__T14 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>) -> Tuple15<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple15<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14>{
          match this {
            Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => {
              Tuple15::_T15 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9),
                _10: f_10.clone()(_10),
                _11: f_11.clone()(_11),
                _12: f_12.clone()(_12),
                _13: f_13.clone()(_13),
                _14: f_14.clone()(_14)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq, T10: DafnyType + Eq, T11: DafnyType + Eq, T12: DafnyType + Eq, T13: DafnyType + Eq, T14: DafnyType + Eq> Eq
    for Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash, T10: DafnyType + Hash, T11: DafnyType + Hash, T12: DafnyType + Hash, T13: DafnyType + Hash, T14: DafnyType + Hash> Hash
    for Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple15::_T15{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state);
          ::std::hash::Hash::hash(_10, _state);
          ::std::hash::Hash::hash(_11, _state);
          ::std::hash::Hash::hash(_12, _state);
          ::std::hash::Hash::hash(_13, _state);
          ::std::hash::Hash::hash(_14, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default, T10: DafnyType + Default, T11: DafnyType + Default, T12: DafnyType + Default, T13: DafnyType + Default, T14: DafnyType + Default> Default
    for Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {
    fn default() -> Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {
      Tuple15::_T15 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default(),
        _10: ::std::default::Default::default(),
        _11: ::std::default::Default::default(),
        _12: ::std::default::Default::default(),
        _13: ::std::default::Default::default(),
        _14: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType> AsRef<Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
    for &Tuple15<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple16<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType, T10: crate::DafnyType, T11: crate::DafnyType, T12: crate::DafnyType, T13: crate::DafnyType, T14: crate::DafnyType, T15: crate::DafnyType> {
    _T16 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9,
      _10: T10,
      _11: T11,
      _12: T12,
      _13: T13,
      _14: T14,
      _15: T15
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType> Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _9,
      }
    }
    pub fn _10(&self) -> &T10 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _10,
      }
    }
    pub fn _11(&self) -> &T11 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _11,
      }
    }
    pub fn _12(&self) -> &T12 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _12,
      }
    }
    pub fn _13(&self) -> &T13 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _13,
      }
    }
    pub fn _14(&self) -> &T14 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _14,
      }
    }
    pub fn _15(&self) -> &T15 {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => _15,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType> Debug
    for Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType> DafnyPrint
    for Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_10, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_11, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_12, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_13, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_14, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_15, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType> Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType, r#__T10: crate::DafnyType, r#__T11: crate::DafnyType, r#__T12: crate::DafnyType, r#__T13: crate::DafnyType, r#__T14: crate::DafnyType, r#__T15: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>, f_10: ::std::rc::Rc<impl ::std::ops::Fn(T10) -> r#__T10 + 'static>, f_11: ::std::rc::Rc<impl ::std::ops::Fn(T11) -> r#__T11 + 'static>, f_12: ::std::rc::Rc<impl ::std::ops::Fn(T12) -> r#__T12 + 'static>, f_13: ::std::rc::Rc<impl ::std::ops::Fn(T13) -> r#__T13 + 'static>, f_14: ::std::rc::Rc<impl ::std::ops::Fn(T14) -> r#__T14 + 'static>, f_15: ::std::rc::Rc<impl ::std::ops::Fn(T15) -> r#__T15 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>) -> Tuple16<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14, r#__T15>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple16<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14, r#__T15>{
          match this {
            Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => {
              Tuple16::_T16 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9),
                _10: f_10.clone()(_10),
                _11: f_11.clone()(_11),
                _12: f_12.clone()(_12),
                _13: f_13.clone()(_13),
                _14: f_14.clone()(_14),
                _15: f_15.clone()(_15)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq, T10: DafnyType + Eq, T11: DafnyType + Eq, T12: DafnyType + Eq, T13: DafnyType + Eq, T14: DafnyType + Eq, T15: DafnyType + Eq> Eq
    for Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash, T10: DafnyType + Hash, T11: DafnyType + Hash, T12: DafnyType + Hash, T13: DafnyType + Hash, T14: DafnyType + Hash, T15: DafnyType + Hash> Hash
    for Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple16::_T16{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state);
          ::std::hash::Hash::hash(_10, _state);
          ::std::hash::Hash::hash(_11, _state);
          ::std::hash::Hash::hash(_12, _state);
          ::std::hash::Hash::hash(_13, _state);
          ::std::hash::Hash::hash(_14, _state);
          ::std::hash::Hash::hash(_15, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default, T10: DafnyType + Default, T11: DafnyType + Default, T12: DafnyType + Default, T13: DafnyType + Default, T14: DafnyType + Default, T15: DafnyType + Default> Default
    for Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {
    fn default() -> Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {
      Tuple16::_T16 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default(),
        _10: ::std::default::Default::default(),
        _11: ::std::default::Default::default(),
        _12: ::std::default::Default::default(),
        _13: ::std::default::Default::default(),
        _14: ::std::default::Default::default(),
        _15: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType> AsRef<Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
    for &Tuple16<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple17<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType, T10: crate::DafnyType, T11: crate::DafnyType, T12: crate::DafnyType, T13: crate::DafnyType, T14: crate::DafnyType, T15: crate::DafnyType, T16: crate::DafnyType> {
    _T17 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9,
      _10: T10,
      _11: T11,
      _12: T12,
      _13: T13,
      _14: T14,
      _15: T15,
      _16: T16
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType> Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _9,
      }
    }
    pub fn _10(&self) -> &T10 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _10,
      }
    }
    pub fn _11(&self) -> &T11 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _11,
      }
    }
    pub fn _12(&self) -> &T12 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _12,
      }
    }
    pub fn _13(&self) -> &T13 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _13,
      }
    }
    pub fn _14(&self) -> &T14 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _14,
      }
    }
    pub fn _15(&self) -> &T15 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _15,
      }
    }
    pub fn _16(&self) -> &T16 {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => _16,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType> Debug
    for Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType> DafnyPrint
    for Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_10, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_11, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_12, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_13, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_14, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_15, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_16, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType> Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType, r#__T10: crate::DafnyType, r#__T11: crate::DafnyType, r#__T12: crate::DafnyType, r#__T13: crate::DafnyType, r#__T14: crate::DafnyType, r#__T15: crate::DafnyType, r#__T16: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>, f_10: ::std::rc::Rc<impl ::std::ops::Fn(T10) -> r#__T10 + 'static>, f_11: ::std::rc::Rc<impl ::std::ops::Fn(T11) -> r#__T11 + 'static>, f_12: ::std::rc::Rc<impl ::std::ops::Fn(T12) -> r#__T12 + 'static>, f_13: ::std::rc::Rc<impl ::std::ops::Fn(T13) -> r#__T13 + 'static>, f_14: ::std::rc::Rc<impl ::std::ops::Fn(T14) -> r#__T14 + 'static>, f_15: ::std::rc::Rc<impl ::std::ops::Fn(T15) -> r#__T15 + 'static>, f_16: ::std::rc::Rc<impl ::std::ops::Fn(T16) -> r#__T16 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>) -> Tuple17<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14, r#__T15, r#__T16>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple17<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14, r#__T15, r#__T16>{
          match this {
            Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => {
              Tuple17::_T17 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9),
                _10: f_10.clone()(_10),
                _11: f_11.clone()(_11),
                _12: f_12.clone()(_12),
                _13: f_13.clone()(_13),
                _14: f_14.clone()(_14),
                _15: f_15.clone()(_15),
                _16: f_16.clone()(_16)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq, T10: DafnyType + Eq, T11: DafnyType + Eq, T12: DafnyType + Eq, T13: DafnyType + Eq, T14: DafnyType + Eq, T15: DafnyType + Eq, T16: DafnyType + Eq> Eq
    for Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash, T10: DafnyType + Hash, T11: DafnyType + Hash, T12: DafnyType + Hash, T13: DafnyType + Hash, T14: DafnyType + Hash, T15: DafnyType + Hash, T16: DafnyType + Hash> Hash
    for Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple17::_T17{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state);
          ::std::hash::Hash::hash(_10, _state);
          ::std::hash::Hash::hash(_11, _state);
          ::std::hash::Hash::hash(_12, _state);
          ::std::hash::Hash::hash(_13, _state);
          ::std::hash::Hash::hash(_14, _state);
          ::std::hash::Hash::hash(_15, _state);
          ::std::hash::Hash::hash(_16, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default, T10: DafnyType + Default, T11: DafnyType + Default, T12: DafnyType + Default, T13: DafnyType + Default, T14: DafnyType + Default, T15: DafnyType + Default, T16: DafnyType + Default> Default
    for Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {
    fn default() -> Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {
      Tuple17::_T17 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default(),
        _10: ::std::default::Default::default(),
        _11: ::std::default::Default::default(),
        _12: ::std::default::Default::default(),
        _13: ::std::default::Default::default(),
        _14: ::std::default::Default::default(),
        _15: ::std::default::Default::default(),
        _16: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType> AsRef<Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>
    for &Tuple17<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple18<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType, T10: crate::DafnyType, T11: crate::DafnyType, T12: crate::DafnyType, T13: crate::DafnyType, T14: crate::DafnyType, T15: crate::DafnyType, T16: crate::DafnyType, T17: crate::DafnyType> {
    _T18 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9,
      _10: T10,
      _11: T11,
      _12: T12,
      _13: T13,
      _14: T14,
      _15: T15,
      _16: T16,
      _17: T17
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType> Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _9,
      }
    }
    pub fn _10(&self) -> &T10 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _10,
      }
    }
    pub fn _11(&self) -> &T11 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _11,
      }
    }
    pub fn _12(&self) -> &T12 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _12,
      }
    }
    pub fn _13(&self) -> &T13 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _13,
      }
    }
    pub fn _14(&self) -> &T14 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _14,
      }
    }
    pub fn _15(&self) -> &T15 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _15,
      }
    }
    pub fn _16(&self) -> &T16 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _16,
      }
    }
    pub fn _17(&self) -> &T17 {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => _17,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType> Debug
    for Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType> DafnyPrint
    for Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_10, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_11, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_12, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_13, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_14, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_15, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_16, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_17, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType> Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType, r#__T10: crate::DafnyType, r#__T11: crate::DafnyType, r#__T12: crate::DafnyType, r#__T13: crate::DafnyType, r#__T14: crate::DafnyType, r#__T15: crate::DafnyType, r#__T16: crate::DafnyType, r#__T17: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>, f_10: ::std::rc::Rc<impl ::std::ops::Fn(T10) -> r#__T10 + 'static>, f_11: ::std::rc::Rc<impl ::std::ops::Fn(T11) -> r#__T11 + 'static>, f_12: ::std::rc::Rc<impl ::std::ops::Fn(T12) -> r#__T12 + 'static>, f_13: ::std::rc::Rc<impl ::std::ops::Fn(T13) -> r#__T13 + 'static>, f_14: ::std::rc::Rc<impl ::std::ops::Fn(T14) -> r#__T14 + 'static>, f_15: ::std::rc::Rc<impl ::std::ops::Fn(T15) -> r#__T15 + 'static>, f_16: ::std::rc::Rc<impl ::std::ops::Fn(T16) -> r#__T16 + 'static>, f_17: ::std::rc::Rc<impl ::std::ops::Fn(T17) -> r#__T17 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>) -> Tuple18<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14, r#__T15, r#__T16, r#__T17>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple18<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14, r#__T15, r#__T16, r#__T17>{
          match this {
            Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => {
              Tuple18::_T18 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9),
                _10: f_10.clone()(_10),
                _11: f_11.clone()(_11),
                _12: f_12.clone()(_12),
                _13: f_13.clone()(_13),
                _14: f_14.clone()(_14),
                _15: f_15.clone()(_15),
                _16: f_16.clone()(_16),
                _17: f_17.clone()(_17)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq, T10: DafnyType + Eq, T11: DafnyType + Eq, T12: DafnyType + Eq, T13: DafnyType + Eq, T14: DafnyType + Eq, T15: DafnyType + Eq, T16: DafnyType + Eq, T17: DafnyType + Eq> Eq
    for Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash, T10: DafnyType + Hash, T11: DafnyType + Hash, T12: DafnyType + Hash, T13: DafnyType + Hash, T14: DafnyType + Hash, T15: DafnyType + Hash, T16: DafnyType + Hash, T17: DafnyType + Hash> Hash
    for Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple18::_T18{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state);
          ::std::hash::Hash::hash(_10, _state);
          ::std::hash::Hash::hash(_11, _state);
          ::std::hash::Hash::hash(_12, _state);
          ::std::hash::Hash::hash(_13, _state);
          ::std::hash::Hash::hash(_14, _state);
          ::std::hash::Hash::hash(_15, _state);
          ::std::hash::Hash::hash(_16, _state);
          ::std::hash::Hash::hash(_17, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default, T10: DafnyType + Default, T11: DafnyType + Default, T12: DafnyType + Default, T13: DafnyType + Default, T14: DafnyType + Default, T15: DafnyType + Default, T16: DafnyType + Default, T17: DafnyType + Default> Default
    for Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {
    fn default() -> Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {
      Tuple18::_T18 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default(),
        _10: ::std::default::Default::default(),
        _11: ::std::default::Default::default(),
        _12: ::std::default::Default::default(),
        _13: ::std::default::Default::default(),
        _14: ::std::default::Default::default(),
        _15: ::std::default::Default::default(),
        _16: ::std::default::Default::default(),
        _17: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType> AsRef<Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>>
    for &Tuple18<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple19<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType, T10: crate::DafnyType, T11: crate::DafnyType, T12: crate::DafnyType, T13: crate::DafnyType, T14: crate::DafnyType, T15: crate::DafnyType, T16: crate::DafnyType, T17: crate::DafnyType, T18: crate::DafnyType> {
    _T19 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9,
      _10: T10,
      _11: T11,
      _12: T12,
      _13: T13,
      _14: T14,
      _15: T15,
      _16: T16,
      _17: T17,
      _18: T18
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType, T18: DafnyType> Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _9,
      }
    }
    pub fn _10(&self) -> &T10 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _10,
      }
    }
    pub fn _11(&self) -> &T11 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _11,
      }
    }
    pub fn _12(&self) -> &T12 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _12,
      }
    }
    pub fn _13(&self) -> &T13 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _13,
      }
    }
    pub fn _14(&self) -> &T14 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _14,
      }
    }
    pub fn _15(&self) -> &T15 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _15,
      }
    }
    pub fn _16(&self) -> &T16 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _16,
      }
    }
    pub fn _17(&self) -> &T17 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _17,
      }
    }
    pub fn _18(&self) -> &T18 {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => _18,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType, T18: DafnyType> Debug
    for Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType, T18: DafnyType> DafnyPrint
    for Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_10, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_11, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_12, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_13, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_14, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_15, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_16, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_17, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_18, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType, T18: DafnyType> Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType, r#__T10: crate::DafnyType, r#__T11: crate::DafnyType, r#__T12: crate::DafnyType, r#__T13: crate::DafnyType, r#__T14: crate::DafnyType, r#__T15: crate::DafnyType, r#__T16: crate::DafnyType, r#__T17: crate::DafnyType, r#__T18: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>, f_10: ::std::rc::Rc<impl ::std::ops::Fn(T10) -> r#__T10 + 'static>, f_11: ::std::rc::Rc<impl ::std::ops::Fn(T11) -> r#__T11 + 'static>, f_12: ::std::rc::Rc<impl ::std::ops::Fn(T12) -> r#__T12 + 'static>, f_13: ::std::rc::Rc<impl ::std::ops::Fn(T13) -> r#__T13 + 'static>, f_14: ::std::rc::Rc<impl ::std::ops::Fn(T14) -> r#__T14 + 'static>, f_15: ::std::rc::Rc<impl ::std::ops::Fn(T15) -> r#__T15 + 'static>, f_16: ::std::rc::Rc<impl ::std::ops::Fn(T16) -> r#__T16 + 'static>, f_17: ::std::rc::Rc<impl ::std::ops::Fn(T17) -> r#__T17 + 'static>, f_18: ::std::rc::Rc<impl ::std::ops::Fn(T18) -> r#__T18 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>) -> Tuple19<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14, r#__T15, r#__T16, r#__T17, r#__T18>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple19<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14, r#__T15, r#__T16, r#__T17, r#__T18>{
          match this {
            Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => {
              Tuple19::_T19 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9),
                _10: f_10.clone()(_10),
                _11: f_11.clone()(_11),
                _12: f_12.clone()(_12),
                _13: f_13.clone()(_13),
                _14: f_14.clone()(_14),
                _15: f_15.clone()(_15),
                _16: f_16.clone()(_16),
                _17: f_17.clone()(_17),
                _18: f_18.clone()(_18)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq, T10: DafnyType + Eq, T11: DafnyType + Eq, T12: DafnyType + Eq, T13: DafnyType + Eq, T14: DafnyType + Eq, T15: DafnyType + Eq, T16: DafnyType + Eq, T17: DafnyType + Eq, T18: DafnyType + Eq> Eq
    for Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash, T10: DafnyType + Hash, T11: DafnyType + Hash, T12: DafnyType + Hash, T13: DafnyType + Hash, T14: DafnyType + Hash, T15: DafnyType + Hash, T16: DafnyType + Hash, T17: DafnyType + Hash, T18: DafnyType + Hash> Hash
    for Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple19::_T19{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state);
          ::std::hash::Hash::hash(_10, _state);
          ::std::hash::Hash::hash(_11, _state);
          ::std::hash::Hash::hash(_12, _state);
          ::std::hash::Hash::hash(_13, _state);
          ::std::hash::Hash::hash(_14, _state);
          ::std::hash::Hash::hash(_15, _state);
          ::std::hash::Hash::hash(_16, _state);
          ::std::hash::Hash::hash(_17, _state);
          ::std::hash::Hash::hash(_18, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default, T10: DafnyType + Default, T11: DafnyType + Default, T12: DafnyType + Default, T13: DafnyType + Default, T14: DafnyType + Default, T15: DafnyType + Default, T16: DafnyType + Default, T17: DafnyType + Default, T18: DafnyType + Default> Default
    for Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {
    fn default() -> Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {
      Tuple19::_T19 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default(),
        _10: ::std::default::Default::default(),
        _11: ::std::default::Default::default(),
        _12: ::std::default::Default::default(),
        _13: ::std::default::Default::default(),
        _14: ::std::default::Default::default(),
        _15: ::std::default::Default::default(),
        _16: ::std::default::Default::default(),
        _17: ::std::default::Default::default(),
        _18: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType, T18: DafnyType> AsRef<Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>>
    for &Tuple19<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> {
    fn as_ref(&self) -> Self {
      self
    }
  }

  #[derive(PartialEq, Clone)]
  pub enum Tuple20<T0: crate::DafnyType, T1: crate::DafnyType, T2: crate::DafnyType, T3: crate::DafnyType, T4: crate::DafnyType, T5: crate::DafnyType, T6: crate::DafnyType, T7: crate::DafnyType, T8: crate::DafnyType, T9: crate::DafnyType, T10: crate::DafnyType, T11: crate::DafnyType, T12: crate::DafnyType, T13: crate::DafnyType, T14: crate::DafnyType, T15: crate::DafnyType, T16: crate::DafnyType, T17: crate::DafnyType, T18: crate::DafnyType, T19: crate::DafnyType> {
    _T20 {
      _0: T0,
      _1: T1,
      _2: T2,
      _3: T3,
      _4: T4,
      _5: T5,
      _6: T6,
      _7: T7,
      _8: T8,
      _9: T9,
      _10: T10,
      _11: T11,
      _12: T12,
      _13: T13,
      _14: T14,
      _15: T15,
      _16: T16,
      _17: T17,
      _18: T18,
      _19: T19
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType, T18: DafnyType, T19: DafnyType> Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {
    pub fn _0(&self) -> &T0 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _0,
      }
    }
    pub fn _1(&self) -> &T1 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _1,
      }
    }
    pub fn _2(&self) -> &T2 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _2,
      }
    }
    pub fn _3(&self) -> &T3 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _3,
      }
    }
    pub fn _4(&self) -> &T4 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _4,
      }
    }
    pub fn _5(&self) -> &T5 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _5,
      }
    }
    pub fn _6(&self) -> &T6 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _6,
      }
    }
    pub fn _7(&self) -> &T7 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _7,
      }
    }
    pub fn _8(&self) -> &T8 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _8,
      }
    }
    pub fn _9(&self) -> &T9 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _9,
      }
    }
    pub fn _10(&self) -> &T10 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _10,
      }
    }
    pub fn _11(&self) -> &T11 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _11,
      }
    }
    pub fn _12(&self) -> &T12 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _12,
      }
    }
    pub fn _13(&self) -> &T13 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _13,
      }
    }
    pub fn _14(&self) -> &T14 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _14,
      }
    }
    pub fn _15(&self) -> &T15 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _15,
      }
    }
    pub fn _16(&self) -> &T16 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _16,
      }
    }
    pub fn _17(&self) -> &T17 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _17,
      }
    }
    pub fn _18(&self) -> &T18 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _18,
      }
    }
    pub fn _19(&self) -> &T19 {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => _19,
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType, T18: DafnyType, T19: DafnyType> Debug
    for Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> ::std::fmt::Result {
      crate::DafnyPrint::fmt_print(self, f, true)
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType, T18: DafnyType, T19: DafnyType> DafnyPrint
    for Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {
    fn fmt_print(&self, _formatter: &mut ::std::fmt::Formatter, _in_seq: bool) -> std::fmt::Result {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => {
          write!(_formatter, "(")?;
          crate::DafnyPrint::fmt_print(_0, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_1, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_2, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_3, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_4, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_5, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_6, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_7, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_8, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_9, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_10, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_11, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_12, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_13, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_14, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_15, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_16, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_17, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_18, _formatter, false)?;
          write!(_formatter, ", ")?;
          crate::DafnyPrint::fmt_print(_19, _formatter, false)?;
          write!(_formatter, ")")?;
          Ok(())
        },
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType, T18: DafnyType, T19: DafnyType> Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {
    pub fn coerce<r#__T0: crate::DafnyType, r#__T1: crate::DafnyType, r#__T2: crate::DafnyType, r#__T3: crate::DafnyType, r#__T4: crate::DafnyType, r#__T5: crate::DafnyType, r#__T6: crate::DafnyType, r#__T7: crate::DafnyType, r#__T8: crate::DafnyType, r#__T9: crate::DafnyType, r#__T10: crate::DafnyType, r#__T11: crate::DafnyType, r#__T12: crate::DafnyType, r#__T13: crate::DafnyType, r#__T14: crate::DafnyType, r#__T15: crate::DafnyType, r#__T16: crate::DafnyType, r#__T17: crate::DafnyType, r#__T18: crate::DafnyType, r#__T19: crate::DafnyType>(f_0: ::std::rc::Rc<impl ::std::ops::Fn(T0) -> r#__T0 + 'static>, f_1: ::std::rc::Rc<impl ::std::ops::Fn(T1) -> r#__T1 + 'static>, f_2: ::std::rc::Rc<impl ::std::ops::Fn(T2) -> r#__T2 + 'static>, f_3: ::std::rc::Rc<impl ::std::ops::Fn(T3) -> r#__T3 + 'static>, f_4: ::std::rc::Rc<impl ::std::ops::Fn(T4) -> r#__T4 + 'static>, f_5: ::std::rc::Rc<impl ::std::ops::Fn(T5) -> r#__T5 + 'static>, f_6: ::std::rc::Rc<impl ::std::ops::Fn(T6) -> r#__T6 + 'static>, f_7: ::std::rc::Rc<impl ::std::ops::Fn(T7) -> r#__T7 + 'static>, f_8: ::std::rc::Rc<impl ::std::ops::Fn(T8) -> r#__T8 + 'static>, f_9: ::std::rc::Rc<impl ::std::ops::Fn(T9) -> r#__T9 + 'static>, f_10: ::std::rc::Rc<impl ::std::ops::Fn(T10) -> r#__T10 + 'static>, f_11: ::std::rc::Rc<impl ::std::ops::Fn(T11) -> r#__T11 + 'static>, f_12: ::std::rc::Rc<impl ::std::ops::Fn(T12) -> r#__T12 + 'static>, f_13: ::std::rc::Rc<impl ::std::ops::Fn(T13) -> r#__T13 + 'static>, f_14: ::std::rc::Rc<impl ::std::ops::Fn(T14) -> r#__T14 + 'static>, f_15: ::std::rc::Rc<impl ::std::ops::Fn(T15) -> r#__T15 + 'static>, f_16: ::std::rc::Rc<impl ::std::ops::Fn(T16) -> r#__T16 + 'static>, f_17: ::std::rc::Rc<impl ::std::ops::Fn(T17) -> r#__T17 + 'static>, f_18: ::std::rc::Rc<impl ::std::ops::Fn(T18) -> r#__T18 + 'static>, f_19: ::std::rc::Rc<impl ::std::ops::Fn(T19) -> r#__T19 + 'static>) -> ::std::rc::Rc<impl ::std::ops::Fn(Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>) -> Tuple20<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14, r#__T15, r#__T16, r#__T17, r#__T18, r#__T19>> {
      ::std::rc::Rc::new(move |this: Self| -> Tuple20<r#__T0, r#__T1, r#__T2, r#__T3, r#__T4, r#__T5, r#__T6, r#__T7, r#__T8, r#__T9, r#__T10, r#__T11, r#__T12, r#__T13, r#__T14, r#__T15, r#__T16, r#__T17, r#__T18, r#__T19>{
          match this {
            Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => {
              Tuple20::_T20 {
                _0: f_0.clone()(_0),
                _1: f_1.clone()(_1),
                _2: f_2.clone()(_2),
                _3: f_3.clone()(_3),
                _4: f_4.clone()(_4),
                _5: f_5.clone()(_5),
                _6: f_6.clone()(_6),
                _7: f_7.clone()(_7),
                _8: f_8.clone()(_8),
                _9: f_9.clone()(_9),
                _10: f_10.clone()(_10),
                _11: f_11.clone()(_11),
                _12: f_12.clone()(_12),
                _13: f_13.clone()(_13),
                _14: f_14.clone()(_14),
                _15: f_15.clone()(_15),
                _16: f_16.clone()(_16),
                _17: f_17.clone()(_17),
                _18: f_18.clone()(_18),
                _19: f_19.clone()(_19)
              }
            },
          }
        })
    }
  }

  impl<T0: DafnyType + Eq, T1: DafnyType + Eq, T2: DafnyType + Eq, T3: DafnyType + Eq, T4: DafnyType + Eq, T5: DafnyType + Eq, T6: DafnyType + Eq, T7: DafnyType + Eq, T8: DafnyType + Eq, T9: DafnyType + Eq, T10: DafnyType + Eq, T11: DafnyType + Eq, T12: DafnyType + Eq, T13: DafnyType + Eq, T14: DafnyType + Eq, T15: DafnyType + Eq, T16: DafnyType + Eq, T17: DafnyType + Eq, T18: DafnyType + Eq, T19: DafnyType + Eq> Eq
    for Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {}

  impl<T0: DafnyType + Hash, T1: DafnyType + Hash, T2: DafnyType + Hash, T3: DafnyType + Hash, T4: DafnyType + Hash, T5: DafnyType + Hash, T6: DafnyType + Hash, T7: DafnyType + Hash, T8: DafnyType + Hash, T9: DafnyType + Hash, T10: DafnyType + Hash, T11: DafnyType + Hash, T12: DafnyType + Hash, T13: DafnyType + Hash, T14: DafnyType + Hash, T15: DafnyType + Hash, T16: DafnyType + Hash, T17: DafnyType + Hash, T18: DafnyType + Hash, T19: DafnyType + Hash> Hash
    for Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {
    fn hash<_H: ::std::hash::Hasher>(&self, _state: &mut _H) {
      match self {
        Tuple20::_T20{_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, } => {
          ::std::hash::Hash::hash(_0, _state);
          ::std::hash::Hash::hash(_1, _state);
          ::std::hash::Hash::hash(_2, _state);
          ::std::hash::Hash::hash(_3, _state);
          ::std::hash::Hash::hash(_4, _state);
          ::std::hash::Hash::hash(_5, _state);
          ::std::hash::Hash::hash(_6, _state);
          ::std::hash::Hash::hash(_7, _state);
          ::std::hash::Hash::hash(_8, _state);
          ::std::hash::Hash::hash(_9, _state);
          ::std::hash::Hash::hash(_10, _state);
          ::std::hash::Hash::hash(_11, _state);
          ::std::hash::Hash::hash(_12, _state);
          ::std::hash::Hash::hash(_13, _state);
          ::std::hash::Hash::hash(_14, _state);
          ::std::hash::Hash::hash(_15, _state);
          ::std::hash::Hash::hash(_16, _state);
          ::std::hash::Hash::hash(_17, _state);
          ::std::hash::Hash::hash(_18, _state);
          ::std::hash::Hash::hash(_19, _state)
        },
      }
    }
  }

  impl<T0: DafnyType + Default, T1: DafnyType + Default, T2: DafnyType + Default, T3: DafnyType + Default, T4: DafnyType + Default, T5: DafnyType + Default, T6: DafnyType + Default, T7: DafnyType + Default, T8: DafnyType + Default, T9: DafnyType + Default, T10: DafnyType + Default, T11: DafnyType + Default, T12: DafnyType + Default, T13: DafnyType + Default, T14: DafnyType + Default, T15: DafnyType + Default, T16: DafnyType + Default, T17: DafnyType + Default, T18: DafnyType + Default, T19: DafnyType + Default> Default
    for Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {
    fn default() -> Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {
      Tuple20::_T20 {
        _0: ::std::default::Default::default(),
        _1: ::std::default::Default::default(),
        _2: ::std::default::Default::default(),
        _3: ::std::default::Default::default(),
        _4: ::std::default::Default::default(),
        _5: ::std::default::Default::default(),
        _6: ::std::default::Default::default(),
        _7: ::std::default::Default::default(),
        _8: ::std::default::Default::default(),
        _9: ::std::default::Default::default(),
        _10: ::std::default::Default::default(),
        _11: ::std::default::Default::default(),
        _12: ::std::default::Default::default(),
        _13: ::std::default::Default::default(),
        _14: ::std::default::Default::default(),
        _15: ::std::default::Default::default(),
        _16: ::std::default::Default::default(),
        _17: ::std::default::Default::default(),
        _18: ::std::default::Default::default(),
        _19: ::std::default::Default::default()
      }
    }
  }

  impl<T0: DafnyType, T1: DafnyType, T2: DafnyType, T3: DafnyType, T4: DafnyType, T5: DafnyType, T6: DafnyType, T7: DafnyType, T8: DafnyType, T9: DafnyType, T10: DafnyType, T11: DafnyType, T12: DafnyType, T13: DafnyType, T14: DafnyType, T15: DafnyType, T16: DafnyType, T17: DafnyType, T18: DafnyType, T19: DafnyType> AsRef<Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>>
    for &Tuple20<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> {
    fn as_ref(&self) -> Self {
      self
    }
  }
}