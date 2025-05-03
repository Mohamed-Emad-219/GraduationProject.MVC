using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.ViewModels
{
    public class CourseReportVM
    {
        public string CourseCode { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public int TotalEnrolled { get; set; }
        public double AverageGPA { get; set; }
        public double PassRate { get; set; }
        public double FailRate { get; set; }
    }
}
