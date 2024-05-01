﻿using JobsFinder_Main.Identity;
using Model.DAO;
using Model.EF;
using System;
using System.Linq;
using System.Web.Mvc;

namespace JobsFinder_Main.Areas.Admin.Controllers
{
    public class JobController : BaseController
    {
        // GET: Admin/Job
        [HttpGet]
        public ActionResult Index(string searchName, string searchLocation, string fillterCareer, string fillterCategory, string fillterGender, string fillterEXP, int page = 1, int pageSize = 10)
        {
            var dao = new JobDao();
            var model = dao.ListAllPaging(searchName, searchLocation, fillterCareer, fillterCategory, fillterGender, fillterEXP, page, pageSize);
            ViewBag.SearchName = searchName;

            return View(model);
        }

        // GET: Admin/Job/Create
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        // GET: Admin/Job/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var job = new JobDao().ViewDetail(id);
            long? userId = null;
            if (!string.IsNullOrEmpty(job.UserID) && long.TryParse(job.UserID, out long parsedUserId))
            {
                userId = parsedUserId;
            }
            SetViewBag(job.CategoryID);
            SetViewBag(job.CarrerID);
            SetViewBag(job.CompanyID);
            SetViewBag(userId);
            return View(job);
        }

        // GET: Admin/Job/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Job job)
        {
            try
            {
                var dao = new JobDao();
                long id = dao.Insert(job);
                if (id > 0)
                {
                    SetAlert("Thêm bản ghi mới thành công", "success");
                    return RedirectToAction("Index", "Job");
                }
                else
                {
                    SetAlert("Thêm bản ghi mới KHÔNG thành công", "warning");
                    return View(job);
                }
            }
            catch (Exception ex)
            {
                SetAlert("Lỗi khi thêm bản ghi mới: " + ex.Message, "error");
                return RedirectToAction("Index", "Job");
            }
        }

        // GET: Admin/Job/Edit/{id}
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Job job)
        {
            try
            {
                var dao = new JobDao();

                var result = dao.Update(job);
                if (result)
                {
                    SetAlert("Cập nhật danh mục công việc thành công", "success");
                    return RedirectToAction("Index", "Job");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật danh mục công việc không thành công");
                }
                SetViewBag(job.CategoryID);
                SetViewBag(job.CarrerID);
                SetViewBag(job.CompanyID);
                return View("Index");
            }
            catch (Exception ex)
            {
                SetAlert("Cập nhật danh mục công việc thất bại: " + ex.Message, "error");
                return RedirectToAction("Index", "Job");
            }
        }

        // DELETE: Admin/Job/Delete/{id}
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new JobDao().Delete(id);
            return RedirectToAction("Index");
        }

        // POST: Admin/Job/ChangeStatus/{id}
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new JobDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public void SetViewBag(long? selectedId = null)
        {
            var JobCategorydao = new JobCategoryDao();
            ViewBag.JobCategoryId = new SelectList(JobCategorydao.ListAll(), "ID", "Name", selectedId);

            var careerDao = new CareerDao();
            ViewBag.JobCareerId = new SelectList(careerDao.ListAll(), "ID", "Name", selectedId);

            var companyDao = new CompanyDao();
            ViewBag.CompanyId = new SelectList(companyDao.ListAll(), "ID", "Name", selectedId);

            using (var dbContext = new AppDbContext())
            {
                var users = dbContext.Users.Where(u => u.Status == true && u.Name != null).Select(u => new { ID = u.Id, Name = u.Name }).ToList();
                ViewBag.UserId = new SelectList(users, "ID", "Name", selectedId);
            }
        }
    }
}