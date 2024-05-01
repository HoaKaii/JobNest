using BotDetect.Web.Mvc;
using JobsFinder_Main.Identity;
using JobsFinder_Main.Models;
using Microsoft.AspNet.Identity;
using Model.DAO;
using Model.EF;
using System.Web.Helpers;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JobsFinder_Main.ApiControllers
{
    public class UserController : ApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public UserController()
        {
            _userManager = new UserManager<AppUser>(new UserStore<AppUser>(new AppDbContext()));
        }

        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCaptcha", "The verification code is incorrect!")]
        public IHttpActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var passwdHash = Crypto.HashPassword(model.Password);
            var user = new AppUser()
            {
                Email = model.Email,
                UserName = model.UserName,
                PasswordHash = passwdHash,
                Address = model.Address,
                PhoneNumber = model.Phone,
                Name = model.Name,
                Avatar = "./Assets/Client/JobsFinder/img/Logo.png"
            };

            IdentityResult identityResult = _userManager.Create(user);
            if (identityResult.Succeeded)
            {
                _userManager.AddToRole(user.Id, "JobSeeker");

                var profile = new Profile()
                {
                    UserID = user.Id,
                    HoVaTen = model.Name,
                    DiaChiHienTai = model.Address,
                    SoDienThoai = model.Phone,
                    AnhCaNhan = model.Avatar,
                };

                var profileDao = new ProfileDao();
                profileDao.Insert(profile);

                return Ok(new { Message = "Registration successful!" });
            }
            else
            {
                return BadRequest("Registration unsuccessful!");
            }
        }
    }
}
