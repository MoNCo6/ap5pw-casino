using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Identity;

namespace Casino.Domain.Entities
{
    public class Deposit
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Amount { get; set; }
        public int UserId { get; set; } 
        public virtual User User { get; set; }
    }
}
