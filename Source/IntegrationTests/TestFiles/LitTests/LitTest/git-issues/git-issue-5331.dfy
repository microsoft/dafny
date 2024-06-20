// RUN: %baredafny verify %args --type-system-refresh "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

trait Ins {
    function step(s:State):State //requires safe?(s)
}
type Code = seq<Ins>
datatype State = S(
    clock:nat
) {
    function fetch_():Ins
    const fetch := fetch_()
    const step := fetch.step(this)
    function run():State decreases clock {
        if clock == 0 then this else step.(clock := clock - 1).run()
    }
}