using Newtonsoft.Json;

namespace API.Models
{
    public class TopPlayed
    {
        public string puuid { get; set; }
        public int championId { get; set; }
        public int championLevel { get; set; }
        public int championPoints { get; set; }
        public object lastPlayTime { get; set; }
        public int championPointsSinceLastLevel { get; set; }
        public int championPointsUntilNextLevel { get; set; }
        public int markRequiredForNextLevel { get; set; }
        public int tokensEarned { get; set; }
        public int championSeasonMilestone { get; set; }
        public NextSeasonMilestone nextSeasonMilestone { get; set; }
    }
    public class NextSeasonMilestone
    {
        public RequireGradeCounts requireGradeCounts { get; set; }
        public int rewardMarks { get; set; }
        public bool bonus { get; set; }
        public int totalGamesRequires { get; set; }
    }

    public class RequireGradeCounts
    {
        [JsonProperty("A-")]
        public int A { get; set; }
    }
}
