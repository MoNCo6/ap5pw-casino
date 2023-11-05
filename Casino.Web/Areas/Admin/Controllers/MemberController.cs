using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
        IMemberAdminService _memberService;

        public MemberController(IMemberAdminService memberService)
        {
            _memberService = memberService;
        }

        public IActionResult Index()
        {
            IList<Member> members = _memberService.Select();
            return View(members);
        }
    }
}
