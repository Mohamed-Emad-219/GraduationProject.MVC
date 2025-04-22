using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GP.DAL.Models
{
    public class GPUser :IdentityUser
    {
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        public virtual FacultyMember FacultyMember { get; set; }
        public virtual Student Student{ get; set; }
        public virtual Advisor Advisor { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual FinancialAffairs FinancialAffairs { get; set; }
        public virtual StudentAffairs StudentAffairs { get; set; }
        public virtual FollowUp FollowUp { get; set; }
    }
}
