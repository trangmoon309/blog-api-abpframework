using blog.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace blog
{
    [DependsOn(
        typeof(blogEntityFrameworkCoreTestModule)
        )]
    public class blogDomainTestModule : AbpModule
    {

    }
}