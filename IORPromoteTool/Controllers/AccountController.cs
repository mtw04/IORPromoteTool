using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using IORPromoteTool.Models;
using LeankitLibrary.EntitiesCustom;

namespace IORPromoteTool.Controllers
{
    public class AccountController : BaseApiController
    {
        #region Protected Variables

        private IORSetting iorSettings = null;
        protected IORSetting IORSettings
        {
            get
            {
                if (this.iorSettings == null)
                {
                    this.iorSettings = this.db.IORSettings.Single(m => m.Id == 1);
                }
                return this.iorSettings;
            }
        }

        private PromoteUser currentUser = null;
        public PromoteUser CurrentUser
        {
            get
            {
                if (this.currentUser == null)
                {
                    this.currentUser = this.GetCurrentUser();
                }
                return this.currentUser;
            }
        }

        private List<PromoteUser> validList = null;
        protected List<PromoteUser> ValidList
        {
            get
            {
                if (this.validList == null)
                {
                    this.validList = this.db.PromoteUsers.ToList();
                }
                return this.validList;
            }
        }

        #endregion

        #region Public Functions

        // Check administrator
        public bool ValidUserRights()
        {
            if (this.GetUserName() == "mike.wu" || this.GetUserName() == "helen.wu")
                return true;
            else
                return false;
        }

        public bool ValidRejectRights()
        {
            if (this.GetUserName() == "mike.wu" 
                || this.GetUserName() == "kurt" 
                || this.GetUserName() == "nathan.nandi" 
                || this.GetUserName() == "tony.huang" 
                || this.GetUserName() == "bgeng" 
                || this.GetUserName() == "bkoskicki")
                return true;
            else
                return false;
        }

        public string GetUserFullName()
        {
            return this.CurrentUser.FullName;
        }

        public string GetUserName()
        {
            return (System.Web.HttpContext.Current.User.Identity.Name).Substring(11).ToLower(); //omit "Laserfiche" domain from user name
        }

        #endregion

        #region Internal Functions

        private PromoteUser GetCurrentUser()
        {
            // Check user is admin
            foreach (PromoteUser user in this.ValidList)
            {
                if (user.Name.ToLower() == GetUserName())
                {
                    return user;
                }
            }
            return null; // return null if not found in admin list
        }

        #endregion
    }
}
