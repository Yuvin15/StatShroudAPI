namespace API.Models
{
    public class ChampionDetails
    {
        public string championPassive { get; set; }
        public string Passive_SpellName { get; set; }
        public string Passive_Description { get; set; }

        //Q
        public string ChampionQ { get; set; }
        public string Q_SpellName { get; set; }
        public string Q_Description { get; set; }

        //W
        public string ChampionW { get; set; }
        public string W_SpellName { get; set; }
        public string W_Description { get; set; }

        //E
        public string ChampionE { get; set; }
        public string E_SpellName { get; set; }
        public string E_Description { get; set; }

        //R
        public string ChampionR { get; set; }
        public string R_SpellName { get; set; }
        public string R_Description { get; set; }

        //Other stuff
        public string ChampName { get; set; }
        public string ChampionLore { get; set; }
        public List<Skins> ChampSkins { get; set; }

        //Extras
        public int Hp { get; set; }
        public int HpPerLevel { get; set; }
        public int Mp { get; set; }
        public int MpPerLevel { get; set; }
        public int MoveSpeed { get; set; }
        public int Armor { get; set; }
        public double ArmorPerLevel { get; set; }
        public int SpellBlock { get; set; }
        public double SpellBlockPerLevel { get; set; }
        public int AttackRange { get; set; }
        public double HpRegen { get; set; }
        public double HpRegenPerLevel { get; set; }
        public double MpRegen { get; set; }
        public double MpRegenPerLevel { get; set; }
        public int Crit { get; set; }
        public int CritPerLevel { get; set; }
        public int AttackDamage { get; set; }
        public int AttackDamagePerLevel { get; set; }
        public double AttackSpeedPerLevel { get; set; }
        public double AttackSpeed { get; set; }

    }
}
