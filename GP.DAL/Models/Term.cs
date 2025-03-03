using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class Term
    {
        public int Id { get; set; }
        public SemesterType Semester {  get; set; }
        public int List {  get; set; }
        public int AcademicYear { get; set; }
        public int Level { get; set; }
        public ICollection<CoursesTerm>? CoursesTerms { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
