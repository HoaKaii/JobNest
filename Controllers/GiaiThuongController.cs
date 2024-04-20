using JobsFinder_Main.Common;
using JobsFinder_Main.Models;
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
    public class GiaiThuongController : Controller
    {
        // GET: GiaiThuong
        public ActionResult Index()
        {
            return View();
        }

        // GET: GiaiThuong/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: GiaiThuong/Create
        [HttpPost]
        public ActionResult Create(GiaiThuongModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new GiaiThuongDao();

                var giaiThuong = new GiaiThuong
                {
                    TenGiaiThuong = model.TenGiaiThuong,
                    ToChuc = model.ToChuc,
                    ThangNhan = model.ThangNhan,
                    NamNhan = model.NamNhan,
                    Img = model.Img,
                    LienKet = model.LienKet,
                    UserID = User.Identity.GetUserId(),
                    CreatedBy = User.Identity.GetUserName(),
            };

                var result = dao.Insert(giaiThuong);
                if (result == true)
                {
                    model = new GiaiThuongModel();
                    TempData["Message"] = "Create successfull!";
                    TempData["MessageType"] = "success";
                    TempData["Type"] = "Success";
                    return RedirectToAction("Index", "Profile", model);
                }
                else
                {
                    TempData["Message"] = "Create unsuccessfull!";
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

        [HttpGet]
        public ActionResult Update(long id)
        {
            var giaiThuong = new GiaiThuongDao().ViewDetail(id);
            return PartialView(giaiThuong);
        }

        [HttpPost]
        public ActionResult Update(GiaiThuong model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new GiaiThuongDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(model.TenGiaiThuong))
                        {
                            entity.TenGiaiThuong = model.TenGiaiThuong;
                        }
                        if (!string.IsNullOrEmpty(model.ToChuc))
                        {
                            entity.ToChuc = model.ToChuc;
                        }
                        if (!string.IsNullOrEmpty(model.ThangNhan.ToString()))
                        {
                            entity.ThangNhan = model.ThangNhan;
                        }
                        if (!string.IsNullOrEmpty(model.NamNhan.ToString()))
                        {
                            entity.NamNhan = model.NamNhan;
                        }
                        entity.ModifiedBy = User.Identity.Name;
                        entity.ModifiedDate = DateTime.Now;

                        var result = dao.Update(entity);
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
                        TempData["Message"] = "An error occurred! Please try again!";
                        TempData["MessageType"] = "warning";
                        TempData["Type"] = "Warning";
                        return RedirectToAction("Index", "Profile");
                    }
                }
                else
                {
                    TempData["Message"] = "An error occurred! Please try again";
                    TempData["MessageType"] = "warning";
                    TempData["Type"] = "Warning";
                    return RedirectToAction("Index", "Profile");
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
            var giaiThuong = new GiaiThuongDao().ViewDetail(id);
            return PartialView(giaiThuong);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(GiaiThuong model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new GiaiThuongDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    var result = dao.Delete(entity);
                    if (result)
                    {
                        TempData["Message"] = "Delete successfull!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Success";
                        return RedirectToAction("Index", "Profile");
                    }
                    else
                    {
                        TempData["Message"] = "Delete unsuccessfull!";
                        TempData["MessageType"] = "error";
                        TempData["Type"] = "Error";
                        return RedirectToAction("Index", "Profile");
                    }
                }
                else
                {
                    TempData["Message"] = "An error occurred! Please try again!";
                    TempData["MessageType"] = "warning";
                    TempData["Type"] = "Warning";
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