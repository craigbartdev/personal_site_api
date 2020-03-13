using Microsoft.AspNet.Identity;
using personal_site_api.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using static personal_site_api.Models.AccountBindingModels;

namespace personal_site_api.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(AppUserManager.Users.ToList().Select(u => TheModelFactory.Create(u)));
        }

        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await AppUserManager.FindByIdAsync(Id);

            if (user != null)
                return Ok(TheModelFactory.Create(user));

            return NotFound();
        }

        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser()
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email
            };

            IdentityResult addUserResult = await AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
                return GetErrorResult(addUserResult);

            //email configuration
            string code = await AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code }));
            await AppUserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your email by clicking <a href=\"" + callbackUrl + "\">here</a>");

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
                return Ok();
            else
                return GetErrorResult(result);
        }

        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.Identity.GetUserId();

            IdentityResult result = await AppUserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
                return GetErrorResult(result);

            await AppUserManager.SendEmailAsync(userId, "Password Change", "Your password has been changed");

            return Ok();
        }

        [HttpDelete]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            var appUser = await AppUserManager.FindByIdAsync(id);

            if (appUser == null)
                return NotFound();

            IdentityResult result = await AppUserManager.DeleteAsync(appUser);

            if (!result.Succeeded)
                return GetErrorResult(result);

            return Ok();
        }
    }
}