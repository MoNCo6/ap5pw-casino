using Casino.Application.ViewModels;
using Casino.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Areas.Security.Controllers
{
    // Controller for the coin flip game in the Security area
    [Area("Security")]
    public class CoinFlipController : Controller
    {
        // UserManager and SignInManager for handling user-related operations
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        // Constructor to inject UserManager and SignInManager
        public CoinFlipController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // GET method to show the coin flip game page
        [HttpGet]
        public async Task<IActionResult> FlipCoin()
        {
            // Get the current user
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Create and populate the CoinFlipPageViewModel
            var coinFlipPageViewModel = new CoinFlipPageViewModel
            {
                UserProfile = user,
                CoinFlip = new CoinFlipViewModel()
            };

            // Render the view with the ViewModel
            return View(coinFlipPageViewModel);
        }

        // POST method to handle the coin flip action
        [HttpPost]
        public async Task<IActionResult> FlipCoin(CoinFlipPageViewModel model)
        {
            // Get the current user
            var user = await userManager.GetUserAsync(User);
            int amount = model.CoinFlip.BetAmount;
            string choice = model.CoinFlip.SelectedChoice;

            // Check if the user's balance is sufficient for the bet
            if (user.Balance < amount)
            {
                // Add error to ModelState and return the view with the current model
                ModelState.AddModelError(string.Empty, "Insufficient balance to place the bet.");
                return View(model);
            }

            // Generate the coin flip result (Heads or Tails)
            var random = new Random();
            var flipResult = random.Next(2) == 0 ? "Heads" : "Tails";

            // Update the ViewModel with the result of the flip
            model.CoinFlip.LastFlipResult = flipResult;

            // Determine if the user's choice matches the flip result
            if ((choice == "Heads" && flipResult == "Heads") || (choice == "Tails" && flipResult == "Tails"))
            {
                user.Balance += amount * 2; // User wins the bet
            }
            else
            {
                user.Balance -= amount; // User loses the bet
            }

            // Ensure the user's balance does not go negative
            if (user.Balance < 0)
            {
                user.Balance = 0;
            }

            // Update the user's information in the database
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Refresh the user's sign-in session
                await signInManager.RefreshSignInAsync(user);

                // Update the balance in the ViewModel
                model.UserProfile.Balance = user.Balance;
            }

            // Render the view with the updated model
            return View(model);
        }
    }
}