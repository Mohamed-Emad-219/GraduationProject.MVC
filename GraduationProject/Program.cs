using GP.BLL.Interfaces;
using GP.BLL.Repositories;
using GP.DAL.Context;
using GP.DAL.Models;
using GP.DAL.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
});
builder.Services.AddIdentity<GPUser, IdentityRole>(
              config =>
              {

                  //config.Password.RequiredUniqueChars = 2;
                  //config.Password.RequireDigit = true;
                  //config.Password.RequireLowercase = true;
                  //config.Password.RequireUppercase = true;
                  //config.Password.RequireNonAlphanumeric = true;
                  //config.User.RequireUniqueEmail = true;
                  //config.Lockout.MaxFailedAccessAttempts = 5;
                  //config.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(5);
                  //config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
              }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(
               config =>
               {
                   config.LoginPath = "/Account/SignIn";
                   //config.ExpireTimeSpan = System.TimeSpan.FromDays(5);
                   //config.SlidingExpiration = true;
               });

//builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();
builder.Services.AddScoped<IInstructorScheduleRepositroy,InstructorScheduleRepositroy>();
builder.Services.AddScoped<ICourseRepository,CourseRepository>();
builder.Services.AddScoped<IFacultyMemberRepsitory,FacultyMemberRepsitory>();
builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();
builder.Services.AddScoped<IStudentScheduleRepository, StudentScheduleRepository>();
builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

builder.Services.AddControllersWithViews();
#region Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();
    var env = services.GetRequiredService<IHostEnvironment>();
    var userManager = services.GetRequiredService<UserManager<GPUser>>();
    //await DbInitializer.SeedRoles(services);
    //await DbInitializer.CreateAdvisors(userManager, services, env);
    //await DbInitializer.SeedFacultyWithoutDept(userManager, dbContext, env);
    //DbInitializer.SeedCollege(dbContext, env);
    //DbInitializer.SeedDapertment(dbContext, env);
    //DbInitializer.SeedFacultyWithDept(dbContext, env);
    //DbInitializer.SeedCourses(dbContext, env);
    //DbInitializer.SeedCoursesPre(dbContext, env);
    //DbInitializer.SeedPlace(dbContext, env);
    //await DbInitializer.SeedFollowUp(userManager, dbContext, env);
    //await DbInitializer.SeedStudentAffairs(userManager, dbContext, env);
    //await DbInitializer.SeedFinancialAffairs(userManager, dbContext, env);
    //await DbInitializer.SeedStudents(userManager, dbContext, env);
    //DbInitializer.SeedReceipts(dbContext, env);
    //DbInitializer.SeedApplications(dbContext, env);
    //DbInitializer.SeedEnrollments(dbContext, env);
    //await DbInitializer.SeedInstructorAssistants(userManager, dbContext, env);
    //DbInitializer.SeedInstructorSchedules(dbContext, env);
    //DbInitializer.SeedStudentSchedules(dbContext, env);
    //DbInitializer.SeedFollowUpSchedules(dbContext, env);

    //await DbInitializer.SeedUsers(services);

}
#endregion
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
