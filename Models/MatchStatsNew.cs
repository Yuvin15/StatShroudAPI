namespace API.Models
{
    public class MatchStatsNew
    {
        public string GameID { get; set; }
        public string GameWinner { get; set; }
        public string GameMode { get; set; }
        public string GameTime { get; set; }
        public int TotalBlueKills { get; set; }
        public int TotalRedKills { get; set; }
        public int TotalBlueDragonKills { get; set; }
        public int TotalRedDragonKills { get; set; }
        public int TotalBlueBaronKills { get; set; }
        public int TotalRedBaronKills { get; set; }
        public int TotalBlueTurrets { get; set; }
        public int TotalRedTurrets { get; set; }
        public int TotalBlueInhib { get; set; }
        public int TotalRedInhib { get; set; }
        public int BlueHearldKills { get; set; }
        public int RedHearldKills { get; set; }
        public int BlueAtakhanKills { get; set; }
        public int RedAtakhanKills { get; set; }
        public List<string>? BlueTeamBans { get; set; }
        public List<string>? RedTeamBans { get; set; }
        public List<PlayerMatchDetails> Players{ get; set; }
    }
}
