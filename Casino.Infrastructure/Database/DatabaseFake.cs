using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Entities;

namespace Casino.Infrastructure.Database
{
    public class DatabaseFake
    {
        public static List<Member> Members { get; set; }

        static DatabaseFake()
        {
            DatabaseInit dbInit = new DatabaseInit();
            Members = dbInit.GetMembers().ToList();
        }
    }
}
