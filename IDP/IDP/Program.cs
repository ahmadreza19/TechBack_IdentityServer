using Duende.IdentityServer.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// ✨ افزودن سرویس‌های MVC به برنامه برای پشتیبانی از کنترلرها و نماها
builder.Services.AddControllersWithViews();

// ✨ افزودن IdentityServer به برنامه برای احراز هویت و مدیریت دسترسی کاربران
builder.Services.AddIdentityServer()
    // ⚠ ایجاد یک گواهی امضای موقت برای محیط توسعه
    // 🚨 هشدار: این روش فقط برای **محیط توسعه** مناسب است و نباید در محیط **تولید** استفاده شود.
    .AddDeveloperSigningCredential()

    // ✨ تعریف کاربران تستی برای لاگین در سیستم
    .AddTestUsers(new List<Duende.IdentityServer.Test.TestUser>
    {
        new Duende.IdentityServer.Test.TestUser
        {
            Username = "ahmad", // نام کاربری
            Password = "123456", // رمز عبور
            IsActive = true, // فعال بودن کاربر
            SubjectId = "1", // شناسه یکتا کاربر

            // 🎯 اطلاعات کاربر به صورت **Claim** ذخیره می‌شود.
            Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "ahmadreza@gmail.com"), // ایمیل
                new Claim(ClaimTypes.MobilePhone, "09918394008"), // شماره موبایل
                new Claim("FullName", "AhmadReza Jafari"), // نام کامل
                new Claim("website", "https://TechBack.com") // وب‌سایت
            }
        }
    })

    // ✨ تعریف منابع **هویتی** (Identity Resources)
    .AddInMemoryIdentityResources(new List<IdentityResource>
    {
        new IdentityResources.OpenId(),  // شناسایی کاربر
        new IdentityResources.Phone(),   // شماره موبایل
        new IdentityResources.Profile(), // اطلاعات پروفایل
        new IdentityResources.Email(),   // ایمیل
        new IdentityResources.Address(), // آدرس
    })

    // ✨ تعریف منابع **API** (در اینجا لیست خالی است، اما می‌توان APIهای محافظت‌شده را اضافه کرد)
    .AddInMemoryApiResources(new List<ApiResource>
    {
        // در اینجا منابع API محافظت‌شده اضافه می‌شود.
    })

    // ✨ تعریف **کلاینت‌هایی** که می‌توانند به سیستم احراز هویت متصل شوند
    .AddInMemoryClients(new List<Client>
    {
        new Client
        {
            ClientId = "TechBack", // شناسه کلاینت (باید در کلاینت نیز مقدار مشابهی تنظیم شود)
            ClientSecrets = new List<Secret> { new Secret("123456".Sha256()) }, // رمز کلاینت (باید هش شده باشد)
            
            AllowedGrantTypes = GrantTypes.Code, // استفاده از **Authorization Code Flow**
            RequirePkce = true, // فعال کردن PKCE برای امنیت بیشتر

            // ✨ مسیرهای مهم برای هدایت کاربر بعد از ورود و خروج
            RedirectUris = { "https://localhost:44334/signin-oidc" }, // بازگشت بعد از ورود
            PostLogoutRedirectUris = { "https://localhost:44334/signout-callback-oidc" }, // بازگشت بعد از خروج

            // ✨ مشخص کردن **سطح دسترسی (Scopes)** که کلاینت مجاز به دریافت آن‌ها است
            AllowedScopes = new List<string>
            {
                "openid",   // شناسه کاربری
                "profile",  // اطلاعات پروفایل
                "email"     // ایمیل کاربر
            },

            RequireConsent = true, // فعال کردن صفحه تأیید مجوزها برای امنیت بیشتر
        }
    });

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
