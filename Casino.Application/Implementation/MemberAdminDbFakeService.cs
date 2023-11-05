using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Casino.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Application.Implementation
{
    public class MemberAdminDbFakeService : IMemberAdminService
    {
        public IList<Member> Select()
        {
            return DatabaseFake.Members;
        }

        public void Create(Member member)
        {
            if (DatabaseFake.Members != null &&
                DatabaseFake.Members.Count > 0)
            {
                member.Id = DatabaseFake.Members.Last().Id + 1;
            }
            else
            {
                member.Id = 1;
            }


            if (DatabaseFake.Members != null)
                DatabaseFake.Members.Add(member);
        }

        public bool Delete(int id)
        {
            bool deleted = false;

            Member? member =
                DatabaseFake.Members.FirstOrDefault(user => user.Id == id);

            if (member != null)
            {
                deleted = DatabaseFake.Members.Remove(member);
            }

            return deleted;
        }

        public Member? Find(int id)
        {
            return DatabaseFake.Members.FirstOrDefault(user => user.Id == id);
        }

        public void Update(Member member)
        {
            Member origMember = Find(member.Id);
            int index = DatabaseFake.Members.IndexOf(origMember);
            DatabaseFake.Members[index] = member;
        }
    }
}
