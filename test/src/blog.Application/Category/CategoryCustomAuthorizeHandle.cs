using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.Category
{
    public class CategoryCustomAuthorizeHandle : AuthorizationHandler<CategoryCustomAuthorize>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CategoryCustomAuthorize requirement)
        {
            var stream = requirement.token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
            var permission = tokenS.Claims.First(claim => claim.Type == "blogproject.Category.Create").Value;

            if (permission.Equals("true"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
