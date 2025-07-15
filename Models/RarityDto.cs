namespace StatSanctum.Models
{
    public class RarityDto
    {
        public string Name { get; set; }
        public string Image { get; set; } = string.Empty;

        public RarityDto(string name, string image)
        {
            Name = name;
            Image = image;
        }
    }
}
