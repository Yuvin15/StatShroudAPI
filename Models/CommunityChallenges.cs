namespace API.Models
{
    public class CommunityChallenges
    {
        public Dictionary<string, CDragonChallenge> challenges { get; set; }
    }
    public class CDragonChallenge
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
