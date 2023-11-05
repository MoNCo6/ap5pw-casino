using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Application.Abstraction;
using Casino.Application.ViewModels;
using Casino.Infrastructure.Database;

namespace Casino.Application.Implementation
{
    public class HomeService : IHomeService
    {
        public MemberViewModel GetHomeViewModel()
        {
            MemberViewModel viewModel = new MemberViewModel();

            viewModel.Members = DatabaseFake.Members;

            return viewModel;
        }
    }
}
