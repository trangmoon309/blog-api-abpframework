using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace blog.Pages
{
    public class Index_Tests : blogWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
