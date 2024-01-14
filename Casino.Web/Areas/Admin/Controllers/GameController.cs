using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Casino.Domain.Identity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace Casino.Web.Areas.Admin.Controllers
{
    // Controller for managing game-related actions in the Admin area
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class GameController : Controller
    {
        // Service for handling game administration tasks
        private readonly IGameAdminService _gameService;

        // Constructor to inject the game administration service
        public GameController(IGameAdminService gameService)
        {
            _gameService = gameService;
        }

        // Action method to display the list of games
        public IActionResult Index()
        {
            // Retrieve all games using the service
            IList<Game> games = _gameService.Select();
            return View(games);
        }

        // GET method to show the game creation form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST method to handle game creation
        [HttpPost]
        public async Task<IActionResult> Create(GameCreate game)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Create the game using the service
                await _gameService.Create(game);

                // Redirect to the game index page
                return RedirectToAction(nameof(GameController.Index));
            }
            else
            {
                // If model state is not valid, show the form again with the provided data
                return View(game);
            }
        }

        // Action method to delete a game
        public IActionResult Delete(int Id)
        {
            // Delete the game using the service
            bool deleted = _gameService.Delete(Id);

            // Check if deletion was successful
            if (deleted)
            {
                // Redirect to the game index page if deleted
                return RedirectToAction(nameof(GameController.Index));
            }
            else
            {
                // Return NotFound if game could not be found or deleted
                return NotFound();
            }
        }

        // GET method to show the game edit form
        public IActionResult Edit(int? id)
        {
            // Check if the provided ID is valid
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Find the game using the service
            var game = _gameService.Find((int)id);

            // Check if the game exists
            if (game == null)
            {
                return NotFound();
            }

            // Map game data to GameEdit ViewModel
            GameEdit gameEditViewModel = new GameEdit()
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Rules = game.Rules,
                Image = null,
            };

            // Show the edit form with the game data
            return View(gameEditViewModel);
        }

        // POST method to handle game editing
        [HttpPost]
        public async Task<IActionResult> Edit(GameEdit obj)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Update the game using the service
                await _gameService.Update(obj);

                // Store success message in TempData and redirect to the index page
                TempData["success"] = "The register was updated successfully";
                return RedirectToAction("Index");
            }

            // If model state is not valid, show the form again with the provided data
            return View(obj);
        }
    }
}