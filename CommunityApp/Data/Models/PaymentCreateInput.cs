using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Data.Models
{
    public class PaymentCreateInput
    {
        [Required]
        public int LeaseId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Display(Name = "Payment Date")]
        [Required(ErrorMessage = "Payment Date is required")]
        public DateTime PaymentDate { get; set; } = DateTime.Today;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }
    }

}
