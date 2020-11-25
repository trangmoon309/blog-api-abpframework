using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Users;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace blog.Web
{
    public class PasswordlessLoginProvider<TUser> : TotpSecurityStampBasedTokenProvider<TUser>
          where TUser : class
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false);
        }

        public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            var userId = await manager.GetUserIdAsync(user);

            return "PasswordlessLogin:" + purpose + ":" + userId;
        }
    }
}
