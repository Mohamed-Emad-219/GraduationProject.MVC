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
    }
}
