namespace GP.BLL.ViewModels
{
    public class EnrollmentReportVM
    {
        public string? CourseCode { get; set; }
        public string? CourseTitle { get; set; }
        public string? Instructor { get; set; }
        public int? Credits { get; set; }
        public List<EnrollmentViewModel>? Enrollments { get; set; }
        public int? TotalEnrolled { get; set; }
        public int? TotalCapacity { get; set; }
        public double? PercentageSeatsFilled { get; set; }
    }

    public class EnrollmentViewModel
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? Major { get; set; }
        public int? Level { get; set; }
    }
}
