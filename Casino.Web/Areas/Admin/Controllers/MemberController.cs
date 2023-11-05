using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Member member)
        {
            _memberService.Create(member);

            return RedirectToAction(nameof(MemberController.Index));
        }


        public IActionResult Delete(int Id)
        {
            bool deleted = _memberService.Delete(Id);

            if (deleted)
            {
                return RedirectToAction(nameof(MemberController.Index));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
