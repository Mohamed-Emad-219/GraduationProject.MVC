using GP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Interfaces
{
    public interface IStudentRepository
    {
        Student GetStudentByUserId(string UserId);
        Student GetStudentById(int Id);
        Student GetStudentByApplicationId(int id);
        IEnumerable<Student> GetStudentsByLevel(int level);
        int AddStudent(Student student);
        int AddApplicationIdToStudent(int stdId, int applId);
        void UpdateStudent(Student student);
    }
}
