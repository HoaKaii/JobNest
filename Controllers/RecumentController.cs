using JobsFinder_Main.Common;
using Microsoft.AspNet.Identity;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsFinder_Main.Controllers
{
    public class RecumentController : Controller
    {
        // GET: Recument
        public ActionResult Index()
        {
            return View();
        }

        // GET: Recument/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Recument/Create
        [HttpPost]
        public ActionResult Create(Recument model)
        {
            if (User.Identity.IsAuthenticated)
            {
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
                var dao = new RecumentDao();

                var confirm = dao.CheckApply(userID, model.JobID);

                if (confirm == false)
                {
                    TempData["Message"] = "Update unsuccessfull!";
                    TempData["MessageType"] = "error";
                    TempData["Type"] = "Error";
                    return RedirectToAction("Detail", "Job", model);
                }
                else
                {
                    var recument = new Recument
                    {
                        JobID = model.JobID,
                        UserID = userID,
                        LetterInfo = model.LetterInfo,
                        CreateDate = DateTime.Now,
                        Status = 0,
                        Name = Name,
                        Phone = Phone,
                        Email = Email,
                        Address = Address,
                        ProfileID = model.ProfileID,
                    };

                    var result = dao.Insert(recument);
                    if (result == true)
                    {
                        TempData["Message"] = "Update successfull!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Success";
                        return RedirectToAction("Detail", "Job", model.JobID);
                    }
                    else
                    {
                        TempData["Message"] = "Update unsuccessfull!";
                        TempData["MessageType"] = "error";
                        TempData["Type"] = "Error";
                        return RedirectToAction("Detail", "Job", model.JobID);
                    }
                }

            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet]
        public ActionResult DeleteConfirm(long id)
        {
            var recument = new RecumentDao().ViewDetail(id);
            return PartialView(recument);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(Recument model)
        {
           if (User.Identity.IsAuthenticated)
            { 
                var dao = new RecumentDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    var result = dao.Delete(entity);
                    if (result)
                    {
                        TempData["Message"] = "Update successfull!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Success";
                        return RedirectToAction("Detail", "Job", model.JobID);
                    }
                    else
                    {
                        TempData["Message"] = "Update unsuccessfull!";
                        TempData["MessageType"] = "error";
                        TempData["Type"] = "Error";
                        return RedirectToAction("Detail", "Job", model.JobID);
                    }
                }
                else
                {
                    TempData["Message"] = "An error occurred! Please try again!";
                    TempData["MessageType"] = "warning";
                    TempData["Type"] = "Warning";
                    return RedirectToAction("Detail", "Job", model.JobID);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult Detail(long id)
        {
            var recument = new RecumentDao().ViewDetail(id);
            return View(recument);
        }

        [HttpGet]
        public ActionResult Confirm(long id)
        {
            var recument = new RecumentDao().ViewDetail(id);
            return PartialView(recument);
        }

        [HttpPost]
        public ActionResult Confirm(Recument model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new RecumentDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    var result = dao.Confirm(entity);
                    if (result == true)
                    {
                        TempData["Message"] = "Update sucessfull!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Success";
                        return RedirectToAction("ListJobs", "User");
                    }
                    else
                    {
                        TempData["Message"] = "Update unsucessfull!";
                        TempData["MessageType"] = "error";
                        TempData["Type"] = "Error";
                        return RedirectToAction("ListJobs", "User");
                    }
                }
                else
                {
                    TempData["Message"] = "Update unsucessfull!";
                    TempData["MessageType"] = "error";
                    TempData["Type"] = "Error";
                    return RedirectToAction("ListJobs", "User");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        [HttpGet]
        public ActionResult Cance(long id)
        {
            var recument = new RecumentDao().ViewDetail(id);
            return PartialView(recument);
        }

        [HttpPost]
        public ActionResult Cance(Recument model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new RecumentDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    var result = dao.Delete(entity);
                    if (result == true)
                    {
                        TempData["Message"] = "Cancle sucessfull!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Success";
                        return RedirectToAction("ListJobs", "User");
                    }
                    else
                    {
                        TempData["Message"] = "Cancle unsucessfull!";
                        TempData["MessageType"] = "error";
                        TempData["Type"] = "Error";
                        return RedirectToAction("ListJobs", "User");
                    }
                }
                else
                {
                    TempData["Message"] = "Cancle unsucessfull!";
                    TempData["MessageType"] = "error";
                    TempData["Type"] = "Error";
                    return RedirectToAction("ListJobs", "User");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
    }
}