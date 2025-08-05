namespace StatSanctum.Models
{
    public class ItemRarityDto
    {
        public int ItemID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Level { get; set; }
        public RarityDto? Rarity { get; set; }
    }
}
