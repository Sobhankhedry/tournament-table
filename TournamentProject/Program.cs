using Microsoft.EntityFrameworkCore;
using TournamentProject.Areas.Identity.Data;
using TournamentProject.Models;



var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TournamentProjectContextConnection") ?? throw new InvalidOperationException("Connection string 'TournamentProjectContextConnection' not found.");
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDBContext>();

//builder.Services.AddIdentity<AppUser, IdentityRole>(
//    options =>
//    {
//        options.SignIn.RequireConfirmedPhoneNumber = false;
//        options.SignIn.RequireConfirmedAccount = false;
//        options.Password.RequiredUniqueChars = 0;
//        options.Password.RequireNonAlphanumeric = false;
//        options.Password.RequireDigit = false;
//        options.Password.RequireLowercase = false;
//        options.Password.RequireUppercase = false;
//        options.Password.RequiredLength = 3;
//        options.SignIn.RequireConfirmedEmail = false;
//        options.Lockout.AllowedForNewUsers = false;
//        // options.User.RequireUniqueEmail = true;
//    })
//    .AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();


//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";

});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
