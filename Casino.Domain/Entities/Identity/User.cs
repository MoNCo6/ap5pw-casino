using Casino.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public virtual string? FirstName { get; set; }
        public virtual string? LastName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ImagePath { get; set; }
        public int Balance { get; set; }
        public virtual ICollection<Deposit> Deposits { get; set; }
    }
}

