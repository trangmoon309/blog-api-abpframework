using blog.Identity;
using blog.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace blog.Account
{
        public static class GlobalVar
        {
            public static Guid userId { get; set; }
            public static IdentityRoleDto role { get; set; }       
            public static GetPermissionListResultDto permissions { get; set; }
            public static string userToken { get; set; }
            public static string permissionName { get; set; }

    }
}
