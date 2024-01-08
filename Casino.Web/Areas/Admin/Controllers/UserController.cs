using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Casino.Infrastructure.Identity;
using Casino.Infrastructure.Identity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

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
                return RedirectToAction(nameof(GameController.Index));
            }
            else
            {
                return NotFound();
            }
        }
    }
}