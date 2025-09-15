using System.Text.Json.Serialization;

namespace API.Models
{
    public class AllItems
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
        public bool inStore { get; set; }
        public List<int> from { get; set; }
        public List<int> to { get; set; }
        public List<string> categories { get; set; }
        public int maxStacks { get; set; }
        public string requiredChampion { get; set; }
        public string requiredAlly { get; set; }
        public string requiredBuffCurrencyName { get; set; }
        public int requiredBuffCurrencyCost { get; set; }
        public int specialRecipe { get; set; }
        public bool isEnchantment { get; set; }
        public int price { get; set; }
        public int priceTotal { get; set; }
        public bool displayInItemSets { get; set; }
        public string iconPath { get; set; }
    }        
}
