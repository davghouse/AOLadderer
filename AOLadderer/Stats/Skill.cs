using System.Collections.Generic;

namespace AOLadderer.Stats
{
    public sealed class Skill : Stat
    {
        private Skill(string name)
            : base(name)
        { }

        public override int GetShinyStatIncrease(int implantQL)
            => (int)(5.5025126 + 0.4974874 * implantQL + 0.5);

        public override int GetBrightStatIncrease(int implantQL)
            => (int)(2.6984925 + 0.3015075 * implantQL + 0.5);

        public override int GetFadedStatIncrease(int implantQL)
            => (int)(1.7989950 + 0.2010050 * implantQL + 0.5);

        public static readonly Skill
            OneHandBlunt = new Skill("1h Blunt"),
            OneHandEdgedWeapon = new Skill("1h Edged Weapon"),
            TwoHandBlunt = new Skill("2h Blunt"),
            TwoHandEdged = new Skill("2h Edged"),
            Adventuring = new Skill("Adventuring"),
            AimedShot = new Skill("Aimed Shot"),
            AssaultRif = new Skill("Assault Rif"),
            BioMetamor = new Skill("Bio.Metamor"),
            BodyDev = new Skill("Body Dev"),
            Bow = new Skill("Bow"),
            BowSpcAtt = new Skill("Bow Spc Att"),
            Brawling = new Skill("Brawling"),
            BreakAndEntry = new Skill("Break & Entry"),
            Burst = new Skill("Burst"),
            Chemistry = new Skill("Chemistry"),
            CompLiter = new Skill("Comp. Liter"),
            Concealment = new Skill("Concealment"),
            Dimach = new Skill("Dimach"),
            DodgeRng = new Skill("Dodge-Rng"),
            DuckExp = new Skill("Duck-Exp"),
            ElecEngi = new Skill("Elec. Engi"),
            EvadeClsC = new Skill("Evade-ClsC"),
            FastAttack = new Skill("Fast Attack"),
            FirstAid = new Skill("First Aid"),
            FlingShot = new Skill("Fling Shot"),
            FullAuto = new Skill("Full Auto"),
            Grenade = new Skill("Grenade"),
            HeavyWeapons = new Skill("Heavy Weapons"),
            MartialArts = new Skill("Martial Arts"),
            MattMetam = new Skill("Matt.Metam"),
            MatterCrea = new Skill("Matter Crea"),
            MechEngi = new Skill("Mech. Engi"),
            MeleeEner = new Skill("Melee Ener"),
            MeleeInit = new Skill("Melee. Init"),
            MGSMG = new Skill("MG / SMG"),
            MultMelee = new Skill("Mult. Melee"),
            MultiRanged = new Skill("Multi Ranged"),
            NanoPool = new Skill("Nano Pool"),
            NanoProgra = new Skill("Nano Progra"),
            NanoResist = new Skill("Nano Resist"),
            NanoCInit = new Skill("NanoC. Init"),
            Parry = new Skill("Parry"),
            Perception = new Skill("Perception"),
            PharmaTech = new Skill("Pharma Tech"),
            PhysicInit = new Skill("Physic. Init"),
            Piercing = new Skill("Piercing"),
            Pistol = new Skill("Pistol"),
            PsychoModi = new Skill("Psycho Modi"),
            Psychology = new Skill("Psychology"),
            QuantumFT = new Skill("Quantum FT"),
            RangedEner = new Skill("Ranged Ener"),
            RangedInit = new Skill("Ranged. Init"),
            Rifle = new Skill("Rifle"),
            Riposte = new Skill("Riposte"),
            RunSpeed = new Skill("Run Speed"),
            SensoryImpr = new Skill("Sensory Impr"),
            SharpObj = new Skill("Sharp Obj"),
            Shotgun = new Skill("Shotgun"),
            SneakAtck = new Skill("Sneak Atck"),
            TimeAndSpace = new Skill("Time & Space"),
            TrapDisarm = new Skill("Trap Disarm"),
            Treatment = new Skill("Treatment"),
            Tutoring = new Skill("Tutoring"),
            VehicleAir = new Skill("Vehicle Air"),
            VehicleGrnd = new Skill("Vehicle Grnd"),
            VehicleHydr = new Skill("Vehicle Hydr"),
            WeaponSmt = new Skill("Weapon Smt");

        public static readonly IReadOnlyList<Skill> Skills = new[] { OneHandBlunt, OneHandEdgedWeapon, TwoHandBlunt, TwoHandEdged, Adventuring, AimedShot, AssaultRif, BioMetamor, BodyDev, Bow, BowSpcAtt, Brawling, BreakAndEntry, Burst, Chemistry, CompLiter, Concealment, Dimach, DodgeRng, DuckExp, ElecEngi, EvadeClsC, FastAttack, FirstAid, FlingShot, FullAuto, Grenade, HeavyWeapons, MartialArts, MattMetam, MatterCrea, MechEngi, MeleeEner, MeleeInit, MGSMG, MultMelee, MultiRanged, NanoPool, NanoProgra, NanoResist, NanoCInit, Parry, Perception, PharmaTech, PhysicInit, Piercing, Pistol, PsychoModi, Psychology, QuantumFT, RangedEner, RangedInit, Rifle, Riposte, RunSpeed, SensoryImpr, SharpObj, Shotgun, SneakAtck, TimeAndSpace, TrapDisarm, Treatment, Tutoring, VehicleAir, VehicleGrnd, VehicleHydr, WeaponSmt };
    }
}
