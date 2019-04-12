using FootballArenaCrawler.Models;
using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FootballArenaCrawler
{
    public class ApiClient
    {
        private readonly HttpClient httpClient;
        private readonly HtmlParser htmlParser;

        public ApiClient(HttpClient httpClient, HtmlParser htmlParser)
        {
            Ensure.NotNull(httpClient, "httpClient");
            Ensure.NotNull(htmlParser, "htmlParser");
            this.httpClient = httpClient;
            this.htmlParser = htmlParser;
        }

        public async Task<Authentication> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            Dictionary<string, string> formParameters = new Dictionary<string, string>();
            formParameters["nick"] = username;
            formParameters["psw"] = password;
            formParameters["login"] = "Vstoupit";
            formParameters["auth"] = "on";
            HttpResponseMessage httpResponse = await httpClient.PostAsync("/login.php", new FormUrlEncodedContent(formParameters), cancellationToken);
            if (httpResponse.StatusCode == HttpStatusCode.Found)
            {
                if (httpResponse.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> cookies))
                {
                    string authentication = null;
                    string sessionId = null;

                    foreach (string cookie in cookies)
                    {
                        string[] parts = cookie.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] keyValue = parts[0].Split(new char[] { '=' });
                        if (keyValue.Length == 2)
                        {
                            if (keyValue[0] == "auth")
                                authentication = keyValue[1];
                            else if (keyValue[0] == "PHPSESSID")
                                sessionId = keyValue[1];
                        }
                    }

                    if (authentication != null && sessionId != null)
                    {
                        return new Authentication()
                        {
                            Token = authentication,
                            SessionId = sessionId
                        };
                    }
                }
            }

            throw Ensure.Exception.InvalidOperation("Login failed.");
        }

        public async Task<IReadOnlyCollection<PlayerIdentity>> GetPlayersAsync(int teamId, CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"?goto=team-player&idteam={teamId}&club_view=1", cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            string htmlBody = await httpResponse.Content.ReadAsStringAsync();
            return htmlParser.ParsePlayers(htmlBody);
        }

        public async Task<PlayerDetail> GetPlayerDetailAsync(int playerId, CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"?goto=team-player-detail&idplayer={playerId}", cancellationToken);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                string htmlBody = await httpResponse.Content.ReadAsStringAsync();
                return htmlParser.ParsePlayerDetail(htmlBody);
            }

            throw Ensure.Exception.InvalidOperation("Get player detail failed.");
        }

        public async Task<int> GetSeasonNumberAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"?goto=team-match-actual", cancellationToken);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                string htmlBody = await httpResponse.Content.ReadAsStringAsync();
                return htmlParser.ParseSeasonNumber(htmlBody);
            }

            throw Ensure.Exception.InvalidOperation("Get season number failed.");
        }
    }
}
