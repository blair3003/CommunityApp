using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Data.Models
{
    public class Home
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CommunityId { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public Type? Type { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        [DisplayName("Square Footage")]
        public int SqFt { get; set; }
        [DisplayName("Year Built")]
        public int YearBuilt { get; set; }
        [DisplayName("Year Renovated")]
        public int YearRenovated { get; set; }
        [DisplayName("Is Furnished?")]
        public bool IsFurnished { get; set; }
        [DisplayName("Has Parking?")]
        public bool HasParking { get; set; }
        [DisplayName("Parking Details")]
        public string? ParkingDetails { get; set; }
        [DisplayName("Security Details")]
        public string? SecurityDetails { get; set; }
        [DisplayName("Utilities Details")]
        public string? UtilitiesDetails { get; set; }
        [DisplayName("Accessibility Details")]
        public string? AccessibilityDetails { get; set; }
        [DisplayName("Pet Details")]
        public string? PetDetails { get; set; }
        [DisplayName("Other Details")]
        public string? OtherDetails { get; set; }
        public Laundry? Laundry { get; set; }
        public Heating? Heating { get; set; }
        public Cooling? Cooling { get; set; }
        [DisplayName("Outdoor Space")]
        public OutdoorSpace? OutdoorSpace { get; set; }
        [DisplayName("Base Rent")]
        public decimal BaseRent { get; set; }
        [DisplayName("Base Deposit")]
        public decimal BaseDeposit { get; set; }

        public Community? Community { get; set; }
    }

    public enum Type { Apartment, House, Condo, Townhouse, Duplex }
    public enum Laundry { InUnit, [Display(Name = "On-site")] OnSite, Other, None }
    public enum Heating { Central, Electric, Gas, Other, None }
    public enum Cooling { Central, [Display(Name = "Window Unit")] WindowUnit, Other, None }
    public enum OutdoorSpace { Balcony, Patio, Yard, Other, None }
}
