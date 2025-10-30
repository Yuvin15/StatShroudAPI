namespace API.Models
{
    public class LiveGameDTO
    {
        public string GameID { get; set; }
        public string MapID { get; set; }
        public string gameMode { get; set; }
        public string gameType { get; set; }
        public string gameQueueConfigId { get; set; }
        public List<ParticipantDTO> participants { get; set; }

    }

    public class ParticipantDTO
    {
        public string teamId { get; set; }
        public string spell1Id { get; set; }
        public string spell2Id { get; set; }
        public string championId { get; set; }
        public string profileIconId { get; set; }
        public string riotId { get; set; }
        public bool bot { get; set; }
    }
}
