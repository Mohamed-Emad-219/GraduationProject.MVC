using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public GPUser User { get; set; }

        public int Group { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int RegisterYear {  get; set; }
        [StringLength(16, MinimumLength = 16, ErrorMessage = "SSN must be exactly 16 digits.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "SSN must contain only numbers.")]
        public string SSN { get; set; }
        public string Address { get; set; }
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Mobile phone must be exactly 11 digits.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Mobile phone must contain only numbers.")]
        public string MobilePhone { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Home phone must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Home phone must contain only numbers.")]
        public string HomePhone { get; set; }
        public Gender Gender { get; set; }
        public int Level { get; set; }
        public DateOnly BirthDate { get; set; }
        public double HighSchoolGrade { get; set; }
        public int DeptId { get; set; }
        public Department Department { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
        public int AdvisorId { get; set; }
        public Advisor Advisor { get; set; }
        public Application Application { get; set; }
        public ICollection<StudentSchedule> StudentSchedules { get; set; }
    }
    public enum Gender { Male, Female }
}
