namespace API.Models
{
    public class PlayerMatchDetails
    {
        public string PlayerName { get; set; }
        public string TeamID { get; set; }
        public string ChampionName { get; set; }
        public string LaneName { get; set; }
        public string KDA { get; set; }
        public int GoldSpent { get; set; } 
        public int GoldEarned { get; set; }
        public int Damage { get; set; }
        public int DamageTaken { get; set; }
        public int TowerDamage { get; set; }
        public int ObjDamage { get; set; }
        public int SkillshotsHit { get; set; }
        public int SkillshotsMissed { get; set; }
        public int Farm { get; set; }
        public double HealShield { get; set; }
        public Items PlayerItems { get; set; }
        public Runes? Runes { get; set; }
    }
}
