using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using TournamentProject.Data;
using TournamentProject.Models;
//using TournamentProject.Services;



var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TournamentProjectContextConnection") ?? throw new InvalidOperationException("Connection string 'TournamentProjectContextConnection' not found.");
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 3;
        options.SignIn.RequireConfirmedEmail = true;
        options.Lockout.AllowedForNewUsers = false;
        options.User.RequireUniqueEmail = true;
    }).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders().AddErrorDescriber<CustomIdentityErrorDescriber>();



builder.Services.AddRazorPages();



// Add services to the container.
builder.Services.AddControllersWithViews();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await CreateRoles(serviceProvider);
    await AssignAdminRole(serviceProvider, "Sobh6n@gmail.com");
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

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

async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "User" }; // Define your roles

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

async Task AssignAdminRole(IServiceProvider serviceProvider, string email)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
    var user = await userManager.FindByEmailAsync(email);

    if (user != null)
    {
        // Check if the user already has the Admin role
        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}


