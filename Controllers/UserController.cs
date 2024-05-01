using BotDetect.Web.Mvc;
using JobsFinder_Main.Models;
using Model.DAO;
using Model.EF;
using System;
using System.Linq;
using System.Web.Mvc;
using ServiceStack;
using System.Text.RegularExpressions;
using JobsFinder_Main.Identity;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using JobsFinder_Main.Filters;
using System.Threading.Tasks;
using JobsFinder_Main.Common;

namespace JobsFinder_Main.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailService _emailService;
        public UserController()
        {
            _userManager = new UserManager<AppUser>(new UserStore<AppUser>(new AppDbContext()));
            _emailService = new EmailService();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCaptcha", "The verification code is incorrect!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var passwdHash = Crypto.HashPassword(model.Password);
                var user = new AppUser()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    PasswordHash = passwdHash,
                    Address = model.Address,
                    PhoneNumber = model.Phone,
                    Name = model.Name,
                    Avatar = "./Assets/Client/JobsFinder/img/Logo.png",
                    CreatedDate = DateTime.Now
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

                    var authenManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenManager.SignIn(new AuthenticationProperties(), userIdentity);

                    model = new RegisterModel();
                    TempData["Message"] = "Registration successful!";
                    TempData["MessageType"] = "success";
                    TempData["Type"] = "Success";
                    return RedirectToAction("Login", "User", model);
                }
                else
                {
                    TempData["Message"] = "Registration unsuccessful!";
                    TempData["MessageType"] = "error";
                    TempData["Type"] = "Error";
                    return RedirectToAction("Register", "User");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid data");
                return View(model);
            }
        }

        public ActionResult Register_Recruiter()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCaptcha", "The verification code is incorrect!")]
        public ActionResult Register_Recruiter(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
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
                    _userManager.AddToRole(user.Id, "Recruiter");
                    var authenManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenManager.SignIn(new AuthenticationProperties(), userIdentity);

                    model = new RegisterModel();
                    TempData["Message"] = "Registration successful!";
                    TempData["MessageType"] = "success";
                    TempData["Type"] = "Success";
                    return RedirectToAction("Login_Recruiter", "User", model);
                }
                else
                {
                    TempData["Message"] = "Registration unsuccessful!";
                    TempData["MessageType"] = "error";
                    TempData["Type"] = "Error";
                    return RedirectToAction("Register_Recruiter", "User");
                }
            }
            else
            {
                ModelState.AddModelError("New Error", "Invalid data");
                return View(model);
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var user = _userManager.Find(model.UserName, model.Password);

            if (user != null && _userManager.IsInRole(user.Id, "JobSeeker"))
            {
                var authenManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenManager.SignIn(new AuthenticationProperties(), userIdentity);
                return RedirectToAction("Index", "Home");
            }
            else if (user != null && !user.Status)
            {
                ModelState.AddModelError("", "Your account is currently inactive.");
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password for a jobseeker account");
                return View();
            }
        }
        public ActionResult Login_Recruiter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login_Recruiter(LoginModel model)
        {
            var user = _userManager.Find(model.UserName, model.Password);

            if (user != null && _userManager.IsInRole(user.Id, "Recruiter"))
            {
                var authenManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenManager.SignIn(new AuthenticationProperties(), userIdentity);
                return RedirectToAction("Index", "Home");
            }
            else if (user != null && !user.Status)
            {
                ModelState.AddModelError("", "Your account is currently inactive.");
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password for a recruiter account");
                return View();
            }
        }

        public ActionResult Logout()
        {
            var authenManager = HttpContext.GetOwinContext().Authentication;
            authenManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Update(AppUser model, HttpPostedFileBase imgfile)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _userManager.FindByName(User.Identity.Name);

                if (currentUser != null)
                {
                    if (!string.IsNullOrEmpty(model.Name))
                    {
                        currentUser.Name = model.Name;
                    }
                    if (!string.IsNullOrEmpty(model.PasswordHash))
                    {
                        var newPasswordHash = _userManager.PasswordHasher.HashPassword(model.PasswordHash);
                        currentUser.PasswordHash = newPasswordHash;
                    }
                    if (!string.IsNullOrEmpty(model.PhoneNumber))
                    {
                        currentUser.PhoneNumber = model.PhoneNumber;
                    }
                    if (!string.IsNullOrEmpty(model.Address))
                    {
                        currentUser.Address = model.Address;
                    }
                    if (imgfile != null)
                    {
                        model.Avatar = imgfile.FileName;
                        string path = Server.MapPath("~/Content/Profile/" + imgfile.FileName);
                        imgfile.SaveAs(path);
                        currentUser.Avatar = "/Content/Profile/" + model.Avatar;
                    }

                    var result = _userManager.Update(currentUser);

                    if (User.IsInRole("JobSeeker"))
                    {
                        var profile = new Profile
                        {
                            UserID = currentUser.Id,
                            HoVaTen = model.Name,
                            AnhCaNhan = model.Avatar,
                            SoDienThoai = model.PhoneNumber,
                            DiaChiHienTai = model.Address
                        };
                        var profileDao = new ProfileDao();
                        var profileUpdated = profileDao.Update(profile);

                        if (result.Succeeded && profileUpdated)
                        {
                            TempData["Message"] = "Update successful!";
                            TempData["MessageType"] = "success";
                            return RedirectToAction("Manager", "User");
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Update successful!";
                        TempData["MessageType"] = "success";
                        return RedirectToAction("Manager", "User");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                else
                {
                    TempData["Message"] = "An error occurred! Please try again!";
                    TempData["MessageType"] = "warning";
                }
            }
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpGet]
        [AuthenFilter]
        public ActionResult Manager()
        {
            var userId = User.Identity.GetUserId();
            var user = _userManager.FindById(userId);

            if (user != null)
            {
                if (User.IsInRole("JobSeeker"))
                {
                    return RedirectToAction("Manager_JobSeeker", "User");
                }
                else
                {
                    return View(user);
                }
            }
            else
            {
                return RedirectToAction("Login_Recruiter", "User");
            }
        }

        [HttpGet]
        [AuthenFilter]
        public ActionResult Manager_JobSeeker()
        {
            var userId = User.Identity.GetUserId();
            var user = _userManager.FindById(userId);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet]
        [RecruiterAuthorization]
        public ActionResult CompanyManager()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login_Recruiter", "User");
            }
            else
            {
                var currentUser = _userManager.FindByName(User.Identity.Name);

                if (currentUser != null)
                {
                    var companyDao = new CompanyDao();
                    var company = new CompanyModel();
                    var model = companyDao.ListInUser(currentUser.UserName);

                    foreach (var item in model)
                    {
                        company.Name = item.Name;
                        company.Avatar = item.Avatar;
                        company.Background = item.Background;
                        company.LinkPage = item.LinkPage;
                        company.Employees = item.Employees;
                        company.Location = item.Location;
                        company.Description = item.Description;
                    }

                    return View(company);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpGet]
        [RecruiterAuthorization]
        public ActionResult JobManager(AppUser user)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login_Recruiter", "User");
            }
            else
            {
                var jobDao = new JobDao();
                var job = new JobModel();
                var model = jobDao.ListInUser(user.UserName);

                foreach (var item in model)
                {
                    job.ID = item.ID;
                    job.Name = item.Name;
                    job.MetaTitle = item.MetaTitle;
                    job.Description = item.Description;
                    job.RequestCandidate = item.RequestCandidate;
                    job.Interest = item.Interest;
                    job.Image = jobDao.GetAvatarFromCompany(item.CompanyID, item.UserID);
                    job.Salary = item.Salary;
                    job.SalaryMin = item.SalaryMin;
                    job.SalaryMax = item.SalaryMax;
                    job.Quantity = item.Quantity;
                    job.CategoryID = item.CategoryID;
                    job.Details = item.Details;
                    job.Deadline = item.Deadline;
                    job.Rank = item.Rank;
                    job.Gender = item.Gender;
                    job.Experience = item.Experience;
                    job.WorkLocation = item.WorkLocation;
                    job.CompanyID = item.CompanyID;
                    job.CarrerID = item.CarrerID;
                    job.UserID = item.UserID;
                    job.code = item.Code;
                }
                return View(job);
            }
        }

        [HttpGet]
        [ChildActionOnly]
        [RecruiterAuthorization]
        public ActionResult CreateCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [RecruiterAuthorization]
        public ActionResult CreateCompany(CompanyModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login_Recruiter", "User");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var currentUser = _userManager.FindByName(User.Identity.Name);

                    if (currentUser != null)
                    {
                        var company = new Company();
                        company.Name = model.Name;
                        company.LinkPage = model.LinkPage;
                        company.Description = model.Description;
                        company.Avatar = model.Avatar;
                        company.Background = model.Background;
                        company.Employees = model.Employees;
                        company.Location = model.Location;
                        string name = model.Name;
                        string slug = Regex.Replace(name, @"[^a-zA-Z0-9]", "-").ToLower();
                        company.MetaTitle = slug;
                        company.CreatedDate = DateTime.Now;
                        company.Status = true;
                        company.CreatedBy = currentUser.UserName;

                        var companyDao = new CompanyDao();
                        var result = companyDao.Insert(company);

                        if (result > 0)
                        {
                            TempData["Message"] = "Create successful!";
                            TempData["MessageType"] = "success";
                            TempData["Type"] = "Success";
                            return RedirectToAction("Manager", "User");
                        }
                        else
                        {
                            TempData["Message"] = "Create unsuccessful!";
                            TempData["MessageType"] = "error";
                            TempData["Type"] = "Error";
                            return RedirectToAction("Manager", "User");
                        }
                    }
                    else
                    {
                        TempData["Message"] = "An error occurred! Please try again!";
                        TempData["MessageType"] = "warning";
                        TempData["Type"] = "Warning";
                        return RedirectToAction("Manager", "User");
                    }
                }
                else
                {
                    return View(model);
                }
            }
        }

        [HttpDelete]
        [RecruiterAuthorization]
        public ActionResult DeleteCompany(int ID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login_Recruiter", "User");
            }
            else
            {
                var dao = new CompanyDao();
                var result = dao.Delete(ID);

                if (result)
                {
                    TempData["Message"] = "Delete successfull!";
                    TempData["MessageType"] = "success";
                    TempData["Type"] = "Success";
                    return RedirectToAction("Manager", "User");
                }
                else
                {
                    TempData["Message"] = "Delete unsuccessfull!";
                    TempData["MessageType"] = "error";
                    TempData["Type"] = "Error";
                    return RedirectToAction("Manager", "User");
                }
            }
        }

        [HttpDelete]
        [RecruiterAuthorization]
        public ActionResult DeleteJob(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login_Recruiter", "User");
            }

            var dao = new JobDao();
            var result = dao.Delete(id);

            if (result)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }


        [HttpGet]
        [RecruiterAuthorization]
        public ActionResult EditCompany(int ID)
        {
            var company = new CompanyDao().ViewDetail(ID);
            return View(company);
        }

        [HttpPost]
        [ValidateInput(false)]
        [RecruiterAuthorization]
        public ActionResult EditCompany(Company model, HttpPostedFileBase imgfile, HttpPostedFileBase coverfile)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login_Recruiter", "User");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var currentUser = _userManager.FindByName(User.Identity.Name);

                    if (currentUser != null)
                    {
                        var dao = new CompanyDao();
                        var company = dao.ViewDetail(model.ID);
                        if (company != null)
                        {
                            if (!string.IsNullOrEmpty(model.Name))
                            {
                                company.Name = model.Name;
                            }
                            if (model.Employees != null)
                            {
                                company.Employees = model.Employees;
                            }
                            if (!string.IsNullOrEmpty(model.Location))
                            {
                                company.Location = model.Location;
                            }
                            if (imgfile != null)
                            {
                                model.Avatar = imgfile.FileName;
                                string path = Server.MapPath("~/Content/Profile/" + imgfile.FileName);
                                imgfile.SaveAs(path);
                                company.Avatar = "/Content/Profile/" + model.Avatar;
                            }
                            if (coverfile != null)
                            {
                                model.Background = coverfile.FileName;
                                string path = Server.MapPath("~/Content/Profile/" + coverfile.FileName);
                                coverfile.SaveAs(path);
                                company.Background = "/Content/Profile/" + model.Background;
                            }
                            if (!string.IsNullOrEmpty(model.Description))
                            {
                                company.Description = model.Description;
                            }
                            company.ModifiedDate = DateTime.Now;
                            company.ModifiedBy = currentUser.UserName;
                            var result = dao.Update(company);

                            if (result)
                            {
                                TempData["Message"] = "Update successfull!";
                                TempData["MessageType"] = "success";
                                TempData["Type"] = "Success";
                                return RedirectToAction("CompanyManager", "User", new { company.ID });
                            }
                            else
                            {
                                TempData["Message"] = "Update unsuccessfull!";
                                TempData["MessageType"] = "error";
                                TempData["Type"] = "Error";
                                return RedirectToAction("CompanyManager", "User", new { company.ID });
                            }
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [RecruiterAuthorization]
        public ActionResult CreateJob()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [RecruiterAuthorization]
        public ActionResult CreateJob(JobModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login_Recruiter", "User");
            }
            else
            {
                var job = new Job();
                var dao = new JobDao();

                job.Name = model.Name;
                string name = model.Name;
                string slug = Regex.Replace(name, @"[^a-zA-Z0-9]", "-").ToLower();
                job.MetaTitle = slug;
                job.Description = model.Description;
                job.RequestCandidate = model.RequestCandidate;
                job.Interest = model.Interest;
                job.Image = model.Image;
                job.Salary = model.Salary;
                job.SalaryMin = model.SalaryMin;
                job.SalaryMax = model.SalaryMax;
                job.Quantity = model.Quantity;
                Random random = new Random();

                string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string randomLetters = new string(Enumerable.Repeat(letters, 3).Select(s => s[random.Next(s.Length)]).ToArray());
                string numbers = "0123456789";
                string randomNumbers = new string(Enumerable.Repeat(numbers, 7).Select(s => s[random.Next(s.Length)]).ToArray());
                string code = randomLetters + randomNumbers;
                job.Code = code;

                job.CategoryID = model.CategoryID;
                job.Details = model.Details;
                job.Deadline = model.Deadline;
                job.Rank = model.Rank;
                job.Gender = model.Gender;
                job.Experience = model.Experience;
                job.WorkLocation = model.WorkLocation;
                job.CompanyID = model.CompanyID;
                job.CarrerID = model.CarrerID;
                job.UserID = model.UserID;
                job.CreatedBy = User.Identity.Name;
                job.Status = true;

                var result = dao.Insert(job);

                if (result > 0)
                {
                    model = new JobModel();

                    TempData["Message"] = "Create successfull!";
                    TempData["MessageType"] = "success";
                    TempData["Type"] = "Success";
                    return RedirectToAction("Manager", "User", model);
                }
                else
                {
                    TempData["Message"] = "Create unsuccessfull!";
                    TempData["MessageType"] = "error";
                    TempData["Type"] = "Error";
                    return RedirectToAction("Manager", "User");
                }
            }
        }

        [HttpGet]
        [RecruiterAuthorization]
        public ActionResult ListJobs()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login_Recruiter", "User");
            }
            else
            {
                var currentUser = _userManager.FindByName(User.Identity.Name);

                if (currentUser == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var companyDao = new CompanyDao();
                var list = companyDao.ListInUser(currentUser.UserName);
                return View(list);
            }
        }

        [HttpGet]
        [RecruiterAuthorization]
        public ActionResult ListCompany()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login_Recruiter", "User");
            }

            var currentUser = _userManager.FindByName(User.Identity.Name);

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var companyDao = new CompanyDao();
            var list = companyDao.ListInUser(currentUser.UserName);
            return View(list);
        }

        [HttpGet]
        [RecruiterAuthorization]
        public ActionResult EditJob(int ID)
        {
            var job = new JobDao().ViewDetail(ID);
            return View(job);
        }

        [HttpPost]
        [ValidateInput(false)]
        [RecruiterAuthorization]
        public ActionResult EditJob(Job model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login_Recruiter", "User");
            }
            else
            {
                var dao = new JobDao();
                var job = dao.ViewDetail(model.ID);
                if (job != null)
                {
                    if (!string.IsNullOrEmpty(model.Name))
                    {
                        job.Name = model.Name;
                    }
                    if (!string.IsNullOrEmpty(model.Description))
                    {
                        job.Description = model.Description;
                    }
                    if (!string.IsNullOrEmpty(model.RequestCandidate))
                    {
                        job.RequestCandidate = model.RequestCandidate;
                    }
                    if (!string.IsNullOrEmpty(model.Interest))
                    {
                        job.Interest = model.Interest;
                    }
                    if (!string.IsNullOrEmpty(model.Details))
                    {
                        job.Details = model.Details;
                    }
                    job.Salary = model.Salary;
                    job.SalaryMin = model.SalaryMin;
                    job.SalaryMax = model.SalaryMax;

                    job.Quantity = model.Quantity;
                    job.CategoryID = model.CategoryID;
                    job.CarrerID = model.CarrerID;
                    job.Deadline = model.Deadline;
                    job.Experience = model.Experience;
                    job.Gender = model.Gender;
                    job.WorkLocation = model.WorkLocation;
                    var result = dao.Update(job);

                    if (result)
                    {
                        TempData["Message"] = "Update successfull!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Success";
                        return RedirectToAction("ListJobs", "User");
                    }
                    else
                    {
                        TempData["Message"] = "Update unsuccessfull!";
                        TempData["MessageType"] = "error";
                        TempData["Type"] = "Error";
                        return RedirectToAction("ListJobs", "User");
                    }
                }
            }
            return View(model);
        }
        public void SetViewBag(long? selectedId = null)
        {
            var JobCategorydao = new JobCategoryDao();
            ViewBag.JobCategoryId = new SelectList(JobCategorydao.ListAll(), "ID", "Name", selectedId);

            var careerDao = new CareerDao();
            ViewBag.JobCareerId = new SelectList(careerDao.ListAll(), "ID", "Name", selectedId);

            var companyDao = new CompanyDao();
            ViewBag.CompanyId = new SelectList(companyDao.ListAll(), "ID", "Name", selectedId);
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View("ForgotPasswordConfirmation");
                }

                var token = TokenGenerator.GenerateToken();
                var callbackUrl = Url.Action("ResetPassword", "User", new { userId = user.Id,token = token }, protocol: Request.Url.Scheme);
                _emailService.SendEmail(user.Email, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "User");
            }

            return View(model);
        }

        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ResetPassword(string userId, string token)
        {
            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var newPassword = _userManager.PasswordHasher.HashPassword(model.NewPassword);
            user.PasswordHash = newPassword;
            var result = _userManager.Update(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Reset password successfull!";
                TempData["MessageType"] = "success";
                TempData["Type"] = "Success";
                return RedirectToAction("Login", "User");
            }
            else
            {
                TempData["Message"] = "Reset password unsuccessfull!";
                TempData["MessageType"] = "error";
                TempData["Type"] = "Error";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }
        }
    }
}