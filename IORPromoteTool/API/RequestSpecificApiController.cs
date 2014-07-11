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

        // GET specificApi/Request
        public ChartData GetChartYears()
        {
            IEnumerable<IORRequest> requestsYear = this.db.IORRequests.GroupBy(m => m.Request_End_Date.Value.Year).Select(grp => grp.FirstOrDefault()).Where(m => m.Request_End_Date != null);
            //IEnumerable<IORRequest> requestsMonth = this.db.IORRequests.GroupBy(m => m.Request_End_Date.Value.Month).Select(grp => grp.FirstOrDefault()).Where(m => m.Request_End_Date != null);
            ChartData chartData = new ChartData { Years = new List<int>(), Months = new List<int>() };
            
            // Get list of Years
            foreach (IORRequest request in requestsYear)
            {
                chartData.Years.Add(request.Request_End_Date.Value.Year);
            }

            // Get list of Months
            //foreach (IORRequest request in requestsMonth)
            //{
            //    chartData.Months.Add(request.Request_End_Date.Value.Month);
            //}

            return chartData;
        }

        // GET specificApi/Request
        public ChartData GetChartMonths(int year)
        {
            IEnumerable<IORRequest> requestsMonth = this.db.IORRequests.GroupBy(m => m.Request_End_Date.Value.Month).Select(grp => grp.FirstOrDefault()).Where(m => m.Request_End_Date != null && m.Request_End_Date.Value.Year == year);
            ChartData chartData = GetChartYears(); //Get Years data

            // Get list of Months
            foreach (IORRequest request in requestsMonth)
            {
                chartData.Months.Add(request.Request_End_Date.Value.Month);
            }

            return chartData;
        }

        // Dispose
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

    public class ChartData
    {
        public List<int> Years { get; set; }
        public List<int> Months { get; set; }
    }
}