using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.Configuration;
using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configuration;
using DiscountManagement.Infrastructure.Configuration;
using InventoryManagement.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using ServiceHost;
using ShopManagement.Infrastructure.Configuration;
using _0_Framework.Application.ZarinPal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

var cs = builder.Configuration.GetConnectionString("LampShapeDb");
ShopManagementBootstrapper.Config(builder.Services,cs);
DiscountManagementBootstrapper.Config(builder.Services, cs);
InventoryManagementBootstrapper.Config(builder.Services, cs);
BlogManagementBootstrapper.Config(builder.Services, cs);
CommentManagementBootstrapper.Config(builder.Services, cs);
AccountManagementBootstrapper.Config(builder.Services, cs);

builder.Services.AddTransient<IFileUploader, FileUploader>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IAuthHelper, AuthHelper>();
builder.Services.AddTransient<IZarinPalFactory, ZarinPalFactory>();



builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.LoginPath = new PathString("/account/login");
        o.LogoutPath = new PathString("/account/login");
        o.AccessDeniedPath = new PathString("/AccessDenied");
    });

    builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminArea",
        builder => builder.RequireRole(new List<string> { Roles.Administrator, Roles.ContentUploader}));

    options.AddPolicy("Administration",
        builder => builder.RequireRole(new List<string> { Roles.Administrator }));
    options.AddPolicy("evrey",
        builder => builder.RequireRole(new List<string> {Roles.SystemUser, Roles.Administrator, Roles.ContentUploader }));


});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
}
else
{
    app.UseDeveloperExceptionPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCookiePolicy();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

