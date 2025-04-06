using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GP.DAL.Models;
using GP.DAL.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GP.DAL.Context
{
    public class AppDbContext : IdentityDbContext<GPUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());


            #region Idenity
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
            #endregion  





            // College and dean (facultymembers) 1-1
            modelBuilder.Entity<College>()
                .HasOne(d => d.Dean)
                .WithOne()
                .HasForeignKey<College>(d => d.DeanId)
                .OnDelete(DeleteBehavior.Restrict);
            //college and department 1-m
            modelBuilder.Entity<College>()
                .HasMany(c => c.Departments)
                .WithOne(c => c.College)
                .HasForeignKey(c => c.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            //college and student 1-m
            modelBuilder.Entity<College>()
                .HasMany(c => c.Students)
                .WithOne(c => c.College)
                .HasForeignKey(c => c.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            //college and receipts 1-m
            modelBuilder.Entity<College>()
                .HasMany(c => c.Receipts)
                .WithOne(c => c.College)
                .HasForeignKey(c => c.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);
            // department and head (facultymember) 1-1
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Head)
                .WithOne()
                .HasForeignKey<Department>(d => d.HeadId)
                .OnDelete(DeleteBehavior.Restrict);
            // department and facultymember 1-m
            modelBuilder.Entity<Department>()
                .HasMany(d => d.FacultyMembers)
                .WithOne(fm => fm.Department)
                .HasForeignKey(fm => fm.DeptId)
                .OnDelete(DeleteBehavior.Restrict);
            // department and StudentSchedule 1-m
            modelBuilder.Entity<Department>()
                .HasMany(d => d.StudentSchedules)
                .WithOne(fm => fm.Department)
                .HasForeignKey(fm => fm.DeptId)
                .OnDelete(DeleteBehavior.Restrict);
            // department and course 1-m
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Courses)
                .WithOne(c => c.Department)
                .HasForeignKey(c => c.DeptId)
                .OnDelete(DeleteBehavior.Restrict);
            // students and department 1-m
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Students)
                .WithOne(c => c.Department)
                .HasForeignKey(c => c.DeptId)
                .OnDelete(DeleteBehavior.Restrict);
            // student affairs and receipts 1-m
            modelBuilder.Entity<StudentAffairs>()
                .HasMany(c => c.Receipts)
                .WithOne(c => c.StudentAffairs)
                .HasForeignKey(c => c.StudentAffairsId)
                .OnDelete(DeleteBehavior.Restrict);
            // manager and student affairs 1-1
            modelBuilder.Entity<StudentAffairs>()
                .HasOne(sa => sa.Manager)
                .WithMany(sa => sa.Subordinates)
                .HasForeignKey(sa => sa.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
            // financial affairs and receipts 1-m
            modelBuilder.Entity<FinancialAffairs>()
                .HasMany(c => c.Receipts)
                .WithOne(c => c.FinancialAffairs)
                .HasForeignKey(c => c.FinancialAffairsId)
                .OnDelete(DeleteBehavior.Restrict);
            // manager and financial affairs 1-1
            modelBuilder.Entity<FinancialAffairs>()
                .HasOne(sa => sa.Manager)
                .WithMany(sa => sa.Subordinates)
                .HasForeignKey(sa => sa.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
            // One-to-Many relationship between Course and Schedule
            modelBuilder.Entity<StudentSchedule>()
                .HasOne(s => s.Course)
                .WithMany(c => c.StudentSchedules)
                .HasForeignKey(s => s.CourseCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InstructorSchedule>()
                .HasOne(s => s.Course)
                .WithMany(c => c.InstructorSchedules)
                .HasForeignKey(s => s.CourseCode)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Instructor and Schedule
            modelBuilder.Entity<InstructorSchedule>()
                .HasOne(s => s.Instructor)
                .WithMany(fm => fm.InstructorSchedules)
                .HasForeignKey(s => s.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(s => s.Term)
                .WithMany(fm => fm.Enrollments)
                .HasForeignKey(s => s.TermId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many relationship between Assistant and Schedule
            modelBuilder.Entity<InstructorSchedule>()
                .HasOne(s => s.Assistant)
                .WithMany(fm => fm.AssistantSchedules)
                .HasForeignKey(s => s.AssistantId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Place and Schedule
            modelBuilder.Entity<StudentSchedule>()
                .HasOne(s => s.Place)
                .WithMany(p => p.StudentSchedules)
                .HasForeignKey(s => s.PlaceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InstructorSchedule>()
                .HasOne(s => s.Place)
                .WithMany(p => p.InstructorSchedules)
                .HasForeignKey(s => s.PlaceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                    .HasKey(e => new { e.StudentId, e.CourseCode });

            // Configure relationship between Student and Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship between Course and Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseCode)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship between Application and StudentAffairs
            modelBuilder.Entity<Application>()
                .HasOne(e => e.StudentAffairs)
                .WithMany(c => c.Applications)
                .HasForeignKey(e => e.StudentAffairsId)
                .OnDelete(DeleteBehavior.Restrict);

            // advisor and student 1-m
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Advisor)
                .WithMany(s => s.Students)
                .HasForeignKey(s => s.AdvisorId)
                .OnDelete(DeleteBehavior.Restrict);

            // application and student 1-m
            modelBuilder.Entity<Application>()
                .HasOne(d => d.Student)
                .WithOne()
                .HasForeignKey<Student>(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receipt>()
                .HasOne(s => s.Student)
                .WithMany(s => s.Receipts)
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // advisor to schedules 1-m
            modelBuilder.Entity<StudentSchedule>()
                .HasOne(s => s.Advisor)
                .WithMany(a => a.StudentSchedules)
                .HasForeignKey(s => s.AdvisorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InstructorSchedule>()
                .HasOne(s => s.Advisor)
                .WithMany(a => a.InstructorSchedules)
                .HasForeignKey(s => s.AdvisorId)
                .OnDelete(DeleteBehavior.Restrict);


            var dateConverter = new ValueConverter<DateOnly, DateTime>(
                v => v.ToDateTime(TimeOnly.MinValue), // Convert DateOnly -> DateTime
                v => DateOnly.FromDateTime(v));       // Convert DateTime -> DateOnly

            modelBuilder.Entity<Student>()
                .Property(e => e.BirthDate)
                .HasConversion(dateConverter)
                .HasColumnType("date");




            modelBuilder.Entity<Advisor>()
                .HasIndex(e => e.SSN)
                .IsUnique();
            modelBuilder.Entity<FollowUp>()
                .HasIndex(e => e.SSN)
                .IsUnique();
            modelBuilder.Entity<StudentAffairs>()
                .HasIndex(e => e.SSN)
                .IsUnique();
            modelBuilder.Entity<FinancialAffairs>()
                .HasIndex(e => e.SSN)
                .IsUnique();
            modelBuilder.Entity<Student>()
                .HasIndex(e => e.SSN)
                .IsUnique();
            modelBuilder.Entity<FacultyMember>()
                .HasIndex(e => e.SSN)
                .IsUnique();

            modelBuilder.Entity<CoursePrerequisite>()
                .HasKey(cp => new { cp.CourseCode, cp.PrerequisiteCode });

            modelBuilder.Entity<CoursePrerequisite>()
                .HasOne(cp => cp.Course)
                .WithMany(c => c.Prerequisites)
                .HasForeignKey(cp => cp.CourseCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CoursePrerequisite>()
                .HasOne(cp => cp.Prerequisite)
                .WithMany(c => c.RequiredFor)
                .HasForeignKey(cp => cp.PrerequisiteCode)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Application)
                .WithOne(a => a.Student)
                .HasForeignKey<Application>(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-One: GPUser → InstructorProfile
            modelBuilder.Entity<GPUser>()
                .HasOne(u => u.FacultyMember)
                .WithOne(i => i.User)
                .HasForeignKey<FacultyMember>(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-One: GPUser → StudentProfile
            modelBuilder.Entity<GPUser>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure unique UserId in Students
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.UserId)
                .IsUnique();


            // One-to-One: GPUser → AdvisorProfile
            modelBuilder.Entity<GPUser>()
                .HasOne(u => u.Advisor)
                .WithOne(a => a.User)
                .HasForeignKey<Advisor>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-One: GPUser → FinancialAffairs
            modelBuilder.Entity<GPUser>()
                .HasOne(u => u.FinancialAffairs)
                .WithOne(a => a.User)
                .HasForeignKey<FinancialAffairs>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-One: GPUser → StudentAffairs
            modelBuilder.Entity<GPUser>()
                .HasOne(u => u.StudentAffairs)
                .WithOne(a => a.User)
                .HasForeignKey<StudentAffairs>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-One: GPUser → AdminProfile
            modelBuilder.Entity<GPUser>()
                .HasOne(u => u.Admin)
                .WithOne(a => a.User)
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define User-Role relationship
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<GPUser>()
                .HasMany<IdentityUserRole<string>>(u => u.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<CoursesTerm>()
                    .HasKey(e => new { e.TermId, e.CourseCode });

            // Configure relationship between Term and CoursesTerm
            modelBuilder.Entity<CoursesTerm>()
                .HasOne(e => e.Term)
                .WithMany(s => s.CoursesTerms)
                .HasForeignKey(e => e.TermId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship between Course and CoursesTerm
            modelBuilder.Entity<CoursesTerm>()
                .HasOne(e => e.Course)
                .WithMany(c => c.CoursesTerms)
                .HasForeignKey(e => e.CourseCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ResultPetition>()
            .HasOne(rp => rp.PetitionCourse)  // Define the relationship with PetitionCourse
            .WithMany()  // This assumes no navigation property back to ResultPetition on PetitionCourse
            .HasForeignKey(rp => rp.PetitionCourseId)  // Specify the foreign key
            .OnDelete(DeleteBehavior.NoAction);  // Change cascading delete to NoAction

            modelBuilder.Entity<ResultPetition>()
                .HasOne(rp => rp.Advisor)  // Define the relationship with Advisor
                .WithMany()  // This assumes no navigation property back to ResultPetition on Advisor
                .HasForeignKey(rp => rp.AdvisorId)  // Specify the foreign key
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FollowUpEntry>()
                .HasOne(f => f.Schedule)
                .WithMany(s => s.FollowUps)
                .HasForeignKey(f => f.ScheduleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FollowUpEntry>()
                .HasOne(f => f.FollowUp)
                .WithMany(fu => fu.Entries)
                .HasForeignKey(f => f.FollowUpId)
                .OnDelete(DeleteBehavior.NoAction);

        }
        #region Models
        public DbSet<Advisor> Advisors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<CoursesTerm> CoursesTerms { get; set; }
        public DbSet<CoursePrerequisite> CoursePrerequisites { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<FacultyMember> FacultyMembers { get; set; }
        public DbSet<FinancialAffairs> FinancialAffairs { get; set; }
        public DbSet<FollowUp> FollowUps { get; set; }
        public DbSet<FollowUpEntry> FollowUpEntries { get; set; }
        public DbSet<InstructorSchedule> InstructorSchedules { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentAffairs> StudentAffairs { get; set; }
        public DbSet<StudentSchedule> StudentSchedules { get; set; } 

        public DbSet<PetitionRequest> PetitionRequests { get; set; }
        public DbSet<PetitionCourse> PetitionCourses { get; set; }

        public DbSet<ResultPetition> ResultPetitions { get; set; }

        #endregion

    }
}
