using Duende.IdentityServer;
using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// اضافه کردن سرویس IdentityServer به کانتینر سرویس‌های برنامه
builder.Services.AddIdentityServer()
    // افزودن یک گواهی امضای موقت برای محیط توسعه
    // هشدار: این روش فقط برای محیط توسعه مناسب است، نه برای محیط تولید
    .AddDeveloperSigningCredential()

    // تعریف کاربران آزمایشی برای احراز هویت
    // در حال حاضر لیست خالی است و باید کاربران آزمایشی را اضافه کنید
    .AddTestUsers(new List<Duende.IdentityServer.Test.TestUser>()
    { })

    // تعریف منابع API که می‌خواهید محافظت کنید
    // این لیست در حال حاضر خالی است و نیاز به تکمیل دارد
    .AddInMemoryApiResources(new List<ApiResource>()
    { })

    // تعریف کلاینت‌هایی که می‌توانند به API‌ها دسترسی داشته باشند
    // این لیست نیز خالی است و باید با کلاینت‌های مورد نظر پر شود
    .AddInMemoryClients(new List<Client>() { }); var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseIdentityServer();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
