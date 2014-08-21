#ifndef IMPLANT_CONFIGURATION_H_
#define IMPLANT_CONFIGURATION_H_

#include <fstream>
#include <iostream>
#include <string>
#include <vector>
#include <numeric>
#include "implant.h"

// An ImplantConfiguration is a set of up to 13 implants, with an order of insertion.
// The implants are stored in a vector, with a predetermined order.
class ImplantConfiguration{
  friend class MainWindow;
  friend class Ladder;
public:
  typedef std::vector<Implant>::size_type size_type;
  // Create an empty configuration.
  ImplantConfiguration();
  double AverageQL() const;
  // Insert an implant into the configuration, noting the order of its insertion.
  void UpdateConfig(const Implant& implant);
 
  size_type size() const{ return config_.size(); }
  const Implant& operator[](int i) const{ return config_[i]; }
  Implant& operator[](int i){ return config_[i]; }
  std::vector<int>::const_reverse_iterator rbegin() const{ return order_.rbegin(); }
  std::vector<int>::const_reverse_iterator rend() const{ return order_.rend(); }
  std::vector<int>::const_iterator begin() const{ return order_.begin(); }
  std::vector<int>::const_iterator end() const{ return order_.end(); }

private:
  // slot order: head, eye, ear, chest, rarm, larm, waist, rwrist, lwrist, leg, rhand, lhand, feet
  //             0     1    2    3      4     5     6      7       8       9    10     11     12
  std::vector<Implant> config_;
  // Order of insertion.
  std::vector<int> order_;
};

inline void ImplantConfiguration::UpdateConfig(const Implant& implant)
{
  config_[implant.slot_int()] = implant;
  order_.push_back(implant.slot_int());
}

#endif // IMPLANT_CONFIGURATION_H_
