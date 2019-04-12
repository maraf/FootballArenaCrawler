using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballArenaCrawler.Models
{
    public class Export
    {
        public DateTime DateTime { get; } = DateTime.Now;
        public int SeasonNumber { get; set; }
        public List<PlayerExport> Players { get; } = new List<PlayerExport>();
    }
}
