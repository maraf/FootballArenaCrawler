using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballArenaCrawler.Models
{
    [DebuggerDisplay("League: {League}, Cup: {Cup}, Friendly: {Friendly}, InternationalCups: {InternationalCups}, NationalTeam: {NationalTeam}")]
    public class PlayerStats : IEquatable<PlayerStats>
    {
        public PlayerStatsGroup League { get; } = new PlayerStatsGroup();
        public PlayerStatsGroup Cup { get; } = new PlayerStatsGroup();
        public PlayerStatsGroup Friendly { get; } = new PlayerStatsGroup();
        public PlayerStatsGroup InternationalCups { get; } = new PlayerStatsGroup();
        public PlayerStatsGroup NationalTeam { get; } = new PlayerStatsGroup();

        public override bool Equals(object obj) => Equals(obj as PlayerStats);

        public bool Equals(PlayerStats other)
        {
            return other != null &&
                EqualityComparer<PlayerStatsGroup>.Default.Equals(League, other.League) &&
                EqualityComparer<PlayerStatsGroup>.Default.Equals(Cup, other.Cup) &&
                EqualityComparer<PlayerStatsGroup>.Default.Equals(Friendly, other.Friendly) &&
                EqualityComparer<PlayerStatsGroup>.Default.Equals(InternationalCups, other.InternationalCups) &&
                EqualityComparer<PlayerStatsGroup>.Default.Equals(NationalTeam, other.NationalTeam);
        }

        public override int GetHashCode()
        {
            var hashCode = -1903038347;
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStatsGroup>.Default.GetHashCode(League);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStatsGroup>.Default.GetHashCode(Cup);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStatsGroup>.Default.GetHashCode(Friendly);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStatsGroup>.Default.GetHashCode(InternationalCups);
            hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStatsGroup>.Default.GetHashCode(NationalTeam);
            return hashCode;
        }
    }
}
