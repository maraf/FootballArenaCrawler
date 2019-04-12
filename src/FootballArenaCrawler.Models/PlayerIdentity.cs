using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballArenaCrawler.Models
{
    [DebuggerDisplay("PlayerIdentity: {Name}")]
    public class PlayerIdentity : IEquatable<PlayerIdentity>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj) => Equals(obj as PlayerIdentity);

        public bool Equals(PlayerIdentity other) => 
            other != null &&
            Id == other.Id &&
            Name == other.Name;

        public override int GetHashCode()
        {
            var hashCode = -1919740922;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
