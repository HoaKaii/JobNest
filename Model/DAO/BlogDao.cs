using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Model.DAO
{
    public class BlogDao
    {
        private readonly JobsFinderDBContext _db = null;

        public BlogDao()
        {
            _db = new JobsFinderDBContext();
        }

        public long InsertOrUpdate(Blog entity)
        {
            if (entity.ID == 0)
            {
                if (entity.Status == null)
                    entity.Status = true;

                if (entity.CreatedDate == null)
                    entity.CreatedDate = DateTime.Now;

                if (entity.MetaTitle == null)
                {
                    string name = entity.Name;
                    string slug = Regex.Replace(name, @"[^a-zA-Z0-9]", "").ToLower();
                    slug = slug.Replace(" ", "-");
                    entity.MetaTitle = slug;
                }

                if (entity.Image == null)
                    entity.Image = "/Data/images/Test/blog.jpg";

                if (entity.ViewCount == null)
                    entity.ViewCount = 0;

                _db.Blogs.Add(entity);
            }
            else
            {
                var blog = _db.Blogs.FirstOrDefault(b => b.ID == entity.ID);
                if (blog != null)
                {
                    blog.Name = entity.Name;
                    blog.Summary = entity.Summary;
                    blog.Description = entity.Description;
                    blog.Image = entity.Image;
                    blog.CategoryID = entity.CategoryID;
                    blog.ModifiedDate = DateTime.Now;
                    blog.TopHot = entity.TopHot;
                }
            }

            _db.SaveChanges();
            return entity.ID;
        }

        public IPagedList<Blog> ListAllPaging(string searchName, string fillterCategory, int page, int pageSize)
        {
            IQueryable<Blog> model = _db.Blogs;

            if (!string.IsNullOrEmpty(searchName))
            {
                model = model.Where(x => x.Name.Contains(searchName));
            }
            if (!string.IsNullOrEmpty(fillterCategory))
            {
                model = model.Where(x => x.CategoryID.ToString().Contains(fillterCategory));
            }

            return model.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public Blog GetById(long id)
        {
            return _db.Blogs.FirstOrDefault(x => x.ID == id);
        }

        public bool Delete(int id)
        {
            var blog = _db.Blogs.FirstOrDefault(x => x.ID == id);
            if (blog != null)
            {
                _db.Blogs.Remove(blog);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ChangeStatus(int id)
        {
            var blog = _db.Blogs.FirstOrDefault(x => x.ID == id);
            if (blog != null)
            {
                blog.Status = !blog.Status;
                _db.SaveChanges();
                return (bool)blog.Status;
            }
            return false;
        }

        public Blog ViewDetail(int id)
        {
            return _db.Blogs.Find(id);
        }

        public int CountBlogs()
        {
            return _db.Blogs.Count();
        }
        public Blog GetForYouBlog()
        {
            return _db.Blogs.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        }

        public Blog GetPopularBlog()
        {
            return _db.Blogs.OrderByDescending(x => x.ViewCount).FirstOrDefault();
        }

        public List<Blog> GetTrendBlogs()
        {
            return _db.Blogs.Where(x => x.TopHot == true).OrderByDescending(x => x.ViewCount).Take(3).ToList();
        }
        public string GetCategory(long? cateID)
        {
            var category = new BlogCategoryDao();
            long? categoryID = GetCategoryID(cateID);
            string name;
            if (categoryID != null)
            {
                name = category.GetName(categoryID);
            }
            else
            {
                name = "Không có danh mục";
            }
            return name;
        }
        public long? GetCategoryID(long? id)
        {
            var job = _db.Blogs.FirstOrDefault(x => x.CategoryID == id);
            if (job != null)
            {
                return job.CategoryID;
            }
            return null;
        }
        public int CountBlogsCreatedToday()
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today;
            DateTime endDate = today.AddDays(1).AddSeconds(-1);

            return _db.Blogs.Count(j => j.CreatedDate >= startDate && j.CreatedDate <= endDate);
        }
        public int CountBlogsByCategoryId(long categoryId)
        {
            return _db.Blogs.Count(j => j.CategoryID == categoryId);
        }
        public void UpdateViewCount(long blogId)
        {
            var blog = _db.Blogs.FirstOrDefault(j => j.ID == blogId);
            if (blog != null)
            {
                blog.ViewCount++;
                _db.SaveChanges();
            }
        }
    }
}
