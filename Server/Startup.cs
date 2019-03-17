using AutoMapper;
using Common.DTO;
using Common.Services;
using Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Server.ConfigurationDTO;
using Server.Contexts;
using Server.DTO;
using Server.MapperProfiles;
using Server.Validation.Handlers;
using Server.Validation.Requirements;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ArticleArticleHeaderProfile>();
                cfg.AddProfile<ArticleArticleDtoProfile>();
            });

            services.AddDbContext<AuthorizationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("Database"), optionsBuilder =>
                    optionsBuilder.MigrationsAssembly("Identity")));

            services.AddDbContext<ArticleDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("Database"), optionsBuilder =>
                    optionsBuilder.MigrationsAssembly("Server")));

            var identityBuilder = services.AddIdentityCore<IdentityUser>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            identityBuilder.AddEntityFrameworkStores<AuthorizationDbContext>();

            // Register the ConfigurationBuilder instance of AuthSettings
            var authSettings = Configuration.GetSection(nameof(AuthSettings));
            services.Configure<AuthSettings>(authSettings);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings[nameof(AuthSettings.SecretKey)]));
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim("rol", "api_access"));
                options.AddPolicy("AuthorAuthorization", policy =>
                    policy.Requirements.Add(new AuthorAuthorizationRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, AuthorAuthorizationHandler>();
            services.AddTransient<IArticleContextFactory>(s => new ArticleContextFactory(Configuration.GetConnectionString("Database")));

            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var listDependencyInjectors = GetInjectors();

            foreach (var inj in listDependencyInjectors)
            {
                inj.RegisterServices(services);
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(
                builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();
            app.UseMvc();
        }

        private List<IDependencyInjector> GetInjectors()
        {
            var response = new List<IDependencyInjector>
            {
                new Common.DependencyInjection.DependencyInjector(),
                new Identity.DependencyInjection.DependencyInjector(),
                new Server.DependencyInjection.DependencyInjector()
            };

            return response;
        }
    }
}