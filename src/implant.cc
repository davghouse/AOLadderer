#include "implant.h"
#include "ladder_helper.h"

using std::string;
using namespace ladder_helper;

Implant::Implant() {
  used_to_ladder_ = false;
  ql_ = 0;
  remove_ = true;
}

Implant::Implant(int ql,
                 string slot_name,
                 string ability_name,
                 string shining_abbr,
                 string bright_abbr,
                 string faded_abbr,
                 bool lock,
                 bool remove)
  : ql_(ql),
    slot_name_(slot_name),
    ability_name_(ability_name),
    shining_abbr_(shining_abbr),
    bright_abbr_(bright_abbr),
    faded_abbr_(faded_abbr) {

  if((shining_abbr != "shi" || bright_abbr != "bri" || faded_abbr != "fad") && ability_name != "abi")
    used_to_ladder_ = true;
  else
    used_to_ladder_ = false;
  slot_int_ = SlotNameToInt(slot_name);
  ability_int_ = AbilityToInt(ability_name);
  shining_int_ = ClusterToInt(shining_abbr);
  bright_int_ = ClusterToInt(bright_abbr);
  faded_int_ = ClusterToInt(faded_abbr);
  lock_ = lock;
  remove_ = remove;
}
