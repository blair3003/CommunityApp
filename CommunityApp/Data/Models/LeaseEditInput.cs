using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Data.Models
{
    public class LeaseEditInput : IValidatableObject
    {
        [Required(ErrorMessage = "Tenant Name is required")]
        [StringLength(100, ErrorMessage = "Tenant Name cannot be longer than 100 characters")]
        public string? TenantName { get; set; }

        [Required(ErrorMessage = "Tenant Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? TenantEmail { get; set; }

        [Required(ErrorMessage = "Tenant Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? TenantPhone { get; set; }

        [Required(ErrorMessage = "Monthly Payment is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Monthly Payment must be a positive number")]
        public decimal MonthlyPayment { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Deposit Amount must be a positive number")]
        public decimal? DepositAmount { get; set; }

        [Required(ErrorMessage = "Payment Due Day is required")]
        [Range(1, 31, ErrorMessage = "Payment Due Day must be between 1 and 31")]
        public int PaymentDueDay { get; set; }

        [Required(ErrorMessage = "Lease Start Date is required")]
        public DateTime LeaseStartDate { get; set; }

        [Required(ErrorMessage = "Lease End Date is required")]
        public DateTime LeaseEndDate { get; set; }

        public string? Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LeaseEndDate <= LeaseStartDate)
            {
                yield return new ValidationResult(
                    "Lease End Date must be after Lease Start Date.",
                    [nameof(LeaseEndDate)]
                );
            }
        }
    }
}
