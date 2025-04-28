using GP.BLL.ViewModels;

namespace GraduationProject.ViewModels
{
    public class AdmissionsDashboardViewModel
    {
        public int TotalApplications { get; set; }
        public int TotalApproved { get; set; }
        public int TotalRejected { get; set; }
        public List<CollegeApplicationStats> CollegeApplicationStats { get; set; } = new();
        public GenderDistribution GenderDistribution { get; set; } = new();
    }
}
