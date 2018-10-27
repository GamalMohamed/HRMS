using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.DataAccessLayer
{
    public class RoleDataAccess
    {
        private readonly ApplicationDbContext _db;

        public RoleDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public void CreateRole(Role role)
        {
            _db.Roles.Add(role);
            _db.SaveChanges();
        }

        public void EditRole(Role role)
        {
            _db.Entry(role).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public List<Role> GetAllRoles()
        {
            return _db.Roles.ToList();
        }

        public Role GetRoleById(int? id)
        {
            return _db.Roles.Find(id);
        }


        public void DeleteRole(Role role)
        {
            _db.Roles.Remove(role);
            _db.SaveChanges();
        }

        public void DbDispose()
        {
            _db.Dispose();
        }
    }
}