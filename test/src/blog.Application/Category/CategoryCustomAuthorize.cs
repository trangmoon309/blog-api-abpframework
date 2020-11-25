using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.Category
{
    public class CategoryCustomAuthorize : IAuthorizationRequirement
    {
        public string token { get; set; }

        public CategoryCustomAuthorize(string token)
        {
            this.token = token;
        }

    }
}
