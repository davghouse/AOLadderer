#ifndef GUARD_Config_h_
#define GUARD_Config_h_

#include <fstream>
#include <iostream>
#include <string>
#include <vector>
#include <numeric>
#include "Implant.h"
#include "Helper.h"

class Config{
  friend class MainWindow;
public:
  typedef std::vector<Implant>::size_type size_type;

  Config(){
    config.reserve(13);
    for(size_type i = 0; i != 13; i++)
      config.push_back(Implant(0, intToSlot(i), "abi", "shi", "bri", "fad"));
  }

  double avgQL() const{ 
    double avg = 0;
    int usedImplantCount = 0;
    for(std::vector<Implant>::size_type i = 0; i != config.size(); i++){
      if(config[i].abi() != "abi" && config[i].ql() > 0 && config[i].mustRemove()){
        avg += config[i].ql();
        usedImplantCount++;
      }
    }
    if(avg == 0 || usedImplantCount == 0)
      return 0;
    return avg/usedImplantCount;
  }

  void updateConfig(Implant imp, int qualityLevel = 0){
    if(qualityLevel == 0){
      config[imp.slotI()] = imp;
      order.push_back(imp.slotI());
    }
    else{
      config[imp.slotI()] = imp;
      order.push_back(imp.slotI());
      config[imp.slotI()].setQL(qualityLevel);
    }
  }
 
  size_type size() const{ return config.size(); }
  const Implant& operator[](int i) const { return config[i]; }
  std::vector<int>::const_reverse_iterator rbegin() const{ return order.rbegin(); }
  std::vector<int>::const_reverse_iterator rend() const{ return order.rend(); }
  std::vector<int>::const_iterator begin() const{ return order.begin(); }
  std::vector<int>::const_iterator end() const{ return order.end(); }

private:
  // slot order: head, eye, ear, chest, rarm, larm, waist, rwrist, lwrist, leg, rhand, lhand, feet
  //             0     1    2    3      4     5     6      7       8       9    10     11     12
  std::vector<Implant> config;
  std::vector<int> order; // used in zero height solution to get proper ordering
};
#endif
