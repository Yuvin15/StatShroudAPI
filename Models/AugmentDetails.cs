namespace API.Models
{
    public class AugmentDetails
    {
        public List<Augment> augments { get; set; }

        public class Augment
        {
            public string apiName { get; set; }
            public Dictionary<string, Calculation> calculations { get; set; }
            public Dictionary<string, double> dataValues { get; set; }
            public string desc { get; set; }
            public string iconLarge { get; set; }
            public string iconSmall { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int rarity { get; set; }
            public string tooltip { get; set; }
        }

        public class Calculation
        {
            public string __type { get; set; }
            public List<FormulaPart> mFormulaParts { get; set; }
            public Multiplier mMultiplier { get; set; }
        }

        public class FormulaPart
        {
            public string __type { get; set; }
            public string mDataValue { get; set; }
            public int? mStat { get; set; }
        }

        public class Multiplier
        {
            public string __type { get; set; }
            public double mNumber { get; set; }
        }
    }
}
