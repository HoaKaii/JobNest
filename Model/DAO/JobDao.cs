using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;
using ServiceStack;
using ServiceStack.Script;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Data.Entity.Migrations;

namespace Model.DAO
{
    public class JobDao
    {
        private readonly JobsFinderDBContext db = null;
        public JobDao()
        {
            db = new JobsFinderDBContext();
        }
        public long Insert(Job entity)
        {
            if (entity.Status == null)
            {
                entity.Status = true;
            }
            if (entity.CreatedDate == null)
            {
                entity.CreatedDate = DateTime.Now;
            }
            if (entity.Gender == null)
            {
                entity.Gender = "All";
            }
            if(entity.MetaTitle == null)
            {
                string name = entity.Name;
                string slug = Regex.Replace(name, @"[^a-zA-Z0-9]", "").ToLower();
                slug = slug.Replace(" ", "-");
                entity.MetaTitle = slug;
            }
            if(entity.Code == null)
            {
                Random random = new Random();

                string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string randomLetters = new string(Enumerable.Repeat(letters, 3).Select(s => s[random.Next(s.Length)]).ToArray());
                string numbers = "0123456789";
                string randomNumbers = new string(Enumerable.Repeat(numbers, 7).Select(s => s[random.Next(s.Length)]).ToArray());
                string code = randomLetters + randomNumbers;

                entity.Code = code;
            }
            if(entity.Experience == null)
            {
                entity.Experience = "0";
            }
            if (entity.CreatedBy == null)
            {
                entity.CreatedBy = "admin";
            }
            if (entity.Deadline == null)
            {
                entity.Deadline = DateTime.Now.AddDays(30);
            }    
            db.Jobs.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool ApplyJob(Job entity)
        {
            db.Jobs.AddOrUpdate(entity);
            db.SaveChanges();
            return true;
        }

        public List<Job> ListInUser(string UserName)
        {
            return db.Jobs.Where(x => x.CreatedBy == UserName).ToList();
        }

        public List<Job> LisInCompany(int? companyID)
        {
            return db.Jobs.Where(x  => x.CompanyID == companyID).ToList();
        }

        public bool Update(Job entity)
        {
            try
            {
                var job = db.Jobs.Find(entity.ID);
                job.Name = entity.Name;
                job.Description = entity.Description;
                job.RequestCandidate = entity.RequestCandidate;
                job.Interest = entity.Interest;
                job.Salary = entity.Salary;
                job.SalaryMin = entity.SalaryMin;
                job.SalaryMax = entity.SalaryMax;
                job.Quantity = entity.Quantity;
                job.CategoryID =  entity.CategoryID;
                job.CarrerID =  entity.CarrerID;
                job.Details = entity.Details;
                job.Deadline = entity.Deadline;
                job.Rank = entity.Rank;
                job.Gender = entity.Gender;
                job.Experience = entity.Experience;
                job.WorkLocation = entity.WorkLocation;
                job.ModifiedDate = DateTime.Now;

                db.SaveChanges();
                return true;
            } catch (Exception)
            {
                return false;
            }
            
        }
        public IPagedList<Job> ListAllPaging(string searchName, string searchLocation, string fillterCareer, string fillterCategory, string fillterGender, string fillterEXP, int page, int pageSize)
        {
            IQueryable<Job> model = db.Jobs;

            if (!string.IsNullOrEmpty(searchName))
            {
                model = model.Where(x => x.Name.Contains(searchName));
            }
            if (!string.IsNullOrEmpty(searchLocation))
            {
                model = model.Where(x => x.WorkLocation.Contains(searchLocation));
            }
            if (!string.IsNullOrEmpty(fillterCareer))
            {
                if (int.TryParse(fillterCareer, out int careerID))
                {
                    model = model.Where(x => x.CarrerID == careerID);
                }
            }

            if (!string.IsNullOrEmpty(fillterCategory))
            {
                if (int.TryParse(fillterCategory, out int categoryID))
                {
                    model = model.Where(x => x.CategoryID == categoryID);
                }
            }

            if (!string.IsNullOrEmpty(fillterGender))
            {
                model = model.Where(x => x.Gender == fillterGender);
            }
            if (!string.IsNullOrEmpty(fillterEXP))
            {
                model = model.Where(x  => x.Experience == fillterEXP);
            }
            return model.Where(x => x.Status == true)
            .OrderByDescending(x => x.CreatedDate)
            .ToPagedList(page, pageSize);
        }
        public List<Job> ListAll()
        {
            return db.Jobs.ToList();
        }
        public List<Job> ListNew()
        {
            return db.Jobs.OrderByDescending(x => x.CreatedDate).Take(6).ToList();
        }

        public List<Job> ListFeatured()
        {
            return db.Jobs.OrderByDescending(x => x.ViewCount).Take(6).ToList();
        }

        public List<Job> ListSuggest()
        {
            return db.Jobs.Where(x => x.Status == true ).ToList();
        }

        public Job GetByID(string jobName)
        {
            return db.Jobs.SingleOrDefault(x => x.Name == jobName);
        }
        public long? GetCategoryID(long? id)
        {
            var job =  db.Jobs.FirstOrDefault(x => x.CategoryID == id);
            if(job != null)
            {
                return job.CategoryID;
            }
            return null;
        }
        public int? GetCompanyID(int? id)
        {
            var job = db.Jobs.FirstOrDefault(x => x.CompanyID == id);
            if (job != null)
            {
                return job.CompanyID;
            }
            return null;
        }

        public List<Job> GetJobInCompany(int? id)
        {
            var companyId =  GetCompanyID(id);
            return db.Jobs.Where(x  => x.Status == true &&  x.CompanyID == companyId).ToList();
        }

        public string GetUserID(string id)
        {
            var job = db.Jobs.FirstOrDefault(x => x.UserID == id);
            if (job != null)
            {
                return job.UserID;
            }
            return null;
        }

        public Job ViewDetail(long id)
        {
            return db.Jobs.Find(id);
        }
        public int CountJob()
        {
            return db.Jobs.Count();
        }
        public int CountJobNew()
        {
            DateTime currentDate = DateTime.Now;
            DateTime yesterday = currentDate.AddDays(-1);
            return db.Jobs.Count(x => x.CreatedDate > yesterday && x.CreatedDate <= currentDate);
        }

        public bool ChangeStatus(long id)
        {
            var job = db.Jobs.Find(id);
            job.Status = !job.Status;

            db.SaveChanges();

            return (bool)job.Status;
        }
        public bool Delete(int id)
        {
            try
            {
                var job = db.Jobs.Find(id);
                db.Jobs.Remove(job);
                db.SaveChanges();
                return true;
            } catch (Exception)
            {
                return false;
            }
        }
        public string FormatSalary(int? id)
        {
            var job = db.Jobs.Find(id);
            if(job.Salary == true)
            {
                decimal? salaryMin = job.SalaryMin;
                decimal? salaryMax = job.SalaryMax;

                string formattedSalaryMin = (salaryMin).ToString() + " $";
                string formattedSalaryMax = (salaryMax).ToString() + " $";
                return formattedSalaryMin + " - " + formattedSalaryMax;
            } else
            {
                return "Negotiable";
            }
        }
        public string FormatTime(int? id)
        {
            var job = db.Jobs.Find(id);
            DateTime? createdDate = job.CreatedDate;
            TimeSpan timeSinceCreation;

            if (createdDate != null)
            {
                timeSinceCreation = DateTime.Now - createdDate.Value;
            }
            else
            {
                timeSinceCreation = TimeSpan.Zero;
            }

            string timeAgo;

            if (timeSinceCreation.TotalHours >= 24)
            {
                int daysAgo = (int)timeSinceCreation.TotalDays;
                timeAgo = $"{daysAgo} days ago";
            }
            else if (timeSinceCreation.TotalHours >= 1)
            {
                int hoursAgo = (int)timeSinceCreation.TotalHours;
                timeAgo = $"{hoursAgo} hours ago";
            }
            else
            {
                int minutesAgo = (int)timeSinceCreation.TotalMinutes;
                timeAgo = $"{minutesAgo} minute ago";
            }
            return timeAgo;
        }

        public string CountTime(int? id)
        {
            var job = db.Jobs.Find(id);
            DateTime? deadline = job.Deadline;
            TimeSpan remainingTime;

            if (deadline != null)
            {
                remainingTime = deadline.Value - DateTime.Now;
            }
            else
            {
                remainingTime = TimeSpan.Zero;
            }

            int remainingDays = (int)remainingTime.TotalDays;

            if (remainingDays <= 0)
            {
                job.Status = false;
                db.SaveChanges();
            }
            return $"You have {remainingDays} days left to apply";
        }

        public string SubTitle(int? comID, string uID)
        {
            var companyDao = new CompanyDao();
            int? companyID =  GetCompanyID(comID);
            string userID =  GetUserID(uID);
            string name;
            if (companyID != null)
            {
                name = companyDao.GetName(companyID);
            }
            else if (userID != null)
            {
                name = "JobNest";
            }
            else
            {
                name = "JobNest";
            }
            return name;
        }
        public string GetCategory(long? cateID)
        {
            var category = new JobCategoryDao();
            long? categoryID = GetCategoryID(cateID);
            string name;
            if (categoryID != null)
            {
                name = category.GetName(categoryID);
            } else
            {
                name = "No categories available.";
            }
            return name;
        }
        public string MetaTitle(int? comID)
        {
            var companyDao = new CompanyDao();
            int? companyID =  GetCompanyID(comID);
            string name;
            if (companyID != null)
            {
                name = companyDao.GetMetaTitle(companyID);
            }
            else
            {
                name = "JobNest";
            }
            return name;
        }
        public string GetAvatarFromCompany(int? comID, string uID)
        {
            var companyDao = new CompanyDao();
            int? companyID =  GetCompanyID(comID);
            string userID =  GetUserID(uID);
            string name;
            if (companyID != null)
            {
                name = companyDao.GetAvatar(companyID);
            }
            else if (userID != null)
            {
                name = "./Assets/Client/JobsFinder/img/Logo.png";
            }
            else
            {
                name = "./Assets/Client/JobsFinder/img/Logo.png";
            }
            return name;
        }
        public string GetCareer(long? careerID)
        {
            var career = new CareerDao();
            string name;
            if (careerID != null)
            {
                name = career.GetName(careerID);
            }
            else
            {
                name = "No careers available.";
            }
            return name;
        }
        public void UpdateViewCount(long jobId)
        {
            var job = db.Jobs.FirstOrDefault(j => j.ID == jobId);
            if (job != null)
            {
                job.ViewCount++;
                db.SaveChanges();
            }
        }
        public int CountJobsByCategoryId(long categoryId)
        {
            return db.Jobs.Count(j => j.CategoryID == categoryId);
        }
        public int CountJobsByCareerId(long careerId)
        {
            return db.Jobs.Count(j => j.CarrerID == careerId);
        }
        public int CountJobsByGender(string gender)
        {
            return db.Jobs.Count(j => j.Gender.ToLower() == gender.ToLower());
        }
        public int CountJobsCreatedToday()
        {
            DateTime today = DateTime.Today;

            DateTime startDate = today;

            DateTime endDate = today.AddDays(1).AddSeconds(-1);

            return db.Jobs.Count(j => j.CreatedDate >= startDate && j.CreatedDate <= endDate);
        }
    }
}
