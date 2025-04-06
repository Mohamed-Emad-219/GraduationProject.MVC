using GP.BLL.Interfaces;
using GP.BLL.ViewModels;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ITermRepository _termRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITermCourseRepository _termCourseRepository;
        private readonly AppDbContext _context;

        public EnrollmentRepository(
            IStudentRepository studentRepository,
            ITermRepository termRepository,
            ICourseRepository courseRepository,
            ITermCourseRepository termCourseRepository,
            AppDbContext context)
        {
            _studentRepository = studentRepository;
            _termRepository = termRepository;
            _courseRepository = courseRepository;
            _context = context;
            _termCourseRepository = termCourseRepository;
        }
        public Term GetLastTermForStudent(int studentId)
        {
            return _context.Enrollments
                      .Where(e => e.StudentId == studentId)
                      .OrderByDescending(e => e.Term.AcademicYear) // Latest year first
                      .ThenByDescending(e => e.Term.Semester) // Latest semester first
                      .Select(e => e.Term)
                      .FirstOrDefault();
        }
        public void EnrollPassedStudentsToNextTerm(int level)
        {
            // 1. Get students in the given level
            var students = _studentRepository.GetStudentsByLevel(level);

            // 2. Iterate through each student
            foreach (var student in students)
            {
                // 3. Get last term the student took
                var lastTerm = GetLastTermForStudent(student.Id);
                if (lastTerm == null) continue; // Skip if no term data

                // 4. Check if the student passed all courses in last term
                var courses = _termCourseRepository.GetCoursesPerTerm(lastTerm.Id);
                bool hasPassedAll = courses.All(course =>
                    HasStudentPassedCourse(student.Id, course.Code));

                // 5. If passed all, enroll in next semester courses
                if (hasPassedAll)
                {
                    var nextTerm = GetNextTerm(lastTerm);
                    var nextCourses = _termCourseRepository.GetCoursesPerTerm(nextTerm.Id);

                    foreach (var course in nextCourses)
                    {
                        var enrollment = new Enrollment
                        {
                            StudentId = student.Id,
                            CourseCode = course.Code,
                            TermId = nextTerm.Id,
                            Grade = "F"
                        };
                        _context.Enrollments.Add(enrollment);
                    }
                }
            }

            _context.SaveChanges(); // Save to DB
        }

        private Term GetNextTerm(Term lastTerm)
        {
            SemesterType nextSemester = lastTerm.Semester == SemesterType.Spring ? SemesterType.Fall : lastTerm.Semester = SemesterType.Spring;
            int nextAcademicYear = lastTerm.Semester == SemesterType.Spring ? lastTerm.AcademicYear + 1 : lastTerm.AcademicYear;

            return _termRepository.GetTermByDetails(lastTerm.Level, nextSemester, nextAcademicYear);
        }
        public bool HasStudentPassedCourse(int studentId, string courseCode)
        {
            var enrollment = _context.Enrollments
                .FirstOrDefault(e => e.StudentId == studentId && e.CourseCode == courseCode);

            if (enrollment == null)
                return false; // Student has not taken this course

            return IsPassingGrade(enrollment.Grade);
        }

        private bool IsPassingGrade(string grade)
        {
            var passingGrades = new HashSet<string> { "A", "A+","A-","B+", "B","B-","C+", "C","C-" ,"D+", "D" };
            return passingGrades.Contains(grade.ToUpper());
        }

        public int GetCompletedHoursForStudent(int studentId)
        {
            return _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .Sum(e => e.Course.CreditHour);
        }
        public IEnumerable<Enrollment> GetCompletedCoursesForStudent(int studentId)
        {
            return _context.Enrollments
           .Where(e => e.StudentId == studentId)
           .Include(e => e.Course)
           .Include(e => e.Term)
           .ToList();
        }
        public EnrollmentReportVM GetEnrollmentReport(string courseCode, SemesterType semester, int year)
        {
            var term = _termRepository.GetTermBySemesterYear(semester, year);
            

            var enrollments = term.Enrollments
                                  .Where(e => e.Course.Code == courseCode)
                                  .ToList();

            var totalEnrolled = enrollments.Count;
            var totalCapacity = enrollments.Count;  // Assuming total seats = enrolled + waitlist
            var percentageFilled = totalCapacity > 0 ? (totalEnrolled * 100) / totalCapacity : 0;

            var e =  new EnrollmentReportVM
            {
                CourseCode = courseCode,
                CourseTitle = enrollments.FirstOrDefault()?.Course.Name ?? "N/A",
                Instructor = enrollments.FirstOrDefault()?.Course.InstructorSchedules
                .Select(i => $"{i.Instructor.FirstName} {i.Instructor.MiddleName} {i.Instructor.LastName}")
                .FirstOrDefault() ?? "N/A",
                Credits = enrollments.FirstOrDefault()?.Course.CreditHour ?? 0,
                Enrollments = enrollments.Select(e => new EnrollmentViewModel
                {
                    StudentId = e.StudentId,
                    StudentName = $"{e.Student.FirstName} {e.Student.MiddleName} {e.Student.LastName}",
                    Major = e.Student.Department.Name,
                    Level = e.Student.Level
                }).ToList(),
                TotalEnrolled = totalEnrolled,
                TotalCapacity = totalCapacity,
                PercentageSeatsFilled = percentageFilled
            };
            return e;
        }


    }
}
