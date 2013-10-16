#include <math.h>
#include "character_stats.h"
#include "interpolation_coefficients.h"

using std::string; using std::vector;
using std::min;
using namespace interpolation_coefficients;

// anonymous namespace as only used in this .cpp file
namespace {

// rounding here is easy
inline int QLToTre(int QL)
{
  return QL*tre_req_m + tre_req_c + .5;
}

// rounding is detailed here - be really explicit to avoid errors
inline int treToQL(double tre)
{
  int QL = ceil((tre - tre_req_c)/tre_req_m);
  if(QLToTre(QL + 1) <= tre)
    return min(QL + 1, 200);
  if(QLToTre(QL) <= tre)
    return min(QL, 200);
  if(QLToTre(QL - 1) <= tre)
    return min(QL - 1, 200);
  if(QLToTre(QL - 2) <= tre)
    return min(QL - 2, 200);
  return min(QL - 3, 200);
}

inline int QLToAbi(int QL)
{
  return QL*abi_req_m + abi_req_c + .5;
}

inline int abiToQL(int abi)
{
  return std::min((abi - abi_req_c)/abi_req_m, 200);
}

// 0 - shining, 1 - bright, 2 - faded
inline int abiModFromQL(int type, int QL)
{
  if(type == 0)
    return shi_abi_m*QL + shi_abi_c + .5;
  if(type == 1)
    return bri_abi_m*QL + bri_abi_c + .5;
  return fad_abi_m*QL + fad_abi_c + .5;
}

inline int skiModFromQL(int type, int QL)
{
  if(type == 0)
    return shi_ski_m*QL + shi_ski_c + .5;
  if(type == 1)
    return bri_ski_m*QL + bri_ski_c + .5;
  return fad_ski_m*QL + fad_ski_c + .5;
}

inline double treTrickleFromAbiMod(int abi, int abiMod)
{
  // treatment trickle: (.3*agi + .5*int + .2*sen)/4
  // strength, agility, stamina, intelligence, sense, psychic
  // 0         1        2        3             4      5
  if(abi == 1)
    return (.3*abiMod)/4;
  else if(abi == 3)
    return (.5*abiMod)/4;
  else if(abi == 4)
    return (.2*abiMod)/4;
  return 0;
}

} // namespace

Stats::Stats() : treatment(0.0)
{
  abis.reserve(6);
}

void Stats::updateStats(const vector<int>& abisHolder, double treatmentHolder)
{
  for(std::vector<int>::size_type i = 0; i != 6; ++i)
    abis.push_back(abisHolder[i]);
  treatment = treatmentHolder;
}

int Stats::updateStats(const Implant& imp, bool inserting, int QL)
{
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

