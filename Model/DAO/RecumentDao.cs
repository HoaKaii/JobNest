﻿using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class RecumentDao
    {
        private readonly JobsFinderDBContext db = null;
        public RecumentDao()
        {
            db = new JobsFinderDBContext();
        }

        public bool Insert(Recument entity)
        {
            if (entity.Status == null)
            {
                entity.Status = 0;
            }
            try
            {
                db.Recuments.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thực hiện SaveChanges(): " + ex.InnerException.Message);
                return false;
            }
        }

        public string getProfileID(long? ID)
        {
            var item = db.Recuments.Where(x => x.ID == ID).FirstOrDefault();
            return item.UserID;
        }

        public bool Delete(Recument entity)
        {
            try
            {
                var Recument = db.Recuments.Find(entity.ID);
                if (Recument != null)
                {
                    db.Recuments.Remove(Recument);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Recument GetByID(long ID)
        {
            return db.Recuments.SingleOrDefault(x => x.ID == ID);
        }

        public List<Recument> ListAll(int jobID)
        {
            return db.Recuments.Where(x => x.JobID == jobID && x.Status == 0).ToList();
        }

        public Recument ViewDetail(long id)
        {
            return db.Recuments.Find(id);
        }

        public List<Recument> ListApply(int jobID)
        {
            return db.Recuments.Where(x => x.JobID == jobID && x.Status == 1).ToList();
        }

        public bool CheckApply(string userID, int jobID)
        {
            var check = db.Recuments.Where(x => x.UserID == userID && x.JobID == jobID).FirstOrDefault();
            if (check != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Recument> ListRecumented(string userID)
        {
            return db.Recuments.Where(x => x.UserID == userID).ToList();
        }

        public string GetJobName(int jobID)
        {
           var job = db.Jobs.Where(x => x.ID == jobID).FirstOrDefault();
            return job.Name;
        }

        public bool Confirm(Recument entity)
        {
            try
            {
                var recument = db.Recuments.Find(entity.ID);
                recument.Status = 1;

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string GetJobName(long jobID)
        {
            var job = db.Jobs.FirstOrDefault(j => j.ID == jobID);
            return job.Name;
        }

        public Job GetJob(int jobID)
        {
            return db.Jobs.Where(x => x.ID == jobID).FirstOrDefault();
        }
    }
}
