using GP.BLL.ViewModels;
using GP.DAL.Models;
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
        bool HasStudentPassedCourse(int studentId, string courseCode);
        Term GetLastTermForStudent(int studentId);
        int GetCompletedHoursForStudent(int studentId);
        IEnumerable<Enrollment> GetCompletedCoursesForStudent(int studentId);
        EnrollmentReportVM GetEnrollmentReport(string courseCode, SemesterType semester, int year);
    }
}
