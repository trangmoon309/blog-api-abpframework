using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace blogprojectabp.Pages
{
    public class Index_Tests : blogprojectabpWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
