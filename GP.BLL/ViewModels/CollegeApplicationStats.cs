using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.ViewModels
{
    public class CollegeApplicationStats
    {
        public string CollegeName { get; set; } = string.Empty;
        public int TotalApplications { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
    }
}
