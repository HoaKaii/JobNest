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
    public class DuAnController : Controller
    {
        // GET: DuAn
        public ActionResult Index()
        {
            return View();
        }

        // GET: DuAn/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: DuAn/Create
        [HttpPost]
        public ActionResult Create(DuAnModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new DuAnDao();

                var duAn = new DuAn
                {
                    TenDuAn = model.TenDuAn,
                    TenKhachHang = model.TenKhachHang,
                    SoThanhVien = model.SoThanhVien,
                    ViTri = model.ViTri,
                    NhiemVu = model.NhiemVu,
                    CongNgheSuDung = model.CongNgheSuDung,
                    ThangBatDau = model.ThangBatDau,
                    NamBatDau = model.NamBatDau,
                    ThangKetThuc = model.ThangKetThuc,
                    NamKetThuc = model.NamKetThuc,
                    MoTaChiTiet = model.MoTaChiTiet,
                    Img = model.Img,
                    LienKet = model.LienKet,
                    UserID = User.Identity.GetUserId(),
                    CreatedBy = User.Identity.GetUserName(),
                };

                var result = dao.Insert(duAn);
                if (result == true)
                {
                    model = new DuAnModel();
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
            var duAn = new DuAnDao().ViewDetail(id);
            return PartialView(duAn);
        }

        [HttpPost]
        public ActionResult Update(DuAn model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new DuAnDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(model.TenDuAn))
                        {
                            entity.TenDuAn = model.TenDuAn;
                        }
                        if (!string.IsNullOrEmpty(model.TenKhachHang))
                        {
                            entity.TenKhachHang = model.TenKhachHang;
                        }
                        if (!string.IsNullOrEmpty(model.ViTri))
                        {
                            entity.ViTri = model.ViTri;
                        }
                        if (!string.IsNullOrEmpty(model.NhiemVu))
                        {
                            entity.NhiemVu = model.NhiemVu;
                        }
                        if (!string.IsNullOrEmpty(model.CongNgheSuDung))
                        {
                            entity.CongNgheSuDung = model.CongNgheSuDung;
                        }
                        if (!string.IsNullOrEmpty(model.SoThanhVien.ToString()))
                        {
                            entity.SoThanhVien = model.SoThanhVien;
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
            var duAn = new DuAnDao().ViewDetail(id);
            return PartialView(duAn);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(DuAn model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new DuAnDao();
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