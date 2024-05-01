using JobsFinder_Main.Common;
using JobsFinder_Main.Filters;
using Microsoft.AspNet.Identity;
using Model.DAO;
using Model.EF;
using System;
using System.Linq;
using System.Web.Mvc;

namespace JobsFinder_Main.Controllers
{
    public class RecumentController : Controller
    {
        private readonly EmailService _emailService;
        private readonly RecumentDao dao;
        private readonly JobDao jobDao;
        private readonly CompanyDao companyDao;
        public RecumentController()
        {
            _emailService = new EmailService();
            dao = new RecumentDao();
            jobDao = new JobDao();
            companyDao = new CompanyDao();
        }
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
            var recument = dao.ViewDetail(id);
            return View(recument);
        }

        [HttpGet]
        [RecruiterAuthorization]
        public ActionResult Confirm(long id)
        {
            var recument = dao.ViewDetail(id);
            return PartialView(recument);
        }

        [HttpPost]
        [RecruiterAuthorization]
        public ActionResult Confirm(Recument model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    var result = dao.Confirm(entity);
                    if (result == true)
                    {
                        string emailSubject = "Your application has been accepted.";
                        string emailBody = "Hello, " + entity.Name + "!" + "<br/><br/>Your application for the " + dao.GetJobName(entity.JobID) + " job has been accepted by " + companyDao.GetCompany(entity.JobID) + "!<br/><br/>Best regards!<br/><br/>Recruitment team: JobNest.";
                        _emailService.SendEmail("hoa1520265@huce.edu.vn", emailSubject, emailBody);

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
        [RecruiterAuthorization]
        public ActionResult Cance(long id)
        {
            var recument = dao.ViewDetail(id);
            return PartialView(recument);
        }

        [HttpPost]
        [RecruiterAuthorization]
        public ActionResult Cance(Recument model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    var result = dao.Delete(entity);
                    if (result == true)
                    {
                        string emailSubject = "Your application has been cancled.";
                        string emailBody = "Hello, " + entity.Name + "!" + "<br/><br/>Your application for the " + dao.GetJobName(entity.JobID) + " job has been cancled by " + companyDao.GetCompany(entity.JobID) + "<br/><br/>Best regards!<br/><br/>Recruitment team: JobNest.";
                        _emailService.SendEmail("hoa1520265@huce.edu.vn", emailSubject, emailBody);

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