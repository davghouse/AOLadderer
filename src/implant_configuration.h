#ifndef IMPLANT_CONFIGURATION_H_
#define IMPLANT_CONFIGURATION_H_

#include <fstream>
#include <iostream>
#include <string>
#include <vector>
#include <numeric>
#include "implant.h"

class ImplantConfiguration{
  friend class MainWindow;

public:
  typedef std::vector<Implant>::size_type size_type;

  ImplantConfiguration();
  double AverageQL() const;
  void UpdateConfig(Implant implant, int ql = 0);
 
  size_type size() const{ return config_.size(); }
  const Implant& operator[](int i) const { return config_[i]; }
  std::vector<int>::const_reverse_iterator rbegin() const{ return order_.rbegin(); }
  std::vector<int>::const_reverse_iterator rend() const{ return order_.rend(); }
  std::vector<int>::const_iterator begin() const{ return order_.begin(); }
  std::vector<int>::const_iterator end() const{ return order_.end(); }

private:
  // slot order: head, eye, ear, chest, rarm, larm, waist, rwrist, lwrist, leg, rhand, lhand, feet
  //             0     1    2    3      4     5     6      7       8       9    10     11     12
  std::vector<Implant> config_;
  std::vector<int> order_; // used in zero height solution to get proper ordering
};

inline void ImplantConfiguration::UpdateConfig(Implant implant, int ql)
{
  if(ql == 0){
    config_[implant.slot_int()] = implant;
    order_.push_back(implant.slot_int());
  }
  else{
    config_[implant.slot_int()] = implant;
    order_.push_back(implant.slot_int());
    config_[implant.slot_int()].set_ql(ql);
  }
}

#endif // IMPLANT_CONFIGURATION_H_
