using blog.Categories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using blog.Identity;
using blog.Account;
using blog.ExtensionMethods;

namespace blog.Controllers
{
    [Route("api/category")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : AbpController
    {
        protected ICateAppService cateAppService { get; }

        public CategoryController(ICateAppService cateAppService)
        {
            this.cateAppService = cateAppService;
        }

        [HttpGet]
        public async Task<List<CategoryDto>> GetListAsync()
        {
            return await cateAppService.GetCateList();
        }

        [HttpGet]
        [Route("{cateid}")]
        public async Task<CategoryDto> GetAsync(Guid cateid)
        {
            var result = await cateAppService.GetSepecificCate(cateid);
            result.userId = HttpContext.User.Identity.Name;
            return result;
        }

        [HttpPost]
        [MyCustom("", "blogproject.Category.Create")]
        public async Task<CreateUpdateCateResponse> CreateAsync(CategoryDto createdCategory)
        {
            var result = await cateAppService.CreateCategory(createdCategory);
            var response = new CreateUpdateCateResponse();
            var x = CurrentUser.Id;
            if (result)
            {

                response.categoryDto = createdCategory;
                response.IsSuccess = true;
                response.IdUser = x.ToString();
            }
            else
            {
                response.IsSuccess = false;
                response.ErrorMesseage = new string[] { "This title has already existed!" };
                response.IdUser = x.ToString();
            }
            return response;
        }

        [HttpPut]
        [Route("{cateid}")]
        [MyCustom("", "blogproject.Category.Update")]
        public async Task<CreateUpdateCateResponse> UpdateAsync(CategoryDto updatedCategory,Guid cateid)
        {
            updatedCategory.Id = cateid;
            var result =  await cateAppService.UpdateCategory(updatedCategory);
            var response = new CreateUpdateCateResponse();
            if (result)
            {

                response.categoryDto = updatedCategory;
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.ErrorMesseage = new string[] { "Update Fail!" };
            }
            return response;
        }

        [HttpDelete]
        [Route("{cateid}")]
        [MyCustom("", "blogproject.Category.Update")]
        public async Task<CreateUpdateCateResponse> DeleteAsync(Guid cateid)
        {
            var result =  await cateAppService.DeleteCategory(cateid);
            var response = new CreateUpdateCateResponse();
            if (result) response.IsSuccess = true;
            else
            {
                response.IsSuccess = false;
                response.ErrorMesseage = new string[] { "Update Fail!" };
            }
            return response;
        }
    }
}
