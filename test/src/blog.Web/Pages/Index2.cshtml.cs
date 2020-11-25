using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace blog.Web.Pages
{
	public class Index2Model : blogPageModel
	{
		protected IdentityUserManager UserManager { get; }

		private readonly IIdentityUserRepository _userRepository;

		public string PasswordlessLoginUrl { get; set; }

		public string Email { get; set; }

		public Index2Model(IdentityUserManager userManager, IIdentityUserRepository userRepository)
		{
			UserManager = userManager;
			_userRepository = userRepository;
		}

		public ActionResult OnGet()
		{
			if (!CurrentUser.IsAuthenticated)
			{
				return Redirect("/Account/Login");
			}

			return Page();
		}

		//added for passwordless authentication
		public async Task<IActionResult> OnPostGeneratePasswordlessTokenAsync()
		{
			var adminUser = await _userRepository.FindByNormalizedUserNameAsync("admin");

			var token = await UserManager.GenerateUserTokenAsync(adminUser, "PasswordlessLoginProvider", "passwordless-auth");

			PasswordlessLoginUrl = Url.Action("Login", "Passwordless", new { token = token, userId = adminUser.Id.ToString() }, Request.Scheme);

			return Page();
		}
	}
}