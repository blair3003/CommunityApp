using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace CommunityApp.Data.Repositories
{
    public class HomeRepository(ApplicationDbContext context) : IHomeRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Home>> GetAllAsync()
        {
            var allHomes = await _context.Homes.ToListAsync();
            return allHomes;
        }

        public async Task<Home?> GetByIdAsync(int id)
        {
            var home = await _context.Homes.FindAsync(id);
            return home;
        }

        public async Task<Home?> AddAsync(Home home)
        {
            await _context.Homes.AddAsync(home);
            await _context.SaveChangesAsync();
            return home;
        }

        public async Task<Home?> UpdateAsync(int id, Home home)
        {
            if (id != home.Id)
            {
                return null;
            }

            var existingHome = await _context.Homes.FindAsync(id);

            if (existingHome == null)
            {
                return null;
            }

            existingHome.Floor = home.Floor;
            existingHome.Number = home.Number;
            existingHome.Street = home.Street;
            existingHome.City = home.City;
            existingHome.State = home.State;
            existingHome.Zip = home.Zip;
            existingHome.Country = home.Country;
            existingHome.Type = home.Type;
            existingHome.Bedrooms = home.Bedrooms;
            existingHome.Bathrooms = home.Bathrooms;
            existingHome.SqFt = home.SqFt;
            existingHome.YearBuilt = home.YearBuilt;
            existingHome.YearRenovated = home.YearRenovated;
            existingHome.IsFurnished = home.IsFurnished;
            existingHome.HasParking = home.HasParking;
            existingHome.ParkingDetails = home.ParkingDetails;
            existingHome.SecurityDetails = home.SecurityDetails;
            existingHome.UtilitiesDetails = home.UtilitiesDetails;
            existingHome.AccessibilityDetails = home.AccessibilityDetails;
            existingHome.PetDetails = home.PetDetails;
            existingHome.OtherDetails = home.OtherDetails;
            existingHome.Laundry = home.Laundry;
            existingHome.Heating = home.Heating;
            existingHome.Cooling = home.Cooling;
            existingHome.OutdoorSpace = home.OutdoorSpace;
            existingHome.BaseRent = home.BaseRent;
            existingHome.BaseDeposit = home.BaseDeposit;

            await _context.SaveChangesAsync();
            return existingHome;
        }

        public async Task<Home?> DeleteAsync(int id)
        {
            var home = await _context.Homes.FindAsync(id);

            if (home == null)
            {
                return null;
            }

            _context.Homes.Remove(home);
            await _context.SaveChangesAsync();
            return home;
        }
    }
}
