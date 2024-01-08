using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Casino.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Infrastructure.Identity;


namespace Casino.Application.Implementation
{
    public class UserAdminService : IUserAdminService
    {
        CasinoDbContext _casinoDbContext;
        public UserAdminService(CasinoDbContext casinoDbContext)
        {
            _casinoDbContext = casinoDbContext;
        }
        public IList<User> Select()
        {
            return _casinoDbContext.Users.ToList();

        }

        public bool Delete(int id)
        {
            bool deleted = false;

            User? user =
                _casinoDbContext.Users.FirstOrDefault(user => user.Id == id);

            if (user != null)
            {
                _casinoDbContext.Users.Remove(user);
                _casinoDbContext.SaveChanges();

                deleted = true;
            }

            return deleted;
        }
    }
}
