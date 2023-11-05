using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Entities;

namespace Casino.Infrastructure.Database
{
    internal class DatabaseInit
    {
        public IList<Member> GetMembers()
        {
            IList<Member> members = new List<Member>();

            members.Add(
                new Member()
                {
                    Id = 1,
                    Forename = "Jozef",
                    Surname = "Dlhy",
                    Email = "jozef@dlhy.com",
                    Username = "Dlhy69",
                    Password = "456321",
                }
            );

            members.Add(
                new Member()
                {
                    Id = 1,
                    Forename = "Jozef2",
                    Surname = "Dlhy",
                    Email = "jozef2@dlhy.com",
                    Username = "Dlhy692",
                    Password = "4563212",
                }
            );

            return members;
        }
        
    }
}
