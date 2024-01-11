using Casino.Domain.Entities;
using Casino.Domain.Implementation.Validations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Application.ViewModels
{
    public class BaseUserProfileViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Balance { get; set; }
    }


    public class UserProfileViewModel : BaseUserProfileViewModel
    {
        public string ImagePath { get; set; }
    }

    public class EditUserProfileViewModel : BaseUserProfileViewModel
    {

        [FileContent("image")]
        public IFormFile? Image { get; set; }

    }

    public class AdminEditUserProfileViewModel : EditUserProfileViewModel 
    {
        public int Id { get; set; }
    }
}
