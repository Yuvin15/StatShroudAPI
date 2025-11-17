using API.Models;
using FirebaseAdmin.Messaging;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text.Json;
using System.Text.RegularExpressions;
using static API.Models.MatchData;
using static API.Models.RiotChallenges;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RiotController : Controller
    {
        public string api;
        private readonly FirestoreDb _firestoreDb;
        public RiotController(IConfiguration config)
        {
            api = config["RiotAPI:Token"];
            _firestoreDb = FirestoreDb.Create("leaguestats-9a390");
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
            { 480, "SwiftPlay" },
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
            { 2020, "Tutorial 3" },
            { 3100, "Custom Game" },
            { 3140, "Practice tool" }
        };

        public Dictionary<string, int> FarmPerRank = new Dictionary<string, int>
        {
            { "Unranked",  5 },
            { "Iron",  5 },
            { "Bronze" , 5 },
            { "Silver" , 6},
            { "Gold" , 7},
            { "Platinum" , 8},
            { "Diamond" , 9},
            { "Master" , 10},
            { "Grandmaster" , 10},
            { "Challenger" , 10}
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
                "ru" => "europe",
                "me" => "europe"
            };

            string whatStreak = "";
            string farmStatus = "";
            string gameDuration = "";
            string playerSurvivability = "";
            string otpOrNot = "";
            List<int> killAssistCount = new List<int>();  
            List<int> deathCount = new List<int>();
            List<int> farmDeficit = new List<int>();
            List<string> lane = new List<string>();
            List<string> otpCheck = new List<string>();

            //To get the wr for each champion recently played
            int totalGamesplayed = 0;
            Dictionary<string, List<string>> champWinOrLose = new Dictionary<string, List<string>>();
            //Dictionary for champion and their wr in recent games
            Dictionary<string, double> recentWR = new Dictionary<string, double>();

            // Fetch data needed for account details
            var url = new RestClient($"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("X-Riot-Token", api);
            var restResponse = await url.ExecuteAsync(request);
            var response = JsonConvert.DeserializeObject<NewRiotAccount>(restResponse.Content);

            if (response.puuid == null)
            {
                return NotFound("Account not found");
            }

            /*Old details 
            //Need this because I need to get the details for the player from their puuid
            var accountUrl = new RestClient($"https://{region}.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{response.puuid}");
            var accountRequest = new RestRequest("", Method.Get);
            accountRequest.AddHeader("X-Riot-Token", api);
            var accountResponse = await accountUrl.ExecuteAsync(accountRequest);
            var accountResponse2 = JsonConvert.DeserializeObject<RiotAccount>(accountResponse.Content);
            */

            //Match ids
            var matchUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/by-puuid/{response.puuid}/ids?start=0&count=20");
            var matchRequest = new RestRequest("", Method.Get);
            matchRequest.AddHeader("X-Riot-Token", api);
            var matchResponse = await matchUrl.ExecuteAsync(matchRequest);
            var matchResponse2 = JsonConvert.DeserializeObject<List<string>>(matchResponse.Content);

            //Just a double sure cause its possible for someone to play less than 20
            totalGamesplayed = matchResponse2.Count;

            var allPlayerDetails = new List<PlayerMatchHistory>();
            List<string> gameWinnerCheck = new List<string>();
            List<string> reverseGameWinnerCheck = new List<string>();

            //Need this because I need tp get the stats for the player from their puuid
            var accountDetailsTask = Task.Run(async () =>
            {
                var accountUrl = new RestClient($"https://{region}.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{response.puuid}");
                var accountRequest = new RestRequest("", Method.Get);
                accountRequest.AddHeader("X-Riot-Token", api);
                var accountResponse = await accountUrl.ExecuteAsync(accountRequest);
                var accountResponse2 = JsonConvert.DeserializeObject<RiotAccount>(accountResponse.Content);

                return accountResponse2;
            });

            /* Get account name
            var accountNameTask = Task.Run(async () =>
            {
                var newUrl = new RestClient($"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
                var newRequest = new RestRequest("", Method.Get);
                newRequest.AddHeader("X-Riot-Token", api);
                var newRestResponse = await newUrl.ExecuteAsync(newRequest);
                var newResponse = JsonConvert.DeserializeObject<NewRiotAccount>(newRestResponse.Content);

                return newResponse;
            });
            */

            // Get ranked data
            var rankedDetailsTask = Task.Run(async () =>
            {
                var rankedUrl = new RestClient($"https://{region}.api.riotgames.com/lol/league/v4/entries/by-puuid/{response.puuid}");
                var rankedRequest = new RestRequest("", Method.Get);
                rankedRequest.AddHeader("X-Riot-Token", api);
                var rankedResponse = await rankedUrl.ExecuteAsync(rankedRequest);
                var rankedResponse2 = JsonConvert.DeserializeObject<List<RiotRanked>>(rankedResponse.Content);

                return rankedResponse2;
            });

            await Task.WhenAll(accountDetailsTask, rankedDetailsTask /*accountNameTask*/);

            var accountDetails = accountDetailsTask.Result;
            var rankedAccountDetails = rankedDetailsTask.Result;
            //var accountNameDetails = accountNameTask.Result;

            /*Old Get ranked data
            var rankedUrl = new RestClient($"https://{region}.api.riotgames.com/lol/league/v4/entries/by-puuid/{response.puuid}");
            var rankedRequest = new RestRequest("", Method.Get);
            rankedRequest.AddHeader("X-Riot-Token", api); 
            var rankedResponse = await rankedUrl.ExecuteAsync(rankedRequest);
            var rankedResponse2 = JsonConvert.DeserializeObject<List<RiotRanked>>(rankedResponse.Content);
            */

            //Check every match id
            foreach (var item in matchResponse2)
            {
                var playersInMatch = new List<PlayerMatchDetails>();
                var matchDataUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/{item}");
                var matchDataRequest = new RestRequest("", Method.Get);
                matchDataRequest.AddHeader("X-Riot-Token", api);
                var matchDataRestResponse = await matchDataUrl.ExecuteAsync(matchDataRequest);
                var matchDataResponse = JsonConvert.DeserializeObject<MatchData>(matchDataRestResponse.Content);

                var participants = matchDataResponse.info.participants.Where(i => string.Equals(i.puuid, response.puuid, StringComparison.OrdinalIgnoreCase));
                var player = participants.FirstOrDefault();

                gameDuration = TimeSpan.FromSeconds(matchDataResponse.info.gameDuration).ToString(@"mm\:ss");
                int gameDurationSeconds = matchDataResponse.info.gameDuration;
                double gameMinutes = gameDurationSeconds / 60.0;

                int totalFarm = (int)(player.totalMinionsKilled + player.neutralMinionsKilled);

                killAssistCount.Add(player.kills + int.Parse(player.assists));
                deathCount.Add(player.deaths);

                //Get the game winner for that match
                string gameWinner = "";
                foreach (var team in matchDataResponse.info.teams)
                {
                    if (team.win)
                    {
                        if (team.teamId == player.teamId)
                        {
                            gameWinner = "Victory";
                            gameWinnerCheck.Add(gameWinner);
                        }
                        else
                        {
                            gameWinner = "Defeat";
                            gameWinnerCheck.Add(gameWinner);
                        }
                        break;
                    }
                }

                //I have to do this check cause game files are different to champion names
                string champName;
                if (player.championName == "Wukong")
                {
                    champName = "MonkeyKing";
                }
                else if (player.championName == "FiddleSticks")
                {
                    champName = "Fiddlesticks";
                }
                else
                {
                    champName = player.championName;
                }

                otpCheck.Add(champName);

                if (!champWinOrLose.ContainsKey(champName))
                    champWinOrLose[champName] = new List<string>();

                champWinOrLose[champName].Add(gameWinner);

                //Check solo queue rank cause thats the rank people check the most
                var SoloRank = rankedAccountDetails.FirstOrDefault(r => r?.queueType == "RANKED_SOLO_5x5");
                string rank = SoloRank?.tier?.ToLower() switch
                {
                    "unranked" => "Unranked",
                    "iron" => "Iron",
                    "bronze" => "Bronze",
                    "silver" => "Silver",
                    "gold" => "Gold",
                    "platinum" => "Platinum",
                    "diamond" => "Diamond",
                    "master" => "Master",
                    "grandmaster" => "Grandmaster",
                    "challenger" => "Challenger",
                    _ => "Unranked"
                };

                //Claude helped me fix the farming logic here
                // Only check farming for non-support roles
                if (player.individualPosition != "UTILITY")
                {
                    //Do this to check the cs/min against their rank
                    int targetCSPerMin = FarmPerRank[rank];
                    double expectedFarm = targetCSPerMin * gameMinutes;
                    double actualCSPerMin = totalFarm / gameMinutes;

                    // Fixed logic: if actual CS per min is LESS than target, add to deficit
                    if (actualCSPerMin < targetCSPerMin)
                    {
                        double deficit = expectedFarm - totalFarm;
                        farmDeficit.Add(Math.Abs((int)Math.Round(deficit)));
                        lane.Add(player.individualPosition);
                    }
                    else
                    {
                        // Player farmed well, add 0 deficit or handle separately
                        farmDeficit.Add(0);
                        lane.Add(player.individualPosition);
                    }
                }
                // For support players, don't add to farm tracking lists at all
                // This prevents them from affecting farm statistics

                var uniqueDetails = new PlayerMatchHistory
                {
                    MatchID = matchDataResponse.metadata.matchId,
                    GameWinner = gameWinner,
                    GameMode = queueDictionary[matchDataResponse.info.queueId],
                    MatchDuration = gameDuration,
                    ChampionName = champName ?? "Unknown",
                    Lane = player.individualPosition ?? "Unknown",
                    KDA = $"{player.kills}/{player.deaths}/{player.assists}",
                    Farm = totalFarm
                };

                allPlayerDetails.Add(uniqueDetails);
            }

            //Check the player's stats for their last few games
            int goodFarmCheck = 0;

            //Check for loss streak or win streak
            reverseGameWinnerCheck = gameWinnerCheck.AsEnumerable().Reverse().TakeLast(3).ToList();

            if (reverseGameWinnerCheck.Count() < 3) 
            {
                whatStreak = "Less than 3 games";
            }
            else if (reverseGameWinnerCheck?[0] == "Victory"
                     && reverseGameWinnerCheck?[1] == "Victory"
                     && reverseGameWinnerCheck?[2] == "Victory")
            {
                whatStreak = "Winning Streak";
            }
            else if (reverseGameWinnerCheck?[0] == "Defeat"
                     && reverseGameWinnerCheck?[1] == "Defeat"
                     && reverseGameWinnerCheck?[2] == "Defeat")
            {
                whatStreak = "Losing Streak";
            }
            else
            {
                whatStreak = "No streak";
            }

            //Check if they farmed well based on their rank (supports are already excluded)
            for (int i = 0; i < farmDeficit.Count; i++)
            {
                // Since we only added non-support games to farmDeficit, no need to check lane again
                if (farmDeficit[i] <= 50) // Changed logic: good farming means LOW deficit
                {
                    goodFarmCheck++;
                }
            }

            //Determine farm status based on non-support games only
            int totalNonSupportGames = farmDeficit.Count;
            if (totalNonSupportGames == 0)
            {
                farmStatus = "Player only played support roles - farming not applicable";
            }
            else if (goodFarmCheck >= (totalNonSupportGames * 0.6)) // 60% threshold
            {
                farmStatus = "Player farmed above their ranked expectation in majority of their non-support games";
            }
            else if (goodFarmCheck >= (totalNonSupportGames * 0.3)) // 30% threshold
            {
                farmStatus = "Player farmed as intended for their rank";
            }
            else
            {
                farmStatus = "Player needs to work on their farming skills";
            }

            // Check for player's survivability based on KDA
            int goodKDA = 0;
            int badKDA = 0;

            for (int i = 0; i < killAssistCount.Count(); i++)
            {
                if (killAssistCount[i] > deathCount[i])
                {
                    goodKDA++;
                }
                else
                {
                    badKDA++;
                }
            }

            if (goodKDA > badKDA)
            {
                playerSurvivability = "Player has a good Kill and Assist to Death ratio";
            }
            else if (goodKDA < badKDA)
            {
                playerSurvivability = "Player has a bad Kill and Assist to Death ratio";
            }
            else
            {
                playerSurvivability = "Player has an average Kill and Assist to Death ratio";
            }

            //Checks if the player is an otp or not
            var champCounts = otpCheck
                            .GroupBy(x => x)
                            .Select(g => new { Champ = g.Key, Count = g.Count() })
                            .OrderByDescending(g => g.Count)
                            .ToList();

            var topChamp = champCounts.First();

            if (topChamp.Count >= 7)
            {
                otpOrNot += $"Player OTP is {topChamp.Champ}";

                if(topChamp.Champ == "Teemo") 
                {
                    otpOrNot = "This player has no life (Teemo OTP)";
                }

                if (topChamp.Champ == "Yuumi") 
                { 
                    otpOrNot = @"This player is a ""gamer"" (Yuumi OTP)";
                }

            }
            else if (topChamp.Count >= 5)
            {
                otpOrNot += $"Player mains {topChamp.Champ}";
            }
            else
            {
                otpOrNot += "Player plays a wide variety of champions";
            }

            PlayerAchievments playerAchievments = new PlayerAchievments
            {
                FarmPer10 = farmStatus,
                Streak = whatStreak,
                PlayerSurvivability = playerSurvivability,
                OTPStatus = otpOrNot
            };

            foreach (var champ in champWinOrLose.Keys)
            {
                int wins = champWinOrLose[champ].Count(result => result == "Victory");
                int total = champWinOrLose[champ].Count;
                double winRate = (double)wins / total * 100;
                recentWR[champ] = Math.Round(winRate);
            }

            var playerDetails = new RiotAccountDetails
            {
                gameName = $"{response.gameName}#{response.tagLine}",
                summonerLevel = accountDetails.summonerLevel,
                profileIconId = accountDetails.profileIconId,
                recentGamesWinRate = recentWR,
                SoloRank = rankedAccountDetails.FirstOrDefault(r => r?.queueType == "RANKED_SOLO_5x5") is var solo && solo != null
                                                        ? $"{solo.tier} {solo.rank} ({solo.leaguePoints} LP)"
                                                        : "Unranked",
                FlexRank = rankedAccountDetails.FirstOrDefault(r => r?.queueType == "RANKED_FLEX_SR") is var flex && flex != null
                                                        ? $"{flex.tier} {flex.rank} ({flex.leaguePoints} LP)"
                                                        : "Unranked",
                Achievments = playerAchievments,
                BasicMatchDetails = allPlayerDetails
            };

            return Ok(playerDetails);

        }

        [HttpGet("GetChallenges")]
        public async Task<ActionResult<dynamic>> GetChallenges(string gameName, string tagLine, string region)
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

            var cDragURL = new RestClient($"https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/challenges.json");
            var cDragRequest = new RestRequest("", Method.Get);
            var cDragResponse = await cDragURL.ExecuteAsync(cDragRequest);
            var cDragResponse2 = JsonConvert.DeserializeObject<CommunityChallenges>(cDragResponse.Content);

            var detailLookup = cDragResponse2.challenges
            .Where(kvp => kvp.Value != null)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => new {
                    kvp.Value.name,
                    kvp.Value.description
                }
            );

            List<string> ignoredChallenges = new List<string>()
            {
                "imagination",
                "collection",
                "veterancy",
                "expertise",
                "teamwork"
            };
            
            var challengeList = challengesResponse2.challenges
            .Where(c => detailLookup.ContainsKey(c.challengeId) &&
                        !ignoredChallenges.Contains(detailLookup[c.challengeId].name.ToLower()))
            .Select(c => new {
                id = c.challengeId,
                name = detailLookup[c.challengeId].name,
                description = detailLookup[c.challengeId].description,
                level = c.level?.ToLower(),
                value = c.value,
                percentile = c.percentile,
                position = c.position,
                playersInLevel = c.playersInLevel
            })
            .ToList();

            var result = new
            {
                challengesResponse2.totalPoints,
                challengesResponse2.categoryPoints,
                challenges = challengeList
            };

            return Ok(result);

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
                "ru" => "europe",
                "me" => "europe"
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

            return Ok(newMatchData);
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
                "ru" => "europe",
                "me" => "europe"
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


            return Ok(topPlayedList);
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

            return Ok(freeChampNames);

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
                "ru" => "europe",
                "me" => "europe"
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

            return Ok(totalPoroPopped);
        }

        [HttpGet("GetSingleMatchDetailsForNormals")]
        public async Task<ActionResult<MatchStatsNew>> GetMatchDetails(string region, string matchID) 
        {
            Dictionary<string, string> puuidToChampDict = new Dictionary<string, string>();

            // This is for DD to get the latest patch version and all characters to that patch
            var patchUrl = new RestClient("https://ddragon.leagueoflegends.com/api/versions.json");
            var patchRequest = new RestRequest("", Method.Get);
            var patchRestResponse = await patchUrl.ExecuteAsync(patchRequest);
            var patchResponse = JsonConvert.DeserializeObject<List<string>>(patchRestResponse.Content).FirstOrDefault();

            var championUrl = new RestClient($"https://ddragon.leagueoflegends.com/cdn/{patchResponse}/data/en_US/champion.json");
            var championRequest = new RestRequest("", Method.Get);
            var championRestResponse = await championUrl.ExecuteAsync(championRequest);
            var championResponse = JsonConvert.DeserializeObject<Champions>(championRestResponse.Content);

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
                "ru" => "europe",
                "me" => "europe"
            };

            string didWin = "";
            var newMatchData = new MatchStatsNew();
            var playersInMatch = new List<PlayerMatchDetails>();

            /* 
            /// Old way
            var matchDataUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/{matchID}");
            var matchDataRequest = new RestRequest("", Method.Get);
            matchDataRequest.AddHeader("X-Riot-Token", api);
            var matchDataRestResponse = await matchDataUrl.ExecuteAsync(matchDataRequest);
            var matchDataResponse = JsonConvert.DeserializeObject<MatchData>(matchDataRestResponse.Content);
            */
            
            var matchDataTask = Task.Run(async () =>
            {
                var matchDataUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/{matchID}");
                var matchDataRequest = new RestRequest("", Method.Get);
                matchDataRequest.AddHeader("X-Riot-Token", api);
                var matchDataRestResponse = await matchDataUrl.ExecuteAsync(matchDataRequest);
                var matchDataResponse = JsonConvert.DeserializeObject<MatchData>(matchDataRestResponse.Content);

                return matchDataResponse;
            });

            var timelineTask = Task.Run(async () =>
            {
            
                var timelineUrl = new RestClient($"https://{region}.api.riotgames.com/lol/match/v5/matches/{matchID}/timeline");
                var timelineRequest = new RestRequest("", Method.Get);
                timelineRequest.AddHeader("X-Riot-Token", api);
                var timelineRestResponse = await timelineUrl.ExecuteAsync(timelineRequest);
                var timelineResponse = JsonConvert.DeserializeObject<Timeline>(timelineRestResponse.Content);

                return timelineResponse;
            });

            await Task.WhenAll(matchDataTask, timelineTask);

            var matchDataResponse = matchDataTask.Result;
            var timeLineResponse = timelineTask.Result;

            List<string> timelineData = new List<string>();

            // Nullable for people that dont ban and swiftplay and aram
            var blueTeamBans = matchDataResponse.info.teams.FirstOrDefault(x => x.teamId == 100)?.bans;
            var redTeamBans = matchDataResponse.info.teams.FirstOrDefault(x => x.teamId == 200)?.bans;

            List<string> blueBans = new List<string>();
            List<string> redBans = new List<string>();

            foreach(var item in blueTeamBans) 
            {
                var champ = championResponse?.data.Values.FirstOrDefault(i => i.key == item.championId.ToString());

                if (champ != null)
                {
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
                    blueBans.Add(champName);
                }
            }

            foreach (var item in redTeamBans)
            {
                var champ = championResponse?.data.Values.FirstOrDefault(i => i.key == item.championId.ToString());
                if (champ != null)
                {
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
                    redBans.Add(champName);
                }
            }

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

                puuidToChampDict.Add(subItem.puuid, champName);

                var playerDetails = new PlayerMatchDetails
                {
                    PlayerName = subItem.riotIdGameName,
                    PlayerTag = subItem.riotIdTagline,
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
                GameTime = TimeSpan.FromSeconds(matchDataResponse.info.gameDuration).ToString(@"mm\:ss"),
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
                Players = playersInMatch,
                BlueTeamBans = blueBans,
                RedTeamBans = redBans
            };

            newMatchData = matchStats;

            return Ok(newMatchData);
        }

        [HttpGet("GetSingleMatchDetailsForArena")]
        public async Task<ActionResult<SpecialGamemodeStats>> GetSpecialMatchDetails(string region, string matchID) 
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

            var augmentUrl = new RestClient($"https://raw.communitydragon.org/latest/cdragon/arena/en_us.json");
            var augmentRequest = new RestRequest("", Method.Get);
            var augmentRestResponse = await augmentUrl.ExecuteAsync(augmentRequest);
            var augmentResponse = JsonConvert.DeserializeObject<AugmentDetails>(augmentRestResponse.Content);

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
                "ru" => "europe",
                "me" => "europe"
            };

            string gameWinner = "";
            var newMatchData = new SpecialGamemodeStats();
            var playersInMatch = new List<SpecialPlayerDetails>();

            var matchDataUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/{matchID}");
            var matchDataRequest = new RestRequest("", Method.Get);
            matchDataRequest.AddHeader("X-Riot-Token", api);
            var matchDataRestResponse = await matchDataUrl.ExecuteAsync(matchDataRequest);
            var matchDataResponse = JsonConvert.DeserializeObject<MatchData>(matchDataRestResponse.Content);

            List<string> BanList = new List<string>();

            foreach (var team in matchDataResponse.info.teams)
            {
                foreach (var ban in team.bans)
                {
                    var champ = championResponse?.data.Values.FirstOrDefault(i => i.key == ban.championId.ToString());

                    if (champ != null)
                    {
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
                        BanList.Add(champName);
                    }
                }
            }


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

                var augment1 = augmentResponse?.augments?.FirstOrDefault(x => x.id == item.playerAugment1);
                string? augmentName = augment1 != null ? augment1.name : "Null";

                var augment2 = augmentResponse?.augments?.FirstOrDefault(x => x.id == item.playerAugment2);
                string? augmentName2 = augment2 != null ? augment2.name : "Null";

                var augment3 = augmentResponse?.augments?.FirstOrDefault(x => x.id == item.playerAugment3);
                string? augmentName3 = augment3 != null ? augment3.name : "Null";

                var augment4 = augmentResponse?.augments?.FirstOrDefault(x => x.id == item.playerAugment4);
                string? augmentName4 = augment4 != null ? augment4.name : "Null";

                var augment5 = augmentResponse?.augments?.FirstOrDefault(x => x.id == item.playerAugment5);
                string? augmentName5 = augment5 != null ? augment5.name : "Null";

                var augment6 = augmentResponse?.augments?.FirstOrDefault(x => x.id == item.playerAugment6);
                string? augmentName6 = augment6 != null ? augment6.name : "Null";

                Augments playerAugments = new Augments
                {
                    Augments1 = augment1?.iconLarge,
                    Augments1Name = augment1?.name,
                    Augments1Description = augment1?.desc,
                    Augments2 = augment2?.iconLarge,
                    Augments2Name = augment2?.name,
                    Augments2Description = augment2?.desc,
                    Augments3 = augment3?.iconLarge,
                    Augments3Name = augment3?.name,
                    Augments3Description = augment3?.desc,
                    Augments4 = augment4?.iconLarge,
                    Augments4Name = augment4?.name,
                    Augments4Description = augment4?.desc,
                    Augments5 = augment5?.iconLarge,
                    Augments5Name = augment5?.name,
                    Augments5Description = augment5?.desc,
                    Augments6 = augment6?.iconLarge,
                    Augments6Name = augment6?.name,
                    Augments6Description = augment6?.desc
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
                BanList = BanList,
                SpecialGamePlayerStats = playersInMatch
            };

            newMatchData = matchStats;

            return Ok(newMatchData);
        }

        [HttpGet("GetChampionData")]
        public async Task<ActionResult<ChampionDetails>> GetChampionData(string championName) 
        {
            if (championName.Equals("FiddleSticks"))
            {
                championName = "Fiddlesticks";
            }

            var patchUrl = new RestClient("https://ddragon.leagueoflegends.com/api/versions.json");
            var patchRequest = new RestRequest("", Method.Get);
            var patchRestResponse = await patchUrl.ExecuteAsync(patchRequest);
            var patchResponse = JsonConvert.DeserializeObject<List<string>>(patchRestResponse.Content).FirstOrDefault();

            var Url = new RestClient($"https://ddragon.leagueoflegends.com/cdn/{patchResponse}/data/en_US/champion/{championName}.json");
            var urlRequest = new RestRequest("", Method.Get);
            var urlRestResponse = await Url.ExecuteAsync(urlRequest);
            var urlResponse = JsonConvert.DeserializeObject<dynamic>(urlRestResponse.Content);

            var champ = urlResponse.data[championName];

            var champSkins = new List<Skins>();

            foreach (var item in champ.skins)
            {
                string champNameForCase;

                if ((string)item.name == "default") 
                {
                    champNameForCase = "Default";
                }
                else 
                {
                    champNameForCase = (string)item.name;
                }

                champSkins.Add(new Skins
                {
                    skinName = champNameForCase,
                    skinNum = (int)item.num
                });
            }

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
                ChampionLore = (string)champ.lore,

                ChampSkins = champSkins,

                Hp = champ.stats.hp,
                HpPerLevel = champ.stats.hpperlevel,
                Mp = champ.stats.mp,
                MpPerLevel = champ.stats.mpperlevel,
                MoveSpeed = champ.stats.movespeed,
                Armor = champ.stats.armor,
                ArmorPerLevel = champ.stats.armorperlevel,
                SpellBlock = champ.stats.spellblock,
                SpellBlockPerLevel = champ.stats.spellblockperlevel,
                AttackRange = champ.stats.attackrange,
                HpRegen = champ.stats.hpregen,
                HpRegenPerLevel = champ.stats.hpregenperlevel,
                MpRegen = champ.stats.mpregen,
                MpRegenPerLevel = champ.stats.mpregenperlevel,
                Crit = champ.stats.crit,
                CritPerLevel = champ.stats.critperlevel,
                AttackDamage = champ.stats.attackdamage,
                AttackDamagePerLevel = champ.stats.attackdamageperlevel,
                AttackSpeedPerLevel = champ.stats.attackspeedperlevel,
                AttackSpeed = champ.stats.attackspeed
            };

            //return championData;

            //return null;
        }

        [HttpGet("GetTopPlayers")]
        public async Task<ActionResult<List<Leaderboard>>> GetTopPlayers(string region) 
        {
            List<Leaderboard> leaderboards = new List<Leaderboard>();

            var soloQueue = Task.Run(async () =>
            {
                var solourl = $"https://{region}.api.riotgames.com/lol/league/v4/challengerleagues/by-queue/RANKED_SOLO_5x5";
                var soloRestClient = new RestClient(solourl);
                var soloRestRequest = new RestRequest("", Method.Get);
                soloRestRequest.AddHeader("X-Riot-Token", api);
                var soloRestResponse = await soloRestClient.ExecuteAsync(soloRestRequest);
                var soloLearboardEntries = JsonConvert.DeserializeObject<Leaderboard>(soloRestResponse.Content);

                leaderboards.Add(soloLearboardEntries);
            });

            var flexQueue = Task.Run(async () =>
            {
                var flexurl = $"https://{region}.api.riotgames.com/lol/league/v4/challengerleagues/by-queue/RANKED_FLEX_SR";
                var flexRestClient = new RestClient(flexurl);
                var flexRestRequest = new RestRequest("", Method.Get);
                flexRestRequest.AddHeader("X-Riot-Token", api);
                var flexRestResponse = await flexRestClient.ExecuteAsync(flexRestRequest);
                var flexLearboardEntries = JsonConvert.DeserializeObject<Leaderboard>(flexRestResponse.Content);

                leaderboards.Add(flexLearboardEntries);
            });

            await Task.WhenAll(soloQueue, flexQueue);

            return Ok(leaderboards);
        }

        /// <summary>
        /// Putting on a hold
        /// </summary>
        /// <param name="region"></param>
        /// <param name="matchID"></param>
        /// <returns></returns>
        [HttpGet("GetMatchTimeLine/{matchID}/{region}")]
        public async Task<ActionResult<Timeline>> GetTimeLine(string matchID, string region)
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
                "ru" => "europe",
                "me" => "europe"
            };

            var timelineUrl = new RestClient($"https://{mmRegion}.api.riotgames.com/lol/match/v5/matches/{matchID}/timeline");
            var timelineRequest = new RestRequest("", Method.Get);
            timelineRequest.AddHeader("X-Riot-Token", api);
            var timelineRestResponse = await timelineUrl.ExecuteAsync(timelineRequest);
            var timelineResponse = JsonConvert.DeserializeObject<Timeline>(timelineRestResponse.Content);

            return timelineResponse;

        }

        [HttpGet("GetItems")]
        public async Task<ActionResult<List<ItemDescriptions>>> GetItems() 
        {
            var itemUrl = new RestClient("https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/v1/items.json");
            var itemRequest = new RestRequest("", Method.Get);
            var itemRestResponse = await itemUrl.ExecuteAsync(itemRequest);
            var itemResponse = JsonConvert.DeserializeObject<List<AllItems>>(itemRestResponse.Content);

            var listOfItems = new List<ItemDescriptions>();

            foreach (var item in itemResponse)
            {

                if (item.inStore == true && 
                    !item.iconPath.Contains("Strawberry") &&
                    item.requiredChampion == "" &&
                    item.id.ToString().Length == 4 &&
                    item.displayInItemSets == true)
                {

                    var itemFrom = new List<int>();
                    var itemTo = new List<int>();

                    foreach (var subItemId in item.from)
                    {
                        var fromSubItemId = itemResponse.FirstOrDefault(x => x.id == subItemId);

                        if (fromSubItemId != null &&
                            fromSubItemId.inStore == true &&
                            !fromSubItemId.iconPath.Contains("Strawberry") &&
                            fromSubItemId.requiredChampion == "" &&
                            fromSubItemId.id.ToString().Length == 4 &&
                            fromSubItemId.displayInItemSets == true)
                        {
                            itemFrom.Add(fromSubItemId.id);
                        }
                    }

                    foreach (var subItemId in item.to)
                    {
                        var toSubItemId = itemResponse.FirstOrDefault(x => x.id == subItemId);

                        if (toSubItemId != null &&
                            toSubItemId.inStore == true &&
                            !toSubItemId.iconPath.Contains("Strawberry") &&
                            toSubItemId.requiredChampion == "" &&
                            toSubItemId.id.ToString().Length == 4 &&
                            toSubItemId.displayInItemSets == true)
                        {
                            itemTo.Add(toSubItemId.id);
                        }
                    }

                    var items = new ItemDescriptions
                    {
                        ItemID = item.id,
                        ItemName = item.name,
                        ItemDetail = item.description,
                        IsActive = item.active,
                        Price = item.price,
                        PriceTotal = item.priceTotal,   
                        BuildFrom = itemFrom,
                        BuildTo = itemTo,
                        ItemCategories = item.categories
                    };
                    listOfItems.Add(items);
                }
            }

            return listOfItems.OrderBy(x => x.PriceTotal)
                              .ThenBy(x => x.ItemCategories.Contains("Boots"))
                              .ToList();
        }

        [HttpGet("CheckServerStatus")]
        public async Task<ActionResult<List<LOLServerStatus>>> CheckServerStatus()
        {
            List<string> allServers = new List<string>() 
            {
                "BR1", "EUN1", "EUW1", "JP1", "KR", "LA1", "LA2",
                "ME1", "NA1", "OC1", "PBE1", "RU", "SG2", "TR1",
                "VN2"
            };

            var tasks = allServers.Select(async server =>
            {
                var serverUrl = new RestClient($"https://{server}.api.riotgames.com/lol/status/v4/platform-data");
                var serverRequest = new RestRequest("", Method.Get);
                serverRequest.AddHeader("X-Riot-Token", api);
                var serverRestResponse = await serverUrl.ExecuteAsync(serverRequest);
                var serverResponse = JsonConvert.DeserializeObject<LOLServerStatusDTO>(serverRestResponse.Content);

                return new LOLServerStatus
                {
                    RegionName = server,
                    ServerStatus = new LOLServerStatusDTO
                    {
                        id = serverResponse.id,
                        name = serverResponse.name,
                        //Shows only the english versions and not the other languages, maybe later if people use my site I will show all?
                        locales = serverResponse.locales.Where(x => x == "en_US").ToList(),
                        maintenances = serverResponse.maintenances?
                                        .Select(m => new Maintenance
                                        {
                                            id = m.id,
                                            created_at = m.created_at,
                                            updated_at = m.updated_at,
                                            archive_at = m.archive_at,
                                            maintenance_status = m.maintenance_status,
                                            incident_severity = m.incident_severity,
                                            platforms = m.platforms,
                                            titles = m.titles
                                                    .Where(t => t.locale == "en_US")
                                                    .ToList(),
                                            updates = m.updates?
                                                .Select(u => new Update
                                                {
                                                    id = u.id,
                                                    created_at = u.created_at,
                                                    updated_at = u.updated_at,
                                                    publish = u.publish,
                                                    author = u.author,
                                                    publish_locations = u.publish_locations,
                                                    translations = u.translations
                                                                    .Where(tr => tr.locale == "en_US")
                                                                    .ToList()
                                                })
                                                .ToList()
                                        })
                                        .ToList(),

                        incidents = serverResponse.incidents?
                                        .Select(i => new Incident
                                        {
                                            id = i.id,
                                            created_at = i.created_at,
                                            updated_at = i.updated_at,
                                            archive_at = i.archive_at,
                                            platforms = i.platforms,
                                            maintenance_status = i.maintenance_status,
                                            incident_severity = i.incident_severity,
                                            titles = i.titles
                                                    .Where(t => t.locale == "en_US")
                                                    .ToList(),
                                            updates = i.updates?
                                                .Select(u => new Update
                                                {
                                                    id = u.id,
                                                    created_at = u.created_at,
                                                    updated_at = u.updated_at,
                                                    publish = u.publish,
                                                    author = u.author,
                                                    publish_locations = u.publish_locations,
                                                    translations = u.translations
                                                                .Where(tr => tr.locale == "en_US")
                                                                .ToList()
                                                })
                                                .ToList()
                                        })
                                        .ToList()
                    }
                };

            });

            var serverStatusList = (await Task.WhenAll(tasks)).ToList();

            return Ok(serverStatusList);
            //foreach (var item in allServers) 
            //{
            //    var serverUrl = new RestClient($"https://{item}.api.riotgames.com/lol/status/v4/platform-data");
            //    var serverRequest = new RestRequest("", Method.Get);
            //    serverRequest.AddHeader("X-Riot-Token", api);
            //    var serverRestResponse = await serverUrl.ExecuteAsync(serverRequest);
            //    var serverResponse = JsonConvert.DeserializeObject<LOLServerStatusDTO>(serverRestResponse.Content);

            //    LOLServerStatus serverStatus = new LOLServerStatus
            //    {
            //        RegionName = item,
            //        ServerStatus = serverResponse
            //    };

            //    serverStatusList.Add(serverStatus);
            //}

        }

        [HttpPost("AddChampionHelp")]
        public async Task<ActionResult<string>> AddChampionHelp(string championName, [FromBody]ChampionHelpTextDTO championText) 
        {
            try 
            {
                if (championText == null || string.IsNullOrEmpty(championText.HelpText)) 
                {
                    return BadRequest("Empty/Value missing");
                }

                // Reference to the champion node
                CollectionReference championCollection = _firestoreDb.Collection("championHelp").Document(championName).Collection("entries");

                // Add a new help entry
                await championCollection.AddAsync(new
                {
                    championText.Author,
                    championText.HelpText,
                    CreatedTime = Timestamp.GetCurrentTimestamp()
                });

                return Ok("Champion help added successfully!");
            }
            catch(Exception ex) 
            {
                return BadRequest($"Error: {ex.Message}");
            }
            
        }

        [HttpGet("GetChampionHelp/{championName}")]
        public async Task<ActionResult<List<object>>> GetChampionHelp(string championName)
        {
            // Reference the subcollection for this champion
            var championCollection = _firestoreDb.Collection("championHelp")
                                                 .Document(championName)
                                                 .Collection("entries");

            // Get all documents in the subcollection
            var snapshot = await championCollection.GetSnapshotAsync();
            if (snapshot.Count == 0) 
            {
                //I had no content but it didnt work cause the way my frontend worked broke it, so I just returned an empty object
                //to my frontend so my .parse can still work
                return Ok(new List<object>());
            }

            var results = snapshot.Documents.Select(x => x.ToDictionary()).ToList();

            return Ok(results);
                
        }

        [HttpGet("GetSimpleAccountDetails")]
        public async Task<ActionResult<SimpleAccountDetails>> SimpleAccountDetails(string gameName, string tagLine, string region) 
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
                "ru" => "europe",
                "me" => "europe"
            };

            // Fetch data needed for account details
            var riotIdUrl = $"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}";
            var riotClient = new RestClient(riotIdUrl);
            var riotRequest = new RestRequest("", Method.Get);
            riotRequest.AddHeader("X-Riot-Token", api);
            var riotResponse = await riotClient.ExecuteAsync(riotRequest);
            var riotAccount = JsonConvert.DeserializeObject<NewRiotAccount>(riotResponse.Content);

            if (riotAccount?.puuid == null)
            {
                return NotFound("Account not found");
            }

            // Get player account details
            var playerTask = Task.Run(async () =>
            {
                var accountUrl = new RestClient($"https://{region}.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{riotAccount.puuid}");
                var accountRequest = new RestRequest("", Method.Get);
                accountRequest.AddHeader("X-Riot-Token", api);
                var response = await accountUrl.ExecuteAsync(accountRequest);
                return JsonConvert.DeserializeObject<RiotAccount>(response.Content);
            });

            /*Get player account name
            var accountNameTask = Task.Run(async () =>
            {
                var newUrl = new RestClient($"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
                var newRequest = new RestRequest("", Method.Get);
                newRequest.AddHeader("X-Riot-Token", api);
                var newRestResponse = await newUrl.ExecuteAsync(newRequest);
                return JsonConvert.DeserializeObject<NewRiotAccount>(newRestResponse.Content);
            });
            */
            //Get player ranked details
            var rankedTask = Task.Run(async () =>
            {
                var rankedUrl = new RestClient($"https://{region}.api.riotgames.com/lol/league/v4/entries/by-puuid/{riotAccount.puuid}");
                var rankedRequest = new RestRequest("", Method.Get);
                rankedRequest.AddHeader("X-Riot-Token", api);
                var response = await rankedUrl.ExecuteAsync(rankedRequest);
                return JsonConvert.DeserializeObject<List<RiotRanked>>(response.Content);
            });

            await Task.WhenAll(playerTask, rankedTask/*, accountNameTask*/);

            var account = playerTask.Result;
            var ranked = rankedTask.Result;
            //var accountDetails = accountNameTask.Result;

            if (account?.puuid == null)
            {
                return NotFound("Account not found");
            }

            return new SimpleAccountDetails
            {
                gameName = $"{riotAccount.gameName}#{riotAccount.tagLine}",
                summonerLevel = account.summonerLevel,
                profileIconId = account.profileIconId,
                SoloRank = ranked.FirstOrDefault(r => r?.queueType == "RANKED_SOLO_5x5") is var solo && solo != null
                           ? $"{solo.tier} {solo.rank} ({solo.leaguePoints} LP)"
                           : "Unranked",
                FlexRank = ranked.FirstOrDefault(r => r?.queueType == "RANKED_FLEX_SR") is var flex && flex != null
                           ? $"{flex.tier} {flex.rank} ({flex.leaguePoints} LP)"
                           : "Unranked",
            };

        }
        
        [HttpGet("IsPlayerInGame")]
        public async Task<ActionResult<LiveGameDTO>> IsPlayerInGame(string gameName, string tagLine, string region)
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
                "ru" => "europe",
                "me" => "europe"
            };

            var patchUrl = new RestClient("https://ddragon.leagueoflegends.com/api/versions.json");
            var patchRequest = new RestRequest("", Method.Get);
            var patchRestResponse = await patchUrl.ExecuteAsync(patchRequest);
            var patchResponse = JsonConvert.DeserializeObject<List<string>>(patchRestResponse.Content).FirstOrDefault();
            
            var championUrl = new RestClient($"https://ddragon.leagueoflegends.com/cdn/{patchResponse}/data/en_US/champion.json");
            var championRequest = new RestRequest("", Method.Get);
            var championRestResponse = await championUrl.ExecuteAsync(championRequest);
            var championResponse = JsonConvert.DeserializeObject<Champions>(championRestResponse.Content);
            
            var riotIdUrl = $"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}";
            var riotClient = new RestClient(riotIdUrl);
            var riotRequest = new RestRequest("", Method.Get);
            riotRequest.AddHeader("X-Riot-Token", api);
            var riotResponse = await riotClient.ExecuteAsync(riotRequest);
            var riotAccount = JsonConvert.DeserializeObject<NewRiotAccount>(riotResponse.Content);

            var liveUrl = $"https://{region}.api.riotgames.com/lol/spectator/v5/active-games/by-summoner/{riotAccount.puuid}";
            var liveClient = new RestClient(liveUrl);
            var liveRequest = new RestRequest("", Method.Get);
            liveRequest.AddHeader("X-Riot-Token", api);
            var liveResponse = await liveClient.ExecuteAsync(liveRequest);
            var liveAccount = JsonConvert.DeserializeObject<LiveGame>(liveResponse.Content);

            if (liveAccount.gameId == 0) 
            {
                return NotFound("Not in live game");
            }

            LiveGameDTO LiveGame = new LiveGameDTO
            {

                GameID = liveAccount.gameId.ToString(),
                MapID = liveAccount.mapId.ToString(),
                gameMode = liveAccount.gameMode,
                gameType = liveAccount.gameType,
                gameQueueConfigId = liveAccount.gameQueueConfigId.ToString(),
                participants = liveAccount.participants.Select(p =>
                {
                    var champPlayed = championResponse?.data.Values.FirstOrDefault(i => i.key == p.championId.ToString());
                    string champName = champPlayed?.id;

                    if (champName == "Wukong") champName = "MonkeyKing";
                    else if (champName == "FiddleSticks") champName = "Fiddlesticks";

                    return new ParticipantDTO
                    {
                        teamId = p.teamId.ToString(),
                        spell1Id = p.spell1Id.ToString(),
                        spell2Id = p.spell2Id.ToString(),
                        championId = champName,
                        profileIconId = p.profileIconId.ToString(),
                        riotId = p.riotId,
                        bot = p.bot
                    };
                }).ToList()
            };

            return LiveGame;

        }

        [HttpGet("GetAllChampions")]
        public async Task<ActionResult<List<AllChampionsDTO>>> GetAllChampions() 
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

            List<AllChampionsDTO> allChampions = new List<AllChampionsDTO>();

            foreach (var item in championResponse.data)
            {
                allChampions.Add(new AllChampionsDTO
                {
                    Name = item.Value.name,
                    Image = item.Value.id
                });
            }

            //AllChampionsDTO allChampions = new AllChampionsDTO
            //{
            //    Name = string.Join(",", championResponse.data.Keys),
            //    Image = string.Join(",", championResponse.data.Values.Select(c => c.image.full))
            //};

            return allChampions;
        }

    }

}