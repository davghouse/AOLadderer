#include "Implant.h"
using std::string;
using namespace laddererHelper;

Implant::Implant() {
  Ladder = false;
  Ql = 0;
  remove = true;
}

Implant::Implant(int ql, string slot, string a, string s, string b, string f, bool locked, bool removed)
  : Ql(ql), Slot(slot), a(a), s(s), b(b), f(f) {
  if((s != "shi" || b != "bri" || f != "fad") && a != "abi")
    Ladder = true; // imp can be used to ladder
  else
    Ladder = false;
  SlotI = slotToInt(Slot);
  aI = abiReqToInt(a);
  sI = clusterToInt(s);
  bI = clusterToInt(b);
  fI = clusterToInt(f);
  lock = locked;
  remove = removed;
}
