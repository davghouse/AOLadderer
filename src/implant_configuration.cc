#include "implant_configuration.h"
#include "ladder_helper.h"

using namespace ladder_helper;

Config::Config()
{
  config.reserve(13);
  for(size_type i = 0; i != 13; ++i)
    config.push_back(Implant(0, intToSlot(i), "abi", "shi", "bri", "fad"));
}

double Config::avgQL() const
{
  double avg = 0;
  int usedImplantCount = 0;
  for(std::vector<Implant>::size_type i = 0; i != config.size(); ++i){
    if(config[i].abi() != "abi" && config[i].ql() > 0 && config[i].mustRemove()){
      avg += config[i].ql();
      ++usedImplantCount;
    }
  }
  if(avg == 0 || usedImplantCount == 0)
    return 0;
  return avg/usedImplantCount;
}

