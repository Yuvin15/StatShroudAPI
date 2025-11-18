namespace API.Models
{
    public class Challenges
    {
        public RiotChallenges.TotalPoints totalPoints { get; set; }
        public RiotChallenges.CategoryPoints categoryPoints { get; set; }
        public ChallengeList challengeList { get; set; }
    }

    public class ChallengeList
    {
        public string? id { get; set; }
        public string? name  { get; set; }
        public string? description   { get; set; }
        public string? level { get; set; }
        public string? value { get; set; }
        public string? percentile    { get; set; }
        public string? position  { get; set; }
        public string? playersInLevel    { get; set; }
    }
}
