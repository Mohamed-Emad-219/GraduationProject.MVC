﻿using System;
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
            // followup and followupschedules 1-m
            modelBuilder.Entity<FollowUp>()
                .HasMany(f=>f.FollowUpSchedules)
                .WithOne(c => c.FollowUp)
                .HasForeignKey(c => c.FollowUpId)
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
            modelBuilder.Entity<FollowUpSchedule>()
                .HasOne(s => s.Course)
                .WithMany(c => c.FollowUpSchedules)
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
            modelBuilder.Entity<FollowUpSchedule>()
                .HasOne(s => s.Place)
                .WithMany(p => p.FollowUpSchedules)
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

            
        }
        #region Models
        public DbSet<Advisor> Advisors { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CoursePrerequisite> CoursePrerequisites { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<FacultyMember> FacultyMembers { get; set; }
        public DbSet<FinancialAffairs> FinancialAffairs { get; set; }
        public DbSet<FollowUp> FollowUps { get; set; }
        public DbSet<FollowUpSchedule> FollowUpSchedules { get; set; }
        public DbSet<InstructorSchedule> InstructorSchedules { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentAffairs> StudentAffairs { get; set; }
        public DbSet<StudentSchedule> StudentSchedules { get; set; } 
        #endregion
        //public DbSet<GPUser> GPUsers { get; set; }
    }
}
