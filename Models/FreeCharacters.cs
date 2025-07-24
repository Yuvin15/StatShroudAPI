namespace API.Models
{
    public class FreeCharacters
    {
        public List<int> freeChampionIds { get; set; }
        public List<int> freeChampionIdsForNewPlayers { get; set; }
        public int maxNewPlayerLevel { get; set; }
    }
}
