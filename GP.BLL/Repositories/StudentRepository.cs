﻿using GP.BLL.Interfaces;
using GP.DAL.Context;
using GP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UserManager<GPUser> userManager;
        private readonly AppDbContext context;
        public StudentRepository(UserManager<GPUser> userManager, AppDbContext _context)
        {
            this.userManager = userManager;
            context = _context;
        }
        public Student GetStudentByUserId(string UserId)
        {
            return context.Students.FirstOrDefault(f => f.UserId == UserId);
        }
        public Student GetStudentById(int Id)
        {
            return context.Students.Include(s=>s.Department).Include(s=>s.Advisor).FirstOrDefault(f => f.Id == Id);
        }
        public IEnumerable<Student> GetStudentsByLevel(int level)
        {
            return context.Students
                       .Where(s => s.Level == level)
                       .ToList();
        }
        public int AddStudent(Student student)
        {
            student.RegisterYear = DateTime.Now.Year;
            context.Add(student);
            return context.SaveChanges();
        }
        public int AddApplicationIdToStudent(int stdId, int applId)
        {
            var std = GetStudentById(stdId);
            std.ApplicationId = applId;
            return context.SaveChanges();
        }
        public Student GetStudentByApplicationId(int id)
        {
            return context.Students.Include(s => s.College).FirstOrDefault(s => s.ApplicationId == id);
        }
        public void UpdateStudent(Student student)
        {
            context.Students.Update(student);
            context.SaveChanges();
        }
        public async Task<int> UpdateStudentAsync(int Id, string Email, string Address, string MobilePhone)
        {
            var faculty = context.Students.FirstOrDefault(f => f.Id == Id);
            if (faculty == null)
            {
                return 0; // not found
            }

            await userManager.SetEmailAsync(faculty.User, Email);
            faculty.Address = Address;
            faculty.MobilePhone = MobilePhone;

            context.Students.Update(faculty);
            return context.SaveChanges(); // returns number of affected rows
        }
    }
}
