namespace API.Models
{
    public class SpecialPlayerDetails
    {
        public string PlayerName { get; set; }
        public string ChampionName { get; set; }
        public string PlayerTeamName { get; set; }
        public int PlayerTeamPosition { get; set; }
        public string KDA { get; set; }
        public int Damage { get; set; }
        public int DamageTaken { get; set; }
        public int SkillshotsHit { get; set; }
        public int SkillshotsMissed { get; set; }
        public double HealShield { get; set; }
        public Items Items { get; set; }
        public string SummonerSpell1 { get; set; }
        public string SummonerSpell2 { get; set; }
        public Augments Augments { get; set; }

    }
}
