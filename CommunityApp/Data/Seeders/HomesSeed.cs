using CommunityApp.Data.Models;

namespace CommunityApp.Data.Seeders
{
    public class HomesSeed(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task InitializeAsync()
        {
            if (!_context.Homes.Any())
            {
                var communities = _context.Communities;

                foreach (var community in communities)
                {
                    var homes = new List<Home>
                    {
                        new Home
                        {
                            CommunityId = community.Id,
                            Floor = 1,
                            Number = "Unit 101",
                            Street = "Main Street",
                            City = "Cityville",
                            State = "Stateville",
                            Zip = "12345",
                            Country = "Countryland",
                            Type = Models.Type.Apartment,
                            Bedrooms = 2,
                            Bathrooms = 2,
                            SqFt = 1200,
                            YearBuilt = 2005,
                            YearRenovated = 2020,
                            IsFurnished = true,
                            HasParking = true,
                            ParkingDetails = "Garage",
                            SecurityDetails = "Secure access",
                            UtilitiesDetails = "Included",
                            AccessibilityDetails = "Wheelchair accessible",
                            PetDetails = "Pets allowed",
                            OtherDetails = "Additional details",
                            Laundry = Laundry.InUnit,
                            Heating = Heating.Central,
                            Cooling = Cooling.Central,
                            OutdoorSpace = OutdoorSpace.Balcony,
                            BaseRent = 1600,
                            BaseDeposit = 1000
                        },
                        new Home
                        {
                            CommunityId = community.Id,
                            Floor = 2,
                            Number = "Unit 202",
                            Street = "Main Street",
                            City = "Cityville",
                            State = "Stateville",
                            Zip = "12345",
                            Country = "Countryland",
                            Type = Models.Type.Apartment,
                            Bedrooms = 1,
                            Bathrooms = 1,
                            SqFt = 800,
                            YearBuilt = 2010,
                            YearRenovated = 2021,
                            IsFurnished = false,
                            HasParking = true,
                            ParkingDetails = "Open parking",
                            SecurityDetails = "Basic security",
                            UtilitiesDetails = "Excluded",
                            AccessibilityDetails = "Not accessible",
                            PetDetails = "No pets",
                            OtherDetails = "No additional details",
                            Laundry = Laundry.OnSite,
                            Heating = Heating.Electric,
                            Cooling = Cooling.WindowUnit,
                            OutdoorSpace = OutdoorSpace.Patio,
                            BaseRent = 1200,
                            BaseDeposit = 800
                        },
                        new Home
                        {
                            CommunityId = community.Id,
                            Floor = 1,
                            Number = "Unit 103",
                            Street = "Main Street",
                            City = "Cityville",
                            State = "Stateville",
                            Zip = "12345",
                            Country = "Countryland",
                            Type = Models.Type.House,
                            Bedrooms = 3,
                            Bathrooms = 2,
                            SqFt = 1800,
                            YearBuilt = 1995,
                            YearRenovated = 2018,
                            IsFurnished = true,
                            HasParking = true,
                            ParkingDetails = "Driveway",
                            SecurityDetails = "Gated community",
                            UtilitiesDetails = "Included",
                            AccessibilityDetails = "Accessible",
                            PetDetails = "Pets allowed with restrictions",
                            OtherDetails = "Fireplace included",
                            Laundry = Laundry.InUnit,
                            Heating = Heating.Central,
                            Cooling = Cooling.Central,
                            OutdoorSpace = OutdoorSpace.Yard,
                            BaseRent = 2200,
                            BaseDeposit = 1500
                        },
                        new Home
                        {
                            CommunityId = community.Id,
                            Floor = 3,
                            Number = "Unit 304",
                            Street = "Main Street",
                            City = "Cityville",
                            State = "Stateville",
                            Zip = "12345",
                            Country = "Countryland",
                            Type = Models.Type.Condo,
                            Bedrooms = 2,
                            Bathrooms = 2,
                            SqFt = 1400,
                            YearBuilt = 2008,
                            YearRenovated = 2022,
                            IsFurnished = true,
                            HasParking = true,
                            ParkingDetails = "Underground parking",
                            SecurityDetails = "24/7 security",
                            UtilitiesDetails = "Included",
                            AccessibilityDetails = "Wheelchair accessible",
                            PetDetails = "No pets",
                            OtherDetails = "Balcony with view",
                            Laundry = Laundry.InUnit,
                            Heating = Heating.Electric,
                            Cooling = Cooling.Central,
                            OutdoorSpace = OutdoorSpace.Balcony,
                            BaseRent = 1800,
                            BaseDeposit = 1200
                        },
                        new Home
                        {
                            CommunityId = community.Id,
                            Floor = 2,
                            Number = "Unit 205",
                            Street = "Main Street",
                            City = "Cityville",
                            State = "Stateville",
                            Zip = "12345",
                            Country = "Countryland",
                            Type = Models.Type.Townhouse,
                            Bedrooms = 4,
                            Bathrooms = 3,
                            SqFt = 2200,
                            YearBuilt = 2015,
                            YearRenovated = 2021,
                            IsFurnished = false,
                            HasParking = true,
                            ParkingDetails = "Garage",
                            SecurityDetails = "Keycard access",
                            UtilitiesDetails = "Excluded",
                            AccessibilityDetails = "Stairs only",
                            PetDetails = "Pets allowed",
                            OtherDetails = "Corner unit",
                            Laundry = Laundry.InUnit,
                            Heating = Heating.Central,
                            Cooling = Cooling.Central,
                            OutdoorSpace = OutdoorSpace.Patio,
                            BaseRent = 2500,
                            BaseDeposit = 1800
                        }
                    };

                    await _context.Homes.AddRangeAsync(homes);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
