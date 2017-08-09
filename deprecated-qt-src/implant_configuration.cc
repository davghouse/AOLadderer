#include "implant_configuration.h"
#include "ladder_helper.h"

using namespace ladder_helper;

ImplantConfiguration::ImplantConfiguration()
{
  config_.reserve(13);
  for(size_type i = 0; i != 13; ++i)
    config_.push_back(Implant(0, IntToSlotName(i), "abi", "shi", "bri", "fad"));
}

double ImplantConfiguration::AverageQL() const
{
  double avg = 0;
  int used_implant_count = 0;
  for(std::vector<Implant>::size_type i = 0; i != config_.size(); ++i){
    // If the implant is non-empty, non-zero, and not a leftover ladder implant:
    if(config_[i].ability_name() != "abi" && config_[i].ql() > 0 && config_[i].remove()){
      avg += config_[i].ql();
      ++used_implant_count;
    }
  }
  if(avg == 0 || used_implant_count == 0)
    return 0;
  return avg/used_implant_count;
}

