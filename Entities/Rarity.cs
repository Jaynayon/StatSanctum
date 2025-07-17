using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StatSanctum.Helpers;

namespace StatSanctum.Entities
{
    public class Rarity : Base
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RarityID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}
