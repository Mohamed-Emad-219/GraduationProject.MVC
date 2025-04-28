using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class Course
    {
        [Key]
        public string Code { get; set; }
        public string CourseName { get; set; }
        public int? CreditHour { get; set; }
        public int Level { get; set; }
        public SemesterType Semester { get; set; }
        public int? NoOfSec { get; set; }
        public int NeedsLab { get; set; }
        public int? NoOfLec { get; set; }
        public int? DeptId { get; set; }
        public Department Department { get; set; }
        public ICollection<StudentSchedule>? StudentSchedules { get; set; }
        public ICollection<InstructorSchedule>? InstructorSchedules { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
        public ICollection<CoursesTerm>? CoursesTerms { get; set; }
        public ICollection<CoursePrerequisite>? Prerequisites { get; set; }
        public ICollection<CoursePrerequisite>? RequiredFor { get; set; }
        public ICollection<CourseInstructor>? CourseInstructors { get; set; }
    }
}
