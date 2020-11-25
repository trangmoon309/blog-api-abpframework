using System;
using System.Collections.Generic;
using System.Text;

namespace blog.Permissions
{
    public class UpdatePermissionsOfRoleResponse
    {
        public bool IsSuccess { get; set; }
        public string[] ErrorMesseage { get; set; }
    }
}
