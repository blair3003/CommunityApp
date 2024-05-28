using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CommunityApp.Data.Repositories
{
    public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<List<UserDto>> GetAllAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var allUsersWithClaims = new List<UserDto>();

            foreach (var user in allUsers)
            {
                var claims = await _userManager.GetClaimsAsync(user);

                var userWithClaims = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsAdmin = claims.Any(c => c.Type == "IsAdmin" && c.Value == "true"),
                    IsManager = claims.Any(c => c.Type == "IsManager" && c.Value == "true")
                };

                allUsersWithClaims.Add(userWithClaims);
            }

            return allUsersWithClaims;
        }

        public async Task<UserDto?> GetByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var claims = await _userManager.GetClaimsAsync(user);

            var userWithClaims = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsAdmin = claims.Any(c => c.Type == "IsAdmin" && c.Value == "true"),
                IsManager = claims.Any(c => c.Type == "IsManager" && c.Value == "true")
            };

            return userWithClaims;
        }

        public Task<UserDto?> AddAsync(UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto?> UpdateAsync(string userId, UserDto user)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto?> DeleteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var claims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in claims)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }

            var deletedUserDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsAdmin = false,
                IsManager = false
            };

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return null;
            }

            return deletedUserDto;
        }

        public async Task<bool> AddIsManagerClaimAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.AddClaimAsync(user, new Claim("IsManager", "true"));
            return result.Succeeded;
        }

        public async Task<bool> RemoveIsManagerClaimAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.RemoveClaimAsync(user, new Claim("IsManager", "true"));
            return result.Succeeded;
        }
    }
}
