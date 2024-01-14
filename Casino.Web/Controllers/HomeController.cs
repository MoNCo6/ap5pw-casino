using Casino.Application.Abstraction;
using Casino.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Casino.Web.Models;
using Casino.Application.ViewModels;

namespace Casino.Web.Controllers
{
    // Define HomeController which inherits from the base Controller class
    public class HomeController : Controller
    {
        // Declaration of the IHomeService for dependency injection
        IHomeService _homeService;

        // Constructor for HomeController with dependency injection of IHomeService
        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        // Action method for the Index page
        public IActionResult Index()
        {
            // Fetch the home view model from the home service
            CarouselGameViewModel viewModel = _homeService.GetHomeViewModel();
            // Return the Index view, passing the viewModel as a model
            return View(viewModel);
        }

        // Action method for the Privacy page
        public IActionResult Privacy()
        {
            // Return the Privacy view
            return View();
        }

        // Action method for handling errors with caching disabled
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Create a new ErrorViewModel with the current request ID and return the Error view
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Action method for the Contact page
        public IActionResult Contact()
        {
            // Return the Contact view with a new instance of ContactViewModel
            return View(new ContactViewModel());
        }

        // Action method for handling the POST request of the Contact form
        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            // Check if the submitted form is valid
            if (ModelState.IsValid)
            {
                // Set a message in ViewBag and clear the model state
                ViewBag.Message = "Thank you for contacting us.";
                ModelState.Clear();
                // Return the Contact view with a new instance of ContactViewModel
                return View(new ContactViewModel());
            }

            // If the model state is not valid, return the Contact view with the existing model
            return View(model);
        }
    }
}