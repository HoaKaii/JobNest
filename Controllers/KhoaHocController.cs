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
    public class KhoaHocController : Controller
    {
        // GET: KhoaHoc
        public ActionResult Index()
        {
            return View();
        }

        // GET: KhoaHoc/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: KhoaHoc/Create
        [HttpPost]
        public ActionResult Create(KhoaHocModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new KhoaHocDao();

                var khoaHoc = new KhoaHoc
                {
                    TenKhoaHoc = model.TenKhoaHoc,
                    ToChuc = model.ToChuc,
                    ThangBatDau = model.ThangBatDau,
                    NamBatDau = model.NamBatDau,
                    ThangKetThuc = model.ThangKetThuc,
                    NamKetThuc = model.NamKetThuc,
                    MoTaChiTiet = model.MoTaChiTiet,
                    Img = model.Img,
                    LienKet = model.LienKet,
                    UserID = User.Identity.GetUserId(),
                    CreatedBy = User.Identity.GetUserName()
                };

                var result = dao.Insert(khoaHoc);
                if (result == true)
                {
                    model = new KhoaHocModel();
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
            var khoaHoc = new KhoaHocDao().ViewDetail(id);
            return PartialView(khoaHoc);
        }

        [HttpPost]
        public ActionResult Update(KhoaHoc model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new KhoaHocDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(model.TenKhoaHoc))
                        {
                            entity.TenKhoaHoc = model.TenKhoaHoc;
                        }
                        if (!string.IsNullOrEmpty(model.ToChuc))
                        {
                            entity.ToChuc = model.ToChuc;
                        }
                        if (!string.IsNullOrEmpty(model.ThangBatDau.ToString()))
                        {
                            entity.ThangBatDau = model.ThangBatDau;
                        }
                        if (!string.IsNullOrEmpty(model.NamBatDau.ToString()))
                        {
                            entity.NamBatDau = model.NamBatDau;
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
            var khoaHoc = new KhoaHocDao().ViewDetail(id);
            return PartialView(khoaHoc);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(KhoaHoc model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new KhoaHocDao();
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