using GP.DAL.Context;
using GP.DAL.Dto;
using GP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GP.DAL.Seed
{
    public class DbInitializer
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "User",
                "Advisor",
                "Admin",
                "Student",
                "FinancialAffairs",
                "ManagerOfFinancialAffairs",
                "StudentAffairs",
                "ManagerOfStudentAffairs",
                "Instructor",
                "Assistant",
                "Head",
                "Dean",
                "FollowUp" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            await Task.Delay(5000);
            
        }
        public static async Task CreateAdvisors(UserManager<GPUser> userManager, IServiceProvider serviceProvider, IHostEnvironment env)
        {
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            // Get the absolute path to the JSON file in wwwroot
            string filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "advisor.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: JSON file not found at " + filePath);
                return;
            }

            // Read JSON file
            string jsonData = await File.ReadAllTextAsync(filePath);

            // Deserialize JSON to List<AdvisorProfile>
            var advisors = JsonConvert.DeserializeObject<List<Advisor>>(jsonData);
            //Console.WriteLine(advisors);

            foreach (var advisor in advisors)
            {
                string email = $"{advisor.FirstName.ToLower()}.{advisor.LastName.ToLower()}@g.com";
                //Console.WriteLine(email);
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new GPUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };
                    
                    var result = await userManager.CreateAsync(user, "qweQWE123!!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Advisor");

                        // Insert Advisor
                        advisor.UserId = user.Id;
                        dbContext.Advisors.Add(advisor);
                    }
                    else
                    {
                        Console.WriteLine("User creation failed for: " + email);
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
                        }
                    }
                }else
                {
                        Console.WriteLine($"User with username {email} already exists.");
                        continue;
                }
            }
            await dbContext.SaveChangesAsync();
            await Task.Delay(5000);
        }
        public static async Task CreateAdmins(UserManager<GPUser> userManager, IServiceProvider serviceProvider, IHostEnvironment env)
        {
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            // Get the absolute path to the JSON file in wwwroot
            string filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "admins.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: JSON file not found at " + filePath);
                return;
            }

            // Read JSON file
            string jsonData = await File.ReadAllTextAsync(filePath);

            // Deserialize JSON to List<AdvisorProfile>
            var admins = JsonConvert.DeserializeObject<List<Admin>>(jsonData);
            //Console.WriteLine(advisors);

            foreach (var adm in admins)
            {
                string email = $"{adm.FirstName.ToLower()}.{adm.LastName.ToLower()}@g.com";
                //Console.WriteLine(email);
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new GPUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "qweQWE123!!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");

                        adm.UserId = user.Id;
                        dbContext.Admins.Add(adm);
                    }
                    else
                    {
                        Console.WriteLine("User creation failed for: " + email);
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"User with username {email} already exists.");
                    continue;
                }
            }
            await dbContext.SaveChangesAsync();
            await Task.Delay(5000);
        }
        public static void SeedCollege(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "college.json");
            if (!context.Colleges.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<College>>(jsonData);

                if (rooms != null)
                {
                    context.Colleges.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static async Task SeedFacultyWithoutDept(UserManager<GPUser> userManager, AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "facultymemberswithoutdept.json");
            string jsonData = await File.ReadAllTextAsync(filePath);

            // Deserialize JSON to List<AdvisorProfile>
            var fs = JsonConvert.DeserializeObject<List<FacultyMember>>(jsonData);
            //Console.WriteLine(advisors);

            foreach (var f in fs)
            {
                string email = $"{f.FirstName.ToLower()}.{f.LastName.ToLower()}@g.com";
                //Console.WriteLine(email);
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new GPUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "qweQWE123!!");
                    if (result.Succeeded)
                    {
                        if(f.WorkingHours == 3)
                            await userManager.AddToRoleAsync(user, "Dean");
                        if (f.WorkingHours == 6)
                            await userManager.AddToRoleAsync(user, "Head");

                        // Insert Advisor
                        f.UserId = user.Id;
                        context.FacultyMembers.Add(f);
                    }
                    else
                    {
                        Console.WriteLine("User creation failed for: " + email);
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"User with username {email} already exists.");
                    continue;
                }
            }
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            await Task.Delay(5000);
        }
        //public static async Task SeedFacultyWithDept(UserManager<GPUser> userManager, AppDbContext context, IHostEnvironment env)
        //{
        //    var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "facultymemberswithdep.json");
        //    string jsonData = await File.ReadAllTextAsync(filePath);

        //    // Deserialize JSON to List<AdvisorProfile>
        //    var fs = JsonConvert.DeserializeObject<List<FacultyMember>>(jsonData);
        //    //Console.WriteLine(advisors);

        //    foreach (var f in fs)
        //    {
        //        string email = $"{f.FirstName.ToLower()}.{f.LastName.ToLower()}@g.com";
        //        //Console.WriteLine(email);
        //        if (await userManager.FindByEmailAsync(email) == null)
        //        {
        //            var user = new GPUser
        //            {
        //                UserName = email,
        //                Email = email,
        //                EmailConfirmed = true
        //            };

        //            var result = await userManager.CreateAsync(user, "qweQWE123!!");
        //            if (result.Succeeded)
        //            {
        //                if (f.WorkingHours == 3)
        //                    await userManager.AddToRoleAsync(user, "Dean");
        //                if (f.WorkingHours == 6)
        //                    await userManager.AddToRoleAsync(user, "Head");

        //                // Insert Advisor
        //                f.UserId = user.Id;
        //                context.FacultyMembers.Add(f);
        //            }
        //            else
        //            {
        //                Console.WriteLine("User creation failed for: " + email);
        //                foreach (var error in result.Errors)
        //                {
        //                    Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine($"User with username {email} already exists.");
        //            continue;
        //        }
        //    }
        //    await context.SaveChangesAsync();
        //}
        public static async Task SeedFacultyWithDept(UserManager<GPUser> userManager, AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "facultymemberswithdep.json");

            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                var facultyMembers = System.Text.Json.JsonSerializer.Deserialize<List<FacultyMember>>(jsonData,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (facultyMembers != null && facultyMembers.Any())
                {
                    foreach (var member in facultyMembers)
                    {
                        var existingMember = context.FacultyMembers.FirstOrDefault(fm => fm.Id == member.Id);

                        if (existingMember != null)
                        {
                            // Update existing record
                            existingMember.DeptId = member.DeptId;
                        }
                        else
                        {
                            member.FirstName = "Mohamed";
                            member.MiddleName = "Mohamed";
                            member.LastName = "Ali";
                            string email = $"{member.FirstName.ToLower()}.{member.LastName.ToLower()}@g.com";
                            // Add new faculty member
                            var user = new GPUser
                            {
                                UserName = email,
                                Email = email,
                                EmailConfirmed = true
                            };

                            var result = await userManager.CreateAsync(user, "qweQWE123!!");
                            if (result.Succeeded)
                            {
                                member.WorkingHours = 6;
                               await userManager.AddToRoleAsync(user, "Head");
                                member.Address = "Unknown";
                                member.SSN = "5423698214587692";
                                member.MobilePhone = "02365142982";
                                // Insert Advisor
                                member.UserId = user.Id;
                                member.Id = 0;
                                member.DeptId = 10;
                                context.FacultyMembers.Add(member);
                            }
                        }
                    }

                    await context.SaveChangesAsync();
                    await Task.Delay(5000);
                }
            }
            else
            {
                Console.WriteLine("Error: JSON file not found at " + filePath);
            }
        }
        public static void SeedDapertment(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "departments.json");
            if (!context.Departments.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<Department>>(jsonData);

                if (rooms != null)
                {
                    context.Departments.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static void SeedCourses(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "courses.json");
            if (!context.Courses.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<Course>>(jsonData);

                if (rooms != null)
                {
                    context.Courses.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static void SeedCoursesPre(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "courseprerequists.json");
            if (!context.CoursePrerequisites.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<CoursePrerequisite>>(jsonData);

                if (rooms != null)
                {
                    context.CoursePrerequisites.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static void SeedPlace(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "places.json");
            if (!context.Places.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<Place>>(jsonData);

                if (rooms != null)
                {
                    context.Places.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static async Task SeedFollowUp(UserManager<GPUser> userManager, AppDbContext context, IHostEnvironment env)
        {
            string filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "followups.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: JSON file not found at " + filePath);
                return;
            }

            // Read JSON file
            string jsonData = await File.ReadAllTextAsync(filePath);

            // Deserialize JSON to List<AdvisorProfile>
            var fs = JsonConvert.DeserializeObject<List<FollowUp>>(jsonData);
            //Console.WriteLine(advisors);

            foreach (var advisor in fs)
            {
                string email = $"{advisor.FirstName.ToLower()}.{advisor.LastName.ToLower()}@g.com";
                //Console.WriteLine(email);
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new GPUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "qweQWE123!!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "FollowUp");

                        // Insert Advisor
                        advisor.UserId = user.Id;
                        context.FollowUps.Add(advisor);
                    }
                    else
                    {
                        Console.WriteLine("User creation failed for: " + email);
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"User with username {email} already exists.");
                    continue;
                }
            }
            await context.SaveChangesAsync();
            await Task.Delay(5000);
        }
        public static async Task SeedStudentAffairs(UserManager<GPUser> userManager,AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "studentaffairs.json");
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: JSON file not found at " + filePath);
                return;
            }
            if (!context.StudentAffairs.Any()) // Prevent duplicate seeding
            {

                // Read JSON file
                string jsonData = await File.ReadAllTextAsync(filePath);

                // Deserialize JSON to List<AdvisorProfile>
                var sfs = JsonConvert.DeserializeObject<List<StudentAffairs>>(jsonData);
                //Console.WriteLine(advisors);
                var managers = sfs.Where(r => r.ManagerId == null).ToList();
                var employees = sfs.Where(r => r.ManagerId != null).ToList();
                foreach (var s in managers)
                {
                    string email = $"{s.FirstName.ToLower()}.{s.LastName.ToLower()}@g.com";
                    //Console.WriteLine(email);
                    if (await userManager.FindByEmailAsync(email) == null)
                    {
                        var user = new GPUser
                        {
                            UserName = email,
                            Email = email,
                            EmailConfirmed = true
                        };

                        var result = await userManager.CreateAsync(user, "qweQWE123!!");
                        if (result.Succeeded)
                        {
                            
                            await userManager.AddToRoleAsync(user, "ManagerOfStudentAffairs");

                            // Insert Advisor
                            s.UserId = user.Id;
                            context.StudentAffairs.Add(s);
                        }
                        else
                        {
                            Console.WriteLine("User creation failed for: " + email);
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User with username {email} already exists.");
                        continue;
                    }
                }
                foreach (var s in employees)
                {
                    string email = $"{s.FirstName.ToLower()}.{s.LastName.ToLower()}@g.com";
                    //Console.WriteLine(email);
                    if (await userManager.FindByEmailAsync(email) == null)
                    {
                        var user = new GPUser
                        {
                            UserName = email,
                            Email = email,
                            EmailConfirmed = true
                        };

                        var result = await userManager.CreateAsync(user, "qweQWE123!!");
                        if (result.Succeeded)
                        {

                            await userManager.AddToRoleAsync(user, "StudentAffairs");

                            // Insert Advisor
                            s.UserId = user.Id;
                            context.StudentAffairs.Add(s);
                        }
                        else
                        {
                            Console.WriteLine("User creation failed for: " + email);
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User with username {email} already exists.");
                        continue;
                    }
                }
                await context.SaveChangesAsync();
                await Task.Delay(5000);

            }
        }
        public static async Task SeedFinancialAffairs(UserManager<GPUser> userManager, AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "financialaffairs.json");
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: JSON file not found at " + filePath);
                return;
            }
            if (!context.FinancialAffairs.Any()) // Prevent duplicate seeding
            {

                // Read JSON file
                string jsonData = await File.ReadAllTextAsync(filePath);

                // Deserialize JSON to List<AdvisorProfile>
                var sfs = JsonConvert.DeserializeObject<List<FinancialAffairs>>(jsonData);
                //Console.WriteLine(advisors);
                var managers = sfs.Where(r => r.ManagerId == null).ToList();
                var employees = sfs.Where(r => r.ManagerId != null).ToList();
                foreach (var s in managers)
                {
                    string email = $"{s.FirstName.ToLower()}.{s.LastName.ToLower()}@g.com";
                    //Console.WriteLine(email);
                    if (await userManager.FindByEmailAsync(email) == null)
                    {
                        var user = new GPUser
                        {
                            UserName = email,
                            Email = email,
                            EmailConfirmed = true
                        };

                        var result = await userManager.CreateAsync(user, "qweQWE123!!");
                        if (result.Succeeded)
                        {

                            await userManager.AddToRoleAsync(user, "ManagerOfFinancialAffairs");

                            // Insert Advisor
                            s.UserId = user.Id;
                            context.FinancialAffairs.Add(s);
                        }
                        else
                        {
                            Console.WriteLine("User creation failed for: " + email);
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User with username {email} already exists.");
                        continue;
                    }
                }
                foreach (var s in employees)
                {
                    string email = $"{s.FirstName.ToLower()}.{s.LastName.ToLower()}@g.com";
                    //Console.WriteLine(email);
                    if (await userManager.FindByEmailAsync(email) == null)
                    {
                        var user = new GPUser
                        {
                            UserName = email,
                            Email = email,
                            EmailConfirmed = true
                        };

                        var result = await userManager.CreateAsync(user, "qweQWE123!!");
                        if (result.Succeeded)
                        {

                            await userManager.AddToRoleAsync(user, "FinancialAffairs");

                            // Insert Advisor
                            s.UserId = user.Id;
                            context.FinancialAffairs.Add(s);
                        }
                        else
                        {
                            Console.WriteLine("User creation failed for: " + email);
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User with username {email} already exists.");
                        continue;
                    }
                }
                await context.SaveChangesAsync();
                await Task.Delay(5000);

            }
        }
        public static async Task SeedStudents(UserManager<GPUser> userManager, AppDbContext context, IHostEnvironment env)
        {
            string filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "students.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine("❌ Error: JSON file not found at " + filePath);
                return;
            }

            // Read JSON file asynchronously
            string jsonData = await File.ReadAllTextAsync(filePath);

            // Deserialize JSON to List<StudentDTO>
            var studentsDto = JsonConvert.DeserializeObject<List<StudentDTO>>(jsonData);

            if (studentsDto == null || studentsDto.Count == 0)
            {
                Console.WriteLine("⚠ No students found in JSON file.");
                return;
            }

            foreach (var dto in studentsDto)
            {
                string email = $"{dto.FirstName.ToLower()}.{dto.LastName.ToLower()}@g.com";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new GPUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "qweQWE123!!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Student");

                        // Insert Student
                        var student = new Student
                        {
                            FirstName = dto.FirstName,
                            MiddleName = dto.MiddleName,
                            LastName = dto.LastName,
                            BirthDate = DateOnly.FromDateTime(dto.BirthDate),
                            Address = dto.Address,
                            AdvisorId = dto.AdvisorId,
                            DeptId = dto.DeptId,
                            Level = dto.Level,
                            HighSchoolGrade = dto.HighSchoolGrade,
                            Gender = dto.Gender,
                            MobilePhone = dto.MobilePhone,
                            HomePhone = dto.HomePhone,
                            SSN = dto.SSN,
                            RegisterYear = dto.RegisterYear,
                            UserId = user.Id // Associate student with user
                        };

                        context.Students.Add(student);
                    }
                    else
                    {
                        Console.WriteLine("⚠ User creation failed for: " + email);
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"ℹ User with username {email} already exists.");
                    continue;
                }
            }

            await context.SaveChangesAsync();
            await Task.Delay(5000);
        }
        public static void SeedReceipts(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "receipts.json");
            if (!context.Receipts.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<Receipt>>(jsonData);

                if (rooms != null)
                {
                    context.Receipts.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static void SeedTerms(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "terms.json");
            if (!context.Terms.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<Term>>(jsonData);

                if (rooms != null)
                {
                    context.Terms.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static void SeedCoursesTerms(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "coursesterms.json");
            if (!context.CoursesTerms.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<CoursesTerm>>(jsonData);

                if (rooms != null)
                {
                    context.CoursesTerms.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static void SeedApplications(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "application.json");
            if (!context.Applications.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<Models.Application>>(jsonData);

                if (rooms != null)
                {
                    context.Applications.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static void SeedEnrollments(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "enrollment.json");
            if (!context.Enrollments.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<Enrollment>>(jsonData);

                if (rooms != null)
                {
                    context.Enrollments.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static async Task SeedInstructorAssistants(UserManager<GPUser> userManager,AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "inst-assis.json");
            string jsonData = await File.ReadAllTextAsync(filePath);

            // Deserialize JSON to List<AdvisorProfile>
            var fs = JsonConvert.DeserializeObject<List<FacultyMember>>(jsonData);
            //Console.WriteLine(advisors);

            foreach (var f in fs)
            {
                string email = $"{f.FirstName.ToLower()}.{f.LastName.ToLower()}@g.com";
                //Console.WriteLine(email);
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new GPUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, "qweQWE123!!");
                    if (result.Succeeded)
                    {
                        if (f.WorkingHours == 28)
                            await userManager.AddToRoleAsync(user, "Assistant");
                        if (f.WorkingHours == 6)
                            await userManager.AddToRoleAsync(user, "Instructor");

                        // Insert Advisor
                        f.UserId = user.Id;
                        context.FacultyMembers.Add(f);
                    }
                    else
                    {
                        Console.WriteLine("User creation failed for: " + email);
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"➡ Error: {error.Code} - {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"User with username {email} already exists.");
                    continue;
                }
            }
            await context.SaveChangesAsync();
            await Task.Delay(5000);
        }
        public static void SeedInstructorSchedules(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "instructorschedule.json");
            if (!context.InstructorSchedules.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<InstructorSchedule>>(jsonData);

                if (rooms != null)
                {
                    context.InstructorSchedules.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static void SeedStudentSchedules(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "studentschedule.json");
            if (!context.StudentSchedules.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<StudentSchedule>>(jsonData);

                if (rooms != null)
                {
                    context.StudentSchedules.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        public static void SeedFollowUpSchedules(AppDbContext context, IHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "json", "followupschedule.json");
            if (!context.FollowUpSchedules.Any()) // Prevent duplicate seeding
            {
                var jsonData = File.ReadAllText(filePath);
                var rooms = System.Text.Json.JsonSerializer.Deserialize<List<FollowUpSchedule>>(jsonData);

                if (rooms != null)
                {
                    context.FollowUpSchedules.AddRange(rooms);
                    context.SaveChanges();
                }
            }
        }
        

    }
}
