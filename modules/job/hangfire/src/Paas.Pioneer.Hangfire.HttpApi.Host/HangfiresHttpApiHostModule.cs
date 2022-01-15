using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Paas.Pioneer.Domain.Shared.Auth;
using Paas.Pioneer.Hangfire.Application;
using Paas.Pioneer.Hangfire.Domain.Shared.MultiTenancy;
using Paas.Pioneer.Hangfire.EntityFrameworkCore.EntityFrameworkCore;
using Paas.Pioneer.Hangfire.HttpApi.Host.Filter;
using Paas.Pioneer.Knife4jUI.Swagger;
using Paas.Pioneer.Middleware.Middleware.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.Modularity;

namespace Paas.Pioneer.Hangfire.HttpApi.Host
{
    [DependsOn(
        typeof(HangfiresHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(HangfiresApplicationModule),
        typeof(HangfiresEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(Knife4jUISwaggerModule),
        typeof(DomainSharedModule),
        typeof(AbpBackgroundJobsHangfireModule)
    )]
    public class HangfiresHttpApiHostModule : AbpModule
    {
        private IConfiguration Configuration;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = Configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureHangfire(context, configuration);

            ConfigureCors(context, configuration);

            ConfigureAuthentication(context, configuration);
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

        private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddHangfire(config =>
            {
                config.UseStorage(new MySqlStorage(configuration.GetConnectionString("Default"), new MySqlStorageOptions()
                {
                    TablesPrefix = "Hangfire_",                                 //- ���ݿ��б��ǰ׺,Ĭ��Ϊnone
                    DashboardJobListLimit = 50000,                            //- �Ǳ����ҵ�б�����,Ĭ��ֵΪ50000
                    PrepareSchemaIfNecessary = true,                         //- �������Ϊtrue���򴴽����ݿ��,Ĭ����true
                    QueuePollInterval = TimeSpan.FromSeconds(15),             //- ��ҵ������ѯ���,Ĭ��ֵΪ15��
                    TransactionTimeout = TimeSpan.FromMinutes(1),             //- ���׳�ʱ,Ĭ��Ϊ1����
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- ��ҵ���ڼ����(������ڼ�¼),Ĭ��ֵΪ1Сʱ
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- �ۺϼ������ļ��,Ĭ��Ϊ5����
                    TransactionIsolationLevel = IsolationLevel.ReadCommitted //- ������뼶��,Ĭ���Ƕ�ȡ���ύ
                }));
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
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

            // ����Ǳ��
            app.UseHangfireDashboard("/job", new DashboardOptions()
            {
                Authorization = new[] { new CustomAuthorizeFilter() }
            });

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
                            context.HttpContext.Request.Headers["Authorization"] = $"Bearer {accessToken}";
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
