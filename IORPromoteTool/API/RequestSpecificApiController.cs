using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using IORPromoteTool.Models;

namespace IORPromoteTool.Controllers
{
    public class RequestSpecificApiController : BaseApiController
    {
        // GET specificApi/Request
        public DepartmentData GetDepartments(int year, int month)
        {
            IEnumerable<IORRequest> requests = this.db.IORRequests.Where(m => m.Request_End_Date.Value.Year == year && m.Request_End_Date.Value.Month == month).GroupBy(m => m.Department).Select(grp => grp.FirstOrDefault()); 
            DepartmentData depData = new DepartmentData { Departments = new List<string>() }; // Always need to initialize lists

            foreach (IORRequest request in requests)
            {
                depData.Departments.Add(request.Department);
            }

            return depData;
        }

        // GET specificApi/Request
        public int GetDepartmentCount(string department, int year, int month)
        {
            IEnumerable<IORRequest> requests = this.db.IORRequests.Where(m => m.Department == department && m.Request_End_Date.Value.Year == year && m.Request_End_Date.Value.Month == month);
            int count = requests.Count();

            return count;
        }

        // GET specificApi/Request
        public DepartmentDict GetDepDict(int year, int month)
        {
            DepartmentData depData = GetDepartments(year, month);
            DepartmentDict depDict = new DepartmentDict { Departments = new Dictionary<string, int>() };

            foreach (string department in depData.Departments)
            {
                depDict.Departments.Add(department, GetDepartmentCount(department, year, month));
            }

            return depDict;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

    public class DepartmentData
    {
        public List<string> Departments { get; set; }
    }

    public class DepartmentDict
    {
        public Dictionary<string, int> Departments { get; set; }
    }
}