using personal_site_api.Dtos;
using personal_site_api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace personal_site_api.Models
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private ApplicationUserManager _AppUserManager;

        //called in BaseApiController
        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UserDto Create(ApplicationUser appUser)
        {
            return new UserDto
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result
            };
        }
    }
}