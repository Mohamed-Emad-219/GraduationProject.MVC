﻿using GP.BLL.Interfaces;
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
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _dbContext;

        public CourseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Course> GetCourses()
        {
            var result = _dbContext.Courses.Include(c => c.Department).AsNoTracking().ToList();
            return result;
        }
        public IEnumerable<CourseDTO> GetCoursesNameCode()
        {
            return _dbContext.Courses
        .Select(c => new CourseDTO { Value = c.Code,Text = c.Name }) // Avoid circular references
        .ToList();
        }
        public async Task<Course> GetCourseById(string id)
        {
            var course = await _dbContext.Courses.FindAsync(id);//// find op search in cache if found return it else search in database
            return course;
        }
        public int AddCourse(Course course)
        {
            _dbContext.Add(course);
            return _dbContext.SaveChanges();
        }
        public int UpdateCourse(Course course)
        {
            _dbContext.Courses.Update(course);
            return _dbContext.SaveChanges();
        }
        public async Task<int> DeleteCourseAsync(string Code)
        {
            var course = await GetCourseById(Code);
            _dbContext.Remove(course);
            return _dbContext.SaveChanges();
        }
    }
}
