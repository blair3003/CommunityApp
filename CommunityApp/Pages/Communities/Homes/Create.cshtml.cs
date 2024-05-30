using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityApp.Pages.Communities.Homes
{
    public class CreateModel : PageModel
    {
        private readonly HomeService _homeService;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty]
        public CreateHomeInput Input { get; set; } = new CreateHomeInput();

        public CreateModel(HomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var newHome = await _homeService.AddHomeAsync(
                    new Home
                    {
                        CommunityId = CommunityId,
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
                    ?? throw new InvalidOperationException("Home creation failed.");

                return RedirectToPage("./Details/", new { CommunityId, HomeId = newHome.Id });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to create home.");
                return Page();
            }
        }
    }
}