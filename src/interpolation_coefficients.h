#ifndef INTERPOLATION_COEFFICIENTS_H_
#define INTERPOLATION_COEFFICIENTS_H_

// abbreviations:
// req=requirement, tre=treatment, abi=ability, ski=skill
// shi=shining,     bri=bright,    fad=faded
// implant requirements/modifiers increase linearly with QL:
// implant requirements:
// y = m*QL + c;
// tre req = tre_req_m*QL + tre_req_c
// abi req = abi_req_m*QL + abi_req_c
// implant modifiers:
// shi ski = shi_ski_m*QL + shi_ski_c
// bri ski, fad ski are similar, as are shi abi, bri abi, fad abi except:
// clusters don't add as much to abilities as they do to skills
// there are some clusters that add to non-skill, non-abilities, but don't matter here
// (treatment is the only skill that does matter)

namespace interpolation_coefficients {

const double tre_req_m = 4.723618;
const double tre_req_c = 6.276382;

const int abi_req_m = 2;
const int abi_req_c = 4;

const double shi_ski_m = 0.497487;
const double shi_ski_c = 5.502513;
const double bri_ski_m = 0.301507;
const double bri_ski_c = 2.698492;
const double fad_ski_m = 0.201005;
const double fad_ski_c = 1.798994;

const double shi_abi_m = 0.251256;
const double shi_abi_c = 4.748744;
const double bri_abi_m = 0.150754;
const double bri_abi_c = 2.849246;
const double fad_abi_m = 0.100503;
const double fad_abi_c = 1.899497;

} // namespace interpolation_coefficients

#endif // INTERPOLATION_COEFFICIENTS_H_
