using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.MySql;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Paas.Pioneer.AutoWrapper;
using Paas.Pioneer.Domain.Shared.Auth;
using Paas.Pioneer.Middleware.Middleware.Extensions;
using Paas.Pioneer.Workflow.Application;
using Paas.Pioneer.Workflow.Domain.Shared.MultiTenancy;
using Paas.Pioneer.Workflow.EntityFrameworkCore.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Paas.Pioneer.Workflow.HttpApi.Host
{
    [DependsOn(
        typeof(WorkflowsHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(WorkflowsApplicationModule),
        typeof(WorkflowsEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        //typeof(Knife4jUISwaggerModule),
        typeof(DomainSharedModule)
    )]
    public class WorkflowsHttpApiHostModule : AbpModule
    {
        private IConfiguration Configuration;

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                //https://github.com/abpframework/abp/pull/9299
                mvcBuilder.AddControllersAsServices();
                mvcBuilder.AddViewComponentsAsServices();
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = Configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureCors(context, configuration);

            ConfigureAuthentication(context, configuration);

            // ҵ�������չ����
            Configure<AbpExceptionHandlingOptions>(options =>
            {
                options.SendExceptionsDetailsToClients = true;
            });

            ConfigureAutoMapper();

            // ������
            ConfigureElsa(context, configuration);
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<WorkflowsHttpApiHostModule>();
            });
        }

        private void ConfigureElsa(ServiceConfigurationContext context, IConfiguration configuration)
        {
            var elsaSection = configuration.GetSection("Elsa");

            context.Services.AddElsa(elsa =>
            {
                elsa
                    .UseEntityFrameworkPersistence(ef =>
                        DbContextOptionsBuilderExtensions.UseMySql(ef,
                            configuration.GetConnectionString("Default")))
                    .AddConsoleActivities()
                    .AddHttpActivities(elsaSection.GetSection("Server").Bind)
                    .AddEmailActivities(elsaSection.GetSection("Smtp").Bind)
                    .AddQuartzTemporalActivities()
                    .AddJavaScriptActivities()
                    .AddWorkflowsFrom<Startup>();
            });

            context.Services.AddElsaApiEndpoints();
            context.Services.Configure<ApiVersioningOptions>(options =>
            {
                options.UseApiBehavior = false;
            });

            context.Services.AddCors(cors => cors.AddDefaultPolicy(policy => policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .WithExposedHeaders("Content-Disposition"))
            );

            //Disable antiforgery validation for elsa
            Configure<AbpAntiForgeryOptions>(options =>
            {
                options.AutoValidateFilter = type =>
                    type.Assembly != typeof(Elsa.Server.Api.Endpoints.WorkflowRegistry.Get).Assembly;
            });
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

            // ����ȫ��ΨһId
            app.UseCorrelationId();

            //·��
            app.UseRouting();

            // ע���Ҫ��ASP�� ��������HTTP��Ĺ�������NET�����м����  
            //app.UseHttpActivities();

            // ��ʽ��
            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
            {
                ShowIsErrorFlagForSuccessfulResponse = true,
                ExcludePaths = new AutoWrapperExcludePath[] {
                    // �ϸ�ƥ��
                    new AutoWrapperExcludePath("/v1/api-docs", ExcludeMode.StartWith),
                    new AutoWrapperExcludePath("/rpc/", ExcludeMode.StartWith),
                }
            });

            //ȫ����־�м��
            app.UseLoggerMiddleware();

            //����
            app.UseCors();

            // ��֤
            app.UseAuthentication();

            // Jwt
            app.UseJwtTokenMiddleware();

            // ���⻧
            if (MultiTenancyConsts.IsEnabled)
            {
                //app.UseMultiTenancy();
            }

            // ������Ԫ
            app.UseUnitOfWork();

            // ��Ȩ
            app.UseAuthorization();

            // ������
            app.UseHttpActivities();

            // ����ĩ�˵�
            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapFallbackToPage("/_Host");
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
