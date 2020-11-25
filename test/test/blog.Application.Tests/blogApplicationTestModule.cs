using Volo.Abp.Modularity;

namespace blog
{
    [DependsOn(
        typeof(blogApplicationModule),
        typeof(blogDomainTestModule)
        )]
    public class blogApplicationTestModule : AbpModule
    {

    }
}