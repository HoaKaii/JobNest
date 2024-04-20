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
    public class KyNangController : Controller
    {
        // GET: KyNang
        public ActionResult Index()
        {
            return View();
        }

        // GET: KyNang/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: KyNang/Create
        [HttpPost]
        public ActionResult Create(KyNangModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new KyNangDao();

                var kyNang = new KyNang
                {
                    TenKyNang = model.TenKyNang,
                    DanhGia = model.DanhGia,
                    Caption = model.Caption,
                    MoTaChiTiet = model.MoTaChiTiet,
                    UserID = User.Identity.GetUserId(),
                    CreatedBy = User.Identity.GetUserName()
                };

                var result = dao.Insert(kyNang);
                if (result == true)
                {
                    model = new KyNangModel();
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
            var kyNang = new KyNangDao().ViewDetail(id);
            return PartialView(kyNang);
        }

        [HttpPost]
        public ActionResult Update(KyNang model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new KyNangDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(model.TenKyNang))
                        {
                            entity.TenKyNang = model.TenKyNang;
                        }
                        if (!string.IsNullOrEmpty(model.DanhGia.ToString()))
                        {
                            entity.DanhGia = model.DanhGia;
                        }
                        if (!string.IsNullOrEmpty(model.Caption.ToString()))
                        {
                            entity.Caption = model.Caption;
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
            var kyNang = new KyNangDao().ViewDetail(id);
            return PartialView(kyNang);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(KyNang model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new KyNangDao();
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