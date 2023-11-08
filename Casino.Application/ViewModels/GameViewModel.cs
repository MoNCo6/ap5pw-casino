using Casino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Application.ViewModels
{
    internal class GameViewModel
    {
        public IList<Game> Games { get; set; }
    }
}
