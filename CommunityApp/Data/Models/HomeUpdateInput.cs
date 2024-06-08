using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CommunityApp.Data.Models
{
    public class HomeUpdateInput
    {
        [Range(0, 100)]
        public int Floor { get; set; }

        [StringLength(10)]
        public string? Number { get; set; }

        [StringLength(100)]
        public string? Street { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? State { get; set; }

        [StringLength(10)]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid ZIP code format.")]
        public string? Zip { get; set; }

        [StringLength(50)]
        public string? Country { get; set; }

        [EnumDataType(typeof(Type))]
        public Type? Type { get; set; }

        [Range(0, 20)]
        public int Bedrooms { get; set; }

        [Range(0, 20)]
        public int Bathrooms { get; set; }

        [DisplayName("Square Footage")]
        [Range(0, 100000)]
        public int SqFt { get; set; }

        [DisplayName("Year Built")]
        [Range(1800, 2100)]
        public int YearBuilt { get; set; }

        [DisplayName("Year Renovated")]
        [Range(1800, 2100)]
        public int YearRenovated { get; set; }

        [DisplayName("Is Furnished?")]
        public bool IsFurnished { get; set; }

        [DisplayName("Has Parking?")]
        public bool HasParking { get; set; }

        [StringLength(200)]
        [DisplayName("Parking Details")]
        public string? ParkingDetails { get; set; }

        [StringLength(200)]
        [DisplayName("Security Details")]
        public string? SecurityDetails { get; set; }

        [StringLength(200)]
        [DisplayName("Utilities Details")]
        public string? UtilitiesDetails { get; set; }

        [StringLength(200)]
        [DisplayName("Accessibility Details")]
        public string? AccessibilityDetails { get; set; }

        [StringLength(200)]
        [DisplayName("Pet Details")]
        public string? PetDetails { get; set; }

        [StringLength(200)]
        [DisplayName("Other Details")]
        public string? OtherDetails { get; set; }

        [EnumDataType(typeof(Laundry))]
        public Laundry? Laundry { get; set; }

        [EnumDataType(typeof(Heating))]
        public Heating? Heating { get; set; }

        [EnumDataType(typeof(Cooling))]
        public Cooling? Cooling { get; set; }

        [DisplayName("Outdoor Space")]
        [EnumDataType(typeof(OutdoorSpace))]
        public OutdoorSpace? OutdoorSpace { get; set; }

        [DisplayName("Base Rent")]
        [Range(0, double.MaxValue, ErrorMessage = "Base Rent must be a positive number.")]
        public decimal BaseRent { get; set; }

        [DisplayName("Base Deposit")]
        [Range(0, double.MaxValue, ErrorMessage = "Base Deposit must be a positive number.")]
        public decimal BaseDeposit { get; set; }
    }
}
