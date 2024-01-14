using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Casino.Application.Abstraction;
using Casino.Application.ViewModels;
using Casino.Domain.Identity.Enums;
using Casino.Web.Controllers;
using System.Security.Claims;
using Casino.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Casino.Application.Implementation;
using Casino.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Casino.Domain.Entities;
using Microsoft.Extensions.Hosting;

namespace Casino.Web.Areas.Security.Controllers
{
    [Area("Security")]
    public class AccountController : Controller
    {
        // Service for handling account-related operations
        private readonly IAccountService accountService;

        // Constructor to inject the account service
        public AccountController(IAccountService security)
        {
            this.accountService = security;
        }

        // GET method to show the registration form
        public IActionResult Register()
        {
            return View();
        }

        // POST method to handle user registration
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            // Validate the model state
            if (ModelState.IsValid)
            {
                // Register the user and capture any errors
                string[] errors = await accountService.Register(registerVM, Roles.Customer);

                // If no errors, proceed to log in the user
                if (errors == null)
                {
                    LoginViewModel loginVM = new LoginViewModel()
                    {
                        Username = registerVM.Username,
                        Password = registerVM.Password
                    };

                    // Attempt to log in the new user
                    bool isLogged = await accountService.Login(loginVM);
                    if (isLogged)
                        // Redirect to the home page if login is successful
                        return RedirectToAction(nameof(HomeController.Index),
                            nameof(HomeController).Replace(nameof(Controller), string.Empty),
                            new { area = string.Empty });
                    else
                        // Redirect to the login page if login fails
                        return RedirectToAction(nameof(Login));
                }
                else
                {
                    // Add errors to the ViewModel if registration failed
                    // ... (handle errors accordingly)
                }
            }

            // If model state is not valid or registration fails, return the registration form
            return View(registerVM);
        }

        // GET method to show the login form
        public IActionResult Login()
        {
            return View();
        }

        // POST method to handle user login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            // Validate the model state
            if (ModelState.IsValid)
            {
                // Attempt to log in the user
                bool isLogged = await accountService.Login(loginVM);

                // If login is successful, redirect to the home page
                if (isLogged)
                    return RedirectToAction(nameof(HomeController.Index),
                        nameof(HomeController).Replace(nameof(Controller), string.Empty), new { area = string.Empty });

                // If login failed, set the LoginFailed property for showing error message
                loginVM.LoginFailed = true;
            }

            // Return the login form with the current ViewModel (including any errors)
            return View(loginVM);
        }

        // Action method to handle user logout
        [Authorize] // Ensure that only logged-in users can access this method
        public async Task<IActionResult> Logout()
        {
            // Log out the user
            await accountService.Logout();

            // Redirect to the login page after logout
            return RedirectToAction(nameof(Login));
        }
    }
}