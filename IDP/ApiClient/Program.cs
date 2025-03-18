using Duende.IdentityModel.Client; // کتابخانه‌ای برای مدیریت احراز هویت و دریافت توکن‌ها

// ایجاد یک نمونه از HttpClient برای ارسال درخواست‌ها به سرور احراز هویت
HttpClient httpClient = new HttpClient();

// دریافت سند کشف (Discovery Document) از IdentityServer که شامل مسیرهای مورد نیاز برای احراز هویت است
var discoveryDocument = httpClient.GetDiscoveryDocumentAsync("https://localhost:44311").Result;

// بررسی وجود خطا در دریافت Discovery Document
if (discoveryDocument.IsError)
{
    Console.WriteLine(discoveryDocument.Error); // نمایش خطا در صورت وجود مشکل
    return;
}

// درخواست دریافت **توکن دسترسی (Access Token)** با استفاده از احراز هویت مبتنی بر **Client Credentials**
var accessToken = httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = discoveryDocument.TokenEndpoint, // آدرس توکن که از Discovery Document گرفته شده است
    ClientId = "ApiHava", // شناسه کلاینت که در IdentityServer ثبت شده است
    ClientSecret = "123456", // کلید مخفی کلاینت برای احراز هویت
    Scope = "ApiHava" // دامنه دسترسی (Scope) که تعیین‌کننده سطح دسترسی است
}).Result;

// بررسی وجود خطا در دریافت Access Token
if (accessToken.IsError)
{
    Console.WriteLine(accessToken.Error); // نمایش خطا در صورت وجود مشکل
    return;
}

// نمایش توکن دریافت‌شده به صورت JSON در کنسول
Console.WriteLine(accessToken.Json);

// ایجاد یک نمونه جدید از HttpClient برای ارسال درخواست به API محافظت‌شده
HttpClient apiclient = new HttpClient();

// تنظیم توکن دریافتی در هدر Authorization برای احراز هویت درخواست‌ها
apiclient.SetBearerToken(accessToken.AccessToken);

// ارسال درخواست GET به API موردنظر که برای مثال آدرس `Weatherforecast` است
var resulte = apiclient.GetAsync("https://localhost:44395/Weatherforecast").Result;

// خواندن محتوای پاسخ دریافت‌شده از API
var content = resulte.Content.ReadAsStringAsync().Result;

// نمایش محتوای پاسخ API در کنسول
Console.WriteLine(content);
