using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Dto
{
    public class SemesterEvaluationSummaryDto
    {
        public string HighestGPAName { get; set; } = string.Empty;
        public string HighestGPAGrade { get; set; } = string.Empty;
        public string LowestGPAName { get; set; } = string.Empty;
        public string LowestGPAGrade { get; set; } = string.Empty;
        public double AverageGPA { get; set; }
        public double PassRate { get; set; }
        public double FailureRate { get; set; }
    }
}
