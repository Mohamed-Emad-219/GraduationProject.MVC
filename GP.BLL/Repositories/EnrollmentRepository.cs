using GP.BLL.Interfaces;
using GP.BLL.ViewModels;
using GP.DAL.Context;
using GP.DAL.Dto;
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
                      .ThenBy(e => e.Term.Semester) // Latest semester first
                      .Select(e => e.Term)
                      .FirstOrDefault();
        }
        public void EnrollStudentToNextTerm(int studentId)
        {
            var student = _studentRepository.GetStudentById(studentId);
            var lastTerm = GetLastTermForStudent(studentId);
            var nextTerm = GetNextTermForStudent(studentId); 

            if (nextTerm == null) return;

            var nextCourses = _termCourseRepository.GetCoursesPerTerm(nextTerm.Id, student.Level, student.DeptId);

            foreach (var course in nextCourses)
            {
                var enrollment = new Enrollment
                {
                    StudentId = studentId,
                    CourseCode = course.Code,
                    TermId = nextTerm.Id,
                    Grade = "F" // not graded yet
                };
                _context.Enrollments.Add(enrollment);
            }

            _context.SaveChanges();
        }
        public Term GetNextTermForStudent(int studentId)
        {
            var lastTerm = GetLastTermForStudent(studentId);

            if (lastTerm == null)
            {
                // NEW STUDENT
                int currentYear = DateTime.Now.Year;
                var term = _termRepository.GetTermByDetails(1, SemesterType.Fall, currentYear);
                return term;
            }
            else
            {
                // EXISTING STUDENT
                SemesterType nextSemester = (lastTerm.Semester == SemesterType.Spring)
                    ? SemesterType.Fall
                    : SemesterType.Spring;

                int nextAcademicYear = (nextSemester == SemesterType.Spring)
                    ? lastTerm.AcademicYear + 1
                    : lastTerm.AcademicYear;

                int level = (lastTerm.Semester == SemesterType.Spring) 
                    ? lastTerm.Level + 1 
                    : lastTerm.Level;
                var term = _termRepository.GetTermByDetails(level, nextSemester, nextAcademicYear);
                return term;
            }
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
                var courses = _termCourseRepository.GetCoursesPerTerm(lastTerm.Id, student.Level, student.DeptId);
                bool hasPassedAll = courses.All(course =>
                    HasStudentPassedCourse(student.Id, course.Code));

                // 5. If passed all, enroll in next semester courses
                if (hasPassedAll)
                {
                    var nextTerm = GetNextTerm(lastTerm);
                    var nextCourses = _termCourseRepository.GetCoursesPerTerm(nextTerm.Id, student.Level, student.DeptId);

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
            .Sum(e => e.Course.CreditHour ?? 0);
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
                CourseTitle = enrollments.FirstOrDefault()?.Course.CourseName ?? "N/A",
                Instructor = enrollments.FirstOrDefault()?.Course.InstructorSchedules
                .Select(i => $"{i.FacultyMember.FirstName} {i.FacultyMember.MiddleName} {i.FacultyMember.LastName}")
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

        public List<StudentGradeDto> GetStudentGrades(int year, SemesterType semester, string courseCode)
        {
            return _context.Enrollments
                .Where(e => e.Term.Semester == semester && e.Term.AcademicYear == year && e.CourseCode == courseCode)
                .Select(e => new StudentGradeDto
                {
                    StudentId = e.StudentId,
                    StudentName = e.Student.FirstName + " " + e.Student.MiddleName + " " + e.Student.LastName,
                    Grade = e.Grade,
                    GPA = CalculateGPA(e.Grade)
                })
                .ToList();
        }

        public SemesterEvaluationSummaryDto GetSemesterEvaluationSummary(int year, SemesterType semester, string courseCode)
        {
            var grades = _context.Enrollments
                .Where(e => e.Term.Semester == semester && e.Term.AcademicYear == year && e.CourseCode == courseCode)
                .Select(e => new
                {
                    Name = e.Student.FirstName + " " + e.Student.MiddleName + " " + e.Student.LastName,
                    Grade = e.Grade,
                    GPA = CalculateGPA(e.Grade)
                })
                .ToList();

            if (!grades.Any())
                return new SemesterEvaluationSummaryDto();

            var highest = grades.OrderByDescending(g => g.GPA).First();
            var lowest = grades.OrderBy(g => g.GPA).First();
            var averageGPA = grades.Average(g => g.GPA);
            var passCount = grades.Count(g => g.GPA >= 2.0); // Assuming pass if GPA >= 2.0
            var failCount = grades.Count(g => g.GPA < 2.0);
            var total = grades.Count;

            return new SemesterEvaluationSummaryDto
            {
                HighestGPAName = highest.Name,
                HighestGPAGrade = highest.Grade,
                LowestGPAName = lowest.Name,
                LowestGPAGrade = lowest.Grade,
                AverageGPA = Math.Round(averageGPA, 2),
                PassRate = Math.Round(passCount * 100.0 / total, 2),
                FailureRate = Math.Round(failCount * 100.0 / total, 2)
            };
        }

        private static double CalculateGPA(string grade)
        {
            // Simple example, you can customize as needed
            return grade switch
            {
                "A+" => 4.0,
                "A" => 3.8,
                "A-" => 3.6,
                "B+" => 3.4,
                "B" => 3.2,
                "B-" => 3.0,
                "C+" => 2.8,
                "C" => 2.6,
                "C-" => 2.4,
                "D+" => 2.2,
                "D" => 2.0,
                "F" => 0.0,
                _ => 0.0 // In case grade is invalid or missing
            };

        }
    }
}
