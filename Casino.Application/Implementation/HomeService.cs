using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Application.Abstraction;
using Casino.Application.ViewModels;
using Casino.Infrastructure.Database;

namespace Casino.Application.Implementation
{
    // Service class responsible for providing data for the home page of the application
    public class HomeService : IHomeService
    {
        // Database context for accessing the casino database
        private readonly CasinoDbContext _casinoDbContext;

        // Constructor to inject the database context
        public HomeService(CasinoDbContext casinoDbContext)
        {
            _casinoDbContext = casinoDbContext;
        }

        // Retrieves the view model containing data for the home page
        public CarouselGameViewModel GetHomeViewModel()
        {
            // Initialize the view model
            CarouselGameViewModel viewModel = new CarouselGameViewModel();

            // Populate the Games property with all games from the database
            viewModel.Games = _casinoDbContext.Games.ToList();

            // Populate the Carousels property with all carousel images from the database
            viewModel.Carousels = _casinoDbContext.Carousels.ToList();

            // Return the populated view model
            return viewModel;
        }
    }
}