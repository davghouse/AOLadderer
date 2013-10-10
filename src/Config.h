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
  // added for GUI
  friend class MainWindow;
public:
  typedef std::vector<Implant>::size_type size_type;

  Config() {
    config.reserve(13);
      for(size_type i = 0; i != 13; i++){
        config.push_back(Implant(0, intToSlot(i), "abi", "shi", "bri", "fad"));
      }
  }
  Config(std::istream& in){
    // for now, assume reqs.txt will have all 13 implants listed, in the correct order.
	// slots with no required implant will have abi as a req, and shi bri fad as clusters.
    config.reserve(13);
    std::string slot, a, s, b, f;
    while(in >> slot){
      in >> a >> s >> b >> f;
      config.push_back(Implant(0, slot, a, s, b, f));
    }
  }

  double avgQL() const{ 
    double avg = 0;
    int usedImplantCount = 0;
    for(std::vector<Implant>::size_type i = 0; i != config.size(); i++){
      if(config[i].abi() != "abi" && config[i].ql() > 0){
        avg += config[i].ql();
        usedImplantCount++;
      }
    }
    if(avg == 0 || usedImplantCount == 0){
      return 0;
    }
    return avg/usedImplantCount;
  }

  bool notEmpty() const{
    return !order.empty();
  }

  int maxQL() const{
    int max = 0;
    for(std::vector<Implant>::size_type i = 0; i != config.size(); i++){
      if(max < config[i].ql())
        max = config[i].ql();
    }
    return max;
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

  void out(std::ostream& out) const{
    for(std::vector<Implant>::const_iterator it = config.begin();
        it != config.end(); it++){
        if(it->abi() != "abi" && it->ql() > 0){
        it->out(out);
        out << std::endl;
	    }
    }
  }
  
  // mainly for debugging purposes; to show the order of insertion for a configuration
  // not necessary in practice as can just right click many times quickly
  void zeroOut(std::ostream& out) const{
    for(std::vector<int>::size_type i = 0; i != order.size(); i++){
	    if(config[order[i]].abi() != "abi" && config[order[i]].ql() > 0){
	      config[order[i]].out(out);
	      out << std::endl;
	    }
	  }
  }

  int getOrderedIndex(int i ) const{
    return order[i];
  }

  std::vector<int>::const_reverse_iterator rbegin() const{
    return order.rbegin();
  }

  std::vector<int>::const_reverse_iterator rend() const{
    return order.rend();
  }

  std::vector<int>::const_iterator begin() const{
    return order.begin();
  }

  std::vector<int>::const_iterator end() const{
    return order.end();
  }

  // added for GUI
  std::vector<Implant>::iterator impsBegin() {
    return config.begin();
  }

  std::vector<Implant>::iterator impsEnd() {
    return config.end();
  }

private:
  // slot order: head, eye, ear, chest, rarm, larm, waist, rwrist, lwrist, leg, rhand, lhand, feet
  //             0     1    2    3      4     5     6      7       8       9    10     11     12
  std::vector<Implant> config;
  std::vector<int> order; // used in zero height solution to get proper ordering; see zeroOut
};
#endif
