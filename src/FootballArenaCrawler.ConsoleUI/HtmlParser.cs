using FootballArenaCrawler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FootballArenaCrawler
{
    public class HtmlParser
    {
        private const RegexOptions regexOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled;
        private static readonly Regex playerLinkRegex = new Regex("<a.*?(?<attribute>href)=\"(?<url>.*?)\".*?>(?<name>.*?)</a>", regexOptions);

        public IReadOnlyCollection<PlayerIdentity> ParsePlayers(string htmlBody)
        {
            HashSet<PlayerIdentity> result = new HashSet<PlayerIdentity>();

            var matches = playerLinkRegex.Matches(htmlBody);
            foreach (Match match in matches)
            {
                string url = match.Groups["url"].Value;
                if (url.StartsWith("./?goto=team-player-detail"))
                {
                    string[] urlParts = url.Split("&amp;");
                    string[] id = urlParts[1].Split("=");
                    int playerId = Int32.Parse(id[1]);
                    string playerName = match.Groups["name"].Value;

                    result.Add(new PlayerIdentity()
                    {
                        Id = playerId,
                        Name = playerName
                    });
                }
            }

            return result;
        }
    }
}
