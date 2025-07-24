namespace API.Models
{
    public class RiotChallenges
    {
        public TotalPoints totalPoints { get; set; }
        public CategoryPoints categoryPoints { get; set; }
        public List<Challenge> challenges { get; set; }
        public Preferences preferences { get; set; }

        public class CategoryPoints
        {
            public IMAGINATION IMAGINATION { get; set; }
            public COLLECTION COLLECTION { get; set; }
            public VETERANCY VETERANCY { get; set; }
            public EXPERTISE EXPERTISE { get; set; }
            public TEAMWORK TEAMWORK { get; set; }
        }

        public class Challenge
        {
            public string challengeId { get; set; }
            public double percentile { get; set; }
            public string level { get; set; }
            public string value { get; set; }
            public object achievedTime { get; set; }
            public string? position { get; set; }
            public string? playersInLevel { get; set; }
        }

        public class COLLECTION
        {
            public string level { get; set; }
            public string current { get; set; }
            public string max { get; set; }
            public double percentile { get; set; }
        }

        public class EXPERTISE
        {
            public string level { get; set; }
            public string current { get; set; }
            public string max { get; set; }
            public double percentile { get; set; }
        }

        public class IMAGINATION
        {
            public string level { get; set; }
            public string current { get; set; }
            public string max { get; set; }
            public double percentile { get; set; }
        }

        public class Preferences
        {
            public string bannerAccent { get; set; }
            public string title { get; set; }
            public List<string> challengeIds { get; set; }
            public string crestBorder { get; set; }
            public string prestigeCrestBorderLevel { get; set; }
        }

        public class TEAMWORK
        {
            public string level { get; set; }
            public string current { get; set; }
            public string max { get; set; }
            public double percentile { get; set; }
        }

        public class TotalPoints
        {
            public string level { get; set; }
            public string current { get; set; }
            public string max { get; set; }
            public double percentile { get; set; }
        }

        public class VETERANCY
        {
            public string level { get; set; }
            public string current { get; set; }
            public string max { get; set; }
            public double percentile { get; set; }
        }
    }
}