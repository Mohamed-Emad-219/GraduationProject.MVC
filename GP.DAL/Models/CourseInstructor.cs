using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class CourseInstructor
    {
        [ForeignKey("Course")]
        public string CourseCode { get; set; }
        [ForeignKey("FacultyMember")]
        public string TeacherId { get; set; }
        public FacultyMember? FacultyMember { get; set; }
        public Course? Course { get; set; }
    }
}
