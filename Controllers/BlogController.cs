using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace JobsFinder_Main.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        private readonly BlogDao _blogDao;

        public BlogController()
        {
            _blogDao = new BlogDao();
        }

        // GET: Blog
        public ActionResult Index(string searchName, string fillterCategory, int page = 1, int pageSize = 4)
        {
            var dao = new BlogDao();
            var model = dao.ListAllPaging(searchName, fillterCategory, page, pageSize);
            ViewBag.SearchName = searchName;
            ViewBag.FillterCategory = fillterCategory;
            return View(model);
        }

        public ActionResult Detail(int id)
        {
            var blog = _blogDao.GetById(id);

            if (blog == null)
            {
                return HttpNotFound();
            }
            _blogDao.UpdateViewCount(id);

            return View(blog);
        }

        public ActionResult BlogHome()
        {
            BlogDao blogDao = new BlogDao();

            var forYouBlog = blogDao.GetForYouBlog();

            var popularBlog = blogDao.GetPopularBlog();

            var trendBlogs = blogDao.GetTrendBlogs();

            ViewBag.ForYouBlog = forYouBlog;
            ViewBag.PopularBlog = popularBlog;
            ViewBag.TrendBlogs = trendBlogs;

            return PartialView("_BlogHome");
        }
    }
}