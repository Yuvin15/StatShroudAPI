namespace API.Models
{
    public class PlayerMatchHistory
    {
        public string MatchID { get; set; }
        public string GameWinner { get; set; }
        public string GameMode { get; set; }
        public string MatchDuration { get; set; }
        public string ChampionName { get; set; }
        public string Lane { get; set; }
        public string KDA { get; set; }
        public int Farm { get; set; }

    }
}
