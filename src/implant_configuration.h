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
  double avgQL() const;
  void updateConfig(Implant imp, int qualityLevel = 0);
 
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

inline void ImplantConfiguration::updateConfig(Implant imp, int qualityLevel)
{
  if(qualityLevel == 0){
    config_[imp.slotI()] = imp;
    order_.push_back(imp.slotI());
  }
  else{
    config_[imp.slotI()] = imp;
    order_.push_back(imp.slotI());
    config_[imp.slotI()].setQL(qualityLevel);
  }
}

#endif // IMPLANT_CONFIGURATION_H_
