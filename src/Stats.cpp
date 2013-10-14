#include <math.h>
#include "Stats.h"

// rounding is detailed here
// be really explicit to avoid errors
int treToQL(double tre)
{
  int QL = ceil((tre - TREREQc)/TREREQm);
  int temp = 0;
  if(QLToTre(QL + 2) <= tre){
    // can't happen?
    temp = QL + 2;
  }
  else if(QLToTre(QL+1) <= tre){
    // can't happen?
    temp = QL + 1;
  }
  else if(QLToTre(QL) <= tre){
    temp = QL;
  }
  else if(QLToTre(QL - 1) <= tre){
    temp = QL - 1;
  }
  else if(QLToTre(QL - 2) <= tre){
    // can happen
    temp = QL - 2;
  }
  else if(QLToTre(QL - 3) <= tre){
    // can't happen?
    temp = QL - 3;
  }
  return std::min(temp, 200);
}

// treatment required rounds to nearest integer
int QLToTre(int QL)
{
  return QL*TREREQm + TREREQc + .5;
}

int abiToQL(int abi)
{
  return std::min((abi - ABIREQc)/ABIREQm, 200);
}

// 0 - shining, 1 - bright, 2 - faded
int abiModFromQL(int type, int QL)
{
  if(type == 0)
    return SHIABIm*QL + SHIABIc + .5;
  if(type == 1)
    return BRIABIm*QL + BRIABIc + .5;
  return FADABIm*QL + FADABIc + .5;
}

int skiModFromQL(int type, int QL)
{
  if(type == 0)
    return SHISKIm*QL + SHISKIc + .5;
  if(type == 1)
    return BRISKIm*QL + BRISKIc + .5;
  return FADSKIm*QL + FADSKIc + .5;
}

double treTrickleFromAbiMod(int abi, int abiMod)
{
  // treatment trickle: (.5*int + .3*agi * .2*sen)/4
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

int QLToAbi(int QL)
{
  return QL*ABIREQm + ABIREQc + .5;
}
