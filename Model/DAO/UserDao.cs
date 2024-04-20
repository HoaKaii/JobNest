using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class UserDao
    {
        private readonly JobsFinderDBContext db = null;
        public UserDao()
        {
            db = new JobsFinderDBContext();
        }
        public string Insert(User entity)
        {
            if (entity.Status == null)
            {
                entity.Status = false;
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Users.Add(entity);
                    db.SaveChanges();

                    Profile profile = new Profile
                    {
                        UserID = entity.ID,
                        HoVaTen = entity.Name,
                        AnhCaNhan = entity.Avatar,
                        DiaChiHienTai = entity.Address,
                        Email = entity.Email,
                        SoDienThoai = entity.Phone
                    };
                    db.Profiles.Add(profile);
                    db.SaveChanges();

                    transaction.Commit();

                    return entity.ID;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw; // hoặc xử lý ngoại lệ theo ý của bạn
                }
            }
        }
        public string InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else
            {
                return entity.ID;
            }
        }

        public bool Update(User entity)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Find(entity.ID);
                    if (user != null)
                    {
                        user.Name = entity.Name;
                        if (!string.IsNullOrEmpty(entity.Password))
                        {
                            user.Password = entity.Password;
                        }
                        user.Address = entity.Address;
                        user.Email = entity.Email;
                        user.Phone = entity.Phone;
                        user.Avatar = entity.Avatar;
                        user.ModifiedBy = entity.ModifiedBy;
                        user.ModifiedDate = DateTime.Now;

                        // Cập nhật thông tin trong bảng Profile
                        var profile = db.Profiles.FirstOrDefault(p => p.UserID == entity.ID);
                        if (profile != null)
                        {
                            profile.HoVaTen = entity.Name;
                            profile.AnhCaNhan = entity.Avatar;
                            profile.DiaChiHienTai = entity.Address;
                            profile.Email = entity.Email;
                            profile.SoDienThoai = entity.Phone;
                        }

                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public IPagedList<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public List<User> ListAll()
        {
            return db.Users.Where(x => x.Status == true && x.Name != null).ToList();
        }
        public User GetByID(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public string GetName(string UserID)
        {
            var user = db.Users.FirstOrDefault(x => x.ID == UserID);
            if (user != null)
            {
                return user.Name;
            }
            else
            {
                return "JobsFinder";
            }
        }
        public string GetMetaTitle(string UserID)
        {
            var user = db.Users.FirstOrDefault(x => x.ID == UserID);
            if (user != null)
            {
                string name = user.Name;
                string slug = Regex.Replace(name, @"[^a-zA-Z0-9]", "").ToLower();
                slug = slug.Replace(" ", "-");
                user.Name = slug;
                return user.Name;
            }
            else
            {
                return "jobsfinder";
            }
        }
        public string GetAvatar(string UserID)
        {
            var user = db.Users.FirstOrDefault(x => x.ID == UserID);
            if (user != null)
            {
                if (UserID != null && user.Avatar != null)
                {
                    return user.Avatar;
                }
                else
                {
                    return "./Assets/Client/JobsFinder/img/Logo.png";
                }
            }
            else
            {
                return null;
            }

        }
        public User ViewDetail(long id)
        {
            return db.Users.Find(id);
        }
        public int Login(string username, string password)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == username);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == password)
                    {
                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
        }
        public int CountUser()
        {
            return db.Users.Count();
        }


        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;

            db.SaveChanges();

            return (bool)user.Status;
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckUserName(string username)
        {
            return db.Users.Count(x => x.UserName == username) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
    }
}