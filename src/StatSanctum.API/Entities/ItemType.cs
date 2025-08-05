using System.ComponentModel.DataAnnotations;
using StatSanctum.Helpers;

namespace StatSanctum.Entities
{
    public class ItemType : Base
    {
        public int ItemTypeID { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Image { get; set; }
    }
}
