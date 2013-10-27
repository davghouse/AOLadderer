#include "implant.h"
#include "ladder_helper.h"

using namespace ladder_helper;

Implant::Implant() {
  used_to_ladder_ = false;
  ql_ = 0;
  remove_ = true;
}

Implant::Implant(int ql, const string& slot_name, const string& ability_name,
                 const string& shining_abbr, const string& bright_abbr, const string& faded_abbr)
  : ql_(ql), slot_name_(slot_name), ability_name_(ability_name),
    shining_abbr_(shining_abbr), bright_abbr_(bright_abbr), faded_abbr_(faded_abbr) {

  if((shining_abbr != "shi" || bright_abbr != "bri" || faded_abbr != "fad") && ability_name != "abi")
    used_to_ladder_ = true;
  else
    used_to_ladder_ = false;
  slot_int_ = SlotNameToInt(slot_name);
  ability_int_ = AbilityToInt(ability_name);
  shining_int_ = ClusterToInt(shining_abbr);
  bright_int_ = ClusterToInt(bright_abbr);
  faded_int_ = ClusterToInt(faded_abbr);
  lock_ = false;
  remove_ = true;
  aoid_ = 0;
}
