using GP.BLL.Interfaces;
using GP.BLL.Repositories;
using GP.DAL.Context;
using GP.DAL.Models;
using GP.DAL.Seed;
using GraduationProject.Appsettingsconfig;
using GraduationProject.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// context
builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
});
builder.Services.AddIdentity<GPUser, IdentityRole>(
    config => {}).AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
//cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.ConfigureApplicationCookie(
    config =>
    {
        config.LoginPath = "/Account/SignIn";
    }
);

#region Register
builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();
builder.Services.AddScoped<IInstructorScheduleRepositroy,InstructorScheduleRepositroy>();
builder.Services.AddScoped<ICourseRepository,CourseRepository>();
builder.Services.AddScoped<IFacultyMemberRepsitory,FacultyMemberRepsitory>();
builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();
builder.Services.AddScoped<IStudentScheduleRepository, StudentScheduleRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();
builder.Services.AddScoped<IAdvisorRepository, AdvisorRepository>();
builder.Services.AddScoped<IStudentAffairsRepository, StudentAffairsRepository>();
builder.Services.AddScoped<IFinancialAffairsRepository, FinancialAffairsRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<ITermCourseRepository, TermCourseRepository>();
builder.Services.AddScoped<ITermRepository, TermRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IPetitionRequestRepository, PetitionRequestRepository>();
builder.Services.AddScoped<IPetitionCourseRepository, PetitionCourseRepository>();
builder.Services.AddScoped<IResultPetitionRepository, ResultPetitionRepository>();
builder.Services.AddScoped<IFollowUpRepository, FollowUpRepository>();
builder.Services.AddScoped<IStudentDistribution, StudentDistributionRepositroy>();
builder.Services.AddScoped<EmailSettings, EmailSettings>();
builder.Services.AddScoped<IRevenuereport, RevenueReport>();
builder.Services.AddScoped<ITeachingHoursReport, TeachingHoursReport>();

builder.Services.Configure<mailsettings>(builder.Configuration.GetSection("mailsettings"));

#endregion
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
    //await DbInitializer.CreateAdmins(userManager, services, env);
    //await DbInitializer.SeedFacultyWithoutDept(userManager, dbContext, env);
    //DbInitializer.SeedCollege(dbContext, env);
    //DbInitializer.SeedDapertment(dbContext, env);
    //await DbInitializer.SeedFacultyWithDept(userManager, dbContext, env);
    //await DbInitializer.SeedFacultyUpdate(userManager, dbContext, env);
    //DbInitializer.SeedCourses(dbContext, env);
    //DbInitializer.SeedCoursesPre(dbContext, env);
    //DbInitializer.SeedPlace(dbContext, env);
    //await DbInitializer.SeedFollowUp(userManager, dbContext, env);
    //await DbInitializer.SeedStudentAffairs(userManager, dbContext, env);
    //await DbInitializer.SeedFinancialAffairs(userManager, dbContext, env);
    //await DbInitializer.SeedStudents(userManager, dbContext, env);
    //DbInitializer.SeedReceipts(dbContext, env);
    //DbInitializer.SeedTerms(dbContext, env);
    //DbInitializer.SeedCoursesTerms(dbContext, env);
    //DbInitializer.SeedApplications(dbContext, env);
    //DbInitializer.SeedEnrollments(dbContext, env);
}
#endregion
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
