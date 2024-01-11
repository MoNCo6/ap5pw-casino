using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Casino.Domain.Identity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace Casino.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class GameController : Controller
    {
        IGameAdminService _gameService;

        public GameController(IGameAdminService gameService)
        {
            _gameService = gameService;
        }

        public IActionResult Index()
        {
            IList<Game> games = _gameService.Select();
            return View(games);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GameCreate game)
        {
            if (ModelState.IsValid)
            {
                await _gameService.Create(game);

                return RedirectToAction(nameof(GameController.Index));
            }
            else
            {
                return View(game);
            }
        }


        public IActionResult Delete(int Id)
        {
            bool deleted = _gameService.Delete(Id);

            if (deleted)
            {
                return RedirectToAction(nameof(GameController.Index));
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

            var game = _gameService.Find((int)id);

            if (game == null)
            {
                return NotFound();
            }

            GameEdit x = new GameEdit()
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Rules = game.Rules,
                Image = null,
            };

            return View(x);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameEdit obj)
        {
            if (ModelState.IsValid)
            {
                await _gameService.Update(obj);
                TempData["success"] = "The register was updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}

