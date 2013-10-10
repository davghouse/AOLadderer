#include "Helper.h"

int slotToInt(const std::string& slot){
  if(slot == "head")
    return 0;
  else if(slot == "eye")
    return 1;
  else if(slot == "ear")
    return 2;
  else if(slot == "chest")
    return 3;
  else if(slot == "rarm")
    return 4;
  else if(slot == "larm")
    return 5;
  else if(slot == "waist")
    return 6;
  else if(slot == "rwrist")
    return 7;
  else if(slot == "lwrist")
    return 8;
  else if(slot == "leg")
    return 9;
  else if(slot == "rhand")
    return 10;
  else if(slot == "lhand")
    return 11;
  else if(slot == "feet")
    return 12;
  else
    return -2;
}
std::string intToSlot(int i){
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

int abiReqToInt(const std::string& a){
  if(a == "str" || a == "Strength"){
    return 0;
  }
  else if(a == "agi" || a == "Agility"){
    return 1;
  }
  else if(a == "sta" || a == "Stamina"){
    return 2;
  }
  else if(a == "int" || a == "Intelligence"){
    return 3;
  }
  else if(a == "sen" || a == "Sense"){
    return 4;
  }
  else if(a == "psy" || a == "Psychic"){
    return 5;
  }
  else if(a == "Treatment"){
    return 6;
  }
  else if(a == "abi")
    return -1;
  else
    return -2;
}

int abiReqToInt_GUI(std::string& a){
  if(a == "str" || a == "Strength"){
    a = "str";
    return 0;
  }
  else if(a == "agi" || a == "Agility"){
    a = "agi";
    return 1;
  }
  else if(a == "sta" || a == "Stamina"){
    a = "sta";
    return 2;
  }
  else if(a == "int" || a == "Intelligence"){
      a = "int";
    return 3;
  }
  else if(a == "sen" || a == "Sense"){
    a = "sen";
    return 4;
  }
  else if(a == "psy" || a == "Psychic"){
    a = "psy";
    return 5;
  }
  else if(a == "Treatment"){
    a = "tre";
    return 6;
  }
  else if(a == "abi")
    return -1;
  else
    return -2;
}

// added for GUI
std::string intToAbiReq(int i){
  if(i == 0)
    return "str";
  else if(i == 1)
    return "agi";
  else if(i == 2)
    return "sta";
  else if(i == 3)
    return "int";
  else if(i == 4)
    return "sen";
  else if(i == 5)
    return "psy";
  else if(i == 6)
    return "tre";
  else
    return "abi";
}


int clusterToInt(const std::string& cluster){
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

std::string convertAbiReq(const std::string& a)
{
  if(a == "Strength")
    return "str";
  if(a == "Agility")
    return "agi";
  if(a == "Stamina")
    return "sta";
  if(a == "Intelligence")
    return "int";
  if(a == "Sense")
    return "sen";
  if(a == "Psychic")
    return "psy";
  return "xxxx";
}

std::string longSlotName(const std::string& a)
{
  if(a == "head") return "Head";
  if(a == "eye") return "Eye";
  if(a == "ear") return "Ear";
  if(a == "chest") return "Chest";
  if(a == "rarm") return "Right-Arm";
  if(a == "larm") return "Left-Arm";
  if(a == "waist") return "Waist";
  if(a == "rwrist") return "Right-Wrist";
  if(a == "lwrist") return "Left-Wrist";
  if(a == "leg") return "Leg";
  if(a == "rhand") return "Right-Hand";
  if(a == "lhand") return "Left-Hand";
  if(a == "feet") return "Feet";
  return "xxxx";
}

std::string longAbiName(const std::string& a)
{
  if(a == "str") return "Strength";
  if(a == "agi") return "Agility";
  if(a == "sta") return "Stamina";
  if(a == "int") return "Intelligence";
  if(a == "sen") return "Sense";
  if(a == "psy") return "Psychic";
  if(a == "tre") return "Treatment";
  return "xxxx";
}
