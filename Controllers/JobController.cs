using JobsFinder_Main.Common;
using JobsFinder_Main.Filters;
using Microsoft.AspNet.Identity;
using Model.DAO;
using Model.EF;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace JobsFinder_Main.Controllers
{
    public class JobController : Controller
    {
        private readonly EmailService _emailService;
        private readonly JobDao dao;
        private readonly CompanyDao companyDao;
        public JobController()
        {
            _emailService = new EmailService();
            dao = new JobDao();
            companyDao = new CompanyDao();
        }
        // GET: Job
        [HttpGet]
        public ActionResult Index(string searchName, string searchLocation, string fillterCareer, string fillterCategory, string fillterGender, string fillterEXP, int page = 1, int pageSize = 5)
        {
            SetViewBag();
            var model = dao.ListAllPaging(searchName, searchLocation, fillterCareer, fillterCategory, fillterGender, fillterEXP, page, pageSize);
            ViewBag.SearchName = searchName;
            ViewBag.SearchLocation = searchLocation;
            ViewBag.FillterCareer = fillterCareer;
            ViewBag.FillterCategory = fillterCategory;
            ViewBag.FillterGender = fillterGender;
            ViewBag.FillterEXP = fillterEXP;

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Carousel()
        {
            SetViewBag();
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Tab_Fillter()
        {
            return PartialView();
        }
        public void SetViewBag(long? selectedId = null)
        {
            var commonDao = new CommonDao.CityDao();
            var cities = commonDao.GetAllCities().Select(c => new SelectListItem
            {
                Value = c.Value,
                Text = c.Name
            });
            ViewBag.CityList = new SelectList(cities, "Value", "Text", selectedId);
        }
        public ActionResult Detail(long id)
        {
            var job = dao.ViewDetail(id);
            dao.UpdateViewCount(id);
            var recument = new Recument();
            job.recument = recument;
            return View(job);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            dao.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [JobSeekerAuthorization]
        public ActionResult Apply(Job entity)
        {
            if (User.Identity.IsAuthenticated)
            {
                var recumentDao = new RecumentDao();
                var userID = User.Identity.GetUserId();
                string Name = "";
                string Phone = "";
                string Address = "";
                string Email = "";
                using (var dbContext = new Identity.AppDbContext())
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    if (user != null)
                    {
                        Name = user.Name;
                        Phone = user.PhoneNumber;
                        Address = user.Address;
                        Email = user.Email;
                    }
                }
                var confirm = recumentDao.CheckApply(userID, entity.recument.JobID);

                if (confirm == false)
                {
                    TempData["Message"] = "Unsuccessful application!";
                    TempData["MessageType"] = "error";
                    TempData["Type"] = "Error";
                    return RedirectToAction("Index", "Job", entity.ID);
                }
                else
                {
                    var job = new Job
                    {
                        recument = new Recument
                        {
                            JobID = entity.recument.JobID,
                            UserID = userID,
                            LetterInfo = entity.recument.LetterInfo,
                            CreateDate = DateTime.Now,
                            Status = 0,
                            Name = Name,
                            Phone = Phone,
                            Email = Email,
                            Address = Address,
                            ProfileID = entity.recument.ProfileID,
                        }
                    };

                    var result = dao.ApplyJob(job);
                    if (result == true)
                    {
                        string emailSubject = "Your job has been applied.";
                        string emailBody = "Hello, " + companyDao.GetCompany(entity.recument.JobID) + "!<br/><br/>Your " + recumentDao.GetJobName(entity.recument.JobID) + " job has been applied by " + Name + "<br/><br/>Best regards!<br/><br/>Recruitment team: JobNest.";
                        _emailService.SendEmail("hoa1520265@huce.edu.vn", emailSubject, emailBody);

                        TempData["Message"] = "Successful application!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Success";
                        return RedirectToAction("Index", "Job", entity.ID);
                    }
                    else
                    {
                        TempData["Message"] = "An error occurred! Please try again!";
                        TempData["MessageType"] = "warning";
                        TempData["Type"] = "Warning";
                        return RedirectToAction("Index", "Job", entity.ID);
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
    }
}