#ifndef INTERPOLATION_COEFFICIENTS_H_
#define INTERPOLATION_COEFFICIENTS_H_

// req=requirement, tre=treatment, abi=ability, ski=skill
// implant requirements:
// y = m*QL + c;
// tre req = a*QL + b;
// abi req = c*QL + d;
// implant modifiers:
// shi ski = e*QL + f
// bri ski, fad ski, shi abi, bri abi, fad abi similar.

#define TREREQm 4.723618
#define TREREQc 6.276382
#define ABIREQm 2
#define ABIREQc 4

#define SHISKIm 0.497487
#define SHISKIc 5.502513
#define BRISKIm 0.301507
#define BRISKIc 2.698492
#define FADSKIm 0.201005
#define FADSKIc 1.798994

#define SHIABIm 0.251256
#define SHIABIc 4.748744
#define BRIABIm 0.150754
#define BRIABIc 2.849246
#define FADABIm 0.100503
#define FADABIc 1.899497

#endif // INTERPOLATION_COEFFICIENTS_H_
