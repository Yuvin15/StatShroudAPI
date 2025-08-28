using API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.RegularExpressions;
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

        // AI made my life easy here
        public Dictionary<int, string> RuneImagePaths = new()
        {

            { 8000, "perk-images/Styles/7201_Precision.png" },
            { 8005, "perk-images/Styles/Precision/PressTheAttack/PressTheAttack.png" },
            { 8008, "perk-images/Styles/Precision/LethalTempo/LethalTempoTemp.png" },
            { 8021, "perk-images/Styles/Precision/FleetFootwork/FleetFootwork.png" },
            { 8010, "perk-images/Styles/Precision/Conqueror/Conqueror.png" },
            { 9101, "perk-images/Styles/Precision/AbsorbLife/AbsorbLife.png" },
            { 9111, "perk-images/Styles/Precision/Triumph.png" },
            { 8009, "perk-images/Styles/Precision/PresenceOfMind/PresenceOfMind.png" },
            { 9104, "perk-images/Styles/Precision/LegendAlacrity/LegendAlacrity.png" },
            { 9105, "perk-images/Styles/Precision/LegendHaste/LegendHaste.png" },
            { 9103, "perk-images/Styles/Precision/LegendBloodline/LegendBloodline.png" },
            { 8014, "perk-images/Styles/Precision/CoupDeGrace/CoupDeGrace.png" },
            { 8017, "perk-images/Styles/Precision/CutDown/CutDown.png" },
            { 8299, "perk-images/Styles/Sorcery/LastStand/LastStand.png" },

            { 8100, "perk-images/Styles/7200_Domination.png" },
            { 8112, "perk-images/Styles/Domination/Electrocute/Electrocute.png" },
            { 8128, "perk-images/Styles/Domination/DarkHarvest/DarkHarvest.png" },
            { 9923, "perk-images/Styles/Domination/HailOfBlades/HailOfBlades.png" },
            { 8126, "perk-images/Styles/Domination/CheapShot/CheapShot.png" },
            { 8139, "perk-images/Styles/Domination/TasteOfBlood/GreenTerror_TasteOfBlood.png" },
            { 8143, "perk-images/Styles/Domination/SuddenImpact/SuddenImpact.png" },
            { 8137, "perk-images/Styles/Domination/SixthSense/SixthSense.png" },
            { 8140, "perk-images/Styles/Domination/GrislyMementos/GrislyMementos.png" },
            { 8141, "perk-images/Styles/Domination/DeepWard/DeepWard.png" },
            { 8135, "perk-images/Styles/Domination/TreasureHunter/TreasureHunter.png" },
            { 8105, "perk-images/Styles/Domination/RelentlessHunter/RelentlessHunter.png" },
            { 8106, "perk-images/Styles/Domination/UltimateHunter/UltimateHunter.png" },

            { 8200, "perk-images/Styles/7202_Sorcery.png" },
            { 8214, "perk-images/Styles/Sorcery/SummonAery/SummonAery.png" },
            { 8229, "perk-images/Styles/Sorcery/ArcaneComet/ArcaneComet.png" },
            { 8230, "perk-images/Styles/Sorcery/PhaseRush/PhaseRush.png" },
            { 8224, "perk-images/Styles/Sorcery/NullifyingOrb/Pokeshield.png" },
            { 8226, "perk-images/Styles/Sorcery/ManaflowBand/ManaflowBand.png" },
            { 8275, "perk-images/Styles/Sorcery/NimbusCloak/6361.png" },
            { 8210, "perk-images/Styles/Sorcery/Transcendence/Transcendence.png" },
            { 8234, "perk-images/Styles/Sorcery/Celerity/CelerityTemp.png" },
            { 8233, "perk-images/Styles/Sorcery/AbsoluteFocus/AbsoluteFocus.png" },
            { 8237, "perk-images/Styles/Sorcery/Scorch/Scorch.png" },
            { 8232, "perk-images/Styles/Sorcery/Waterwalking/Waterwalking.png" },
            { 8236, "perk-images/Styles/Sorcery/GatheringStorm/GatheringStorm.png" },

            { 8400, "perk-images/Styles/7204_Resolve.png" },
            { 8437, "perk-images/Styles/Resolve/GraspOfTheUndying/GraspOfTheUndying.png" },
            { 8439, "perk-images/Styles/Resolve/VeteranAftershock/VeteranAftershock.png" },
            { 8465, "perk-images/Styles/Resolve/Guardian/Guardian.png" },
            { 8446, "perk-images/Styles/Resolve/Demolish/Demolish.png" },
            { 8463, "perk-images/Styles/Resolve/FontOfLife/FontOfLife.png" },
            { 8401, "perk-images/Styles/Resolve/MirrorShell/MirrorShell.png" },
            { 8429, "perk-images/Styles/Resolve/Conditioning/Conditioning.png" },
            { 8444, "perk-images/Styles/Resolve/SecondWind/SecondWind.png" },
            { 8473, "perk-images/Styles/Resolve/BonePlating/BonePlating.png" },
            { 8451, "perk-images/Styles/Resolve/Overgrowth/Overgrowth.png" },
            { 8453, "perk-images/Styles/Resolve/Revitalize/Revitalize.png" },
            { 8242, "perk-images/Styles/Sorcery/Unflinching/Unflinching.png" },

            { 8300, "perk-images/Styles/7203_Whimsy.png" },
            { 8351, "perk-images/Styles/Inspiration/GlacialAugment/GlacialAugment.png" },
            { 8360, "perk-images/Styles/Inspiration/UnsealedSpellbook/UnsealedSpellbook.png" },
            { 8369, "perk-images/Styles/Inspiration/FirstStrike/FirstStrike.png" },
            { 8306, "perk-images/Styles/Inspiration/HextechFlashtraption/HextechFlashtraption.png" },
            { 8304, "perk-images/Styles/Inspiration/MagicalFootwear/MagicalFootwear.png" },
            { 8321, "perk-images/Styles/Inspiration/CashBack/CashBack.png" },
            { 8313, "perk-images/Styles/Inspiration/PerfectTiming/PerfectTiming.png" },
            { 8352, "perk-images/Styles/Inspiration/TimeWarpTonic/TimeWarpTonic.png" },
            { 8345, "perk-images/Styles/Inspiration/BiscuitDelivery/BiscuitDelivery.png" },
            { 8347, "perk-images/Styles/Inspiration/CosmicInsight/CosmicInsight.png" },
            { 8410, "perk-images/Styles/Resolve/ApproachVelocity/ApproachVelocity.png" },
            { 8316, "perk-images/Styles/Inspiration/JackOfAllTrades/JackofAllTrades2.png" },

            { 5008, "perk-images/StatMods/StatModsAdaptiveForceIcon.png" },
            { 5005, "perk-images/StatMods/StatModsAttackSpeedIcon.png" },  
            { 5007, "perk-images/StatMods/StatModsCDRScalingIcon.png" },   
            { 5010, "perk-images/StatMods/StatModsMovementSpeedIcon.png" },
            { 5001, "perk-images/StatMods/StatModsHealthScalingIcon.png" },
            { 5011, "perk-images/StatMods/StatModsHealthIcon.png" },       
            { 5013, "perk-images/StatMods/StatModsTenacityIcon.png" }      
        };
        // AI made my life easy here
        public Dictionary<int, string> SummonerSpells = new()
        {
            { 1, "SummonerBoost" },
            { 3, "SummonerExhaust" },
            { 4, "SummonerFlash" },
            { 6, "SummonerHaste" },
            { 7, "SummonerHeal" },
            { 11, "SummonerSmite" },
            { 12, "SummonerTeleport" },
            { 13, "SummonerMana" },
            { 14, "SummonerDot" },
            { 21, "SummonerBarrier" },
            { 30, "SummonerPoroRecall" },
            { 31, "SummonerPoroThrow" },
            { 32, "SummonerSnowball" },
            { 39, "SummonerSnowURFSnowball_Mark" },
            { 54, "Summoner_UltBookPlaceholder" },
            { 55, "Summoner_UltBookSmitePlaceholder" },
            { 2201, "SummonerCherryHold" },
            { 2202, "SummonerCherryFlash" }
        };
        // AI made my life easy here
        public Dictionary<int, string> queueDictionary = new Dictionary<int, string>
        {
            { 0, "Custom game" },
            { 2, "Blind Pick" },
            { 4, "Ranked Solo/Duo" },
            { 6, "Ranked Premade" },
            { 7, "Co-op vs AI" },
            { 8, "3v3 Normal" },
            { 9, "3v3 Ranked Flex" },
            { 14, "Draft Pick" },
            { 16, "Dominion Blind Pick" },
            { 17, "Dominion Draft Pick" },
            { 25, "Dominion Co-op vs AI" },
            { 31, "Co-op vs AI Intro Bot" },
            { 32, "Co-op vs AI Beginner Bot" },
            { 33, "Co-op vs AI Intermediate Bot" },
            { 41, "3v3 Ranked Team" },
            { 42, "5v5 Ranked Team" },
            { 52, "Co-op vs AI" },
            { 61, "5v5 Team Builder" },
            { 65, "5v5 ARAM" },
            { 67, "ARAM Co-op vs AI" },
            { 70, "One for All" },
            { 72, "1v1 Snowdown Showdown" },
            { 73, "2v2 Snowdown Showdown" },
            { 75, "6v6 Hexakill" },
            { 76, "Ultra Rapid Fire" },
            { 78, "One For All: Mirror Mode" },
            { 83, "Co-op vs AI Ultra Rapid Fire" },
            { 91, "Doom Bots Rank 1" },
            { 92, "Doom Bots Rank 2" },
            { 93, "Doom Bots Rank 5" },
            { 96, "Ascension" },
            { 98, "6v6 Hexakill" },
            { 100, "5v5 ARAM" },
            { 300, "Legend of the Poro King" },
            { 310, "Nemesis" },
            { 313, "Black Market Brawlers" },
            { 315, "Nexus Siege" },
            { 317, "Definitely Not Dominion" },
            { 318, "ARURF" },
            { 325, "All Random" },
            { 400, "Draft Pick" },
            { 410, "Ranked Dynamic" },
            { 420, "Ranked Solo/Duo" },
            { 430, "Blind Pick" },
            { 440, "Ranked Flex" },
            { 450, "ARAM" },
            { 460, "3v3 Blind Pick" },
            { 470, "3v3 Ranked Flex" },
            { 490, "Normal (Quickplay)" },
            { 600, "Blood Hunt Assassin" },
            { 610, "Dark Star: Singularity" },
            { 700, "Summoner's Rift Clash" },
            { 720, "ARAM Clash" },
            { 800, "Co-op vs. AI Intermediate Bot" },
            { 810, "Co-op vs. AI Intro Bot" },
            { 820, "Co-op vs. AI Beginner Bot" },
            { 830, "Co-op vs. AI Intro Bot" },
            { 840, "Co-op vs. AI Beginner Bot" },
            { 850, "Co-op vs. AI Intermediate Bot" },
            { 870, "Co-op vs. AI Intro Bot" },
            { 880, "Co-op vs. AI Beginner Bot" },
            { 890, "Co-op vs. AI Intermediate Bot" },
            { 900, "ARURF" },
            { 910, "Ascension" },
            { 920, "Legend of the Poro King" },
            { 940, "Nexus Siege" },
            { 950, "Doom Bots Voting" },
            { 960, "Doom Bots Standard" },
            { 980, "Star Guardian Invasion: Normal" },
            { 990, "Star Guardian Invasion: Onslaught" },
            { 1000, "PROJECT: Hunters" },
            { 1010, "Snow ARURF" },
            { 1020, "One for All" },
            { 1030, "Odyssey Extraction: Intro" },
            { 1040, "Odyssey Extraction: Cadet" },
            { 1050, "Odyssey Extraction: Crewmember" },
            { 1060, "Odyssey Extraction: Captain" },
            { 1070, "Odyssey Extraction: Onslaught" },
            { 1090, "Teamfight Tactics" },
            { 1100, "Ranked Teamfight Tactics" },
            { 1110, "Teamfight Tactics Tutorial" },
            { 1111, "Teamfight Tactics test" },
            { 1200, "Nexus Blitz" },
            { 1210, "Teamfight Tactics Choncc's Treasure Mode" },
            { 1300, "Nexus Blitz" },
            { 1400, "Ultimate Spellbook" },
            { 1700, "Arena" },
            { 1710, "Arena" },
            { 1810, "Swarm Mode" },
            { 1820, "Swarm" },
            { 1830, "Swarm" },
            { 1840, "Swarm" },
            { 1900, "Pick URF" },
            { 2000, "Tutorial 1" },
            { 2010, "Tutorial 2" },
            { 2020, "Tutorial 3" }
        };

        [HttpGet("GetAccount")]
        public async Task<ActionResult<RiotAccountDetails>> NewGetAccount(string gameName, string tagLine, string region)
        {
            // AI made my life easy here
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

            if (accountResponse2.puuid == null)
            {
                return NotFound("Account not found");
            }

            var rankedUrl = new RestClient($"https://{region}.api.riotgames.com/lol/league/v4/entries/by-puuid/{response.puuid}");
            var rankedRequest = new RestRequest("", Method.Get);
            rankedRequest.AddHeader("X-Riot-Token", api); 
            var rankedResponse = await rankedUrl.ExecuteAsync(rankedRequest);
            var rankedResponse2 = JsonConvert.DeserializeObject<List<RiotRanked>>(rankedResponse.Content);
            var allPlayerDetails = new List<PlayerMatchHistory>();

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
                    champName = "MonkeyKing";
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
                    GameMode = queueDictionary[matchDataResponse.info.queueId],
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
        public async Task<ActionResult<List<MatchStatsNew>>> GetMatchHistory(string gameName, string tagLine, string region)
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
                            Farm = totalFarm
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
        public async Task<ActionResult<List<MostPlayed>>> GetTopPlayed(string gameName, string tagLine, string region)
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

            // This is for DD to get the latest patch version and all characters to that patch
            var patchUrl = new RestClient("https://ddragon.leagueoflegends.com/api/versions.json");
            var patchRequest = new RestRequest("", Method.Get);
            var patchRestResponse = await patchUrl.ExecuteAsync(patchRequest);
            var patchResponse = JsonConvert.DeserializeObject<List<string>>(patchRestResponse.Content).FirstOrDefault();

            var championUrl = new RestClient($"https://ddragon.leagueoflegends.com/cdn/{patchResponse}/data/en_US/champion.json");
            var championRequest = new RestRequest("", Method.Get);
            var championRestResponse = await championUrl.ExecuteAsync(championRequest);
            var championResponse = JsonConvert.DeserializeObject<Champions>(championRestResponse.Content);

            // Below is to get the stats for the player
            var topPlayedList = new List<MostPlayed>();
            MostPlayed mostPlayed = new MostPlayed();

            var url = new RestClient($"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("X-Riot-Token", api);
            var restResponse = await url.ExecuteAsync(request);
            var response = JsonConvert.DeserializeObject<RiotAccount>(restResponse.Content);

            var topPlayedUrl = new RestClient($"https://{region}.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-puuid/{response.puuid}");
            var topPlayedRequest = new RestRequest("", Method.Get);
            topPlayedRequest.AddHeader("X-Riot-Token", api);
            var topPlayedRestResponse = await topPlayedUrl.ExecuteAsync(topPlayedRequest);
            var topPlayedResponse = JsonConvert.DeserializeObject<List<TopPlayed>>(topPlayedRestResponse.Content).Take(3);

            if (topPlayedResponse.Count() == 0)
            {
                return NotFound("Account not found");
            }

            foreach (var item in topPlayedResponse)
            {
                var champ = championResponse?.data.Values.FirstOrDefault(i => i.key == item.championId.ToString());

                //topPlayedList.Add($"{champ.name} ({item.championPoints})");

                string champName;
                if (champ.id == "Wukong")
                {
                    champName = "MonkeyKing";
                }
                else if (champ.id == "FiddleSticks")
                {
                    champName = "Fiddlesticks";
                }
                else
                {
                    champName = champ.id;
                }

                mostPlayed = new MostPlayed
                {
                    ChampionName = champName,
                    ChampionPoints = item.championPoints.ToString("N0", CultureInfo.InvariantCulture)
                };

                topPlayedList.Add(mostPlayed);
            }


            return topPlayedList;
        }

        [HttpGet("GetFreeCharacters")]
        public async Task<ActionResult<List<string>>> GetFreeCharacters()
        {
            // This is for DD to get the latest patch version and all characters to that patch
            var patchUrl = new RestClient("https://ddragon.leagueoflegends.com/api/versions.json");
            var patchRequest = new RestRequest("", Method.Get);
            var patchRestResponse = await patchUrl.ExecuteAsync(patchRequest);
            var patchResponse = JsonConvert.DeserializeObject<List<string>>(patchRestResponse.Content).FirstOrDefault();

            var championUrl = new RestClient($"https://ddragon.leagueoflegends.com/cdn/{patchResponse}/data/en_US/champion.json");
            var championRequest = new RestRequest("", Method.Get);
            var championRestResponse = await championUrl.ExecuteAsync(championRequest);
            var championResponse = JsonConvert.DeserializeObject<Champions>(championRestResponse.Content);

            var freeToPlayName = new List<string>();

            var freeToPlayUrl = new RestClient($"https://euw1.api.riotgames.com/lol/platform/v3/champion-rotations");
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
        public async Task<ActionResult<int>> PoroPopped(string gameName, string tagLine, string region)
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

        [HttpGet("GetSingleMatchDetailsForNormals")]
        public async Task<ActionResult<MatchStatsNew>> GetMatchDetails(string region, string matchID) 
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

            string didWin = "";
            var newMatchData = new MatchStatsNew();
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

            var blueTeam = matchDataResponse.info.teams.FirstOrDefault(t => t.teamId == 100);
            var redTeam = matchDataResponse.info.teams.FirstOrDefault(t => t.teamId == 200);

            int totalBlueTeamKills = blueTeam?.objectives?.champion?.kills ?? 0;
            int totalRedTeamKills = redTeam?.objectives?.champion?.kills ?? 0;

            int blueDragons = blueTeam?.objectives?.dragon?.kills ?? 0;
            int redDragons = redTeam?.objectives?.dragon?.kills ?? 0;

            int blueBarons = blueTeam?.objectives?.baron?.kills ?? 0;
            int redBarons = redTeam?.objectives?.baron?.kills ?? 0;

            int blueTurretKills = blueTeam?.objectives?.tower?.kills ?? 0;
            int redTurretKills = redTeam?.objectives?.tower?.kills ?? 0;

            int blueInhibKills = blueTeam?.objectives?.inhibitor?.kills ?? 0;
            int redInhibKills = redTeam?.objectives?.inhibitor?.kills ?? 0;

            int blueHearldKills = blueTeam?.objectives?.riftHerald?.kills ?? 0;
            int redHearldKills = redTeam?.objectives?.riftHerald?.kills ?? 0;

            int blueAtakhanKills = blueTeam?.objectives?.atakhan?.kills ?? 0;
            int redAtakhanKills = redTeam?.objectives?.atakhan?.kills ?? 0;

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
                    KeyStone = RuneImagePaths[mainRunePath[0].perk],
                    PrimaryRune1 = RuneImagePaths[mainRunePath[1].perk],
                    PrimaryRune2 = RuneImagePaths[mainRunePath[2].perk],
                    PrimaryRune3 = RuneImagePaths[mainRunePath[3].perk],

                    SecondaryRune1 = RuneImagePaths[secondRunePath[0].perk],
                    SecondaryRune2 = RuneImagePaths[secondRunePath[1].perk],

                    RuneShard1 = RuneImagePaths[subItem.perks.statPerks.offense],
                    RuneShard2 = RuneImagePaths[subItem.perks.statPerks.flex],
                    RuneShard3 = RuneImagePaths[subItem.perks.statPerks.defense]
                };

                // https://ddragon.leagueoflegends.com/cdn/15.14.1/img/item/1001.png Item URL for frontend

                Items playerItems = new Items
                {
                    Item1 = subItem.item0   != 0 ? subItem.item0 : null,
                    Item2 = subItem.item1   != 0 ? subItem.item1 : null,
                    Item3 = subItem.item2   != 0 ? subItem.item2 : null,
                    Item4 = subItem.item3   != 0 ? subItem.item3 : null,
                    Item5 = subItem.item4   != 0 ? subItem.item4 : null,
                    Item6 = subItem.item5   != 0 ? subItem.item5 : null,
                    Ward = subItem.item6    != 0 ? subItem.item6 : null
                };

                string champName;

                if (subItem.championName == "Wukong")
                {
                    champName = "MonkeyKing";
                }
                else if (subItem.championName == "FiddleSticks")
                {
                    champName = "Fiddlesticks";
                }
                else
                {
                    champName = subItem.championName;
                }

                var playerDetails = new PlayerMatchDetails
                {
                    PlayerName = subItem.riotIdGameName,
                    TeamID = teamName,
                    ChampionName = champName,
                    LaneName = subItem.individualPosition,
                    KDA = KDA,
                    GoldSpent = subItem.goldSpent,
                    GoldEarned = subItem.goldEarned,
                    Damage = subItem.totalDamageDealtToChampions,
                    DamageTaken = subItem.totalDamageTaken,
                    TowerDamage = subItem.damageDealtToTurrets,
                    ObjDamage = subItem.damageDealtToObjectives,
                    SkillshotsHit = subItem.challenges?.skillshotsHit ?? 0,
                    SkillshotsMissed = subItem.challenges?.skillshotsDodged ?? 0,
                    Farm = totalFarm,
                    VisionScore = subItem.visionScore,
                    PlayerItems = playerItems,
                    SummonerSpell1 = SummonerSpells[subItem.summoner1Id],
                    SummonerSpell2 = SummonerSpells[subItem.summoner2Id],
                    Runes = playerRunes
                };
                playersInMatch.Add(playerDetails);
            }

            var matchStats = new MatchStatsNew
            {
                GameID = matchDataResponse.metadata.matchId,
                GameWinner = didWin,
                GameMode = queueDictionary[matchDataResponse.info.queueId],
                TotalBlueKills = totalBlueTeamKills,
                TotalRedKills = totalRedTeamKills,
                TotalBlueDragonKills = blueDragons,
                TotalRedDragonKills = redDragons,
                TotalBlueBaronKills = blueBarons,
                TotalRedBaronKills = redBarons,
                TotalBlueTurrets = blueTurretKills,
                TotalRedTurrets = redTurretKills,
                TotalBlueInhib = blueInhibKills, 
                TotalRedInhib = redInhibKills,
                BlueHearldKills =  blueHearldKills,
                RedHearldKills = redHearldKills,
                BlueAtakhanKills = blueAtakhanKills,
                RedAtakhanKills = redAtakhanKills,
                Players = playersInMatch
            };

            newMatchData = matchStats;

            return newMatchData;
        }

        [HttpGet("GetSingleMatchDetailsForArena")]
        public async Task<ActionResult<SpecialGamemodeStats>> GetSpecialMatchDetails(string region, string matchID) 
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

            string gameMode = "";
            string gameWinner = "";
            var newMatchData = new SpecialGamemodeStats();
            var playersInMatch = new List<SpecialPlayerDetails>();

            var matchDataUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/{matchID}");
            var matchDataRequest = new RestRequest("", Method.Get);
            matchDataRequest.AddHeader("X-Riot-Token", api);
            var matchDataRestResponse = await matchDataUrl.ExecuteAsync(matchDataRequest);
            var matchDataResponse = JsonConvert.DeserializeObject<MatchData>(matchDataRestResponse.Content);
            
            foreach (var item in matchDataResponse.info.participants.OrderBy(x => x.placement))
            {

                string playerTeamName = item.playerSubteamId switch
                {
                    1 => "Poro",
                    2 => "Minion",
                    3 => "Scuttle",
                    4 => "Krug",
                    5 => "Raptor",
                    6 => "Sentinel",
                    7 => "Wolf",
                    8 => "Gromp"
                };

                if (gameWinner == "" || gameWinner == null) 
                {
                    gameWinner = playerTeamName;
                }

                string KDA = $"{item.kills}/{item.deaths}/{item.assists}";

                Items playerItems = new Items
                {
                    Item1 = item.item0 != 0 ? item.item0 : null,
                    Item2 = item.item1 != 0 ? item.item1 : null,
                    Item3 = item.item2 != 0 ? item.item2 : null,
                    Item4 = item.item3 != 0 ? item.item3 : null,
                    Item5 = item.item4 != 0 ? item.item4 : null,
                    Item6 = item.item5 != 0 ? item.item5 : null,
                    Ward = item.item6  != 0 ? item.item6 : null
                };

                Augments playerAugments = new Augments
                {
                    Augments1 = item.playerAugment1,
                    Augments2 = item.playerAugment2,
                    Augments3 = item.playerAugment3,
                    Augments4 = item.playerAugment4,
                    Augments5 = item.playerAugment5,
                    Augments6 = item.playerAugment6
                };

                string champName;

                if (item.championName == "Wukong")
                {
                    champName = "MonkeyKing";
                }
                else if (item.championName == "FiddleSticks")
                {
                    champName = "Fiddlesticks";
                }
                else
                {
                    champName = item.championName;
                }


                var playerDetails = new SpecialPlayerDetails
                {
                    PlayerName = item.riotIdGameName,
                    PlayerTeamPosition = item.placement,
                    PlayerTeamName = playerTeamName,
                    ChampionName = champName,
                    KDA = KDA,
                    Damage = item.totalDamageDealtToChampions,
                    DamageTaken = item.totalDamageTaken,
                    SkillshotsHit = item.challenges.skillshotsHit,
                    SkillshotsMissed = item.challenges.skillshotsDodged,
                    HealShield = Math.Round(item.challenges.effectiveHealAndShielding),
                    Items = playerItems,
                    SummonerSpell1 = SummonerSpells[item.summoner1Id],
                    SummonerSpell2 = SummonerSpells[item.summoner2Id],
                    Augments = playerAugments
                };
                playersInMatch.Add(playerDetails);
            }

            var matchStats = new SpecialGamemodeStats
            {
                GameID = matchDataResponse.metadata.matchId,
                GameMode = queueDictionary[matchDataResponse.info.queueId],
                GameWinner = $"Team {gameWinner}",
                SpecialGamePlayerStats = playersInMatch
            };

            newMatchData = matchStats;

            return newMatchData;
        }

        [HttpGet("GetChampionData")]
        public async Task<ActionResult<ChampionDetails>> GetChampionData(string championName) 
        {
            var patchUrl = new RestClient("https://ddragon.leagueoflegends.com/api/versions.json");
            var patchRequest = new RestRequest("", Method.Get);
            var patchRestResponse = await patchUrl.ExecuteAsync(patchRequest);
            var patchResponse = JsonConvert.DeserializeObject<List<string>>(patchRestResponse.Content).FirstOrDefault();

            var Url = new RestClient($"https://ddragon.leagueoflegends.com/cdn/{patchResponse}/data/en_US/champion/{championName}.json");
            var urlRequest = new RestRequest("", Method.Get);
            var urlRestResponse = await Url.ExecuteAsync(urlRequest);
            var urlResponse = JsonConvert.DeserializeObject<dynamic>(urlRestResponse.Content);

            var champ = urlResponse.data[championName];

            return new ChampionDetails 
            {
                championPassive = (string)champ.passive.image.full,
                Passive_SpellName = (string)champ.passive.name,
                Passive_Description = (string)champ.passive.description,

                ChampionQ = (string)champ.spells[0].image.full,
                Q_SpellName = (string)champ.spells[0].name,
                Q_Description = (string)champ.spells[0].description,

                ChampionW = (string)champ.spells[1].image.full,
                W_SpellName = (string)champ.spells[1].name,
                W_Description = (string)champ.spells[1].description,

                ChampionE = (string)champ.spells[2].image.full,
                E_SpellName = (string)champ.spells[2].name,
                E_Description = (string)champ.spells[2].description,
                
                ChampionR = (string)champ.spells[3].image.full,
                R_SpellName = (string)champ.spells[3].name,
                R_Description = (string)champ.spells[3].description,

                ChampName = $"{champ.name} {champ.title}",
                ChampionLore = (string)champ.lore
            };

            //return championData;

            //return null;
        }
    }
}