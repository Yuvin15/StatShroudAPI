using System;
using static API.Models.MatchData;

namespace API.Models
{
    public class LiveGame
    {
        public int gameId { get; set; }
        public int mapId { get; set; }
        public string gameMode { get; set; }
        public string gameType { get; set; }
        public int gameQueueConfigId { get; set; }
        public List<Participant> participants { get; set; }
        public Observers observers { get; set; }
        public string platformId { get; set; }
        public List<object> bannedChampions { get; set; }
        public long gameStartTime { get; set; }
        public int gameLength { get; set; }
    }
    public class Observers
    {
        public string encryptionKey { get; set; }
    }

    public class Participant
    {
        public string puuid { get; set; }
        public int teamId { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public int championId { get; set; }
        public int profileIconId { get; set; }
        public string riotId { get; set; }
        public bool bot { get; set; }
        public List<object> gameCustomizationObjects { get; set; }
        public Perks perks { get; set; }
    }

    public class Perks
    {
        public List<int> perkIds { get; set; }
        public int perkStyle { get; set; }
        public int perkSubStyle { get; set; }
    }
}
