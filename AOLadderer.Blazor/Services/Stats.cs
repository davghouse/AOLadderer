using System;
using System.Collections.Generic;

namespace AOLadderer.Blazor.Services
{
    public class Stats
    {
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Stamina { get; set; }
        public int Intelligence { get; set; }
        public int Sense { get; set; }
        public int Psychic { get; set; }
        public double Treatment { get; set; }

        public event Action OnChange;

        private List<Buff> buffs = new List<Buff>();
        public IReadOnlyCollection<Buff> Buffs => buffs;

        public void Add(Buff buff)
        {
            if (buffs.Contains(buff)) return;
            
            Strength += buff.Strength;
            Agility += buff.Agility;
            Stamina += buff.Stamina;
            Intelligence += buff.Intelligence;
            Sense += buff.Sense;
            Psychic += buff.Psychic;
            Treatment += buff.Treatment;
            buffs.Add(buff);

            OnChange?.Invoke();
        }

        public void Remove(Buff buff)
        {
            if (!buffs.Contains(buff)) return;

            Strength -= buff.Strength;
            Agility -= buff.Agility;
            Stamina -= buff.Stamina;
            Intelligence -= buff.Intelligence;
            Sense -= buff.Sense;
            Psychic -= buff.Psychic;
            Treatment -= buff.Treatment;
            buffs.Remove(buff);

            OnChange?.Invoke();
        }
    }
}
