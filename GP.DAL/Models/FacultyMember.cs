using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class FacultyMember
    {
        [Key]
        public string TeacherId { get; set; }

        public string? UserId { get; set; }
        public GPUser? User { get; set; }

        [StringLength(16, MinimumLength = 16, ErrorMessage = "SSN must be exactly 16 digits.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "SSN must contain only numbers.")]
        public string? SSN { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string FullName { get; set; }
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Mobile phone must be exactly 11 digits.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Mobile phone must contain only numbers.")]
        public string? MobilePhone { get; set; }
        public string? Address { get; set; }
        public int? WorkingHours { get; set; }
        public int? DeptId { get; set; }
        public Department? Department { get; set; }
        public ICollection<InstructorSchedule>? InstructorSchedules { get; set; }
        public ICollection<InstructorSchedule>? AssistantSchedules { get; set; }
        public ICollection<StudentSchedule>? StudentSchedules { get; set; }
        public ICollection<CourseInstructor>? CourseInstructors { get; set; }

    }
}
