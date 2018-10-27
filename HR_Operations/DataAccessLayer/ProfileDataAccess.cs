using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.DataAccessLayer
{
    public class ProfileDataAccess
    {
        private readonly ApplicationDbContext _db;

        public ProfileDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public void CreateProfile(Profile profile)
        {
            _db.Profiles.Add(profile);
            _db.SaveChanges();
        }

        public void DeleteProfile(Profile profile)
        {
            _db.Profiles.Remove(profile);
            _db.SaveChanges();
        }

        public Profile GetProfileById(int? id)
        {
            return _db.Profiles.FirstOrDefault(c => c.Employee.Id == id);
        }

        public List<Profile> GetAllProfiles()
        {
            return _db.Profiles.ToList();
        }

        public void SaveChangesToDb()
        {
            _db.SaveChanges();
        }

        public void DbDispose()
        {
            _db.Dispose();
        }
    }
}