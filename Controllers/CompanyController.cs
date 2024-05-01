using JobsFinder_Main.Filters;
using JobsFinder_Main.Services;
using Model.DAO;
using Model.EF;
using Model.Services;
using ServiceStack;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace JobsFinder_Main.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyDao companyDao;
        private readonly ReviewDao reviewDao;
        public CompanyController()
        {
            companyDao = new CompanyDao();
            reviewDao = new ReviewDao();
        }
        // GET: Company
        [HttpGet]
        public ActionResult Index(string searchName, string searchLocation, int page = 1, int pageSize = 9)
        {
            SetViewBag();
            var model = companyDao.ListAllPaging(searchName, searchLocation, page, pageSize);
            ViewBag.SearchName = searchName;
            ViewBag.SearchLocation = searchLocation;
            return View(model);
        }

        public async Task<ActionResult> Detail(int id) 
        {
            var company = companyDao.ViewDetail(id);
            var reviews = reviewDao.GetReviews(id);

            company.Reviews = reviews;
            ViewBag.TotalReviews = reviews.Count;
            if (reviews.Count != 0) 
            {
                ViewBag.AverageRating = reviews.Average(r => r.Rating);
            }
            else
            {
                ViewBag.AverageRating = 0;
            }    
            return View(company);
        }

        [ChildActionOnly]
        public ActionResult Carousel_Company()
        {
            SetViewBag();
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult Testimonial()
        {
            var model = new CompanyDao().ListAll();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult JobInCompany(Company company)
        {
            var companyID = company.ID;
            var model = new JobDao().GetJobInCompany(companyID);
            return PartialView(model);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new CompanyDao().Delete(id);
            return RedirectToAction("Index");
        }

        public void SetViewBag(long? selectedId = null)
        {
            var commonDao = new CommonDao.CityDao();
            var cities = commonDao.GetAllCities().Select(c => new SelectListItem
            {
                Value = c.Value,
                Text = c.Name
            });
            ViewBag.CityList = new SelectList(cities, "Value", "Text", selectedId);
        }

        [HttpPost]
        [JobSeekerAuthorization]
        public ActionResult AddReview(int companyId, string comment, int rating, string userId, string Name)
        {
            var existingReview = reviewDao.GetReview(userId, companyId);
            if (existingReview != null)
            {
                existingReview.Comment = comment;
                existingReview.Rating = rating;
                reviewDao.UpdateReview(existingReview);
            }
            else 
            {
                var review = new Review
                {
                    CompanyID = companyId,
                    Comment = comment,
                    Rating = rating,
                    UserID = userId,
                    Name = Name
                };
                reviewDao.AddReview(review);
            }
            return RedirectToAction("Detail", new { id = companyId });
        }
    }
}