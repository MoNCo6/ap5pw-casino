using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Application.ViewModels;

namespace Casino.Application.Abstraction
{
    public interface IHomeService
    {
        CarouselGameViewModel GetHomeViewModel();
    }
}
