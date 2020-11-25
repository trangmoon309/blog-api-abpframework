using Volo.Abp.Modularity;

namespace blogprojectabp
{
    [DependsOn(
        typeof(blogprojectabpApplicationModule),
        typeof(blogprojectabpDomainTestModule)
        )]
    public class blogprojectabpApplicationTestModule : AbpModule
    {

    }
}