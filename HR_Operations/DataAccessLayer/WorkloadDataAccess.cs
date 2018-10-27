using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.DataAccessLayer
{
    public class WorkloadDataAccess
    {
        private readonly ApplicationDbContext _db;

        public WorkloadDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public void CreateWorkload(Workload team)
        {
            _db.Workloads.Add(team);
            _db.SaveChanges();
        }

        public void EditWorkload(Workload team)
        {
            _db.Entry(team).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteWorkload(Workload team)
        {
            _db.Workloads.Remove(team);
            _db.SaveChanges();
        }

        public Workload GetWorkloadById(int? id)
        {
            return _db.Workloads.Find(id);
        }

        public void SaveChangesToDb()
        {
            _db.SaveChanges();
        }

        public List<Workload> GetAllWorkloads()
        {
            return _db.Workloads.ToList();
        }


        public void DbDispose()
        {
            _db.Dispose();
        }

    }
}