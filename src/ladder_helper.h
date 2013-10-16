#ifndef LADDER_HELPER_H_
#define LADDER_HELPER_H_

#include <string>
#include <vector>
#include "implant.h"

namespace ladder_helper {

int SlotNameToInt(const std::string& slot);
std::string IntToSlotName(int i);
int AbilityToInt(const std::string& a);
int ClusterToInt(const std::string& cluster);
int AbilityToIntAndMakeFull(std::string& a);
std::string AbilityFullToAbbr(const std::string& a);
std::string SlotAbbrToFull(const std::string& a);
std::string AbilityAndTreatmentAbbrToFull(const std::string& a);
bool SlotTaken(const std::string&, const std::vector<Implant>&);

} // namespace ladder_helper


#endif // LADDER_HELPER_H_
