using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballArenaCrawler.Models
{
    [DebuggerDisplay("PlayerDetail: {Name}")]
    public class PlayerDetail : PlayerIdentity, IEquatable<PlayerDetail>
    {
        public string Nationality { get; set; }
        public int Age { get; set; }
        public PlayerPosition Position { get; set; }
        public int Height { get; set; }
        public decimal Price { get; set; }
        public decimal Salary { get; set; }
        public DateTime SignedAt { get; set; }
        public int Potential { get; set; }

        // TODO: Přejmenovat
        public bool IsHome { get; set; }

        public PlayerQuality Quality { get; } = new PlayerQuality();

        public PlayerStats CurrentSeasonStats { get; } = new PlayerStats();
        public PlayerStats PreviousSeasonStats { get; } = new PlayerStats();
        public PlayerStats SumStats { get; } = new PlayerStats();

        public override bool Equals(object obj) => Equals(obj as PlayerDetail);

        public bool Equals(PlayerDetail other)
        {
            return other != null &&
                base.Equals(other) &&
                Nationality == other.Nationality &&
                Age == other.Age &&
                Position == other.Position &&
                Height == other.Height &&
                Price == other.Price &&
                Salary == other.Salary &&
                SignedAt == other.SignedAt &&
                Potential == other.Potential &&
                IsHome == other.IsHome &&
                EqualityComparer<PlayerQuality>.Default.Equals(Quality, other.Quality) &&
                EqualityComparer<PlayerStats>.Default.Equals(CurrentSeasonStats, other.CurrentSeasonStats) &&
                EqualityComparer<PlayerStats>.Default.Equals(PreviousSeasonStats, other.PreviousSeasonStats) &&
                EqualityComparer<PlayerStats>.Default.Equals(SumStats, other.SumStats);
        }

        public override int GetHashCode()
        {
            var hashCode = -1326469202;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Nationality);
            hashCode = hashCode * -1521134295 + Age.GetHashCode();
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + Price.GetHashCode();
            hashCode = hashCode * -1521134295 + Salary.GetHashCode();
            hashCode = hashCode * -1521134295 + SignedAt.GetHashCode();
            hashCode = hashCode * -1521134295 + Potential.GetHashCode();
            hashCode = hashCode * -1521134295 + IsHome.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerQuality>.Default.GetHashCode(Quality);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStats>.Default.GetHashCode(CurrentSeasonStats);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStats>.Default.GetHashCode(PreviousSeasonStats);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStats>.Default.GetHashCode(SumStats);
            return hashCode;
        }
    }
}
