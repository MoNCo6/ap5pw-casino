using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Application.ViewModels
{
    public class CoinFlipViewModel
    {
        public string? LastFlipResult { get; set; }
        public string? SelectedChoice { get; set; }
        [Required] public int BetAmount { get; set; }
    }
}