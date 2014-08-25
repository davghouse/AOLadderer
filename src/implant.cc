#include "implant.h"
#include "ladder_helper.h"

using namespace ladder_helper;

Implant::Implant() {
  used_to_ladder_ = false;
  ql_ = 0;
  remove_ = true;
}
Implant::Implant(const Implant& implant){
  this->ability_int_ = implant.ability_int_;
  this->ability_name_ = implant.ability_name_;
  this->aoid_ = implant.aoid_;
  this->bright_abbr_ = implant.bright_abbr_;
  this->bright_full_ = implant.bright_full_;
  this->bright_int_ = implant.bright_int_;
  this->shining_abbr_= implant.shining_abbr_;
  this->shining_full_ = implant.shining_full_;
  this->shining_int_ = implant.shining_int_;
  this->faded_abbr_ = implant.faded_abbr_;
  this->faded_full_ = implant.faded_full_;
  this->faded_int_ = implant.faded_int_;
  this->lock_ = implant.lock_;
  this->ql_ = implant.ql_;
  this->remove_ = implant.remove_;
  this->slot_int_ = implant.slot_int_;
  this->slot_name_ = implant.slot_name_;
  this->used_to_ladder_ = implant.used_to_ladder_;
}

bool Implant::operator==(const Implant& rhs) const
{

  return this->aoid_ == rhs.aoid_ && this->ql_ == rhs.ql_;
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
