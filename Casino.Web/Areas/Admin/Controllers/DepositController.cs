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
    // Controller for managing deposits in the Admin area
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class DepositController : Controller
    {
        // Services and context injected through constructor
        private readonly IDepositService _depositService;
        private readonly CasinoDbContext _context;
        private readonly UserManager<User> _userManager;

        // Constructor to inject the deposit service, database context, and user manager
        public DepositController(IDepositService depositService, CasinoDbContext context, UserManager<User> userManager)
        {
            _depositService = depositService;
            _context = context;
            _userManager = userManager;
        }

        // Action method to display the list of deposits
        public IActionResult Index()
        {
            // Retrieve all deposits using the service
            IList<Deposit> deposits = _depositService.Select();
            return View(deposits);
        }

        // Action method to confirm a deposit
        [HttpPost]
        public async Task<IActionResult> Confirm(int depositId)
        {
            // Find the deposit by its ID
            var deposit = await _context.Deposits.FirstOrDefaultAsync(d => d.Id == depositId);

            // If the deposit is found, process it
            if (deposit != null)
            {
                // Find the user associated with the deposit
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == deposit.UserId);

                // If the user is found, add the deposit amount to the user's balance
                if (user != null)
                {
                    user.Balance += deposit.Amount;

                    // Update the user's balance in the database
                    await _userManager.UpdateAsync(user);
                    // Remove the deposit record as it has been processed
                    _context.Deposits.Remove(deposit);
                    // Save the changes to the database
                    await _context.SaveChangesAsync();

                    // Redirect back to the deposit index page
                    return RedirectToAction("Index", "Deposit");
                }
            }

            // If deposit or user is not found, return a NotFound result
            return NotFound();
        }

        // Action method to decline a deposit
        [HttpPost]
        public async Task<IActionResult> Decline(int depositId)
        {
            // Find the deposit by its ID
            var deposit = await _context.Deposits.FirstOrDefaultAsync(d => d.Id == depositId);

            // If the deposit is found, process the decline
            if (deposit != null)
            {
                // Remove the deposit record as it has been declined
                _context.Deposits.Remove(deposit);
                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Redirect back to the deposit index page
                return RedirectToAction("Index", "Deposit");
            }

            // If the deposit is not found, return a NotFound result
            return NotFound();
        }
    }
}