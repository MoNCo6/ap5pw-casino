using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Application.ViewModels;
using Casino.Domain.Entities;
using Casino.Infrastructure.Identity;

namespace Casino.Application.Abstraction
{
    public interface IUserAdminService
    {
        IList<User> Select();
        bool Delete(int id);
        User? Find(int id);
        Task Update(AdminEditUserProfileViewModel m);
        Task<UserProfileViewModel> GetUserProfileAsync(int userId);
        Task<bool> UpdateUserAsync(string userId, UserProfileViewModel model);
    }
}
