using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tawjihy.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Disable user registration
    options.SignIn.RequireConfirmedAccount = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();



builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
    });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Enable caching middleware
app.UseResponseCaching();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.UseStatusCodePagesWithReExecute("/Base/NotFound");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
       name: "identity",
       pattern: "Identity/{*path}",
       defaults: new { controller = "Home", action = "Index" });
});

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.CanConnect();

        DbSeeder.SeedDatabaseAsync(serviceProvider).Wait();
    }
    catch (Exception ex)
    {
        app.UseExceptionHandler("/Base/ErrorDatabase");
    }
}

app.Run();