using JobsFinder_Main.Common;
using Microsoft.AspNet.Identity;
using Model.DAO;
using Model.EF;
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
        // GET: Job
        [HttpGet]
        public ActionResult Index(string searchString, string searchName, string searchLocation, string fillterCareer, string fillterCategory, string fillterGender, string fillterEXP, int page = 1, int pageSize = 10)
        {
            SetViewBag();
            var dao = new JobDao();
            var model = dao.ListAllPaging(searchString, searchName, searchLocation, fillterCareer, fillterCategory, fillterGender, fillterEXP, page, pageSize);
            ViewBag.SearchName = searchString;
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
            var job = new JobDao().ViewDetail(id);
            var recument = new Recument();

            job.recument = recument;
            return View(job);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new CompanyDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Apply(Job entity)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new JobDao();
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