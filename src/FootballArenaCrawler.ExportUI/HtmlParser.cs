using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FootballArenaCrawler.Models;
using Neptuo;
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

        public PlayerDetail ParsePlayerDetail(string htmlBody)
        {
            var parser = new AngleSharp.Html.Parser.HtmlParser();
            IHtmlDocument document = parser.ParseDocument(htmlBody);

            PlayerDetail player = new PlayerDetail();

            IElement mainContent = document.QuerySelector(".main_left");
            for (int i = 0; i < mainContent.Children.Length; i++)
            {
                IElement childElement = mainContent.Children[i];
                i = ParseDetail(player, mainContent, i, childElement);
                i = ParseQuality(player.Quality, mainContent, i, childElement);
                ParseStats(player, childElement);
            }

            return player;
        }

        private int ParseDetail(PlayerDetail player, IElement mainContent, int i, IElement childElement)
        {
            if (childElement.ClassList.Contains("se1"))
            {
                string title = childElement.InnerHtml;
                IElement valueElement = mainContent.Children[++i];
                if (valueElement.ClassList.Contains("se2"))
                {
                    string value = valueElement.Text();

                    TrySetField<int>(title, value, "ID hráče", val => player.Id = val);
                    TrySetField<string>(title, value, "Jméno", val => player.Name = val);
                    TrySetField<string>(title, value, "Národnost", val => player.Nationality = val);
                    TrySetField<int>(title, value, "Věk", val => player.Age = val);
                    TrySetField<string>(title, value, "Pozice", val =>
                    {
                        if (val.Equals("Brankář", StringComparison.InvariantCultureIgnoreCase))
                            player.Position = PlayerPosition.Goalkeeper;
                        else if (val.Equals("Obránce", StringComparison.InvariantCultureIgnoreCase))
                            player.Position = PlayerPosition.Defender;
                        else if (val.Equals("Křídlo", StringComparison.InvariantCultureIgnoreCase))
                            player.Position = PlayerPosition.Winger;
                        else if (val.Equals("Střední záložník", StringComparison.InvariantCultureIgnoreCase))
                            player.Position = PlayerPosition.Midfielder;
                        else if (val.Equals("Útočník", StringComparison.InvariantCultureIgnoreCase))
                            player.Position = PlayerPosition.Forwarder;
                    });
                    TrySetField<int>(title, value, "Výška", val => player.Height = val);
                    TrySetField<decimal>(title, value, "Cena", val => player.Price = val);
                    TrySetField<decimal>(title, value, "Plat", val => player.Salary = val);
                    TrySetField<int>(title, value, "Potenciál", val => player.Potential = val);
                    TrySetField<string>(title, value, "V klubu od", val =>
                    {
                        string[] parts = val.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        player.SignedAt = DateTime.Parse(parts[0]);
                        player.IsHome = parts.Length == 2;
                    });
                }

            }

            return i;
        }

        private int ParseQuality(PlayerQuality quality, IElement mainContent, int i, IElement childElement)
        {
            if (childElement.ClassList.Contains("sk1"))
            {
                string title = childElement.Text();
                IElement valueElement = mainContent.Children[++i];
                if (valueElement.ClassList.Contains("sk2") || valueElement.ClassList.Contains("sk3"))
                {
                    string value = valueElement.Text();

                    TrySetField<double>(title, value, "zkušenosti", val => quality.Experiences = val);
                    TrySetField<int>(title, value, "celkem", val => quality.Attacker = val);
                    TrySetField<int>(title, value, "výdrž", val => quality.Stamina = val);
                    TrySetField<int>(title, value, "brankář", val => quality.Goalkeeper = val);
                    TrySetField<int>(title, value, "odebírání míče", val => quality.Tackling = val);
                    TrySetField<int>(title, value, "hlavičky", val => quality.Header = val);
                    TrySetField<int>(title, value, "křídlo", val => quality.Winger = val);
                    TrySetField<int>(title, value, "tvořivost", val => quality.Creativity = val);
                    TrySetField<int>(title, value, "přihrávky", val => quality.Passing = val);
                    TrySetField<int>(title, value, "útok", val => quality.Attacker = val);
                }
            }

            return i;
        }

        private bool TrySetField<T>(string title, string value, string targetTitle, Action<T> setter)
        {
            if (targetTitle.Equals(title, StringComparison.InvariantCultureIgnoreCase))
            {
                value = value.Trim();

                void StripSuffix(ref string val, string suffix)
                {
                    if (val.EndsWith(suffix))
                        val = val.Substring(0, value.Length - suffix.Length);
                }

                StripSuffix(ref value, " cm");
                StripSuffix(ref value, " Kč");
                StripSuffix(ref value, "%");

                if (Converts.Try(value, out T targetValue))
                {
                    setter(targetValue);
                    return true;
                }
            }

            return false;
        }

        private void ParseStats(PlayerDetail player, IElement childElement)
        {
            if (childElement.TagName.Equals("table", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (IElement rowElement in childElement.QuerySelectorAll("tr"))
                {
                    string title = rowElement.Children[0].Text();
                    if (title == "Liga")
                    {
                        SetStatsGroup(player.CurrentSeasonStats.League, rowElement.Children[1].Text());
                        SetStatsGroup(player.PreviousSeasonStats.League, rowElement.Children[2].Text());
                        SetStatsGroup(player.SumStats.League, rowElement.Children[3].Text());
                    }
                    else if (title == "Pohár")
                    {
                        SetStatsGroup(player.CurrentSeasonStats.Cup, rowElement.Children[1].Text());
                        SetStatsGroup(player.PreviousSeasonStats.Cup, rowElement.Children[2].Text());
                        SetStatsGroup(player.SumStats.Cup, rowElement.Children[3].Text());
                    }
                    else if (title == "Přátelské zápasy")
                    {
                        SetStatsGroup(player.CurrentSeasonStats.Friendly, rowElement.Children[1].Text());
                        SetStatsGroup(player.PreviousSeasonStats.Friendly, rowElement.Children[2].Text());
                        SetStatsGroup(player.SumStats.Friendly, rowElement.Children[3].Text());
                    }
                    else if (title == "Mezinárodní poháry")
                    {
                        SetStatsGroup(player.CurrentSeasonStats.InternationalCups, rowElement.Children[1].Text());
                        SetStatsGroup(player.PreviousSeasonStats.InternationalCups, rowElement.Children[2].Text());
                        SetStatsGroup(player.SumStats.InternationalCups, rowElement.Children[3].Text());
                    }
                    else if (title == "Reprezentace")
                    {
                        SetStatsGroup(player.CurrentSeasonStats.NationalTeam, rowElement.Children[1].Text());
                        SetStatsGroup(player.PreviousSeasonStats.NationalTeam, rowElement.Children[2].Text());
                        SetStatsGroup(player.SumStats.NationalTeam, rowElement.Children[3].Text());
                    }
                }
            }
        }

        private static void SetStatsGroup(PlayerStatsGroup group, string current)
        {
            string[] parts = current.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            group.Appearances = Int32.Parse(parts[0]);
            group.Goals = Int32.Parse(parts[1]);
        }

        public int ParseSeasonNumber(string htmlBody)
        {
            var parser = new AngleSharp.Html.Parser.HtmlParser();
            IHtmlDocument document = parser.ParseDocument(htmlBody);

            IElement optionElement = document.QuerySelector("select[name=season] option:first-child");
            string value = optionElement.GetAttribute("value");
            return Int32.Parse(value);
        }
    }
}
