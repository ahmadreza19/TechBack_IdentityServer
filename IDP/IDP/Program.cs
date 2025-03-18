using Duende.IdentityServer.Models;
using IdentityServerHost.Quickstart.UI;
using IDP.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ✨ افزودن سرویس‌های MVC به برنامه برای پشتیبانی از کنترلرها و نماها
builder.Services.AddControllersWithViews();
// ✨ تنظیم رشته اتصال به پایگاه داده
string connectionString = "Data Source=.;Initial Catalog=IdentityServer;Integrated Security=true;TrustServerCertificate=True;";
var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

// ✨ افزودن IdentityServer به برنامه برای احراز هویت و مدیریت دسترسی کاربران
builder.Services.AddIdentityServer()
    // ⚠ ایجاد یک گواهی امضای موقت برای محیط توسعه
    .AddDeveloperSigningCredential()
    // ✨ تعریف کاربران تستی برای لاگین در سیستم
    .AddTestUsers(ConfigIdentityServer.GetUsers())
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
        options.EnableTokenCleanup = true;
    });
// ✨ تعریف منابع **هویتی** (Identity Resources)
//.AddInMemoryIdentityResources(ConfigIdentityServer.GetIdentityResources())
// ✨ تعریف منابع **API** 
//.AddInMemoryApiResources(ConfigIdentityServer.GetApiResources())
// ✨ تعریف **کلاینت‌هایی** که می‌توانند به سیستم احراز هویت متصل شوند
//.AddInMemoryClients(ConfigIdentityServer.GetClients())
//✨ین کامند در IdentityServer4 برای ثبت دامنه‌های API (API Scopes) در حافظه موقت استفاده می‌شود. دامنه‌های API مشخص می‌کنند که کلاینت‌ها (Clients) به چه بخش‌هایی از API دسترسی دارند.
// .AddInMemoryApiScopes(ConfigIdentityServer.GetScopes());
var app = builder.Build();

// ✨ تنظیمات برای مدیریت درخواست‌ها و مسیرهای HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // فعال کردن هندلر خطا در محیط تولید
    app.UseHsts(); // فعال کردن HSTS برای افزایش امنیت
}

app.UseHttpsRedirection(); // ریدایرکت تمام درخواست‌ها به HTTPS
app.UseRouting(); // فعال کردن مسیریابی درخواست‌ها

app.UseIdentityServer(); // فعال کردن IdentityServer برای احراز هویت

app.UseAuthorization(); // فعال کردن مجوزدهی

// ✨ تنظیم مسیر پیش‌فرض برای کنترلرها
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets(); // پشتیبانی از فایل‌های استاتیک

app.Run();
