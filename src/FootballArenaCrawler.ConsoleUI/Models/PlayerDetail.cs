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
                Quality.Equals(other.Quality);
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Nationality);
            hash.Add(Age);
            hash.Add(Position);
            hash.Add(Height);
            hash.Add(Price);
            hash.Add(Salary);
            hash.Add(SignedAt);
            hash.Add(Potential);
            hash.Add(IsHome);
            hash.Add(Quality.GetHashCode());
            return hash.ToHashCode();
        }
    }
}
