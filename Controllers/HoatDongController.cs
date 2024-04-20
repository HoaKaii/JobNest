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
    public class HoatDongController : Controller
    {
        // GET: HoatDong
        public ActionResult Index()
        {
            return View();
        }

        // GET: HoatDong/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: HoatDong/Create
        [HttpPost]
        public ActionResult Create(HoatDongModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new HoatDongDao();

                var hoatDong = new HoatDong
                {
                    TenHoatDong = model.TenHoatDong,
                    ViTriThamGia = model.ViTriThamGia,
                    ThangThamGia = model.ThangThamGia,
                    NamThamGia = model.NamThamGia,
                    ThangKetThuc = model.ThangKetThuc,
                    NamKetThuc = model.NamKetThuc,
                    MoTaChiTiet = model.MoTaChiTiet,
                    Img = model.Img,
                    LienKet = model.LienKet,
                    UserID = User.Identity.GetUserId(),
                    CreatedBy = User.Identity.GetUserName()
                };

                var result = dao.Insert(hoatDong);
                if (result == true)
                {
                    model = new HoatDongModel();
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
            var hoatDong = new HoatDongDao().ViewDetail(id);
            return PartialView(hoatDong);
        }

        [HttpPost]
        public ActionResult Update(HoatDong model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new HoatDongDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(model.TenHoatDong))
                        {
                            entity.TenHoatDong = model.TenHoatDong;
                        }
                        if (!string.IsNullOrEmpty(model.ViTriThamGia))
                        {
                            entity.ViTriThamGia = model.ViTriThamGia;
                        }
                        if (!string.IsNullOrEmpty(model.ThangThamGia.ToString()))
                        {
                            entity.ThangThamGia = model.ThangThamGia;
                        }
                        if (!string.IsNullOrEmpty(model.NamThamGia.ToString()))
                        {
                            entity.NamThamGia = model.NamThamGia;
                        }
                        if (!string.IsNullOrEmpty(model.ThangKetThuc.ToString()))
                        {
                            entity.ThangKetThuc = model.ThangKetThuc;
                        }
                        if (!string.IsNullOrEmpty(model.NamKetThuc.ToString()))
                        {
                            entity.NamKetThuc = model.NamKetThuc;
                        }
                        if (!string.IsNullOrEmpty(model.MoTaChiTiet))
                        {
                            entity.MoTaChiTiet = model.MoTaChiTiet;
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
            var hoatDong = new HoatDongDao().ViewDetail(id);
            return PartialView(hoatDong);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(HoatDong model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new HoatDongDao();
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