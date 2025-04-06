using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class ResultPetition
    {
        [Key]
        public int Id { get; set; } // Primary Key
        [Required]
        public int PetitionCourseId { get; set; } // Foreign Key

        [ForeignKey("PetitionCourseId")]
        public PetitionCourse? PetitionCourse { get; set; } // Navigation Property

        public bool ErrorInCorrection { get; set; } // Yes/No

        public bool ApplicationSubmitted { get; set; } // Applied/Not Applied

        [Required]
        public string YearWorkAssessment { get; set; } // More than Acceptable, Acceptable, etc.

        [Required]
        public string PracticalExamAssessment { get; set; }

        [Required]
        public string FinalExamAssessment { get; set; }

        public bool TotalGrade { get; set; } // Correct/Incorrect

        [Required]
        public string FinalGrade { get; set; } // A+, A, B+, etc.

        public string Notes { get; set; }

        public int AdvisorId { get; set; } // Name of the advisor
        [ForeignKey("AdvisorId")]
        public Advisor? Advisor { get; set; } // Navigation Property
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Timestamp
    }

}
