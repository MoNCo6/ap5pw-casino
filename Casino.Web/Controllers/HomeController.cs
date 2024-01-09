using Casino.Application.Abstraction;
using Casino.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Casino.Web.Models;
using Casino.Application.ViewModels; 

namespace Casino.Web.Controllers
{
    public class HomeController : Controller
    {
        IHomeService _homeService;
       

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }   

        public IActionResult Index()
        {
            CarouselGameViewModel viewModel = _homeService.GetHomeViewModel();
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contact()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Thank you for contacting us.";
                ModelState.Clear();
                return View(new ContactViewModel());
            }
            return View(model);
        }


    }
}