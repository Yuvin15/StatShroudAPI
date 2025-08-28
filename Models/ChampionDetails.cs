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

    }
}
