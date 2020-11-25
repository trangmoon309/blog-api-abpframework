using System;
using System.Collections.Generic;
using System.Text;

namespace blog.Permissions
{
    public class UpdatePermissionOfRoleRequest
    {
        public string providerName { get; set; }
        public string providerKey { get; set; }
        public UpdatePermissionsDto updatedPermissionDto { get; set; }
    }
}
