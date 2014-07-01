using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text.RegularExpressions; // Regex

using IORPromoteTool.Models;
using LeankitLibrary.Entities;
using LeankitLibrary.Helper;

namespace IORPromoteTool.Controllers
{
    public class RejectCardApiController : BaseApiController
    {
        // POST api/RejectCard
        [HttpPost]
        public HttpResponseMessage PostRejectCard(Card card)
        {
            // Initialize IOR settings
            IORSetting iorSettings = this.db.IORSettings.Single(m => m.Id == 1);

            // 0. Trigger BP for delete through Forms with HTTPRequest
            // Forms request variables
            string uri = @"https://secure1.laserfiche.com/forms/";
            string user = @"iorapprover";
            string pwd = @"a";

            // Get instance ID of card
            Match matchInstanceId = Regex.Match(card.Description, @"Instance ID.*?>(\d+)</a>");
            string instanceId = matchInstanceId.Groups[1].Value;

            // Login to Forms
            HttpWebResponse responseLogin = WebRequestHelper.WebRequestResponse(uri, user, pwd, @"Account/Login", "POST", @"UserName=iorapprover&Password=a&RememberMe=true&RememberMe=false", @"application/x-www-form-urlencoded", null);

            string FormsCookieName = ".LFFORMSAUTH";
            Cookie cookie = WebRequestHelper.GetResponseCookie(responseLogin, FormsCookieName);

            // Open up Forms API
            /* Old API - Find entire list of all BPs*/
            //string bodyFormsAPI = "{\"bpid\":\"0\",\"status\":[\"running\"],\"groupby\":\"starttime\",\"timeRestriction\":\"older\",\"filterParams\":null}";
            //HttpWebResponse responseAPI = WebRequestHelper.WebRequestResponse(uri, user, pwd, @"webapi/v1/task/TasksByBPID", "POST", bodyFormsAPI, @"application/json", cookie);
            
            /* New API - Find BP by instance ID */
            HttpWebResponse responseAPI = WebRequestHelper.WebRequestResponse(uri, user, pwd, @"webapi/v1/task?InstanceId=" + instanceId, "GET", "", @"application/json", cookie);
            string outputAPI = WebRequestHelper.WebRequestOutput(responseAPI);

            // Find Resume ID
            Match matchResumeId = null;
            if (Convert.ToInt16(instanceId) >= 13600)
            {
                matchResumeId = Regex.Match(outputAPI, "Instance ID: " + instanceId + @".*?\""resumeId\"":\""(\w+-\w+-\w+-\w+-\w+)"); //Mike: Use .*? to make it a non-greedy match and return the first instance
            }
            else
            {
                matchResumeId = Regex.Match(outputAPI, "Instance ID: " + instanceId + @".*?\""resumeId\"":\""(\w+)");
            }
            string resumeId = matchResumeId.Groups[1].Value;

            // Open up Trigger Form
            HttpWebResponse responseTrigger = WebRequestHelper.WebRequestResponse(uri, user, pwd, @"form/approval/" + resumeId + @"/1/MyTasks", "GET", "", @"application/json", cookie);
            string outputTrigger = WebRequestHelper.WebRequestOutput(responseTrigger);

            // Find UQID
            Match matchUqid = Regex.Match(outputTrigger, @"\""uqid\""\s+value=\""(\w+-\w+-\w+-\w+-\w+)");
            string uqid = matchUqid.Groups[1].Value;

            // Reject Trigger Form
            string rejectComment = card.Comment.Replace(" ", @"+");
            string action = @"Reject+IOR";
            string triggerFormId = String.Empty;
            string stepId = "196"; //constant
            string timezone = "420"; //constant

            // Mike: update this based on Test vs. Live IOR form. Test = "2760", Live = "2738";
            switch (iorSettings.Environment)
            {
                case "Live":
                    triggerFormId = "2738";
                    break;
                case "Test":
                    triggerFormId = "2760";
                    break;
                default:
                    triggerFormId = "2738";
                    break;
            }

            string bodyTriggerForm = @"comments=" + rejectComment 
                                    + @"&action=" + action 
                                    + @"&formid=" + triggerFormId 
                                    + @"&IsLocked=" 
                                    + @"False&timezone=" + timezone 
                                    + @"&uqid=" + uqid 
                                    + @"&routingResumeId=" + resumeId 
                                    + @"&stepid=" + stepId;

            HttpWebResponse responseReject = WebRequestHelper.WebRequestResponse(uri, user, pwd, @"Form/Submit", "POST", bodyTriggerForm, @"application/x-www-form-urlencoded", cookie);

            // Log out of Forms
            HttpWebResponse responseLogout = WebRequestHelper.WebRequestResponse(uri, user, pwd, @"Account/LogOff", "GET", "", @"application/json", cookie);

            // Output message
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, card);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = card.Id }));
            return response;
        }

        // Dispose
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
