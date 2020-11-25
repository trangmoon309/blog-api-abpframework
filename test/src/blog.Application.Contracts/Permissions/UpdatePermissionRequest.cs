using System;
using System.Collections.Generic;
using System.Text;

namespace blog.Permissions
{
    public class UpdatePermissionRequest
    {
        public string PermissionName { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderName { get; set; }
        public bool IsGranted { get; set; }
    }
}
