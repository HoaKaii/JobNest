using System.Web.Mvc;
using JobsFinder_Main.Common;
using Model.DAO;
using Model.EF;
using JobsFinder_Main.Identity;
using System.Linq;
using PagedList;
using System.Threading.Tasks;
using JobsFinder_Main.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace JobsFinder_Main.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController()
        {
            _userManager = new UserManager<AppUser>(new UserStore<AppUser>(new AppDbContext()));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AppDbContext()));
        }
        // GET: Admin/User
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var users = _userManager.Users;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }

            var model = users.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            ViewBag.Roles = new SelectList(roles);
            return View();
        }

        public ActionResult Edit(string id)
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            ViewBag.Roles = new SelectList(roles);
            var user = _userManager.FindById<AppUser, string>(id);
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserCreate user)
        {
            if (ModelState.IsValid)
            {
               
                var appUser = new AppUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Name = user.FullName,
                    PhoneNumber = user.Phone,
                    Address = user.Address
                };

                var result = await _userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                {
                    foreach (var role in user.SelectedRoles)
                    {
                        await _userManager.AddToRoleAsync(appUser.Id, role);
                    }

                    SetAlert("User added successfully", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AppUser user)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id);

                if (appUser != null)
                {
                    appUser.UserName = user.UserName;
                    appUser.Name = user.Name;
                    appUser.Address = user.Address;
                    appUser.Email = user.Email;
                    appUser.PhoneNumber = user.PhoneNumber;

                    if (!string.IsNullOrEmpty(user.PasswordHash))
                    {
                        var newPasswordHash = _userManager.PasswordHasher.HashPassword(user.PasswordHash);
                        appUser.PasswordHash = newPasswordHash;
                    }

                    var result = await _userManager.UpdateAsync(appUser);

                    if (result.Succeeded)
                    {
                        SetAlert("User updated successfully", "success");
                        return RedirectToAction("Index", "User");
                    }
                }

                ModelState.AddModelError("", "User updated unsuccessfully");
            }

            return View("Index");
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user.Id);
                await _userManager.RemoveFromRolesAsync(user.Id, userRoles.ToArray());

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    SetAlert("User deleted successfully", "success");
                }
                else
                {
                    SetAlert("User deleted unsuccessfully", "danger");
                }
            }
            else
            {
                SetAlert("The user does not exist", "warning");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                appUser.Status = !appUser.Status;

                var result = await _userManager.UpdateAsync(appUser);

                if (result.Succeeded)
                {
                    return Json(new
                    {
                        status = appUser.Status
                    });
                }
            }
            return Json(new
            {
                status = false
            });
        }
    }
}