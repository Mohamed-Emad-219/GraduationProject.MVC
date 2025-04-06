using GP.DAL.Models;

namespace GraduationProject.ViewModels
{
    public class PetitionRequestVM
    {
        public int DeanId { get; set; }
        public int CollegeId { get; set; }
        public int DeptId { get; set; }

        public string StudentName { get; set; }
        public string RegistrationNumber { get; set; }
        public SemesterType Semester { get; set; }

        public List<string> Courses { get; set; } = new List<string>(); // Store selected courses

        public DateTime Date { get; set; }

        public int NumberOfCourses { get; set; }
        public double AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }

    }

}
