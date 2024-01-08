using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Infrastructure.Identity;

namespace Casino.Application.Abstraction
{
    public interface IUserAdminService
    {
        IList<User> Select();
        bool Delete(int id);
    }
}
