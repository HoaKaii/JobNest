using JobsFinder_Main.Common;
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
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session != null)
            {
                var dao = new RecumentDao();

                var confirm = dao.CheckApply(session.UserID, model.JobID);

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
                        UserID = session.UserID,
                        LetterInfo = model.LetterInfo,
                        CreateDate = DateTime.Now,
                        Status = 0,
                        Name = session.Name,
                        Phone = session.Phone,
                        Email = session.Email,
                        Address = session.Address,
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
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session != null)
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
                    TempData["Type"] = "Thất bại";
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
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session != null)
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
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session != null)
            {
                var dao = new RecumentDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    var result = dao.Delete(entity);
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
    }
}