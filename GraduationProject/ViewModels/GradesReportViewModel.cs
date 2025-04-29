using GP.DAL.Dto;

namespace GraduationProject.ViewModels
{
    public class GradesReportViewModel
    {
        public string Department { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public List<StudentGradeDto> StudentGrades { get; set; } = new();
        public SemesterEvaluationSummaryDto Summary { get; set; } = new();
    }
}
