using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Entities;

namespace Casino.Application.Abstraction
{
    public interface IGameAdminService
    {
        IList<Game> Select();
        Task Create(GameCreate game);
        bool Delete(int id);
        Game? Find(int id);
        Task Update(GameEdit m);
    }
}
