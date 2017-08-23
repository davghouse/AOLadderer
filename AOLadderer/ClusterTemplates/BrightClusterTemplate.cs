using AOLadderer.Stats;
using System.Collections.Generic;

namespace AOLadderer.ClusterTemplates
{
    public sealed class BrightClusterTemplate : ClusterTemplate
    {
        private BrightClusterTemplate(Stat stat, ImplantSlot implantSlot)
            : base(stat, ClusterGrade.Bright, implantSlot)
        { }

        public override int GetStatIncrease(int implantQL)
            => Stat.GetBrightStatIncrease(implantQL);

        public override int GetMinimumClusterQL(int implantQL)
            => (int)(.84 * implantQL);

        public static readonly IReadOnlyList<BrightClusterTemplate> BrightClusterTemplates = new[]
        {
            new BrightClusterTemplate(Skill.OneHandBlunt, ImplantSlot.RightWrist),
            new BrightClusterTemplate(Skill.OneHandEdgedWeapon, ImplantSlot.RightWrist),
            new BrightClusterTemplate(Skill.TwoHandBlunt, ImplantSlot.LeftArm),
            new BrightClusterTemplate(Skill.TwoHandEdged, ImplantSlot.LeftArm),
            new BrightClusterTemplate(Skill.Adventuring, ImplantSlot.Waist),
            new BrightClusterTemplate(Ability.Agility, ImplantSlot.Feet),
            new BrightClusterTemplate(Skill.AimedShot, ImplantSlot.RightWrist),
            new BrightClusterTemplate(Skill.AssaultRif, ImplantSlot.RightHand),
            new BrightClusterTemplate(Skill.BioMetamor, ImplantSlot.Chest),
            new BrightClusterTemplate(Skill.BodyDev, ImplantSlot.Waist),
            new BrightClusterTemplate(Skill.Bow, ImplantSlot.LeftArm),
            new BrightClusterTemplate(Skill.BowSpcAtt, ImplantSlot.RightHand),
            new BrightClusterTemplate(Skill.Brawling, ImplantSlot.RightArm),
            new BrightClusterTemplate(Skill.BreakAndEntry, ImplantSlot.LeftArm),
            new BrightClusterTemplate(Skill.Burst, ImplantSlot.RightWrist),
            new BrightClusterTemplate(ArmorClass.ChemicalAC, ImplantSlot.RightArm),
            new BrightClusterTemplate(Skill.Chemistry, ImplantSlot.Eye),
            new BrightClusterTemplate(ArmorClass.ColdAC, ImplantSlot.RightHand),
            new BrightClusterTemplate(Skill.CompLiter, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.Concealment, ImplantSlot.Ear),
            new BrightClusterTemplate(Skill.Dimach, ImplantSlot.Head),
            new BrightClusterTemplate(ArmorClass.DiseaseAC, ImplantSlot.Leg),
            new BrightClusterTemplate(Skill.DodgeRng, ImplantSlot.Feet),
            new BrightClusterTemplate(Skill.DuckExp, ImplantSlot.Waist),
            new BrightClusterTemplate(Skill.ElecEngi, ImplantSlot.Head),
            new BrightClusterTemplate(ArmorClass.EnergyAC, ImplantSlot.Leg),
            new BrightClusterTemplate(Skill.EvadeClsC, ImplantSlot.Leg),
            new BrightClusterTemplate(Skill.FastAttack, ImplantSlot.RightHand),
            new BrightClusterTemplate(ArmorClass.FireAC, ImplantSlot.LeftHand),
            new BrightClusterTemplate(Skill.FirstAid, ImplantSlot.RightHand),
            new BrightClusterTemplate(Skill.FlingShot, ImplantSlot.RightHand),
            new BrightClusterTemplate(Skill.FullAuto, ImplantSlot.RightWrist),
            new BrightClusterTemplate(Skill.Grenade, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.HeavyWeapons, ImplantSlot.Eye),
            new BrightClusterTemplate(ArmorClass.ImpProjAC, ImplantSlot.Chest),
            new BrightClusterTemplate(Ability.Intelligence, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.MartialArts, ImplantSlot.Feet),
            new BrightClusterTemplate(Skill.MattMetam, ImplantSlot.Chest),
            new BrightClusterTemplate(Skill.MatterCrea, ImplantSlot.RightHand),
            new BrightClusterTemplate(MaxHealthOrNano.MaxHealth, ImplantSlot.Waist),
            new BrightClusterTemplate(MaxHealthOrNano.MaxNano, ImplantSlot.Waist),
            new BrightClusterTemplate(Skill.MechEngi, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.MeleeEner, ImplantSlot.LeftWrist),
            new BrightClusterTemplate(Skill.MeleeInit, ImplantSlot.Leg),
            new BrightClusterTemplate(ArmorClass.MeleeMaAC, ImplantSlot.Waist),
            new BrightClusterTemplate(Skill.MGSMG, ImplantSlot.RightHand),
            new BrightClusterTemplate(Skill.MultMelee, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.MultiRanged, ImplantSlot.RightWrist),
            new BrightClusterTemplate(Skill.NanoPool, ImplantSlot.Head),
            new BrightClusterTemplate(Skill.NanoProgra, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.NanoResist, ImplantSlot.RightWrist),
            new BrightClusterTemplate(Skill.NanoCInit, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.Parry, ImplantSlot.LeftWrist),
            new BrightClusterTemplate(Skill.Perception, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.PharmaTech, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.PhysicInit, ImplantSlot.RightArm),
            new BrightClusterTemplate(Skill.Piercing, ImplantSlot.LeftArm),
            new BrightClusterTemplate(Skill.Pistol, ImplantSlot.RightHand),
            new BrightClusterTemplate(Ability.Psychic, ImplantSlot.Chest),
            new BrightClusterTemplate(Skill.PsychoModi, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.Psychology, ImplantSlot.Ear),
            new BrightClusterTemplate(Skill.QuantumFT, ImplantSlot.Eye),
            new BrightClusterTemplate(ArmorClass.RadiationAC, ImplantSlot.LeftArm),
            new BrightClusterTemplate(Skill.RangedEner, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.RangedInit, ImplantSlot.Head),
            new BrightClusterTemplate(Skill.Rifle, ImplantSlot.RightWrist),
            new BrightClusterTemplate(Skill.Riposte, ImplantSlot.LeftWrist),
            new BrightClusterTemplate(Skill.RunSpeed, ImplantSlot.LeftWrist),
            new BrightClusterTemplate(Ability.Sense, ImplantSlot.Waist),
            new BrightClusterTemplate(Skill.SensoryImpr, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.SharpObj, ImplantSlot.RightHand),
            new BrightClusterTemplate(Skill.Shotgun, ImplantSlot.RightHand),
            new BrightClusterTemplate(Skill.SneakAtck, ImplantSlot.RightWrist),
            new BrightClusterTemplate(Ability.Stamina, ImplantSlot.Leg),
            new BrightClusterTemplate(Ability.Strength, ImplantSlot.LeftArm),
            new BrightClusterTemplate(Skill.TimeAndSpace, ImplantSlot.RightHand),
            new BrightClusterTemplate(Skill.TrapDisarm, ImplantSlot.LeftHand),
            new BrightClusterTemplate(Skill.Treatment, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.Tutoring, ImplantSlot.Ear),
            new BrightClusterTemplate(Skill.VehicleAir, ImplantSlot.Ear),
            new BrightClusterTemplate(Skill.VehicleGrnd, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.VehicleHydr, ImplantSlot.Eye),
            new BrightClusterTemplate(Skill.WeaponSmt, ImplantSlot.Head),
        };
    }
}
