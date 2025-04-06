using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class PetitionRequest
    {
        [Key]
        public int Id { get; set; }

        // Foreign keys (related to dropdown selections)
        [Required]
        public int DeanId { get; set; }

        [Required]
        public int CollegeId { get; set; }

        [Required]
        public int DeptId { get; set; }

        // Student details
        [Required]
        [MaxLength(100)]
        public  string StudentName { get; set; }

        [Required]
        [MaxLength(50)]
        public string RegistrationNumber { get; set; }

        [Required]
        public SemesterType Semester { get; set; }
        public DateTime Date { get; set; }

        // Payment & Decision
        public int NumberOfCourses { get; set; }
        public double AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }

        // Navigation Properties (for relationships)
        [ForeignKey("DeanId")]
        public FacultyMember? Dean { get; set; }

        [ForeignKey("CollegeId")]
        public College? College { get; set; }

        [ForeignKey("DeptId")]
        public Department? Department { get; set; }

        // Courses (One-to-Many Relationship)
        public List<PetitionCourse> Courses { get; set; } = new List<PetitionCourse>();

        public string Status { get; set; } = "In Progress";
        
    }
}
