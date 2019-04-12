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
            var hashCode = -1413490195;
            hashCode = hashCode * -1521134295 + Attacker.GetHashCode();
            hashCode = hashCode * -1521134295 + Experiences.GetHashCode();
            hashCode = hashCode * -1521134295 + Stamina.GetHashCode();
            hashCode = hashCode * -1521134295 + Goalkeeper.GetHashCode();
            hashCode = hashCode * -1521134295 + Tackling.GetHashCode();
            hashCode = hashCode * -1521134295 + Header.GetHashCode();
            hashCode = hashCode * -1521134295 + Winger.GetHashCode();
            hashCode = hashCode * -1521134295 + Creativity.GetHashCode();
            hashCode = hashCode * -1521134295 + Passing.GetHashCode();
            hashCode = hashCode * -1521134295 + Attacking.GetHashCode();
            return hashCode;
        }
    }
}
