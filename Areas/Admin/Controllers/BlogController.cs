using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace JobsFinder_Main.Areas.Admin.Controllers
{
    public class BlogController : BaseController
    {
        // GET: Admin/Blog
        public ActionResult Index(string searchString, string fillterCategory, int page = 1, int pageSize = 4)
        {
            var dao = new BlogDao();
            var model = dao.ListAllPaging(searchString, fillterCategory, page, pageSize);
            ViewBag.SearchString = searchString;
            SetViewBag();
            return View(model);
        }

        // GET: Admin/Blog/Create
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        // POST: Admin/Blog/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Blog model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new BlogDao();

                    long id = dao.InsertOrUpdate(model);
                    if (id > 0)
                    {
                        SetAlert("Add new record successfull", "success");
                        return RedirectToAction("Index", "Blog");
                    }
                    else
                    {
                        SetAlert("Add new record unsuccessfull", "warning");
                        return RedirectToAction("Index", "Blog");
                    }
                }
                SetViewBag();
                return View("Index");
            }
            catch (Exception)
            {
                SetAlert("An error occurred, please try again later!", "error");
                return RedirectToAction("Index", "Blog");
            }

        }

        // GET: Admin/Blog/Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new BlogDao();
            var blog = dao.GetById(id);
            var blogDetail = dao.ViewDetail(id);
            SetViewBag(blog.CategoryID);
            return View(blogDetail);
        }
        // POST: Admin/Blog/Edit
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Blog model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new BlogDao();
                    var result = dao.InsertOrUpdate(model);
                    if (result != 0)
                    {
                        SetAlert("Update blog successfull", "success");
                        return RedirectToAction("Index", "Blog");
                    }
                    else
                    {
                        SetAlert("Update blog unsuccessfull", "warning");
                        ModelState.AddModelError("", "Update blog unsuccessfull");
                    }
                }
                SetViewBag(model.CategoryID);
                return View("Index");
            }
            catch (Exception)
            {
                SetAlert("An error occurred, please try again later!", "error");
                return RedirectToAction("Index", "Blog");
            }
        }


        // DELETE: Admin/Blog/Delete/{id}
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new BlogDao().Delete(id);
            return RedirectToAction("Index");
        }

        // POST: Admin/Blog/ChangeStatus/{id}
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new BlogDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new BlogCategoryDao();
            ViewBag.BlogCategoryId = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}