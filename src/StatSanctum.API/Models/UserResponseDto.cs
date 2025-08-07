using StatSanctum.Models;

namespace StatSanctum.API.Models
{
    public class UserResponseDto
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}
