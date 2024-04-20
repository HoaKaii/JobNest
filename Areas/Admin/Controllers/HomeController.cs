using JobsFinder_Main.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Model.DAO;
using Model.EF;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JobsFinder_Main.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
            _userManager = new UserManager<AppUser>(new UserStore<AppUser>(new AppDbContext()));
        }
        private readonly UserManager<AppUser> _userManager;
        // GET: Admin/Home
        public ActionResult Index()
        {
            int companyCount = CountCompany();
            ViewBag.CompanyCount = companyCount;

            int jobCount = CountJob();
            ViewBag.JobCount = jobCount;

            int userCount = UserCount();
            ViewBag.UserCount = userCount;

            int jobcategoryCount = CountJobCategory();
            ViewBag.JobCategoryCount = jobcategoryCount;

            int careerCount = CountCareer();
            ViewBag.CareerCount = careerCount;

            int blogCount = CountBlog();
            ViewBag.BlogCount = blogCount;

            int blogcategoryCount = CountBlogCategory();
            ViewBag.BlogCategoryCount = blogcategoryCount;

            int menuCount = CountMenu();
            ViewBag.MenuCount = menuCount;

            return View();
        }
        public int CountCompany()
        {
            var dao = new CompanyDao();
            int companyCount = dao.CountCompanies();
            return companyCount;
        }
        public int CountJob()
        {
            var dao = new JobDao();
            int jobCount = dao.CountJob();
            return jobCount;
        }
        public int UserCount()
        {
            var userCount = _userManager.Users.Count();
            return userCount;
        }
        public int CountBlogCategory()
        {
            var dao = new BlogCategoryDao();
            int blogcategoryCount = dao.CountBlogCategories();
            return blogcategoryCount;
        }
        public int CountBlog()
        {
            var dao = new BlogDao();
            int blogCount = dao.CountBlogs();
            return blogCount;
        }
        public int CountJobCategory()
        {
            var dao = new BlogCategoryDao();
            int blogcategoryCount = dao.CountBlogCategories();
            return blogcategoryCount;
        }
        public int CountCareer()
        {
            var dao = new CareerDao();
            int careerCount = dao.CountCareers();
            return careerCount;
        }
        public int CountMenu()
        {
            var dao = new MenuDao();
            int menuCount = dao.CountMenus();
            return menuCount;
        }
    }
}