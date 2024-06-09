using System;
using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Data.Models
{
    public class LeaseCreateInput : IValidatableObject
    {
        [Required]
        public int HomeId { get; set; }

        [Display(Name = "Tenant Name")]
        [Required(ErrorMessage = "Tenant Name is required")]
        [StringLength(100, ErrorMessage = "Tenant Name cannot be longer than 100 characters")]
        public string? TenantName { get; set; }

        [Display(Name = "Tenant Email")]
        [Required(ErrorMessage = "Tenant Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? TenantEmail { get; set; }

        [Display(Name = "Tenant Phone")]
        [Required(ErrorMessage = "Tenant Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? TenantPhone { get; set; }

        [Display(Name = "Monthly Payment")]
        [Required(ErrorMessage = "Monthly Payment is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Monthly Payment must be a positive number")]
        public decimal MonthlyPayment { get; set; }

        [Display(Name = "Deposit Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Deposit Amount must be a positive number")]
        public decimal? DepositAmount { get; set; }

        [Display(Name = "Payment Due Date")]
        [Required(ErrorMessage = "Payment Due Day is required")]
        [Range(1, 31, ErrorMessage = "Payment Due Day must be between 1 and 31")]
        public int PaymentDueDay { get; set; } = 1;

        [Display(Name = "Lease Start Date")]
        [Required(ErrorMessage = "Lease Start Date is required")]
        public DateTime LeaseStartDate { get; set; } = DateTime.Now;

        [Display(Name = "Lease End Date")]
        [Required(ErrorMessage = "Lease End Date is required")]
        public DateTime LeaseEndDate { get; set; } = DateTime.Now.AddYears(1);

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
