using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace blog.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(blogHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class blogConsoleApiClientModule : AbpModule
    {
        
    }
}
