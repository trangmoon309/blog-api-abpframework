using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace blogprojectabp.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(blogprojectabpHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class blogprojectabpConsoleApiClientModule : AbpModule
    {
        
    }
}
