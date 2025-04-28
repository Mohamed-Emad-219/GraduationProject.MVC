using GP.DAL.Models;

namespace GraduationProject.ViewModels
{
    public class BulkCoursesTermFormVM
    {
        public int TermId { get; set; }
        public List<CourseEntry> Courses { get; set; } = new List<CourseEntry>();

        // For dropdowns
        public IEnumerable<Term>? AvailableTerms { get; set; }
        public IEnumerable<Course>? AvailableCourses { get; set; }
    }

    public class CourseEntry
    {
        public string CourseCode { get; set; }
        public int Price { get; set; }
    }
}
