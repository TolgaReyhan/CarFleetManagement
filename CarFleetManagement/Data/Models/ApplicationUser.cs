using Microsoft.AspNetCore.Identity;

namespace CarFleetManagement.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeleted { get; set; }
    }
} 