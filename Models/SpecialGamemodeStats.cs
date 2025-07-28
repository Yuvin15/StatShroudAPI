namespace API.Models
{
    public class SpecialGamemodeStats
    {
        public string GameID { get; set; }
        public string GameMode { get; set; }
        public List<SpecialPlayerDetails> SpecialGamePlayerStats { get; set; }
    }
}
