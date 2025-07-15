using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatSanctum.Entities
{
    public class Rarity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RarityID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}
