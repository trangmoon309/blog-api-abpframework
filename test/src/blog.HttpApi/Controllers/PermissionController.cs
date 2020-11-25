using blog.Identity;
using blog.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace blog.Controllers
{
    [AllowAnonymous]
    [Route("api/permissions")]
    [MyCustom("", "blogproject.Permission.Manage")]
    public class PermissionController : AbpController
    {
        protected IPermissionAppService permissionAppService { get; }

        public PermissionController(IPermissionAppService permissionAppService)
        {
            this.permissionAppService = permissionAppService;
        }

        [HttpGet]
        public virtual async Task<GetPermissionListResultDto> GetListAsync()
        {
            return await permissionAppService.GetAllPermissionAsync();
        }

        [HttpGet]
        [Route("specificpermissions")]
        public virtual async Task<GetPermissionListResultDto> GetAsync(GetPermissionOfRoleRequest request)
        {
            return await permissionAppService.GetPermissionOfARoleAsync(request.providerName, request.providerKey);
        }

        [HttpPut]
        public virtual async Task<UpdatePermissionsOfRoleResponse> UpdateAsync(UpdatePermissionRequest request)
        {
            await permissionAppService.UpdateAPermissionAsync(request);
            var response = new UpdatePermissionsOfRoleResponse
            {
                IsSuccess = true,
            };
            return response;
        }

    }
}
