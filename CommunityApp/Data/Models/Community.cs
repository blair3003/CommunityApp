using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Data.Models
{
    public class Community
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public ICollection<Home> Homes { get; set; } = [];
        public ICollection<ApplicationUser> Managers { get; set; } = [];
    }
}
