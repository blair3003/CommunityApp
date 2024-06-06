using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Homes
{
    [Authorize("ManagerOnly")]
    public class EditModel(
        HomeService homeService,
        IAuthorizationService authorizationService,
        ILogger<EditModel> logger
        ) : PageModel
    {
        private readonly HomeService _homeService = homeService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<EditModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int HomeId { get; set; }
        [BindProperty]
        public UpdateHomeInput Input { get; set; } = new UpdateHomeInput();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("GetHomeByIdAsync returned null.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, home.Community, "CommunityManager");

                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                Input = new UpdateHomeInput
                {
                    Floor = home.Floor,
                    Number = home.Number,
                    Street = home.Street,
                    City = home.City,
                    State = home.State,
                    Zip = home.Zip,
                    Country = home.Country,
                    Type = home.Type,
                    Bedrooms = home.Bedrooms,
                    Bathrooms = home.Bathrooms,
                    SqFt = home.SqFt,
                    YearBuilt = home.YearBuilt,
                    YearRenovated = home.YearRenovated,
                    IsFurnished = home.IsFurnished,
                    HasParking = home.HasParking,
                    ParkingDetails = home.ParkingDetails,
                    SecurityDetails = home.SecurityDetails,
                    UtilitiesDetails = home.UtilitiesDetails,
                    AccessibilityDetails = home.AccessibilityDetails,
                    PetDetails = home.PetDetails,
                    OtherDetails = home.OtherDetails,
                    Laundry = home.Laundry,
                    Heating = home.Heating,
                    Cooling = home.Cooling,
                    OutdoorSpace = home.OutdoorSpace,
                    BaseRent = home.BaseRent,
                    BaseDeposit = home.BaseDeposit
                };

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Home {HomeId}.", HomeId);

                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("GetHomeByIdAsync returned null.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, home.Community, "CommunityManager");

                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                home.Floor = Input.Floor;
                home.Number = Input.Number;
                home.Street = Input.Street;
                home.City = Input.City;
                home.State = Input.State;
                home.Zip = Input.Zip;
                home.Country = Input.Country;
                home.Type = Input.Type;
                home.Bedrooms = Input.Bedrooms;
                home.Bathrooms = Input.Bathrooms;
                home.SqFt = Input.SqFt;
                home.YearBuilt = Input.YearBuilt;
                home.YearRenovated = Input.YearRenovated;
                home.IsFurnished = Input.IsFurnished;
                home.HasParking = Input.HasParking;
                home.ParkingDetails = Input.ParkingDetails;
                home.SecurityDetails = Input.SecurityDetails;
                home.UtilitiesDetails = Input.UtilitiesDetails;
                home.AccessibilityDetails = Input.AccessibilityDetails;
                home.PetDetails = Input.PetDetails;
                home.OtherDetails = Input.OtherDetails;
                home.Laundry = Input.Laundry;
                home.Heating = Input.Heating;
                home.Cooling = Input.Cooling;
                home.OutdoorSpace = Input.OutdoorSpace;
                home.BaseRent = Input.BaseRent;
                home.BaseDeposit = Input.BaseDeposit;

                var updatedHome = await _homeService.UpdateHomeAsync(HomeId, home)
                    ?? throw new InvalidOperationException("UpdateHomeAsync returned null.");

                _logger.LogInformation("Updated Home {HomeId}.", HomeId);

                return RedirectToPage("./Details/", new { HomeId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Home {HomeId}.", HomeId);

                ModelState.AddModelError(string.Empty, "Unable to update home.");

                return Page();
            }
        }
    }
}