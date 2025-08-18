using HR_Dash.Data;
using HR_Dash.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Anvandare/LoggaIn";
    options.AccessDeniedPath = "/Anvandare/LoggaIn";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

//S� databasen med anv�ndare och roller (Seeding)
await SeedRolesAsync(app);
await SeedUsersAsync(app);

app.Run();

async Task SeedRolesAsync(WebApplication app)
{
   using (var scope = app.Services.CreateScope())
   {
       var rollManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

       var roller = new[] { "Admin", "Manager", "User", "Anstalld" };

       foreach (var roll in roller)
       {
           if (!await rollManager.RoleExistsAsync(roll))
           {
               await rollManager.CreateAsync(new IdentityRole(roll));


           }
       }

   }
}

async Task SeedUsersAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


        // Användare 1: Admin (Albin)
        await SkapaAnvandare(userManager, "albin@admin.com", "Admin123!", "Admin");

        // Användare 2: Anna (Manager + Anställd)
        await SkapaAnvandare(userManager, "anna.larsson@example.com", "Manager123!", "Manager", "Anstalld");

        // Användare 3: Erik (Anställd)
        await SkapaAnvandare(userManager, "erik.svensson@example.com", "Anstalld123!", "Anstalld");

        // Användare 4: Lisa (Anställd)
        await SkapaAnvandare(userManager, "lisa.nyman@example.com", "Anstalld123!", "Anstalld");

        var erik = await userManager.FindByEmailAsync("erik.svensson@example.com");
        if (erik != null && !context.Anstallda.Any(a => a.ApplicationUserId == erik.Id))
        {
            context.Anstallda.Add(new Anstalld
            {
                AnstalldName = "Erik Svensson",
                Email = erik.Email,
                AnstallningDatum = DateTime.Today.AddYears(-2),
                ApplicationUserId = erik.Id
            });
        }

        var anna = await userManager.FindByEmailAsync("anna.larsson@example.com");
        if (anna != null && !context.Anstallda.Any(a => a.ApplicationUserId == anna.Id))
        {
            context.Anstallda.Add(new Anstalld
            {
                AnstalldName = "Anna Larsson",
                Email = anna.Email,
                AnstallningDatum = DateTime.Today.AddYears(-1),
                ApplicationUserId = anna.Id
            });
        }

        var lisa = await userManager.FindByEmailAsync("lisa.nyman@example.com");
        if (lisa != null && !context.Anstallda.Any(a => a.ApplicationUserId == lisa.Id))
        {
            context.Anstallda.Add(new Anstalld
            {
                AnstalldName = "Lisa Nyman",
                Email = lisa.Email,
                AnstallningDatum = DateTime.Today.AddMonths(-6),
                ApplicationUserId = lisa.Id
            });
        }

        await context.SaveChangesAsync();
    }
}

async Task SkapaAnvandare(UserManager<ApplicationUser> userManager, string email, string password, params string[] roller)
{
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            LockoutEnabled = false
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            foreach (var roll in roller)
            {
                await userManager.AddToRoleAsync(user, roll);
            }
        }
    }
}
