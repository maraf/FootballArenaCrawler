using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballArenaCrawler.Models
{
    public class PlayerStatsGroup : IEquatable<PlayerStatsGroup>
    {
        public int Appearances { get; set; }
        public int Goals { get; set; }

        public override bool Equals(object obj) => Equals(obj as PlayerStatsGroup);

        public bool Equals(PlayerStatsGroup other) => other != null &&
            Appearances == other.Appearances &&
            Goals == other.Goals;

        public override int GetHashCode()
        {
            var hashCode = -1937534557;
            hashCode = hashCode * -1521134295 + Appearances.GetHashCode();
            hashCode = hashCode * -1521134295 + Goals.GetHashCode();
            return hashCode;
        }

        public override string ToString() => $"{Appearances}/{Goals}";
    }
}
