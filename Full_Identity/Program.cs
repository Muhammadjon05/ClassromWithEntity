using Full_Identity.Helper;
using IdentityData.AppDbContext;
using IdentityData.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer("Server=sql.bsite.net\\MSSQL2016;" +
                         "Database=muhammadjon_DB;" +
                         "User Id=muhammadjon_DB;" +
                         "Password=Muha1201; " +
                         "TrustServerCertificate=True");
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserProvider>();
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;  
}).AddEntityFrameworkStores<Context>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/SignIn";
});

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
app.Run();
