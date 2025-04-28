using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace GP.BLL.Repositories
{
    public class InstructorScheduleRepositroy : IInstructorScheduleRepositroy
    {
        private readonly AppDbContext _dbContext;

        public InstructorScheduleRepositroy(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<InstructorSchedule> GetInstructorScheduleByInstructorId(string InstructorId)
        {
            var month = DateTime.Now.Month;
            SemesterType semester;

            if (month >= 9 || month <= 1)
            {
                semester = SemesterType.Fall;
            }
            else if (month >= 2 && month <= 6)
            {
                semester = SemesterType.Spring;
            }
            else
            {
                semester = SemesterType.Fall;
            }
            return _dbContext.InstructorSchedules
                .Where(s => s.TeacherId == InstructorId && s.TeacherId.StartsWith("I") && s.Semester == semester)
                .Include(s => s.Course)
                .Include(s => s.Place)
                .Include(s => s.FacultyMember)
                .ToList();
        }
        public IEnumerable<InstructorSchedule> GetSchedule(string TeacherId)
        {
            var month = DateTime.Now.Month;
            SemesterType semester;

            if (month >= 9 || month <= 1)
            {
                semester = SemesterType.Fall;
            }
            else if (month >= 2 && month <= 6)
            {
                semester = SemesterType.Spring;
            }
            else
            {
                semester = SemesterType.Fall;
            }
            return _dbContext.InstructorSchedules
                .Where(s => s.TeacherId == TeacherId && s.Semester == semester)
                .Include(s => s.Course)
                .Include(s => s.Place)
                .Include(s => s.FacultyMember)
                .ToList();
        }
        
        public IEnumerable<InstructorSchedule> GetAssistantScheduleByAssistantId(string AssistantId)
        {
            var month = DateTime.Now.Month;
            SemesterType semester;

            if (month >= 9 || month <= 1)
            {
                semester = SemesterType.Fall;
            }
            else if (month >= 2 && month <= 6)
            {
                semester = SemesterType.Spring;
            }
            else
            {
                semester = SemesterType.Fall;
            }
            return _dbContext.InstructorSchedules
                .Where(s => s.TeacherId == AssistantId && s.TeacherId.StartsWith("TA") && s.Semester == semester)
                .Include(s => s.Course)
                .Include(s => s.Place)
                .Include(s => s.FacultyMember)
                .ToList();
        }
    }
}
