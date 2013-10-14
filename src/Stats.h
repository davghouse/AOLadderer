#ifndef GUARD_Stats_h_
#define GUARD_Stats_h_

#include <iostream>
#include <string>
#include <fstream>
#include <algorithm>
#include <vector>
#include "Coefficients.h"
#include "Helper.h"
#include "Implant.h"

int treToQL(double);
int QLToTre(int);
int QLToAbi(int);
int abiToQL(int);
int abiModFromQL(int,int);
int skiModFromQL(int,int);
double treTrickleFromAbiMod(int,int);

class Stats{
public:
  Stats() : treatment(0.0) {
    abis.reserve(6);
  }

  Stats(std::istream& in){
    std::string s;
    abis.resize(6);
    in >> s >> abis[0] >> s >> abis[1] >> s >> abis[2]
       >> s >> abis[3] >> s >> abis[4] >> s >> abis[5]
       >> s >> treatment;
  }

  int str() const { return abis[0] ; }
  int agi() const { return abis[1]; }
  int sta() const { return abis[2]; }
  int intel() const { return abis[3]; }
  int sen() const { return abis[4]; }
  int psy() const { return abis[5]; }
  double tre() const { return treatment; }

  int getAbiFromName(const std::string& abiName) const { return abis[abiReqToInt(abiName)]; }
  int getAbiFromIndex(int index) const { return abis[index]; }

  void str(int i) { abis[0] += i; }
  void agi(int i) { abis[1] += i; }
  void sta(int i) { abis[2] += i; }
  void intel(int i) { abis[3] += i; }
  void sen(int i) { abis[4] += i; }
  void psy(int i) { abis[5] += i; }
  void tre(double d) { treatment += d; }

  // added for GUI
  void updateStats(const std::vector<int>& abisHolder, double treatmentHolder)
  {
    for(std::vector<int>::size_type i = 0; i != 6; i++){
      abis.push_back(abisHolder[i]);
    }
    treatment = treatmentHolder;
  }

  int updateStats(const Implant& imp, bool inserting = true, int QL = 0){
    if(inserting){
      // find out what QL imp to put in
      if(QL == 0){
        QL = std::min(treToQL(treatment), abiToQL(abis[imp.abiI()]));
      }
      // update stats
      if(imp.ladder()){
       if(imp.shi() != "shi"){
         if(imp.shiI() <= 5 && imp.shiI() >= 0){
           int abiMod = abiModFromQL(0,QL);
           abis[imp.shiI()] += abiMod;
           treatment += treTrickleFromAbiMod(imp.shiI(), abiMod);
         }
         else if(imp.shiI() == 6)
           treatment += skiModFromQL(0,QL);
       }
       if(imp.bri() != "bri"){
         if(imp.briI() <= 5 && imp.briI() >= 0){
           int abiMod = abiModFromQL(1,QL);
           abis[imp.briI()] += abiMod;
           treatment += treTrickleFromAbiMod(imp.briI(), abiMod);
         }
         else if(imp.briI() == 6)
           treatment += skiModFromQL(1,QL);
       }
       if(imp.fad() != "fad"){
         if(imp.fadI() <= 5 && imp.fadI() >= 0){
           int abiMod = abiModFromQL(2,QL);
           abis[imp.fadI()] += abiMod;
           treatment += treTrickleFromAbiMod(imp.fadI(), abiMod);
         }
         else if(imp.fadI() == 6)
           treatment += skiModFromQL(2,QL);
       }
      }
      return QL;
    }
    else{
      int QL = imp.ql();
      if(imp.shi() != "shi"){
        if(imp.shiI() <= 5 && imp.shiI() >= 0){
          int abiMod = abiModFromQL(0, QL);
          abis[imp.shiI()] -= abiMod;
          treatment -= treTrickleFromAbiMod(imp.shiI(), abiMod);
        }
        else if(imp.shiI() == 6)
          treatment -= skiModFromQL(0,QL);
      }
      if(imp.bri() != "bri"){
        if(imp.briI() <= 5 && imp.briI() >= 0){
          int abiMod = abiModFromQL(1,QL);
          abis[imp.briI()] -= abiMod;
          treatment -= treTrickleFromAbiMod(imp.briI(), abiMod);
        }
        else if(imp.briI() == 6)
          treatment -= skiModFromQL(1,QL);
      }
      if(imp.fad() != "fad"){
        if(imp.fadI() <= 5 && imp.fadI() >= 0){
          int abiMod = abiModFromQL(2,QL);
          abis[imp.fadI()] -= abiMod;
          treatment -= treTrickleFromAbiMod(imp.fadI(), abiMod);
        }
        else if(imp.fadI() == 6)
          treatment -= skiModFromQL(2,QL);
      }
    }
    return 0;
  }
private:
  // strength, agility, stamina, intelligence, sense, psychic
  // 0         1        2        3             4      5
  std::vector<int> abis; 
  double treatment;
};
#endif
