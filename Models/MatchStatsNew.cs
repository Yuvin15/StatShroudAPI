namespace API.Models
{
    public class MatchStatsNew
    {
        public string GameID { get; set; }
        public string GameWinner { get; set; }
        public string GameMode { get; set; }
        public int totalBlueKills { get; set; }
        public int totalRedKills { get; set; }
        //public string WinningTeam { get; set; }
        public List<PlayerMatchDetails> Players{ get; set; }
    }
}
