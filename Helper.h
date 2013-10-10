#ifndef GUARD_Helper_h_
#define GUARD_Helper_h_
#include <string>

int slotToInt(const std::string& slot);
std::string intToSlot(int i);
int abiReqToInt(const std::string& a);
std::string intToAbiReq(int i);
int clusterToInt(const std::string& cluster);
int abiReqToInt_GUI(std::string& a);
std::string convertAbiReq(const std::string& a);
std::string longSlotName(const std::string& a);
std::string longAbiName(const std::string& a);

#endif
