using AOLadderer.Stats;
using System.Collections.Generic;

namespace AOLadderer.ClusterTemplates
{
    public sealed class FadedClusterTemplate : ClusterTemplate
    {
        private FadedClusterTemplate(Stat stat, ImplantSlot implantSlot)
            : base(stat, ClusterGrade.Faded, implantSlot)
        { }

        public override int GetStatIncrease(int implantQL)
            => Stat.GetFadedStatIncrease(implantQL);

        public static readonly IReadOnlyList<FadedClusterTemplate> FadedClusterTemplates = new[]
        {
            new FadedClusterTemplate(Skill.OneHandBlunt, ImplantSlot.RightHand),
            new FadedClusterTemplate(Skill.OneHandEdgedWeapon, ImplantSlot.RightHand),
            new FadedClusterTemplate(Skill.TwoHandBlunt, ImplantSlot.Chest),
            new FadedClusterTemplate(Skill.TwoHandEdged, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.Adventuring, ImplantSlot.Chest),
            new FadedClusterTemplate(Ability.Agility, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.AimedShot, ImplantSlot.RightHand),
            new FadedClusterTemplate(Skill.AssaultRif, ImplantSlot.Eye),
            new FadedClusterTemplate(Skill.BioMetamor, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.BodyDev, ImplantSlot.Leg),
            new FadedClusterTemplate(Skill.Bow, ImplantSlot.Eye),
            new FadedClusterTemplate(Skill.BowSpcAtt, ImplantSlot.RightWrist),
            new FadedClusterTemplate(Skill.Brawling, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.BreakAndEntry, ImplantSlot.Chest),
            new FadedClusterTemplate(Skill.Burst, ImplantSlot.RightHand),
            new FadedClusterTemplate(ArmorClass.ChemicalAC, ImplantSlot.LeftArm),
            new FadedClusterTemplate(Skill.Chemistry, ImplantSlot.RightHand),
            new FadedClusterTemplate(ArmorClass.ColdAC, ImplantSlot.LeftHand),
            new FadedClusterTemplate(Skill.CompLiter, ImplantSlot.RightHand),
            new FadedClusterTemplate(Skill.Concealment, ImplantSlot.Eye),
            new FadedClusterTemplate(Skill.Dimach, ImplantSlot.Waist),
            new FadedClusterTemplate(ArmorClass.DiseaseAC, ImplantSlot.Chest),
            new FadedClusterTemplate(Skill.DodgeRng, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.DuckExp, ImplantSlot.Feet),
            new FadedClusterTemplate(Skill.ElecEngi, ImplantSlot.RightHand),
            new FadedClusterTemplate(ArmorClass.EnergyAC, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.EvadeClsC, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.FastAttack, ImplantSlot.RightArm),
            new FadedClusterTemplate(ArmorClass.FireAC, ImplantSlot.RightHand),
            new FadedClusterTemplate(Skill.FirstAid, ImplantSlot.LeftHand),
            new FadedClusterTemplate(Skill.FlingShot, ImplantSlot.RightWrist),
            new FadedClusterTemplate(Skill.FullAuto, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.Grenade, ImplantSlot.RightHand),
            new FadedClusterTemplate(Skill.HeavyWeapons, ImplantSlot.RightHand),
            new FadedClusterTemplate(ArmorClass.ImpProjAC, ImplantSlot.Waist),
            new FadedClusterTemplate(Ability.Intelligence, ImplantSlot.Ear),
            new FadedClusterTemplate(Skill.MartialArts, ImplantSlot.LeftHand),
            new FadedClusterTemplate(Skill.MattMetam, ImplantSlot.LeftArm),
            new FadedClusterTemplate(Skill.MatterCrea, ImplantSlot.Eye),
            new FadedClusterTemplate(MaxHealthOrNano.MaxHealth, ImplantSlot.Leg),
            new FadedClusterTemplate(MaxHealthOrNano.MaxNano, ImplantSlot.Chest),
            new FadedClusterTemplate(Skill.MechEngi, ImplantSlot.RightArm),
            new FadedClusterTemplate(Skill.MeleeEner, ImplantSlot.RightWrist),
            new FadedClusterTemplate(Skill.MeleeInit, ImplantSlot.Waist),
            new FadedClusterTemplate(ArmorClass.MeleeMaAC, ImplantSlot.Leg),
            new FadedClusterTemplate(Skill.MGSMG, ImplantSlot.Chest),
            new FadedClusterTemplate(Skill.MultMelee, ImplantSlot.RightWrist),
            new FadedClusterTemplate(Skill.MultiRanged, ImplantSlot.Eye),
            new FadedClusterTemplate(Skill.NanoPool, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.NanoProgra, ImplantSlot.RightHand),
            new FadedClusterTemplate(Skill.NanoResist, ImplantSlot.LeftWrist),
            new FadedClusterTemplate(Skill.NanoCInit, ImplantSlot.Chest),
            new FadedClusterTemplate(Skill.Parry, ImplantSlot.RightArm),
            new FadedClusterTemplate(Skill.Perception, ImplantSlot.Head),
            new FadedClusterTemplate(Skill.PharmaTech, ImplantSlot.RightHand),
            new FadedClusterTemplate(Skill.PhysicInit, ImplantSlot.LeftArm),
            new FadedClusterTemplate(Skill.Piercing, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.Pistol, ImplantSlot.Eye),
            new FadedClusterTemplate(Ability.Psychic, ImplantSlot.Ear),
            new FadedClusterTemplate(Skill.PsychoModi, ImplantSlot.Ear),
            new FadedClusterTemplate(Skill.Psychology, ImplantSlot.Eye),
            new FadedClusterTemplate(Skill.QuantumFT, ImplantSlot.RightHand),
            new FadedClusterTemplate(ArmorClass.RadiationAC, ImplantSlot.RightArm),
            new FadedClusterTemplate(Skill.RangedEner, ImplantSlot.LeftHand),
            new FadedClusterTemplate(Skill.RangedInit, ImplantSlot.RightHand),
            new FadedClusterTemplate(Skill.Rifle, ImplantSlot.LeftWrist),
            new FadedClusterTemplate(Skill.Riposte, ImplantSlot.RightArm),
            new FadedClusterTemplate(Skill.RunSpeed, ImplantSlot.Leg),
            new FadedClusterTemplate(Ability.Sense, ImplantSlot.Head),
            new FadedClusterTemplate(Skill.SensoryImpr, ImplantSlot.Chest),
            new FadedClusterTemplate(Skill.SharpObj, ImplantSlot.Eye),
            new FadedClusterTemplate(Skill.Shotgun, ImplantSlot.Waist),
            new FadedClusterTemplate(Skill.SneakAtck, ImplantSlot.Eye),
            new FadedClusterTemplate(Ability.Stamina, ImplantSlot.Waist),
            new FadedClusterTemplate(Ability.Strength, ImplantSlot.Chest),
            new FadedClusterTemplate(Skill.TimeAndSpace, ImplantSlot.Eye),
            new FadedClusterTemplate(Skill.TrapDisarm, ImplantSlot.Head),
            new FadedClusterTemplate(Skill.Treatment, ImplantSlot.RightHand),
            new FadedClusterTemplate(Skill.Tutoring, ImplantSlot.Head),
            new FadedClusterTemplate(Skill.VehicleAir, ImplantSlot.Head),
            new FadedClusterTemplate(Skill.VehicleGrnd, ImplantSlot.Ear),
            new FadedClusterTemplate(Skill.VehicleHydr, ImplantSlot.Ear),
            new FadedClusterTemplate(Skill.WeaponSmt, ImplantSlot.Eye)
        };
    }
}
