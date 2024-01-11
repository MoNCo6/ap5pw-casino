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
        IAccountService accountService;
        private readonly IUserAdminService _userService;
        UserManager<User> userManager;
        private readonly IFileUploadService _fileUploadService;

        public AccountController(IFileUploadService fileUploadService, IUserAdminService userService, IAccountService security, UserManager<User> userManager)
        {
            this.accountService = security;
            _userService = userService;
            this.userManager = userManager;
            _fileUploadService = fileUploadService;

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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditUserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (model.Image != null && model.Image.Length > 0)
            {
                var folderName = "profile_images";
                var imagePath = await _fileUploadService.FileUploadAsync(model.Image, folderName);

                user.ImagePath = imagePath;
            }

            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            return RedirectToAction(nameof(Profile));
        }


        [HttpGet]
        public IActionResult AddDeposit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDeposit(Deposit model)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userIdString);
            var profile = await _userService.GetUserProfileAsync(userId);

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

                bool result = await accountService.AddDepositAsync(deposit);
                if (result)
                {
                    return RedirectToAction("Profile"); 
                }
                else
                {
                    return View("EditProfile", model); 
                }
            }
            return View(model);
        }
       
    }
}