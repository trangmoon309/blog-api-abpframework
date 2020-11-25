using Abp.Authorization;
using blog.Account;
using blog.Categories;
using blog.Category;
using blog.Document;
using blog.Identity;
using blog.Permissions;
using blog.Post;
using blog.Posts;
using blog.UriContracts;
using blog.UriServices;
using blog.Users;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace blog.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<blogWebModule>();
                
            services.AddAuthentication()
                .AddFacebook(facebook =>
                {
                    facebook.AppId = Configuration["FacebookAuth:AppID"];
                    facebook.AppSecret = Configuration["FacebookAuth:AppSecret"];
                })
                .AddGoogle(ggOptions =>
                 {
                     ggOptions.ClientId = Configuration["GoogleAuth:ClientId"];
                     ggOptions.ClientSecret = Configuration["GoogleAuth:ClientSecret"];
                });

            services.AddScoped<ICateAppService, CategoryService>();
            services.AddScoped<IPostAppService, PostService>();
            services.AddScoped<IAccountAppService, AccountAppService>();
            services.AddScoped<IPermissionAppService, PermissionAppService>();
            services.AddScoped<IIdentityRoleAppService, IdentityRoleAppSerivce>();
            services.AddScoped<IIdentityAppService, IdentityAppService>();
            services.AddScoped<IDocumentAppService, DocumentAppService>();

            var jwtSettings = new JwtSetting();
            Configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            //Facebook Auth
            var facebookAuthSettings = new FacebookAuthSettings();
            Configuration.Bind(nameof(FacebookAuthSettings), facebookAuthSettings);
            services.AddSingleton(facebookAuthSettings);
            services.AddHttpClient();
            services.AddSingleton<IFacebookAuthService, FacebookAuthService>();

            //Config uri service
            services.AddScoped<IUriService, UriService>();
            services.AddSingleton<IUriService>(provider =>
            {
                //scheme: https
                //path: api/v1/posts
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
                return new UriService(absoluteUri);
            });

            services.AddTransient<IProfileService, IdentityClaimsProfileService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();

        }
    }
}
