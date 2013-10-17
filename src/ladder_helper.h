#ifndef LADDER_HELPER_H_
#define LADDER_HELPER_H_

#include <string>
#include <vector>
#include "implant.h"

// Helper functions, converting from strings to ints, ints to strings,
// and back a forth between abbreviations and full names.
// TODO: Redesign so most of this isn't necessary
namespace ladder_helper {

int SlotNameToInt(const std::string& slot_name);
std::string IntToSlotName(int i);
int AbilityToInt(const std::string& ability_name);
int ClusterToInt(const std::string& cluster);
int AbilityToIntAndMakeFull(std::string& ability_name);
std::string AbilityFullToAbbr(const std::string& ability_name);
std::string SlotAbbrToFull(const std::string& slot_name);
std::string AbilityAndTreatmentAbbrToFull(const std::string& skill);
bool SlotTaken(const std::string& slot_name, const std::vector<Implant>& implants);

} // namespace ladder_helper


#endif // LADDER_HELPER_H_
