using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballArenaCrawler.Models
{
    public class PlayerInfo : PlayerIdentity, IEquatable<PlayerInfo>
    {
        public string Nationality { get; set; }
        public int Age { get; set; }
        public PlayerPosition Position { get; set; }
        public int Height { get; set; }
        public decimal Price { get; set; }
        public decimal Salary { get; set; }
        public DateTime SignedAt { get; set; }
        public int Potential { get; set; }

        public bool IsHome { get; set; }

        public override bool Equals(object obj) => Equals(obj as PlayerInfo);

        public bool Equals(PlayerInfo other)
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
                IsHome == other.IsHome;
        }

        public override int GetHashCode()
        {
            var hashCode = 1284525823;
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
            return hashCode;
        }
    }
}
