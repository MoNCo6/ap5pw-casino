using Casino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Application.Abstraction
{
    public interface IDepositService
    {
        IList<Deposit> Select();
        Task<bool> AddDepositAsync(int userId, int amount);
    }

}
