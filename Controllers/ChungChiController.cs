using JobsFinder_Main.Models;
using Microsoft.AspNet.Identity;
using Model.DAO;
using Model.EF;
using System;
using System.Web.Mvc;

namespace JobsFinder_Main.Controllers
{
    public class ChungChiController : Controller
    {
        // GET: ChungChi
        public ActionResult Index()
        {
            return View();
        }

        // GET: ChungChi/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: ChungChi/Create
        [HttpPost]
        public ActionResult Create(ChungChiModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new ChungChiDao();

                var chungChi = new ChungChi
                {
                    TenChungChi = model.TenChungChi,
                    ToChuc = model.ToChuc,
                    ThangXacThuc = model.ThangXacThuc,
                    NamXacThuc = model.NamXacThuc,
                    ThangHetHan = model.ThangHetHan,
                    NamHetHan = model.NamHetHan,
                    Img = model.Img,
                    LienKet = model.LienKet,
                    UserID = User.Identity.GetUserId(),
                    CreatedBy = User.Identity.GetUserName(),
                };

                var result = dao.Insert(chungChi);
                if (result == true)
                {
                    model = new ChungChiModel();
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
            var chungChi = new ChungChiDao().ViewDetail(id);
            return PartialView(chungChi);
        }

        [HttpPost]
        public ActionResult Update(ChungChi model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new ChungChiDao();
                var entity = dao.ViewDetail(model.ID);
                if (entity != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(model.TenChungChi))
                        {
                            entity.TenChungChi = model.TenChungChi;
                        }
                        if (!string.IsNullOrEmpty(model.ToChuc))
                        {
                            entity.ToChuc = model.ToChuc;
                        }
                        if (!string.IsNullOrEmpty(model.ThangXacThuc.ToString()))
                        {
                            entity.ThangXacThuc = model.ThangXacThuc;
                        }
                        if (!string.IsNullOrEmpty(model.NamXacThuc.ToString()))
                        {
                            entity.NamXacThuc = model.NamXacThuc;
                        }
                        if (!string.IsNullOrEmpty(model.ThangHetHan.ToString()))
                        {
                            entity.ThangHetHan = model.ThangHetHan;
                        }
                        if (!string.IsNullOrEmpty(model.NamHetHan.ToString()))
                        {
                            entity.NamHetHan = model.NamHetHan;
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
            var chungChi = new ChungChiDao().ViewDetail(id);
            return PartialView(chungChi);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(ChungChi model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dao = new ChungChiDao();
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