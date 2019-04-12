using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballArenaCrawler.Models
{
    public class PlayerQuality : IEquatable<PlayerQuality>
    {
        /// <summary>
        /// Celkem.
        /// </summary>
        public int Attacker { get; set; }

        /// <summary>
        /// Zkušenosti.
        /// </summary>
        public double Experiences { get; set; }

        /// <summary>
        /// Výdrž.
        /// </summary>
        public int Stamina { get; set; }

        /// <summary>
        /// Brankář.
        /// </summary>
        public int Goalkeeper { get; set; }

        /// <summary>
        /// Odebírání míče.
        /// </summary>
        public int Tackling { get; set; }

        /// <summary>
        /// Hlavičky.
        /// </summary>
        public int Header { get; set; }

        /// <summary>
        /// Křídlo.
        /// </summary>
        public int Winger { get; set; }

        /// <summary>
        /// Tvořivost.
        /// </summary>
        public int Creativity { get; set; }

        /// <summary>
        /// Přihrávky.
        /// </summary>
        public int Passing { get; set; }

        /// <summary>
        /// Útok.
        /// </summary>
        public int Attacking { get; set; }

        public override bool Equals(object obj) => Equals(obj as PlayerQuality);

        public bool Equals(PlayerQuality other)
        {
            return other != null &&
                Attacker == other.Attacker &&
                Experiences == other.Experiences &&
                Stamina == other.Stamina &&
                Goalkeeper == other.Goalkeeper &&
                Tackling == other.Tackling &&
                Header == other.Header &&
                Winger == other.Winger &&
                Creativity == other.Creativity &&
                Passing == other.Passing &&
                Attacking == other.Attacking;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Attacker);
            hash.Add(Experiences);
            hash.Add(Stamina);
            hash.Add(Goalkeeper);
            hash.Add(Tackling);
            hash.Add(Header);
            hash.Add(Winger);
            hash.Add(Creativity);
            hash.Add(Passing);
            hash.Add(Attacking);
            return hash.ToHashCode();
        }
    }
}
