namespace API.Models
{
    public class RiotAccountDetails
    {
        public string gameName { get; set; }
        public Dictionary<string, double> recentGamesWinRate { get; set; }
        public int summonerLevel { get; set; }
        public string SoloRank { get; set; }
        public string FlexRank { get; set; }
        public int profileIconId { get; set; }
        public PlayerAchievments? Achievments { get; set; }
        public List<PlayerMatchHistory> BasicMatchDetails { get; set; }
    }
}
