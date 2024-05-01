using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Model.EF;
using Model.Services;

namespace Model.DAO
{
    public class ReviewDao
    {
        private readonly IUserService _userService;
        private readonly JobsFinderDBContext db = null;
        public ReviewDao(IUserService userService)
        {
            db = new JobsFinderDBContext();
            _userService = userService;
        }
        public ReviewDao()
        {
            db = new JobsFinderDBContext();
        }
        public void AddReview(Review review)
        {
            if (review.CreatedAt == null)
            {
                review.CreatedAt = DateTime.Now;
            }
            db.Reviews.Add(review);
            db.SaveChanges();
        }
        public List<Review> GetReviews(int? companyId)
        {
            var reviews = db.Reviews
                            .Where(r => r.CompanyID == companyId)
                            .ToList();

            return reviews;
        }
        public Review GetReview(string userId, int companyId)
        {
            return db.Reviews.FirstOrDefault(r => r.UserID == userId && r.CompanyID == companyId);
        }
        public void UpdateReview(Review review)
        {
            review.CreatedAt = DateTime.Now;
            db.Entry(review).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
