using System.Security.Claims;
using Casino.Application.Abstraction;
using Casino.Application.ViewModels;
using Casino.Domain.Entities;
using Casino.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Areas.Security.Controllers;

// Decorate the controller with the Area attribute to specify its area
[Area("Security")]
public class ProfileController : Controller
{
    // Dependency injection for services
    IAccountService accountService;
    private readonly IUserService _userService;
    UserManager<User> userManager;
    private readonly IFileUploadService _fileUploadService;

    // Constructor for dependency injection
    public ProfileController(IFileUploadService fileUploadService, IUserService userService,
        IAccountService security, UserManager<User> userManager)
    {
        this.accountService = security;
        _userService = userService;
        this.userManager = userManager;
        _fileUploadService = fileUploadService;
    }

    // Action method to display the user's profile. Requires authorization.
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdString);
        var profile = await _userService.GetUserProfileAsync(userId);
        return View(profile);
    }

    // Action method to display the edit profile page. Requires authorization.
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> EditProfile()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdString);
        var profile = await _userService.GetUserProfileAsync(userId);
        var editProfile = new EditUserProfileViewModel()
        {
            UserName = profile.UserName,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Email = profile.Email,
            PhoneNumber = profile.PhoneNumber
        };
        return View(editProfile);
    }

    // Action method to handle the submission of the edit profile form. Requires authorization.
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> EditProfile(EditUserProfileViewModel model)
    {
        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Retrieve the current user from the UserManager
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(); // Return a not found result if the user doesn't exist
        }

        // If an image is uploaded, save it and update the user's image path
        if (model.Image != null && model.Image.Length > 0)
        {
            var folderName = "profile_images";
            var imagePath = await _fileUploadService.FileUploadAsync(model.Image, folderName);

            user.ImagePath = imagePath;
        }

        // Update the user's information
        user.UserName = model.UserName;
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;

        // Save the updated user information
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            // If the update fails, add the errors to the ModelState and return the view
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // Redirect to the Profile view after successful update
        return RedirectToAction(nameof(Profile));
    }

    // Action method to display the add deposit form
    [HttpGet]
    public IActionResult AddDeposit()
    {
        return View();
    }

    // Action method to handle the submission of the add deposit form
    [HttpPost]
    public async Task<IActionResult> AddDeposit(Deposit model)
    {
        // Validate the deposit amount
        if (model.Amount <= 0)
        {
            ModelState.AddModelError(nameof(model.Amount), "The amount must be greater than zero.");
            return View(model); // Return the same view with the model to show the error message
        }

        // Retrieve user information for the deposit
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdString);
        var profile = await _userService.GetUserProfileAsync(userId);

        // Check if the model state is valid
        if (!ModelState.IsValid)
        {
            Deposit deposit = new Deposit
            {
                UserName = profile.UserName,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Amount = model.Amount,
                UserId = userId
            };

            // Process the deposit
            bool result = await accountService.AddDepositAsync(deposit);
            if (result)
            {
                // Redirect to the Profile view if the deposit is successful
                return RedirectToAction("Profile");
            }
            else
            {
                // Return the EditProfile view in case of failure
                return View("EditProfile");
            }
        }

        return View(model);
    }
}