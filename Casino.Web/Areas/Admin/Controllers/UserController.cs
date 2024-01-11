using Casino.Application.Abstraction;
using Casino.Application.ViewModels;
using Casino.Domain.Entities;
using Casino.Domain.Identity;
using Casino.Domain.Identity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Casino.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class UserController : Controller
    {
        IUserAdminService _userService;

        public UserController(IUserAdminService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            IList<User> users = _userService.Select();
            return View(users);
        }


        public IActionResult Delete(int Id)
        {
            bool deleted = _userService.Delete(Id);

            if (deleted)
            {
                return RedirectToAction(nameof(UserController.Index));
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var user = _userService.Find((int)id);

            if (user == null)
            {
                return NotFound();
            }

            AdminEditUserProfileViewModel x = new AdminEditUserProfileViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber, 
                Balance = user.Balance,
            };

            return View(x);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminEditUserProfileViewModel obj)
        {
            if (ModelState.IsValid)
            {
                await _userService.Update(obj);
                TempData["success"] = "The register was updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}