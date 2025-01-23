using Auth_API.Src.Services.Identity;
using HUBT_Social_Base.ASP_Extentions;
using HUBT_Social_Core.Settings;

namespace Auth_API.Configuration
{
    public static class HttpClientRegister
    {
        public static IServiceCollection HttpClientRegisterConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddHttpClientService();
            string? identityPath = configuration.GetSection("IdentityApi").Get<string>();
            if (identityPath != null)
            {
                services.AddRegisterClientService<IAuthService, AuthService>(identityPath);
                return services;
            }
            throw new Exception("Unfound Section");
        }
    }
}
