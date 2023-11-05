using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Entities;

namespace Casino.Application.ViewModels
{
    public class MemberViewModel
    {
        public IList<Member> Members{ get; set; }
    }
}
