var builder = WebApplication.CreateBuilder(args);

// ✨ اضافه کردن MVC برای پشتیبانی از کنترلرها و نماها
builder.Services.AddControllersWithViews();

// ✨ تنظیمات احراز هویت برای استفاده از IdentityServer
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies"; // مقدار پیش‌فرض برای نگهداری اطلاعات احراز هویت در کوکی‌ها
    options.DefaultChallengeScheme = "oidc"; // استفاده از OpenID Connect برای احراز هویت
})
.AddCookie("Cookies") // پشتیبانی از کوکی‌ها برای نگهداری وضعیت ورود
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:44311/"; // آدرس سرور احراز هویت
    options.ClientId = "TechBack"; // شناسه کلاینت (باید با مقدار در IdentityServer یکی باشد)
    options.ClientSecret = "123456"; // رمز کلاینت (در محیط واقعی نباید در کد نوشته شود!)

    options.ResponseType = "code"; // مقدار را روی "code" تنظیم کنید تا از Authorization Code Flow استفاده شود
    options.GetClaimsFromUserInfoEndpoint = true; // دریافت اطلاعات کاربر از سرور احراز هویت
    options.SaveTokens = true; // ذخیره توکن‌ها در کوکی برای استفاده‌های بعدی

    // ✨ تعیین سطح دسترسی‌هایی که کلاینت مجاز به درخواست آن‌ها است
    options.Scope.Clear();
    options.Scope.Add("openid"); // شناسه کاربری
    options.Scope.Add("profile"); // اطلاعات پروفایل
    options.Scope.Add("email"); // ایمیل کاربر

    options.SignInScheme = "Cookies"; // ذخیره اطلاعات ورود در کوکی‌ها
    options.RequireHttpsMetadata = false; // فقط برای تست در محیط **لوکال** (در محیط عملیاتی باید true باشد)
});

var app = builder.Build();

// ✨ تنظیمات مسیر و امنیت
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // فعال کردن مدیریت خطاها
    app.UseHsts(); // افزایش امنیت ارتباطات HTTPS
}

app.UseHttpsRedirection(); // ریدایرکت درخواست‌ها به HTTPS
app.UseRouting(); // فعال کردن مسیریابی درخواست‌ها

app.UseAuthentication(); // فعال کردن **احراز هویت**
app.UseAuthorization(); // فعال کردن **مجوزدهی**

app.MapStaticAssets(); // پشتیبانی از فایل‌های استاتیک

// ✨ تنظیم مسیر پیش‌فرض برای کنترلرها
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
