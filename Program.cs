using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDo.Areas.Identity.Data;

using JavaScriptEngineSwitcher.V8 ;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection ;
using React.AspNet ;

var builder = WebApplication.CreateBuilder(args);
// var connectionString = builder.Configuration.GetConnectionString("ContextConnection");
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ;


// Add services to the container.
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddDefaultIdentity<User>(options => 
    options.SignIn.RequireConfirmedAccount = false
).AddEntityFrameworkStores<Context>();


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false; // consent required
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession( options => 
{

    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(2);
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>() ;
builder.Services.AddReact() ;

builder.Services.AddJsEngineSwitcher(options =>
    options.DefaultEngineName = V8JsEngine.EngineName
).AddV8() ;

builder.Services.AddHttpContextAccessor() ;
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession() ;

app.UseHttpsRedirection();

app.UseReact(config =>
    {}
) ;

app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages() ;

app.UseAuth(connectionString) ;

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
