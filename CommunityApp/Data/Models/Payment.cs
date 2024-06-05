using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Data.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LeaseId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public string? Description { get; set; }

        public Lease? Lease { get; set; }
    }
}