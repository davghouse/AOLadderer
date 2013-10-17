#include "ladder_helper.h"

using std::string; using std::vector;

namespace ladder_helper {

int SlotNameToInt(const std::string& slot_name){
  if(slot_name == "head")
    return 0;
  else if(slot_name == "eye")
    return 1;
  else if(slot_name == "ear")
    return 2;
  else if(slot_name == "chest")
    return 3;
  else if(slot_name == "rarm")
    return 4;
  else if(slot_name == "larm")
    return 5;
  else if(slot_name == "waist")
    return 6;
  else if(slot_name == "rwrist")
    return 7;
  else if(slot_name == "lwrist")
    return 8;
  else if(slot_name == "leg")
    return 9;
  else if(slot_name == "rhand")
    return 10;
  else if(slot_name == "lhand")
    return 11;
  else if(slot_name == "feet")
    return 12;
  else
    return -2;
}

std::string IntToSlotName(int i){
  switch(i){
  case 0:
    return "head";
  case 1:
    return "eye";
  case 2:
    return "ear";
  case 3:
    return "chest";
  case 4:
    return "rarm";
  case 5:
    return "larm";
  case 6:
    return "waist";
  case 7:
    return "rwrist";
  case 8:
    return "lwrist";
  case 9:
    return "leg";
  case 10:
    return "rhand";
  case 11:
    return "lhand";
  case 12:
    return "feet";
  default:
    return "xxxx";
  }
}

int AbilityToInt(const std::string& ability_name){
  if(ability_name == "str" || ability_name == "Strength"){
    return 0;
  }
  else if(ability_name == "agi" || ability_name == "Agility"){
    return 1;
  }
  else if(ability_name == "sta" || ability_name == "Stamina"){
    return 2;
  }
  else if(ability_name == "int" || ability_name == "Intelligence"){
    return 3;
  }
  else if(ability_name == "sen" || ability_name == "Sense"){
    return 4;
  }
  else if(ability_name == "psy" || ability_name == "Psychic"){
    return 5;
  }
  else if(ability_name == "Treatment"){
    return 6;
  }
  else if(ability_name == "abi")
    return -1;
  else
    return -2;
}

int AbilityToIntAndMakeFull(std::string& ability_name){
  if(ability_name == "str" || ability_name == "Strength"){
    ability_name = "str";
    return 0;
  }
  else if(ability_name == "agi" || ability_name == "Agility"){
    ability_name = "agi";
    return 1;
  }
  else if(ability_name == "sta" || ability_name == "Stamina"){
    ability_name = "sta";
    return 2;
  }
  else if(ability_name == "int" || ability_name == "Intelligence"){
      ability_name = "int";
    return 3;
  }
  else if(ability_name == "sen" || ability_name == "Sense"){
    ability_name = "sen";
    return 4;
  }
  else if(ability_name == "psy" || ability_name == "Psychic"){
    ability_name = "psy";
    return 5;
  }
  else if(ability_name == "Treatment"){
    ability_name = "tre";
    return 6;
  }
  else if(ability_name == "abi")
    return -1;
  else
    return -2;
}

// added for GUI
int ClusterToInt(const std::string& cluster){
  if(cluster == "str")
    return 0;
  else if(cluster == "agi")
    return 1;
  else if(cluster == "sta")
    return 2;
  else if(cluster == "int")
    return 3;
  else if(cluster == "sen")
    return 4;
  else if(cluster == "psy")
    return 5;
  else if(cluster == "tre")
    return 6;
  else if(cluster == "shi" || cluster == "bri" || cluster == "fad")
    return -1;
  else
    return -2;
}

std::string AbilityFullToAbbr(const std::string& ability_name)
{
  if(ability_name == "Strength")
    return "str";
  if(ability_name == "Agility")
    return "agi";
  if(ability_name == "Stamina")
    return "sta";
  if(ability_name == "Intelligence")
    return "int";
  if(ability_name == "Sense")
    return "sen";
  if(ability_name == "Psychic")
    return "psy";
  return "xxxx";
}

std::string SlotAbbrToFull(const std::string& slot_name)
{
  if(slot_name == "head") return "Head";
  if(slot_name == "eye") return "Eye";
  if(slot_name == "ear") return "Ear";
  if(slot_name == "chest") return "Chest";
  if(slot_name == "rarm") return "Right-Arm";
  if(slot_name == "larm") return "Left-Arm";
  if(slot_name == "waist") return "Waist";
  if(slot_name == "rwrist") return "Right-Wrist";
  if(slot_name == "lwrist") return "Left-Wrist";
  if(slot_name == "leg") return "Leg";
  if(slot_name == "rhand") return "Right-Hand";
  if(slot_name == "lhand") return "Left-Hand";
  if(slot_name == "feet") return "Feet";
  return "xxxx";
}

std::string AbilityAndTreatmentAbbrToFull(const std::string& skill)
{
  if(skill == "str") return "Strength";
  if(skill == "agi") return "Agility";
  if(skill == "sta") return "Stamina";
  if(skill == "int") return "Intelligence";
  if(skill == "sen") return "Sense";
  if(skill == "psy") return "Psychic";
  if(skill == "tre") return "Treatment";
  return "xxxx";
}

bool SlotTaken(const string& slot_name, const vector<Implant>& implants)
{
  for(vector<Implant>::const_iterator it = implants.begin(); it != implants.end(); ++it)
    if(slot_name == it->slot_name())
      return true;
  if(slot_name == "lwrist" || slot_name == "rwrist" || slot_name == "lhand")
    return true;
  return false;
}

} // namespace ladder_helper
