using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopFront.Application;
using ShopFront.Infrastructure;
using ShopFront.Infrastructure.Settings;
using ShopFront.WebApi.Helpers;
using ShopFront.WebApi.Middleware;
using ShopFront.WebApi.Filters;
using ShopFront.WebApi.Extensions;

namespace ShopFront.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddMvcOptions(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                    options.Filters.Add(typeof(ValidationFilter));
                })
                .AddControllersAsServices();

            services.AddEndpointsApiExplorer();

            services.AddHttpContextAccessor();
            
            services
                .AddApplication()
                .AddInfrastructure();

            services.AddDbContext<ApiDbContext>(options =>
                    options.UseNpgsql(Environment.GetEnvironmentVariable("DEFAULT_CONNECTION") ?? _configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    var corsOrigins = Environment.GetEnvironmentVariable("CORS_ORIGINS")?.Split(',');
                    if (corsOrigins != null)
                    {
                        builder.WithOrigins(corsOrigins)
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    }
                });
            });

            // Settings
            services.Configure<JwtBearerSettings>(_configuration.GetSection("Authentication:JwtBearer"));
            JwtTokenHelper.Configure(services.BuildServiceProvider().GetRequiredService<IOptions<JwtBearerSettings>>());
            
            // MediatR
            services.AddMediatR(c =>
                c.RegisterServicesFromAssemblyContaining<Startup>());

            // Swagger
            services.AddCustomSwaggerGen();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(_configuration.GetSection("Authentication:JwtBearer:SecurityKey").Value)),
                        ValidateIssuer = true,
                        ValidIssuer = _configuration.GetSection("Authentication:JwtBearer:Issuer").Value,
                        ValidateAudience = true,
                        ValidAudience = _configuration.GetSection("Authentication:JwtBearer:Audience").Value
                    };
                });

            // Extensions
            services.RegisterServices();
            services.RegisterValidators();
            services.RegisterCustomHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApiDbContext dbContext)
        {
            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();
            
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            // Middlewares
            app.UseMiddleware<TokenValidationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
