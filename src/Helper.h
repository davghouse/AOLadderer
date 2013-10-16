#ifndef HELPER_H_
#define HELPER_H_
#include <string>

int slotToInt(const std::string& slot);
std::string intToSlot(int i);
int abiReqToInt(const std::string& a);
int clusterToInt(const std::string& cluster);
int abiReqToInt_GUI(std::string& a);
std::string convertAbiReq(const std::string& a);
std::string longSlotName(const std::string& a);
std::string longAbiName(const std::string& a);

#endif // HELPER_H_
