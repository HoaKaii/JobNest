using BotDetect.Web.Mvc;
using JobsFinder_Main.Common;
using JobsFinder_Main.Models;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using System.Configuration;
using BotDetect;
using ServiceStack;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using Microsoft.AspNet.Identity;
using JobsFinder_Main.Filters;


namespace JobsFinder_Main.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        // GET: Profile/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Profile/Create
        [HttpPost]
        [JobSeekerAuthorization]
        public ActionResult Create(Profile profile, HttpPostedFileBase imgfile)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userID = User.Identity.GetUserId();

                var dao = new ProfileDao();
                var updateProfile = dao.GetByID(userID);
                if (updateProfile != null)
                {
                    if (!string.IsNullOrEmpty(profile.HoVaTen))
                    {
                        updateProfile.HoVaTen = profile.HoVaTen;
                    }
                    if (imgfile != null)
                    {
                        profile.AnhCaNhan = imgfile.FileName;
                        string path = Server.MapPath("~/Content/Profile/" + imgfile.FileName);
                        imgfile.SaveAs(path);
                        updateProfile.AnhCaNhan = "/Content/Profile/" + profile.AnhCaNhan;
                    }
                    if (!string.IsNullOrEmpty(profile.GioiTinh))
                    {
                        updateProfile.GioiTinh = profile.GioiTinh;
                    }
                    if (!string.IsNullOrEmpty(profile.NgaySinh.ToString()))
                    {
                        updateProfile.NgaySinh = profile.NgaySinh;
                    }
                    if (!string.IsNullOrEmpty(profile.ThangSinh.ToString()))
                    {
                        updateProfile.ThangSinh = profile.ThangSinh;
                    }
                    if (!string.IsNullOrEmpty(profile.NamSinh.ToString()))
                    {
                        updateProfile.NamSinh = profile.NamSinh;
                    }
                    if (!string.IsNullOrEmpty(profile.DiaChiHienTai))
                    {
                        updateProfile.DiaChiHienTai = profile.DiaChiHienTai;
                    }
                    if (!string.IsNullOrEmpty(profile.Email))
                    {
                        updateProfile.Email = profile.Email;
                    }
                    if (!string.IsNullOrEmpty(profile.SoDienThoai))
                    {
                        updateProfile.SoDienThoai = profile.SoDienThoai;
                    }
                    if (!string.IsNullOrEmpty(profile.GioiThieu))
                    {
                        updateProfile.GioiThieu = profile.GioiThieu;
                    }
                    var result = dao.Update(updateProfile);
                    if (result)
                    {
                        TempData["Message"] = "Update successfull!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Success";
                        return RedirectToAction("Index", "Profile");
                    }
                    else
                    {
                        TempData["Message"] = "Update unsuccessfull!";
                        TempData["MessageType"] = "error";
                        TempData["Type"] = "Error";
                        return RedirectToAction("Index", "Profile");
                    }
                }
                else
                {
                    profile.UserID = userID;
                    var result = dao.Insert(profile);
                    if (result != null)
                    {
                        TempData["Message"] = "Update successfull!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Success";
                        return RedirectToAction("Index", "Profile");
                    }
                    else
                    {
                        TempData["Message"] = "Update unsuccessfull!";
                        TempData["MessageType"] = "error";
                        TempData["Type"] = "Error";
                        return RedirectToAction("Index", "Profile");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public ActionResult Detail(long id)
        {
            var profile = new ProfileDao().ViewDetail(id);
            return View(profile);
        }

        public ActionResult Confirm(long id)
        {
            var recument = new RecumentDao().ViewDetail(id);
            return View(recument);
        }

        [HttpPost]
        public ActionResult Confirm(Recument entity)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session != null)
            {
                var dao = new RecumentDao();
                var recument = new Recument
                {
                    Status = entity.Status,
                };

                var result = dao.Confirm(recument);
                if (result == true)
                {
                    TempData["Message"] = "Update successfull!";
                    TempData["MessageType"] = "success";
                    TempData["Type"] = "Success";
                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    TempData["Message"] = "Update unsuccessfull!";
                    TempData["MessageType"] = "error";
                    TempData["Type"] = "Error";
                    return RedirectToAction("Index", "Profile");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
    }
}