using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Security.Claims;

namespace IDP.Models
{
    public class ConfigIdentityServer
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
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
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),  // شناسایی کاربر
                new IdentityResources.Phone(),   // شماره موبایل
                new IdentityResources.Profile(), // اطلاعات پروفایل
                new IdentityResources.Email(),   // ایمیل
                new IdentityResources.Address(),

            };
        }
        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
              new ApiResource("ApiHava","سرویس هواشناسی")
            };
        }
        public static List<Client> GetClients()
        {
           return new List<Client>()
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
                  },
                  new Client
                  {
                      ClientId="ApiHava",
                      ClientSecrets=new List <Secret>{new Secret("123456".Sha256())},
                      AllowedGrantTypes=GrantTypes.ClientCredentials,
                      AllowedScopes=new []{ "ApiHava" }
                  } 
           };     
        }
        public static List<ApiScope> GetScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("ApiHava","هواشناسی")
            };
        }

    }
}
