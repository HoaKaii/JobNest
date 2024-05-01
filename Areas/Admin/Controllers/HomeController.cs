using JobsFinder_Main.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JobsFinder_Main.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
            var context = new AppDbContext();
            _userManager = new UserManager<AppUser>(new UserStore<AppUser>(context));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            jobCategoryDao = new JobCategoryDao();
            jobDao = new JobDao();
            careerDao = new CareerDao();
            companyDao = new CompanyDao();
            blogDao = new BlogDao();
            blogCategoryDao = new BlogCategoryDao();
        }

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JobCategoryDao jobCategoryDao;
        private readonly JobDao jobDao;
        private readonly CareerDao careerDao;
        private readonly CompanyDao companyDao;
        private readonly BlogDao blogDao;
        private readonly BlogCategoryDao blogCategoryDao;
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

            var jobCategories = jobCategoryDao.GetAllCategories();
            List<string> categoryNames = new List<string>();
            List<int> jobCounts = new List<int>();

            foreach (var category in jobCategories)
            {
                categoryNames.Add(category.Name);
                int count = jobDao.CountJobsByCategoryId(category.ID);
                jobCounts.Add(count);
            }

            ViewBag.Categories = categoryNames;
            ViewBag.JobCounts = jobCounts;

            var careers = careerDao.GetAllCareers();
            List<string> careerNames = new List<string>();
            List<int> jobCounts_1 = new List<int>();

            foreach (var career in careers)
            {
                careerNames.Add(career.Name);
                int count = jobDao.CountJobsByCareerId(career.ID);
                jobCounts_1.Add(count);
            }

            ViewBag.Careers = careerNames;
            ViewBag.JobCounts_1 = jobCounts_1;

            ViewBag.MaleJobCount = jobDao.CountJobsByGender("Male");
            ViewBag.FemaleJobCount = jobDao.CountJobsByGender("Female");
            ViewBag.AllJobCount = jobDao.CountJobsByGender("All");

            ViewBag.CountJobToday = jobDao.CountJobsCreatedToday();
            ViewBag.CountCompanyToday = companyDao.CountCompaniesCreatedToday();
            ViewBag.CountBlogToday = blogDao.CountBlogsCreatedToday();
            ViewBag.CountUserToday = CountUsersCreatedToday();

            var blogCategories = blogCategoryDao.GetAllBlogCategories();
            List<string> blogcategoryNames = new List<string>();
            List<int> blogCounts = new List<int>();

            foreach (var blogCategory in blogCategories)
            {
                blogcategoryNames.Add(blogCategory.Name);
                int count = blogDao.CountBlogsByCategoryId(blogCategory.ID);
                blogCounts.Add(count);
            }

            ViewBag.BlogCategories = blogcategoryNames;
            ViewBag.BlogCounts = blogCounts;

            ViewBag.UserCountByRole = UserCountByRole();

            return View();
        }
        public int CountCompany()
        {
            int companyCount = companyDao.CountCompanies();
            return companyCount;
        }
        public int CountJob()
        {
            int jobCount = jobDao.CountJob();
            return jobCount;
        }
        public int UserCount()
        {
            var userCount = _userManager.Users.Count();
            return userCount;
        }
        public int CountBlogCategory()
        {
            int blogcategoryCount = blogCategoryDao.CountBlogCategories();
            return blogcategoryCount;
        }
        public int CountBlog()
        {
            int blogCount = blogDao.CountBlogs();
            return blogCount;
        }
        public int CountJobCategory()
        {
            int jobcategoryCount = jobCategoryDao.CountJobCategories();
            return jobcategoryCount;
        }
        public int CountCareer()
        {
            int careerCount = careerDao.CountCareers();
            return careerCount;
        }
        public int CountUsersCreatedToday()
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today;
            DateTime endDate = today.AddDays(1).AddSeconds(-1);
            return _userManager.Users.Count(u => u.CreatedDate >= startDate && u.CreatedDate >= endDate);
        }
        public Dictionary<string, int> UserCountByRole()
        {
            var userCountByRole = new Dictionary<string, int>();

            foreach (var role in _roleManager.Roles)
            {
                var usersInRole = _userManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));
                int count = usersInRole.Count();
                userCountByRole.Add(role.Name, count);
            }

            return userCountByRole;
        }

        public List<string> GetAllRoles()
        {
            return _roleManager.Roles.Select(r => r.Name).ToList();
        }
    }
}