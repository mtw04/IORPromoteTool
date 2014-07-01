using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

using IORPromoteTool.Models;

namespace IORPromoteTool.Controllers
{
    public class BaseApiController : ApiController
    {
        //public PromoteEntities db = new PromoteEntities();
        public TestPromoteEntities db = new TestPromoteEntities();
    }
}
