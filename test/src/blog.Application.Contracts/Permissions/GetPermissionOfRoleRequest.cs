using System;
using System.Collections.Generic;
using System.Text;

namespace blog.Permissions
{
    public class GetPermissionOfRoleRequest
    {
        public string providerName { get; set; }
        public string providerKey { get; set; }
    }
}
