using StatSanctum.Helpers;
using StatSanctum.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatSanctum.Entities
{
    public class Item : Base
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int Level { get; set; }
        public Rarity? Rarity { get; set; }
        public int RarityID { get; set; }
    }
}
