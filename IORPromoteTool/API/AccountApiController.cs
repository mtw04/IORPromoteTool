using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using IORPromoteTool.Models;

namespace IORPromoteTool.Controllers
{
    public class AccountApiController : ApiController
    {
        private AccountController accountCtrl = new AccountController();

        #region API Functions

        [HttpGet]
        public AccountData GetAccountData()
        {
            try
            {
                AccountData data = new AccountData();
                data.Name = accountCtrl.GetUserName();
                data.UserAdmin = accountCtrl.ValidUserRights();
                data.RejectAdmin = accountCtrl.ValidRejectRights();
                return data;
            }
            catch (Exception ex)
            {
                return new AccountData() { Error = ex.Message + ex.StackTrace };
            }
        }

        #endregion

        // Dispose
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
