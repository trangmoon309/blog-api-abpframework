using blog.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.Identity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyCustomAttribute : Attribute, IAsyncActionFilter
    {
        public string Token { get; set; }
        public string PermissionName { get; set; }
        public MyCustomAttribute(string Token, string permissionName)
        {
            this.Token = Token;
            this.PermissionName = permissionName;
            GlobalVar.permissionName = permissionName;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stream = Token;
            if (string.IsNullOrEmpty(stream)) stream = GlobalVar.userToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
            var permission = tokenS.Claims.First(claim => claim.Type == GlobalVar.permissionName).Value;
            if (permission.Equals("true"))
            {
                await next();
                return;
            }
            else return;
        }
    }
}
