using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Homes
{
    [Authorize("ManagerOnly")]
    public class CreateModel(
        HomeService homeService,
        CommunityService communityService,
        IAuthorizationService authorizationService,
        ILogger<CreateModel> logger
        ) : PageModel
    {
        private readonly HomeService _homeService = homeService;
        private readonly CommunityService _communityService = communityService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<CreateModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty]
        public HomeCreateInput Input { get; set; } = new HomeCreateInput();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (CommunityId <= 0)
            {
                ModelState.AddModelError(string.Empty, "CommunityId is required.");

                return Page();
            }

            try
            {
                var community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, community, "CommunityManager");

                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                var newHome = await _homeService.AddHomeAsync(
                    new Home
                    {
                        CommunityId = community.Id,
                        Floor = Input.Floor,
                        Number = Input.Number,
                        Street = Input.Street,
                        City = Input.City,
                        State = Input.State,
                        Zip = Input.Zip,
                        Country = Input.Country,
                        Type = Input.Type,
                        Bedrooms = Input.Bedrooms,
                        Bathrooms = Input.Bathrooms,
                        SqFt = Input.SqFt,
                        YearBuilt = Input.YearBuilt,
                        YearRenovated = Input.YearRenovated,
                        IsFurnished = Input.IsFurnished,
                        HasParking = Input.HasParking,
                        ParkingDetails = Input.ParkingDetails,
                        SecurityDetails = Input.SecurityDetails,
                        UtilitiesDetails = Input.UtilitiesDetails,
                        AccessibilityDetails = Input.AccessibilityDetails,
                        PetDetails = Input.PetDetails,
                        OtherDetails = Input.OtherDetails,
                        Laundry = Input.Laundry,
                        Heating = Input.Heating,
                        Cooling = Input.Cooling,
                        OutdoorSpace = Input.OutdoorSpace,
                        BaseRent = Input.BaseRent,
                        BaseDeposit = Input.BaseDeposit
                    })
                    ?? throw new InvalidOperationException("AddHomeAsync returned null.");

                _logger.LogInformation("Created new Home {HomeId}.", newHome.Id);

                return RedirectToPage("./Details/", new { HomeId = newHome.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Home.");

                ModelState.AddModelError(string.Empty, "Unable to create home.");

                return Page();
            }
        }
    }
}