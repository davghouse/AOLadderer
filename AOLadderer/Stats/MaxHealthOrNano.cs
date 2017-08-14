using System.Collections.Generic;

namespace AOLadderer.Stats
{
    public sealed class MaxHealthOrNano : Stat
    {
        private MaxHealthOrNano(string name)
            : base(name)
        { }

        public override int GetShinyStatIncrease(int implantQL)
            => (int)(5.0000000 + 2.0000000 * implantQL + 0.5);

        public override int GetBrightStatIncrease(int implantQL)
            => (int)(2.7989950 + 1.2010050 * implantQL + 0.5);

        public override int GetFadedStatIncrease(int implantQL)
            => (int)(2.2010050 + 0.7989950 * implantQL + 0.5);

        public static readonly MaxHealthOrNano
            MaxHealth = new MaxHealthOrNano("Max Health"),
            MaxNano = new MaxHealthOrNano("Max Nano");

        public static readonly IReadOnlyList<MaxHealthOrNano> MaxHealthAndNano = new[] { MaxHealth, MaxNano };
    }
}
