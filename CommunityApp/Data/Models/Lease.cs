using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Data.Models
{
    public class Lease
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HomeId { get; set; }
        public string? TenantId { get; set; }
        [Required]
        public string TenantName { get; set; } = string.Empty;
        [Required]
        public string TenantEmail { get; set; } = string.Empty;
        [Required]
        public string TenantPhone { get; set; } = string.Empty;
        [Required]
        public decimal MonthlyPayment { get; set; }
        public decimal? DepositAmount { get; set; }
        [Required]
        public int PaymentDueDay { get; set; }
        [Required]
        public DateTime LeaseStartDate { get; set; }
        [Required]
        public DateTime LeaseEndDate { get; set; }
        public string? Notes { get; set; }

        public bool IsActive => DateTime.Now >= LeaseStartDate && DateTime.Now <= LeaseEndDate;

        public Home? Home { get; set; }
        public ApplicationUser? Tenant { get; set; }
        public ICollection<Payment> Payments { get; set; } = [];
    }
}