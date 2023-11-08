using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace Casino.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public IActionResult Create(Game game)
        {
            _gameService.Create(game);

            return RedirectToAction(nameof(GameController.Index));
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

            return View(game);
        }

        [HttpPost]
        public IActionResult Edit(Game obj)
        {
            if (ModelState.IsValid)
            {
                _gameService.Update(obj);
                TempData["success"] = "The register was updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}

