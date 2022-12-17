using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quizle.Core.Contracts;
using Quizle.Core.Services;
using Quizle.DB;
using Quizle.DB.Models;
using Quizle.Core.QuartzJobs;
using System.Configuration;
using Quizle.DB.Common;
using Microsoft.AspNetCore.Mvc;
using Quizle.Web.Extensions;
using Quizle.Web.MapperProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<QuizleDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<QuizleDbContext>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Login";
    options.AccessDeniedPath = "/Home/Index";
});
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ViewModelMapperProfile>();
});

builder.Services.Configure<QuartzOptions>(builder.Configuration.GetSection("Quartz"));

builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IBadgeService, BadgeService>();



builder.Services.AddQuartz(q =>
{
    q.SchedulerId = "Scheduler-Core";
    
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();
    q.UseDefaultThreadPool(tp =>
    {
        tp.MaxConcurrency = 10;
    });

    q.ScheduleJob<GetQuestionsJob>(trigger => trigger
            .WithIdentity("Combined Configuration Trigger")
            .StartNow()
			//Will be changed to 1Hr when project is released. Now it is 1min just for development convenience.
			.WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0,0)).WithIntervalInMinutes(1))          
        );
});
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.SeedAdmin();

app.UseEndpoints(endpoints =>
{
    app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

});

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program { }
