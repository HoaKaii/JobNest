﻿using JobsFinder_Main.Common;
using JobsFinder_Main.Models;
using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsFinder_Main.Controllers
{
    public class KinhNghiemController : Controller
    {
        // GET: KinhNghiem
        public ActionResult Index()
        {
            return View();
        }

        // GET: KinhNghiem/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: KinhNghiem/Create
        [HttpPost]
        public ActionResult Create(KinhNghiemModel model)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session != null)
            {
                var kinhNghiem = new KinhNghiem();
                var dao = new KinhNghiemDao();

                kinhNghiem.CongTy = model.CongTy;
                kinhNghiem.ChucVu = model.ChucVu;
                kinhNghiem.ThangBatDau = model.ThangBatDau;
                kinhNghiem.NamBatDau = model.NamBatDau;
                kinhNghiem.ThangKetThuc = model.ThangKetThuc;
                kinhNghiem.NamKetThuc = model.NamKetThuc;
                kinhNghiem.MoTaChiTiet = model.MoTaChiTiet;
                kinhNghiem.HinhAnh = model.HinhAnh;
                kinhNghiem.LienKet = model.LienKet;
                kinhNghiem.UserID = session.UserID;
                kinhNghiem.CreatedBy = session.UserName;

                var result = dao.Insert(kinhNghiem);
                if (result == true)
                {
                    model = new KinhNghiemModel();
                    TempData["Message"] = "Create sucessfull!";
                    TempData["MessageType"] = "success";
                    TempData["Type"] = "Success";
                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    TempData["Message"] = "Create unsucessfull!!";
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

        //GET: KinhNghiem/Update/{id}
        [HttpGet]
        public ActionResult Update(long id)
        {
            var kinhNghiem = new KinhNghiemDao().ViewDetail(id);
            return PartialView(kinhNghiem);
        }

        //POST: KinhNghiem/Update/{id}
        [HttpPost]
        public ActionResult Update(KinhNghiem model)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session != null)
            {
                var dao = new KinhNghiemDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(model.CongTy))
                        {
                            entity.CongTy = model.CongTy;
                        }
                        if (!string.IsNullOrEmpty(model.ChucVu))
                        {
                            entity.ChucVu = model.ChucVu;
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
                        entity.ModifiedBy = session.UserName;
                        entity.ModifiedDate = DateTime.Now;

                        var result = dao.Update(entity);
                        if (result)
                        {
                            TempData["Message"] = "Cập nhật thành công!";
                            TempData["MessageType"] = "success";
                            TempData["Type"] = "Thành công";
                            return RedirectToAction("Index", "Profile");
                        }
                        else
                        {
                            TempData["Message"] = "Cập nhật không thành công!";
                            TempData["MessageType"] = "error";
                            TempData["Type"] = "Thất bại";
                            return RedirectToAction("Index", "Profile");
                        }
                    }
                    else
                    {
                        TempData["Message"] = "Có lỗi xảy ra! Vui lòng thử lại!";
                        TempData["MessageType"] = "warning";
                        TempData["Type"] = "Thất bại";
                        return RedirectToAction("Index", "Profile");
                    }
                }
                else
                {
                    TempData["Message"] = "Có lỗi xảy ra! Vui lòng thử lại!";
                    TempData["MessageType"] = "warning";
                    TempData["Type"] = "Thất bại";
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
            var kinhNghiem = new KinhNghiemDao().ViewDetail(id);
            return PartialView(kinhNghiem);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(KinhNghiem model)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session != null)
            {
                var dao = new KinhNghiemDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    var result = dao.Delete(entity);
                    if (result)
                    {
                        TempData["Message"] = "Cập nhật thành công!";
                        TempData["MessageType"] = "success";
                        TempData["Type"] = "Thành công";
                        return RedirectToAction("Index", "Profile");
                    }
                    else
                    {
                        TempData["Message"] = "Cập nhật không thành công!";
                        TempData["MessageType"] = "error";
                        TempData["Type"] = "Thất bại";
                        return RedirectToAction("Index", "Profile");
                    }
                }
                else
                {
                    TempData["Message"] = "Có lỗi xảy ra! Vui lòng thử lại!";
                    TempData["MessageType"] = "warning";
                    TempData["Type"] = "Thất bại";
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