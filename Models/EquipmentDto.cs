namespace StatSanctum.Models
{
    public class EquipmentDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Level { get; set; }
        public EquipmentType Type { get; set; }
        public EquipmentDto(string name, string? description, int level, EquipmentType type)
        {
            Name = name;
            Description = description;
            Level = level;
            Type = type;
        }
    }
}
