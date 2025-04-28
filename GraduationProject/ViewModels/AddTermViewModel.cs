using GP.DAL.Models;

namespace GraduationProject.ViewModels
{
    public class AddTermViewModel
    {
        public int AcademicYear { get; set; }
        public int Level { get; set; }
        public SemesterType Semester { get; set; }
        public int List { get; set; } = 2022; // default if you want
    }
}
