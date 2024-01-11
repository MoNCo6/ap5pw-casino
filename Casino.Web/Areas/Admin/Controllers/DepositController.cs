using Casino.Application.Abstraction;
using Casino.Application.Implementation;
using Casino.Domain.Entities;
using Casino.Infrastructure.Database;
using Casino.Domain.Identity;
using Casino.Domain.Identity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Casino.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class DepositController : Controller
    {
        private readonly IDepositService _depositService;
        private readonly CasinoDbContext _context;
        private readonly UserManager<User> _userManager;

        public DepositController(IDepositService depositService, CasinoDbContext context, UserManager<User> userManager)
        {
            _depositService = depositService;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IList<Deposit> deposit = _depositService.Select();
            return View(deposit);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int depositId)
        {
            var deposit = await _context.Deposits.FirstOrDefaultAsync(d => d.Id == depositId);

            if (deposit != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == deposit.UserId);

                if (user != null)
                {
                    user.Balance += deposit.Amount;

                    await _userManager.UpdateAsync(user);
                    _context.Deposits.Remove(deposit);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Deposit");
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Decline(int depositId)
        {
            var deposit = await _context.Deposits.FirstOrDefaultAsync(d => d.Id == depositId);

            if (deposit != null)
            {
                _context.Deposits.Remove(deposit);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Deposit");
                
            }
            return NotFound();
        }

    }

}


