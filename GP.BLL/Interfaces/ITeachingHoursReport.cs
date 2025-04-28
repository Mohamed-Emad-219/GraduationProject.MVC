using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.BLL.ViewModels;
using GP.DAL.Models;
using GraduationProject.ViewModels;

namespace GP.BLL.Interfaces
{
    public interface ITeachingHoursReport
    {
        public List<FacultyMember> GetFacultyMembersDean();
        public List<FacultyMember> GetFacultyMembersHead(int? DepId);
        public List<CourseDisplayVM> GetCoursesHead(int? depid, SemesterType semester, int year);
        public List<CourseDisplayVM> GetCoursesDean(SemesterType semester, int year);
        public int TotalFacultyMembers();
        public int TotalTeachingHoursAssigned();
        public int OverloadTeachingHours();
    }
}
