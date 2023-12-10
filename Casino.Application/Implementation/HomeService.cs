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
    public class HomeService : IHomeService
    {
        CasinoDbContext _casinoDbContext;
        public HomeService(CasinoDbContext casinoDbContext)
        {
            _casinoDbContext = casinoDbContext;
        }

        public CarouselGameViewModel GetHomeViewModel()
        {
            CarouselGameViewModel viewModel = new CarouselGameViewModel();

            viewModel.Games = _casinoDbContext.Games.ToList();
            viewModel.Carousels = _casinoDbContext.Carousels.ToList();

            return viewModel;
        }
    }
}
