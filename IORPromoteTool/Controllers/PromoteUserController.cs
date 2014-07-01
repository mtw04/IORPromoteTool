using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using IORPromoteTool.Models;
using System.Web.Script.Serialization;

namespace IORPromoteTool.Controllers
{
    public class PromoteUserController : BaseController
    {
        #region Redirect

        // Action Filter
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ActionName != "Index")
            {
                base.OnActionExecuting(filterContext); // Stop filtering if redirect is back at Index
            }
            else if (!ValidAdminRights())
            {
                filterContext.Result = RedirectToAction("Index", "PromoteCard");
            }
        }

        #endregion

        #region Private

        private EmployeeEntities employeeDb = new EmployeeEntities();

        #endregion

        #region Actions

        public ActionResult IndexAngular()
        {
            return View(this.ValidList);
        }

        public ActionResult Index()
        {
            //List<PromoteUser> Users = db.PromoteUsers.ToList();
            return View(this.ValidList);
        }

        // GET: /PromoteUsers/Details/1

        public ActionResult Details(int id = 0)
        {
            PromoteUser user = this.ValidList.Single(m => m.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /PromoteUsers/Create

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PromoteList = new SelectList(this.employeeDb.employees, "nwuser", "nwuser");
            return View();
        }

        // POST: /PromoteUsers/Create

        [HttpPost]
        public ActionResult Create(PromoteUser user)
        {
            if (ModelState.IsValid)
            {
                this.db.PromoteUsers.Add(user);
                this.db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: /PromoteUsers/Edit/5

        [HttpGet] // Not necessary since it's the default
        public ActionResult Edit(int id = 0)
        {
            PromoteUser user = this.ValidList.Single(m => m.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.PromoteList = new SelectList(this.employeeDb.employees, "nwuser", "nwuser");
            return View(user);
        }

        // POST: /PromoteUsers/Edit/5

        [HttpPost]
        public ActionResult Edit(PromoteUser user)
        {
            if (ModelState.IsValid)
            {
                this.db.Entry(user).State = EntityState.Modified;
                //this.db.PromoteUsers.Attach(user);
                //this.db.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                this.db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PromoteList = new SelectList(this.employeeDb.employees, "nwuser", "nwuser");
            return View(user);
        }

        // GET: /PromoteUsers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PromoteUser user = this.ValidList.Single(m => m.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /PromoteUsers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            PromoteUser user = this.ValidList.Single(m => m.Id == id);
            this.db.PromoteUsers.Remove(user);
            //this.db.PromoteUsers.DeleteObject(user);
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: /PromoteUsers/Import

        public ActionResult Import()
        {
            List<employee> employeeList = this.GetImportList();
            return View(employeeList);
        }

        [HttpPost, ActionName("Import")]
        public ActionResult ImportConfirmed()
        {
            List<employee> employeeList = this.GetImportList();
            int frequency = this.GetImportFrequency();

            foreach (employee emp in employeeList)
            {
                PromoteUser user = new PromoteUser();
                user.Name = emp.nwuser;
                user.FullName = emp.emp_fname + " " + emp.emp_lname;
                user.Frequency = frequency;
                this.db.PromoteUsers.Add(user);
            }
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region Internal Functions

        // Get list of all managers who are currently employed and not already in table
        private List<employee> GetImportList()
        {
            List<string> importedUsers = this.ValidList.Select(m => m.Name).ToList();
            List<employee> employeeList = this.employeeDb.employees.Where(m => (m.IsManager == true && m.IsEmployed == true) && !importedUsers.Contains(m.nwuser)).ToList();
            return employeeList;
        }

        private int GetImportFrequency()
        {
            return this.IORSettings.PromoteFrequency;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            this.db.Dispose();
            base.Dispose(disposing);
        }
    }
}
