using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Paas.Pioneer.Admin.Core.Application;
using Paas.Pioneer.Admin.Core.Domain.Shared.MultiTenancy;
using Paas.Pioneer.Admin.Core.EntityFrameworkCore.EntityFrameworkCore;
using Paas.Pioneer.Domain.Shared.Auth;
using Paas.Pioneer.Knife4jUI.Swagger;
using Paas.Pioneer.Middleware.Middleware.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating.Scriban;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace Paas.Pioneer.Admin.Core.HttpApi.Host
{
    [DependsOn(
        typeof(AdminsHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AdminsApplicationModule),
        typeof(AdminsEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(Knife4jUISwaggerModule),
        typeof(AbpTextTemplatingScribanModule),
        typeof(DomainSharedModule)
    )]
    public class AdminsHttpApiHostModule : AbpModule
    {
        private IConfiguration Configuration;
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = Configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();


            ConfigureUrls(configuration);

            ConfigureCors(context, configuration);

            ConfigureAuthentication(context, configuration);

            TextTemplatingScriban();
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
               {
                   builder
                       .WithOrigins(
                           configuration["App:CorsOrigins"]
                               .Split(",", StringSplitOptions.RemoveEmptyEntries)
                               .Select(o => o.RemovePostFix("/"))
                               .ToArray()
                       )
                       .WithAbpExposedHeaders()
                       .SetIsOriginAllowedToAllowWildcardSubdomains()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
               });
            });
        }

        private void TextTemplatingScriban()
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AdminsHttpApiHostModule>("Paas.Pioneer.Admin.Core.HttpApi.Host");
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            // ElasticApm
            //app.UseAllElasticApm(Configuration);

            // �ж��Ƿ����ģʽ
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //ȫ����־�м��
            app.UseLoggerMiddleware();

            // ����ȫ��ΨһId
            app.UseCorrelationId();

            //·��
            app.UseRouting();

            //����
            app.UseCors();

            // ��֤
            app.UseAuthentication();

            // Jwt
            app.UseJwtTokenMiddleware();

            // ���⻧
            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            // ������Ԫ
            app.UseUnitOfWork();

            // ��Ȩ
            app.UseAuthorization();

            // ����ĩ�˵�
            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapSwaggerUI();
            });
        }

        /// <summary>
		/// ������֤ģʽ
		/// </summary>
		/// <param name="context"></param>
		/// <param name="configuration"></param>
		private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
                options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/chat-hub"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                            context.HttpContext.Request.Headers["Authorization"] = "Bearer " + accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["jwtconfig:issuer"],
                    ValidAudience = configuration["jwtconfig:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtconfig:securitykey"])),
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { });
        }
    }
}
