using JobsFinder_Main.Common;
using JobsFinder_Main.Identity;
using JobsFinder_Main.Models;
using Microsoft.AspNet.Identity;
using Model.DAO;
using Model.EF;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsFinder_Main.Controllers
{
    public class HocVanController : Controller
    {
        // GET: HocVan
        public ActionResult Index()
        {
            return View();
        }

        // GET: HocVan/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: HocVan/Create
        [HttpPost]
        public ActionResult Create(HocVanModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var hocvan = new HocVan();
                var dao = new HocVanDao();

                hocvan.Truong = model.Truong;
                hocvan.ChuyenNganh = model.ChuyenNganh;
                hocvan.ThangBatDau = model.ThangBatDau;
                hocvan.NamBatDau = model.NamBatDau;
                hocvan.ThangKetThuc = model.ThangKetThuc;
                hocvan.NamKetThuc = model.NamKetThuc;
                hocvan.MoTaChiTiet = model.MoTaChiTiet;
                hocvan.UserID = User.Identity.GetUserId();
                hocvan.CreatedBy = User.Identity.GetUserName();

                var result = dao.Insert(hocvan);
                if (result == true)
                {
                    TempData["Message"] = "Create successful!";
                    TempData["MessageType"] = "success";
                    TempData["Type"] = "Success";
                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    TempData["Message"] = "Create unsuccessful!";
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

        //GET: HocVan/Update/{id}
        [HttpGet]
        public ActionResult Update(long id)
        {
            var hocVan = new HocVanDao().ViewDetail(id);
            return PartialView(hocVan);
        }

        //POST: HocVan/Update/{id}
        [HttpPost]
        public ActionResult Update(HocVan model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new HocVanDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(model.Truong))
                        {
                            entity.Truong = model.Truong;
                        }
                        if (!string.IsNullOrEmpty(model.ChuyenNganh))
                        {
                            entity.ChuyenNganh = model.ChuyenNganh;
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
                            TempData["Message"] = "Update successful!";
                            TempData["MessageType"] = "success";
                            TempData["Type"] = "Success";
                            return RedirectToAction("Index", "Profile");
                        }
                        else
                        {
                            TempData["Message"] = "Update unsuccessful!";
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

        [HttpGet]
        public ActionResult DeleteConfirm(long id)
        {
            var hocVan = new HocVanDao().ViewDetail(id);
            return PartialView(hocVan);
        }

        // DELETE: HocVan/Delete/{id}
        [HttpPost]
        public ActionResult DeleteConfirm(HocVan model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new HocVanDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    var result = dao.Delete(entity);
                    if (result)
                    {
                        TempData["Message"] = "Delete sucessful!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Success";
                        return RedirectToAction("Index", "Profile");
                    }
                    else
                    {
                        TempData["Message"] = "Delete unsucessful!";
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