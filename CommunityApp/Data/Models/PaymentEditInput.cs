using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Data.Models
{
    public class PaymentEditInput
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment Date is required")]
        public DateTime PaymentDate { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }
    }
}
