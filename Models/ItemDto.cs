namespace StatSanctum.Models
{
    public class ItemDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Level { get; set; }
        public ItemType Type { get; set; }
        public ItemDto(string name, string? description, int level, ItemType type)
        {
            Name = name;
            Description = description;
            Level = level;
            Type = type;
        }
    }
}
