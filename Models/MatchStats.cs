namespace API.Models
{
    public class MatchStats
    {
        public string GameID { get; set; }
        public string GameState { get; set; }
        public string PlayerName { get; set; }
        public string ChampionName { get; set; }
        public int ChampionSkin { get; set; }
        public string LaneName { get; set; }
        public string KDA { get; set; }
        public int Damage { get; set; }
        public int DamageTaken { get; set; }
    }
}
