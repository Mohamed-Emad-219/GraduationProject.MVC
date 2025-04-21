using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{

    public class StudentSchedule // model
    {
        public int Id { get; set; }
        public string Day { get; set; }
        [Column(TypeName = "time")] // SQL Server's time type
        public TimeSpan TimeBegin { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan TimeEnd { get; set; }
        public SemesterType Semester { get; set; }
        public int? AcademicYear { get; set; }
        public string? PlaceId { get; set; }
        public string? TeacherId { get; set; }
        public string? CourseCode { get; set; }
        public Place? Place { get; set; }
        public Course? Course { get; set; }
        public FacultyMember? FacultyMember { get; set; }
        public string Group {  get; set; }
        public int Level { get; set; }
    }
}
