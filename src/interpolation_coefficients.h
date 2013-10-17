#ifndef INTERPOLATION_COEFFICIENTS_H_
#define INTERPOLATION_COEFFICIENTS_H_

// abbreviations:
// req=requirement, tre=treatment, abi=ability, ski=skill
// shi=shining,     bri=bright,    fad=faded
// implant requirements/modifiers increase linearly with ql:
// implant requirements:
// y = m*ql + c;
// tre req = tre_req_m*ql + tre_req_c
// abi req = abi_req_m*ql + abi_req_c
// implant modifiers:
// shi ski = shi_ski_m*ql + shi_ski_c
// bri ski, fad ski are similar, as are shi abi, bri abi, fad abi except:
// clusters don't add as much to abilities as they do to skills
// there are some clusters that add to non-skill, non-abilities, but don't matter here
// (treatment is the only skill that does matter)

namespace interpolation_coefficients {

const double kTreReqM = 4.723618;
const double kTreReqC = 6.276382;

const int kAbiReqM = 2;
const int kAbiReqC = 4;

const double kShiSkiM = 0.497487;
const double kShiSkiC = 5.502513;
const double kBriSkiM = 0.301507;
const double kBriSkiC = 2.698492;
const double kFadSkiM = 0.201005;
const double kFadSkiC = 1.798994;

const double kShiAbiM = 0.251256;
const double kShiAbiC = 4.748744;
const double kBriAbiM = 0.150754;
const double kBriAbiC = 2.849246;
const double kFadAbiM = 0.100503;
const double kFadAbiC = 1.899497;

} // namespace interpolation_coefficients

#endif // INTERPOLATION_COEFFICIENTS_H_
