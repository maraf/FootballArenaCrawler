using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballArenaCrawler.Models
{
    [DebuggerDisplay("PlayerExport: {Name}")]
    public class PlayerExport : IEquatable<PlayerExport>
    {
        public PlayerInfo Info { get; } = new PlayerInfo();
        public PlayerQuality Quality { get; } = new PlayerQuality();

        public PlayerStats CurrentSeasonStats { get; } = new PlayerStats();
        public PlayerStats PreviousSeasonStats { get; } = new PlayerStats();
        public PlayerStats SumStats { get; } = new PlayerStats();

        public override bool Equals(object obj) => Equals(obj as PlayerExport);

        public bool Equals(PlayerExport other)
        {
            return other != null &&
                base.Equals(other) &&
                EqualityComparer<PlayerInfo>.Default.Equals(Info, other.Info) &&
                EqualityComparer<PlayerQuality>.Default.Equals(Quality, other.Quality) &&
                EqualityComparer<PlayerStats>.Default.Equals(CurrentSeasonStats, other.CurrentSeasonStats) &&
                EqualityComparer<PlayerStats>.Default.Equals(PreviousSeasonStats, other.PreviousSeasonStats) &&
                EqualityComparer<PlayerStats>.Default.Equals(SumStats, other.SumStats);
        }

        public override int GetHashCode()
        {
            var hashCode = -11966655;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerInfo>.Default.GetHashCode(Info);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerQuality>.Default.GetHashCode(Quality);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStats>.Default.GetHashCode(CurrentSeasonStats);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStats>.Default.GetHashCode(PreviousSeasonStats);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStats>.Default.GetHashCode(SumStats);
            return hashCode;
        }
    }
}
