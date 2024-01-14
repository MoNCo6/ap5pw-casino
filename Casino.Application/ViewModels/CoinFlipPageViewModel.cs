using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Identity;

namespace Casino.Application.ViewModels
{
    public record CoinFlipPageViewModel
    {
        public User UserProfile { get; set; } = new User();
        public CoinFlipViewModel CoinFlip { get; set; } = new CoinFlipViewModel();
    }
}