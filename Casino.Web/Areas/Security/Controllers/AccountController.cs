using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Casino.Application.Abstraction;
using Casino.Application.ViewModels;
using Casino.Infrastructure.Identity.Enums;
using Casino.Web.Controllers;
using System.Security.Claims;
using Casino.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Casino.Application.Implementation;
using Casino.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Casino.Web.Areas.Security.Controllers
{
    [Area("Security")]
    public class AccountController : Controller
    {
        IAccountService accountService;
        private readonly IUserAdminService _userService;


        public AccountController(IUserAdminService userService, IAccountService security)
        {
            this.accountService = security;
            _userService = userService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                string[] errors = await accountService.Register(registerVM, Roles.Customer);

                if (errors == null)
                {
                    LoginViewModel loginVM = new LoginViewModel()
                    {
                        Username = registerVM.Username,
                        Password = registerVM.Password
                    };

                    bool isLogged = await accountService.Login(loginVM);
                    if (isLogged)
                        return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace(nameof(Controller), String.Empty), new { area = String.Empty });
                    else
                        return RedirectToAction(nameof(Login));
                }
                else
                {
                    //error to ViewModel
                }

            }

            return View(registerVM);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                bool isLogged = await accountService.Login(loginVM);
                if (isLogged)
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace(nameof(Controller), String.Empty), new { area = String.Empty });

                loginVM.LoginFailed = true;
            }

            return View(loginVM);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await accountService.Logout();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userIdString);
            var profile = await _userService.GetUserProfileAsync(userId);
            return View(profile);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userIdString);
            var profile = await _userService.GetUserProfileAsync(userId);
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(UserProfileViewModel model, [FromServices] IUserAdminService userService)
        {
            Console.WriteLine(model.LastName);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            var success = await userService.UpdateUserAsync(userId, model);

            if (success)
            {
                return RedirectToAction(nameof(Profile));
            }
            else
            {
                ModelState.AddModelError("", "Error updating user.");
                return View(model);
            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult DepositMoney()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DepositMoney(DepositViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Process the deposit (e.g., integrate with a payment gateway)
                // Update the user's balance

                return RedirectToAction("Index", "Home"); // Redirect to home or a confirmation page
            }

            return View(model);
        }

    }
    
}