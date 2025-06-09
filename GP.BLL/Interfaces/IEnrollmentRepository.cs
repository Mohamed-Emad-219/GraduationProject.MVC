using GP.BLL.ViewModels;
using GP.DAL.Dto;
using GP.DAL.Models;
using GraduationProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IEnrollmentRepository
    {
        void EnrollPassedStudentsToNextTerm(int level);
        bool HasStudentPassedCourse(string studentId, string courseCode);
        Term GetLastTermForStudent(string studentId);
        int GetCompletedHoursForStudent(string studentId);
        IEnumerable<Enrollment> GetCompletedCoursesForStudent(string studentId);
        EnrollmentReportVM GetEnrollmentReport(string courseCode, SemesterType semester, int year);
        public Term GetNextTermForStudent(string studentId);
        public void EnrollStudentToNextTerm(string studentId);
        List<StudentGradeDto> GetStudentGrades(int year, SemesterType semester, string courseCode);
        SemesterEvaluationSummaryDto GetSemesterEvaluationSummary(int year, SemesterType semester, string courseCode);
        List<CourseReportVM> GetCoursesReport(int year, SemesterType semester);
        List<DepartmentReportVM> GetDepartmentsReport(int year, SemesterType semester);
    }
}
