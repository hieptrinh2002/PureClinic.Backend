using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PureLifeClinic.API.Helpers.Authz.PolicyProvider;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Infrastructure.Data;
using System.Security.Claims;
using System.Text;

namespace PureLifeClinic.API.Extensions
{
    public static class SecurityExtension
    {
        public static IServiceCollection RegisterSecurityService(this IServiceCollection services, IConfiguration configuration)
        {
            #region Identity
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;

                // lock account 
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(3);
                options.Lockout.AllowedForNewUsers = true;                 

            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            #endregion

            #region JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(configuration["AppSettings:JwtConfig:Secret"]);
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["AppSettings:JwtConfig:ValidAudience"],
                    ValidIssuer = configuration["AppSettings:JwtConfig:ValidIssuer"],
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
            #endregion

            #region Custom Authorization

            services.AddAuthorization();

            //builder.Services.AddAuthorization(options =>
            //{
            //    // Mặc định yêu cầu người dùng phải xác thực
            //    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            //        .RequireAuthenticatedUser()
            //        .Build();

            //    // Thêm một chính sách cụ thể
            //    options.AddPolicy("Over18YearsOld", policy =>
            //        policy.RequireAssertion(context =>
            //            context.User.HasClaim(c =>
            //                (c.Type == "DateOfBirth" && DateTime.Now.Year - DateTime.Parse(c.Value).Year >= 18)
            //            )));
            //});

            // Register our custom Authorization handler
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            // Overrides the DefaultAuthorizationPolicyProvider with our own
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            #endregion

            return services;
        }
    }
}
