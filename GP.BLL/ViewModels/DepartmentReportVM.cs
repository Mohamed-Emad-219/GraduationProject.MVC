using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.ViewModels
{
    public class DepartmentReportVM
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public double AverageGPA { get; set; }
        public double PassRate { get; set; }
        public double FailRate { get; set; }
    }
}
