using blogprojectabp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace blogprojectabp
{
    [DependsOn(
        typeof(blogprojectabpEntityFrameworkCoreTestModule)
        )]
    public class blogprojectabpDomainTestModule : AbpModule
    {

    }
}