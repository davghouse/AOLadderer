#include "implant.h"
#include "ladder_helper.h"

using std::string;
using namespace ladder_helper;

Implant::Implant() {
  used_to_ladder_ = false;
  ql_ = 0;
  remove_ = true;
}

Implant::Implant(int ql, string slot, string a, string s, string b, string f, bool locked, bool removed)
  : ql_(ql), slot_name_(slot), ability_name_(a), shining_abbr_(s), bright_abbr_(b), faded_abbr_(f) {
  if((s != "shi" || b != "bri" || f != "fad") && a != "abi")
    used_to_ladder_ = true; // imp can be used to ladder
  else
    used_to_ladder_ = false;
  slot_int_ = slotToInt(slot_name_);
  ability_int_ = abiReqToInt(a);
  shining_int_ = clusterToInt(s);
  bright_int_ = clusterToInt(b);
  faded_int_ = clusterToInt(f);
  lock_ = locked;
  remove_ = removed;
}
