using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CollegeId { get; set; }
        public College? College { get; set; }
        // One-to-One relationship for the Head of the Department
        public string? HeadId { get; set; }
        public FacultyMember? Head { get; set; }

        // One-to-Many relationship for Faculty Members
        public ICollection<FacultyMember>? FacultyMembers { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
