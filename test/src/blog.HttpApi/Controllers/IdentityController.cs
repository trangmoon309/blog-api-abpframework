using blog.Account;
using blog.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using blog.ExtensionMethods;
using System;

namespace blog.Controllers
{
    [Route("api/identity")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IdentityController : AbpController
    {
        protected IIdentityAppService identityAppService { get; }
        protected IFacebookAuthService facebookAuthService { get; }
        protected IAccountAppService accountAppService { get; }

        public IdentityController(IIdentityAppService identityAppService, IFacebookAuthService facebookAuthService, IAccountAppService accountAppService)
        {
            this.identityAppService = identityAppService;
            this.facebookAuthService = facebookAuthService;
            this.accountAppService = accountAppService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto input)
        {
            var result = await identityAppService.LoginAsync(input);
            if(result.Success)
            {
                return Ok(new AuthenticationResult()
                {
                    Token = result.Token,
                    Success = true
                });
            }
            else
            {
                return Ok(new AuthenticationResult()
                {
                    Token = result.Token,
                    Success = false,
                    Errors = result.Errors
                });
            }
                    
        }

        //EAAK4ZANJD3ZAcBAGov1O6tt78rJ42AmeHLVWZCjZABE0DEkIx04FWRZCmjXgGKCH6cf9tX1bFtK0rlheOZCUoYfa7mB05UYZAHdvpwSdoLm5Py4kyC4B4dZCU7fr4HKpH7ZBCNYFGL0QHg5lwNLNwjavado0RBHL4jMNTKmkubrWj4eATalPvZBPbzwdJG5xggrigtHDNw98390wZDZD
        [HttpPost]
        [Route("facebookLogin")]
        public async Task<AuthenticationResult> FacebookLogin(string accessToken)
        {
            var userInfo = await facebookAuthService.GetUserInfoAsync(accessToken);
            return await identityAppService.ExternalLogin(userInfo);
        }

        [HttpGet]
        public virtual async Task<Volo.Abp.Identity.IdentityUserDto> GetProfile()
        {
            var userId = HttpContext.GetUserId();
            var user = await accountAppService.GetAsync(userId);
            return user;
        }

        [HttpPut]
        [Route("updateprofile")]
        //[MyCustom("", "blogproject.Profile.Update")]
        public async Task<UpdateProfileResponse> UpdateProfile(UpdateProfileDto userDto)
        {
            var userId = HttpContext.GetUserId();
            userDto.userId = Guid.Parse(userId);
            var result = await accountAppService.UpdateProfile(userDto);
            var response = new UpdateProfileResponse
            {
                userDto = await accountAppService.GetAsync(userId)
            };
            if (result) response.IsSuccess = true;
            else
            {
                response.IsSuccess = false;
                response.ErrorMesseage = new string[] { "Update Failed!" };
            }
            return response;
        }

        [HttpPut]
        [Route("changepassword")]
        //[MyCustom("", "blogproject.Profile.Update")]
        public async Task<UpdateProfileResponse> ChangePassword(ChangePasswordRequest request)
        {
            var response = new UpdateProfileResponse();
            var userId = HttpContext.GetUserId();
            var result = await accountAppService.ChangePassWord(request,userId);
            if (!result)
            {
                response.IsSuccess = false;
                response.ErrorMesseage = new string[] { "Current password don't match/ confirm password don't match!" };
                return response;
            }
            response.IsSuccess = true;
            response.userDto = await accountAppService.GetAsync(userId);
            return response;
        }

    }
}
