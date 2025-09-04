namespace API.Models
{
    public class ItemDescriptions
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemDetail { get; set; }
        public bool IsActive { get; set; }
        public int Price { get; set; }
        public int PriceTotal { get; set; }
        public string ItemImagePath { get; set; }
        public bool CanPurchase { get; set; }
        public List<int>? BuildFrom { get; set; }
        public List<int>? BuildTo { get; set; }
        public List<string> ItemCategories{ get; set; }
    }
}
