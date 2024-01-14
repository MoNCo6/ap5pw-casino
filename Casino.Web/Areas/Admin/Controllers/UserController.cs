using Casino.Application.Abstraction;
using Casino.Application.ViewModels;
using Casino.Domain.Entities;
using Casino.Domain.Identity;
using Casino.Domain.Identity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Casino.Application.Abstraction;

// ... other using statements

namespace Casino.Web.Areas.Admin.Controllers
{
    // Controller responsible for user management in the Admin area
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class UserController : Controller
    {
        // Service for handling user-related operations
        private readonly IUserService _userService;

        // Constructor to inject the user service
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Action method to display the list of users
        public IActionResult Index()
        {
            // Retrieve all users using the service
            IList<User> users = _userService.Select();
            return View(users);
        }

        // Action method to delete a user
        public IActionResult Delete(int Id)
        {
            // Delete the user using the service
            bool deleted = _userService.Delete(Id);

            // Check if deletion was successful
            if (deleted)
            {
                // Redirect to the user index page if deleted
                return RedirectToAction(nameof(UserController.Index));
            }
            else
            {
                // Return NotFound if user could not be found or deleted
                return NotFound();
            }
        }

        // GET method to show the user edit form
        public IActionResult Edit(int? id)
        {
            // Check if the provided ID is valid
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Find the user using the service
            var user = _userService.Find((int)id);

            // Check if the user exists
            if (user == null)
            {
                return NotFound();
            }

            // Map user data to AdminEditUserProfileViewModel
            AdminEditUserProfileViewModel userEditViewModel = new AdminEditUserProfileViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Balance = user.Balance,
            };

            // Show the edit form with the user data
            return View(userEditViewModel);
        }

        // POST method to handle user editing
        [HttpPost]
        public async Task<IActionResult> Edit(AdminEditUserProfileViewModel obj)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Update the user using the service
                await _userService.Update(obj);

                // Store success message in TempData and redirect to the index page
                TempData["success"] = "The register was updated successfully";
                return RedirectToAction("Index");
            }

            // If model state is not valid, show the form again with the provided data
            return View(obj);
        }
    }
}