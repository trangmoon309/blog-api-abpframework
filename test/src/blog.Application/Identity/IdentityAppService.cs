using blog.Account;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Users;

namespace blog.Identity
{
    public class IdentityAppService : IdentityAppServiceBase,IIdentityAppService
    {
        protected IdentityUserManager UserManager { get; }

        protected blog.Permissions.IPermissionAppService permissionAppService { get; set; }
        protected blog.Identity.IIdentityRoleAppService roleAppService { get; set; }
        protected IPermissionManager PermissionManager { get; }

        protected IAccountAppService accountAppService { get; }

        public IdentityAppService(IdentityUserManager userManager, Permissions.IPermissionAppService permissionAppService, IIdentityRoleAppService roleAppService, IPermissionManager permissionManager, IAccountAppService accountAppService)
        {
            UserManager = userManager;
            this.permissionAppService = permissionAppService;
            this.roleAppService = roleAppService;
            PermissionManager = permissionManager;
            this.accountAppService = accountAppService;
        }

        public async Task<AuthenticationResult> LoginAsync(LoginDto input)
        {
            var user = await UserManager.FindByNameAsync(input.userName);
            if (user == null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "User doesn't exist." }
                };
            }

            var userHasValidPassword = await UserManager.CheckPasswordAsync(user, input.passWord);
            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    Errors = new[] { "User/password doesn't match." }
                };
            }
            //await accountAppService.LoginAsync(input);
            GlobalVar.userId = user.Id;
            GlobalVar.role = await roleAppService.GetUserRole(user.Id);
            GlobalVar.permissions = await permissionAppService.GetPermissionOfARoleAsync("R", GlobalVar.role.Name);
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user)
        {
            var tokenHanlder = new JwtSecurityTokenHandler();

            //khóa bí mật cho phần signature
            string securityKey = "this_is_super_long_security_key_for_token_validation_project_2018_09_07$smesk.in";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //var userRole = await roleAppService.GetUserRole(user.Id);
            //var permissionsDefinition = await permissionAppService.GetPermissionOfARoleAsync("R", userRole.Name);

            var userPermissions = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("ClientId", "blog_App"),
                new Claim("id",user.Id.ToString())
            };

            //foreach (var group in permissionsDefinition.Groups)
            //{
            //    if(group.Name.Equals("blogproject"))
            //    {
            //        foreach (var permission in group.Permissions)
            //        {
            //            var grantInfo = await PermissionManager.GetAsync(permission.Name, "R", userRole.Name);
            //            if (grantInfo.IsGranted)
            //            {
            //                userPermissions.Add(new Claim(permission.Name, "true"));
            //            }
            //        }
            //    }    
            //}

            var userRoles = await UserManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                userPermissions.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(userPermissions),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256),
                Audience = "blog",
                Issuer = "issuer",
            };

            var token = tokenHanlder.CreateToken(tokenDescriptor);
            //GlobalVar.userToken = tokenHanlder.WriteToken(token);
            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHanlder.WriteToken(token)
            };
        }


        public async Task<AuthenticationResult> ExternalLogin(FacebookUserInfoResult userInfo)
        {
            var user = await UserManager.FindByEmailAsync(userInfo.Email);
            var input = new LoginDto
            {
                userName = userInfo.Email,
                passWord = null
            };
            if (user == null)
            {
                var newuser = new Volo.Abp.Identity.IdentityUser(GuidGenerator.Create(),input.userName, input.userName);
                await UserManager.CreateAsync(newuser, input.passWord);
                return await LoginAsync(input);
            }    
            else
            {
                return await GenerateAuthenticationResultForUserAsync(user);
            }
        }
    }
}
