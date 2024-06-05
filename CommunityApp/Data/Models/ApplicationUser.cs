using Microsoft.AspNetCore.Identity;

namespace CommunityApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Community> Communities { get; set; } = [];
        public ICollection<Lease> Leases { get; set; } = [];
    }
}
