using API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;
using System.Text.Json;
using static API.Models.MatchData;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RiotController : Controller
    {
        public string api;
        public RiotController(IConfiguration config)
        {
            api = config["RiotAPI:Token"];
        }

        [HttpGet("GetAccount")]
        public async Task<ActionResult<RiotAccountDetails>> NewGetAccount(string gameName, string tagLine, string region)
        {
            string mmRegion = region.ToLower() switch
            {
                "br1" => "americas",
                "eun1" => "europe",
                "euw1" => "europe",
                "jp1" => "asia",
                "kr" => "asia",
                "la1" => "americas",
                "la2" => "americas",
                "na1" => "americas",
                "oc1" => "sea",
                "ph2" => "sea",
                "sg2" => "sea",
                "th2" => "sea",
                "tr1" => "europe",
                "tw2" => "sea",
                "vn2" => "sea",
                "ru" => "europe"
            };

            var url = new RestClient($"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("X-Riot-Token", api);

            var restResponse = await url.ExecuteAsync(request);

            var response = JsonConvert.DeserializeObject<RiotAccount>(restResponse.Content);

            var newUrl = new RestClient($"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var newRequest = new RestRequest("", Method.Get);
            newRequest.AddHeader("X-Riot-Token", api);

            var newRestResponse = await url.ExecuteAsync(newRequest);

            var newResponse = JsonConvert.DeserializeObject<NewRiotAccount>(newRestResponse.Content);

            var accountUrl = new RestClient($"https://{region}.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{response.puuid}");
            var accountRequest = new RestRequest("", Method.Get);
            accountRequest.AddHeader("X-Riot-Token", api);

            var accountResponse = await accountUrl.ExecuteAsync(accountRequest);

            var accountResponse2 = JsonConvert.DeserializeObject<RiotAccount>(accountResponse.Content);

            var rankedUrl = new RestClient($"https://{region}.api.riotgames.com/lol/league/v4/entries/by-puuid/{response.puuid}");
            var rankedRequest = new RestRequest("", Method.Get);
            rankedRequest.AddHeader("X-Riot-Token", api); 
            var rankedResponse = await rankedUrl.ExecuteAsync(rankedRequest);

            var rankedResponse2 = JsonConvert.DeserializeObject<List<RiotRanked>>(rankedResponse.Content);

            var allPlayerDetails = new List<PlayerMatchHistory>();
            string didWin = "";
            string gameMode = "";

            var matchUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/by-puuid/{response.puuid}/ids?start=0&count=10");
            var matchRequest = new RestRequest("", Method.Get);
            matchRequest.AddHeader("X-Riot-Token", api);
            var matchResponse = await matchUrl.ExecuteAsync(matchRequest);
            var matchResponse2 = JsonConvert.DeserializeObject<dynamic>(matchResponse.Content);

            foreach (var item in matchResponse2)
            { 
                var playersInMatch = new List<PlayerMatchDetails>();
                var matchDataUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/{item}");
                var matchDataRequest = new RestRequest("", Method.Get);
                matchDataRequest.AddHeader("X-Riot-Token", api);
                var matchDataRestResponse = await matchDataUrl.ExecuteAsync(matchDataRequest);
                var matchDataResponse = JsonConvert.DeserializeObject<MatchData>(matchDataRestResponse.Content);
                var participants = matchDataResponse.info.participants.Where(i => string.Equals(i.riotIdGameName, gameName, StringComparison.OrdinalIgnoreCase));
                var player = participants.FirstOrDefault();

                int totalFarm = (int)(player.totalMinionsKilled + player.neutralMinionsKilled);

                string gameWinner = "";
                foreach (var team in matchDataResponse.info.teams)
                {
                    if (team.win)
                    {
                        if (team.teamId == player.teamId)
                        {
                            gameWinner = "Victory";
                        }
                        else
                        {
                            gameWinner = "Defeat";
                        }
                        break;
                    }
                }

                string champName;

                if (player.championName == "Wukong") 
                { 
                    champName = "Monkey King";
                }
                else if(player.championName == "FiddleSticks")
                {
                    champName = "Fiddlesticks";
                }
                else
                {
                    champName = player.championName;
                }

                var uniqueDetails = new PlayerMatchHistory
                {
                    MatchID = matchDataResponse.metadata.matchId,
                    GameWinner = gameWinner,
                    GameMode = matchDataResponse.info.gameMode,
                    ChampionName = champName ?? "Unknown",
                    Lane = player.individualPosition ?? "Unknown",
                    KDA = $"{player.kills}/{player.deaths}/{player.assists}",
                    Farm = totalFarm
                };

                allPlayerDetails.Add(uniqueDetails);
            }

            var playerDetails = new RiotAccountDetails
            {
                gameName = $"{newResponse.gameName}#{newResponse.tagLine}",
                summonerLevel = accountResponse2.summonerLevel,
                profileIconId = accountResponse2.profileIconId,
                SoloRank = rankedResponse2.FirstOrDefault(r => r?.queueType == "RANKED_SOLO_5x5") is var solo && solo != null
                                                        ? $"{solo.tier} {solo.rank} ({solo.leaguePoints} LP)"
                                                        : "Unranked",
                FlexRank = rankedResponse2.FirstOrDefault(r => r?.queueType == "RANKED_FLEX_SR") is var flex && flex != null
                                                        ? $"{flex.tier} {flex.rank} ({flex.leaguePoints} LP)"
                                                        : "Unranked",
                BasicMatchDetails  = allPlayerDetails
            };

            return playerDetails;

        }

        [HttpGet("GetChallenges")]
        public async Task<ActionResult<RiotChallenges>> GetChallenges(string gameName, string tagLine, string region)
        {
            var url = new RestClient($"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("X-Riot-Token", api);

            var restResponse = await url.ExecuteAsync(request);

            var response = JsonConvert.DeserializeObject<RiotAccount>(restResponse.Content);

            var challengesURL = new RestClient($"https://{region}.api.riotgames.com/lol/challenges/v1/player-data/{response.puuid}");
            var challengesRequest = new RestRequest("", Method.Get);
            challengesRequest.AddHeader("X-Riot-Token", api);

            var challengesResponse = await challengesURL.ExecuteAsync(challengesRequest);

            var challengesResponse2 = JsonConvert.DeserializeObject<RiotChallenges>(challengesResponse.Content);

            return challengesResponse2;
        }
        
        [HttpGet("GetHistory")]
        public async Task<List<MatchStatsNew>> GetMatchHistory(string gameName, string tagLine, string region)
        {
            string mmRegion = region.ToLower() switch
            {
                "br1" => "americas",
                "eun1" => "europe",
                "euw1" => "europe",
                "jp1" => "asia",
                "kr" => "asia",
                "la1" => "americas",
                "la2" => "americas",
                "na1" => "americas",
                "oc1" => "sea",
                "ph2" => "sea",
                "sg2" => "sea",
                "th2" => "sea",
                "tr1" => "europe",
                "tw2" => "sea",
                "vn2" => "sea",
                "ru" => "europe"
            };

            var allPlayerDetails = new List<MatchStats>();
            var newMatchData = new List<MatchStatsNew>();
            string didWin = "";
            string gameMode = "";

            var url = new RestClient($"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("X-Riot-Token", api);
            var restResponse = await url.ExecuteAsync(request);
            var response = JsonConvert.DeserializeObject<RiotAccount>(restResponse.Content);

            var matchUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/by-puuid/{response.puuid}/ids?start=0&count=10");
            var matchRequest = new RestRequest("", Method.Get);
            matchRequest.AddHeader("X-Riot-Token", api);
            var matchResponse = await matchUrl.ExecuteAsync(matchRequest);
            var matchResponse2 = JsonConvert.DeserializeObject<dynamic>(matchResponse.Content);

            foreach (var item in matchResponse2)
            {
                var playersInMatch = new List<PlayerMatchDetails>();

                var matchDataUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/{item}");
                var matchDataRequest = new RestRequest("", Method.Get);
                matchDataRequest.AddHeader("X-Riot-Token", api);
                var matchDataRestResponse = await matchDataUrl.ExecuteAsync(matchDataRequest);
                var matchDataResponse = JsonConvert.DeserializeObject<MatchData>(matchDataRestResponse.Content);

                foreach (var winnerItem in matchDataResponse.info.teams)
                {
                    if (winnerItem.win)
                    {
                        didWin = winnerItem.teamId == 100 ? "Blue Team" : "Red Team";
                        break;
                    }
                }

                foreach (var subItem in matchDataResponse.info.participants)
                {
                    int totalFarm = subItem.totalMinionsKilled + subItem.neutralMinionsKilled;

                    string KDA = $"{subItem.kills}/{subItem.deaths}/{subItem.assists}";

                    string teamName;

                    if (subItem.teamId == 100) 
                    {
                        teamName = "Blue Team";
                    }
                    else 
                    {
                        teamName = "Red Team";
                    }

                        var playerDetails = new PlayerMatchDetails
                        {
                            PlayerName = subItem.riotIdGameName,
                            TeamID = teamName,
                            ChampionName = subItem.championName,
                            LaneName = subItem.lane,
                            KDA = KDA,
                            Damage = subItem.totalDamageDealtToChampions,
                            DamageTaken = subItem.totalDamageTaken,
                            TowerDamage = subItem.damageDealtToTurrets,
                            ObjDamage = subItem.damageDealtToObjectives,
                            SkillshotsHit = subItem.challenges.skillshotsHit,
                            SkillshotsMissed = subItem.challenges.skillshotsDodged,
                            Farm = totalFarm,
                            HealShield = subItem.challenges.effectiveHealAndShielding
                        };
                    playersInMatch.Add(playerDetails);
                }

                var matchStats = new MatchStatsNew
                {
                    GameID = matchDataResponse.metadata.matchId,
                    GameWinner = didWin,
                    GameMode = matchDataResponse.info.gameMode,
                    Players = playersInMatch
                };
                newMatchData.Add(matchStats);
            }

            return newMatchData;
        }

        [HttpGet("GetTopPlayed")]
        public async Task<List<string>> GetTopPlayed(string gameName, string tagLine, string region)
        {
            // This is for DD to get the latest patch version and all characters to that patch
            var patchUrl = new RestClient("https://ddragon.leagueoflegends.com/api/versions.json");
            var patchRequest = new RestRequest("", Method.Get);
            patchRequest.AddHeader("X-Riot-Token", api);
            var patchRestResponse = await patchUrl.ExecuteAsync(patchRequest);
            var patchResponse = JsonConvert.DeserializeObject<List<string>>(patchRestResponse.Content).FirstOrDefault();

            var championUrl = new RestClient($"https://ddragon.leagueoflegends.com/cdn/{patchResponse}/data/en_US/champion.json");
            var championRequest = new RestRequest("", Method.Get);
            var championRestResponse = await championUrl.ExecuteAsync(championRequest);
            var championResponse = JsonConvert.DeserializeObject<Champions>(championRestResponse.Content);

            // Below is to get the stats for the player
            var topPlayedList = new List<string>();

            var url = new RestClient($"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("X-Riot-Token", api);
            var restResponse = await url.ExecuteAsync(request);
            var response = JsonConvert.DeserializeObject<RiotAccount>(restResponse.Content);

            var topPlayedUrl = new RestClient($"https://{region}.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-puuid/{response.puuid}");
            var topPlayedRequest = new RestRequest("", Method.Get);
            topPlayedRequest.AddHeader("X-Riot-Token", api);
            var topPlayedRestResponse = await topPlayedUrl.ExecuteAsync(topPlayedRequest);
            var topPlayedResponse = JsonConvert.DeserializeObject<List<TopPlayed>>(topPlayedRestResponse.Content).Take(5);

            foreach (var item in topPlayedResponse)
            {
                var champ = championResponse?.data.Values.FirstOrDefault(i => i.key == item.championId.ToString());

                topPlayedList.Add($"{champ.name} ({item.championPoints})");
            }
            
            return topPlayedList;
        }

        [HttpGet("GetFreeCharacters")]
        public async Task<List<string>> GetFreeCharacters(string region)
        {
            // This is for DD to get the latest patch version and all characters to that patch
            var patchUrl = new RestClient("https://ddragon.leagueoflegends.com/api/versions.json");
            var patchRequest = new RestRequest("", Method.Get);
            patchRequest.AddHeader("X-Riot-Token", api);
            var patchRestResponse = await patchUrl.ExecuteAsync(patchRequest);
            var patchResponse = JsonConvert.DeserializeObject<List<string>>(patchRestResponse.Content).FirstOrDefault();

            var championUrl = new RestClient($"https://ddragon.leagueoflegends.com/cdn/{patchResponse}/data/en_US/champion.json");
            var championRequest = new RestRequest("", Method.Get);
            var championRestResponse = await championUrl.ExecuteAsync(championRequest);
            var championResponse = JsonConvert.DeserializeObject<Champions>(championRestResponse.Content);

            var freeToPlayName = new List<string>();

            var freeToPlayUrl = new RestClient($"https://{region}.api.riotgames.com/lol/platform/v3/champion-rotations");
            var freeToPlayRequest = new RestRequest("", Method.Get);
            freeToPlayRequest.AddHeader("X-Riot-Token", api);
            var freeToPlayRestResponse = await freeToPlayUrl.ExecuteAsync(freeToPlayRequest);
            var freeToPlayResponse = JsonConvert.DeserializeObject<FreeCharacters>(freeToPlayRestResponse.Content);

            var idToNameMap = championResponse.data.Values.ToDictionary(c => int.Parse(c.key), c => c.name);

            var freeChampNames = freeToPlayResponse.freeChampionIds
                                    .Select(i => idToNameMap.ContainsKey(i) ? idToNameMap[i] : $"Unknown({i})")
                                    .ToList();

            return freeChampNames;

        }

        [HttpGet("PoroPop")]
        public async Task<int> PoroPopped(string gameName, string tagLine, string region)
        {
            int totalPoroPopped = 0;

            var url = new RestClient($"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("X-Riot-Token", api);
            var restResponse = await url.ExecuteAsync(request);
            var response = JsonConvert.DeserializeObject<RiotAccount>(restResponse.Content);

            string mmRegion = region.ToLower() switch
            {
                "br1" => "americas",
                "eun1" => "europe",
                "euw1" => "europe",
                "jp1" => "asia",
                "kr" => "asia",
                "la1" => "americas",
                "la2" => "americas",
                "na1" => "americas",
                "oc1" => "sea",
                "ph2" => "sea",
                "sg2" => "sea",
                "th2" => "sea",
                "tr1" => "europe",
                "tw2" => "sea",
                "vn2" => "sea",
                "ru" => "europe"
            };

            var matchUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/by-puuid/{response.puuid}/ids?queue=450&start=0&count=50");
            var matchRequest = new RestRequest("", Method.Get);
            matchRequest.AddHeader("X-Riot-Token", api);
            var matchResponse = await matchUrl.ExecuteAsync(matchRequest);
            var matchResponse2 = JsonConvert.DeserializeObject<dynamic>(matchResponse.Content);

            foreach (var item in matchResponse2)
            {
                var playersInMatch = new List<PlayerMatchDetails>();

                var matchDataUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/{item}");
                var matchDataRequest = new RestRequest("", Method.Get);
                matchDataRequest.AddHeader("X-Riot-Token", api);
                var matchDataRestResponse = await matchDataUrl.ExecuteAsync(matchDataRequest);
                var matchDataResponse = JsonConvert.DeserializeObject<MatchData>(matchDataRestResponse.Content);

                var data = matchDataResponse.info.participants.FirstOrDefault(i => string.Equals(i.riotIdGameName, gameName, StringComparison.OrdinalIgnoreCase));
                //var totalPopped = item.challenges.poroExplosions;

                if(matchDataResponse.info.endOfGameResult == "GameComplete") 
                {
                    totalPoroPopped += data.challenges.poroExplosions;
                }

            }

            return totalPoroPopped;
        }

        [HttpGet("GetSingleMatchDetails")]
        public async Task<List<MatchStatsNew>> GetMatchDetails(string region, string matchID) 
        {
            string mmRegion = region.ToLower() switch
            {
                "br1" => "americas",
                "eun1" => "europe",
                "euw1" => "europe",
                "jp1" => "asia",
                "kr" => "asia",
                "la1" => "americas",
                "la2" => "americas",
                "na1" => "americas",
                "oc1" => "sea",
                "ph2" => "sea",
                "sg2" => "sea",
                "th2" => "sea",
                "tr1" => "europe",
                "tw2" => "sea",
                "vn2" => "sea",
                "ru" => "europe"
            };

            var newMatchData = new List<MatchStatsNew>();
            string didWin = "";
            string gameMode = "";
                
            var playersInMatch = new List<PlayerMatchDetails>();

            var matchDataUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/{matchID}");
            var matchDataRequest = new RestRequest("", Method.Get);
            matchDataRequest.AddHeader("X-Riot-Token", api);
            var matchDataRestResponse = await matchDataUrl.ExecuteAsync(matchDataRequest);
            var matchDataResponse = JsonConvert.DeserializeObject<MatchData>(matchDataRestResponse.Content);

            foreach (var winnerItem in matchDataResponse.info.teams)
            {
                if (winnerItem.win)
                {
                    didWin = winnerItem.teamId == 100 ? "Blue Team" : "Red Team";
                    break;
                }
            }

            foreach (var subItem in matchDataResponse.info.participants)
            {
                int totalFarm = subItem.totalAllyJungleMinionsKilled + subItem.totalEnemyJungleMinionsKilled + subItem.totalMinionsKilled + subItem.neutralMinionsKilled;

                string KDA = $"{subItem.kills}/{subItem.deaths}/{subItem.assists}";

                string teamName;

                if (subItem.teamId == 100)
                {
                    teamName = "Blue Team";
                } 
                else
                {
                    teamName = "Red Team";
                }

                List<Selection> mainRunePath = subItem.perks.styles[0].selections;
                List<Selection> secondRunePath = subItem.perks.styles[1].selections;

                Runes playerRunes = new Runes
                {
                    KeyStone = mainRunePath[0].perk,
                    PrimaryRune1 = mainRunePath[1].perk,
                    PrimaryRune2 = mainRunePath[2].perk,
                    PrimaryRune3 = mainRunePath[3].perk,

                    SecondaryRune1 = secondRunePath[0].perk,
                    SecondaryRune2 = secondRunePath[1].perk,

                    RuneShard1 = subItem.perks.statPerks.offense,
                    RuneShard2 = subItem.perks.statPerks.flex,
                    RuneShard3 = subItem.perks.statPerks.defense

                };

                // https://ddragon.leagueoflegends.com/cdn/15.14.1/img/item/1001.png Item URL for frontend

                Items playerItems = new Items
                {
                    Item1 = subItem.item0,
                    Item2 = subItem.item1,
                    Item3 = subItem.item2,
                    Item4 = subItem.item3,
                    Item5 = subItem.item4,
                    Item6 = subItem.item5,
                    Ward = subItem.item6
                };

                var playerDetails = new PlayerMatchDetails
                {
                    PlayerName = subItem.riotIdGameName,
                    TeamID = teamName,
                    ChampionName = subItem.championName,
                    LaneName = subItem.individualPosition,
                    KDA = KDA,
                    Damage = subItem.totalDamageDealtToChampions,
                    DamageTaken = subItem.totalDamageTaken,
                    TowerDamage = subItem.damageDealtToTurrets,
                    ObjDamage = subItem.damageDealtToObjectives,
                    SkillshotsHit = subItem.challenges.skillshotsHit,
                    SkillshotsMissed = subItem.challenges.skillshotsDodged,
                    Farm = totalFarm,
                    HealShield = Math.Round(subItem.challenges.effectiveHealAndShielding),
                    PlayerItems = playerItems
                };
                playersInMatch.Add(playerDetails);
            }

            var matchStats = new MatchStatsNew
            {
                GameID = matchDataResponse.metadata.matchId,
                GameWinner = didWin,
                GameMode = matchDataResponse.info.gameMode,
                Players = playersInMatch
            };
            newMatchData.Add(matchStats);
        
            return newMatchData;

        }

    }
}