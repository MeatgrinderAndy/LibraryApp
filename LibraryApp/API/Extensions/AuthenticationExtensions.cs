using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LibraryApp.API.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration["Jwt:Secret"];
            var issuer = configuration["Jwt:Issuer"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var headerToken = context.Request.Headers["Authorization"].FirstOrDefault();
                            var cookieToken = context.Request.Cookies["cook"];

                            if (!string.IsNullOrEmpty(headerToken) && headerToken.StartsWith("Bearer "))
                            {
                                headerToken = headerToken.Substring(7);
                            }

                            context.Token = headerToken ?? cookieToken;
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
