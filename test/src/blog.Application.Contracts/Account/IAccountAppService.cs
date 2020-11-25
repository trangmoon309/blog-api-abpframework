using Abp.Application.Services;
using System;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace blog.Account
{
   public interface IAccountAppService : IApplicationService
    {
        Task<IdentityUserDto> FindByIdAsync(Guid id);
        Task<bool> UpdateProfile(IdentityUserDto userDto);
        Task<bool> UpdateProfile(UpdateProfileDto userDto);
        Task<bool> UpdateProfileWithPassword(IdentityUserDto userDto, string newPassword);
        Task<bool> CheckCurrentPassword(blog.Identity.ChangePasswordRequest changePasswordRequest);
        Task<IdentityUserDto> GetAsync(string userId);
        Task<bool> ChangePassWord(blog.Identity.ChangePasswordRequest changePasswordRequest, string userId);
        Task LoginAsync(blog.Identity.LoginDto userDto);

    }
}
