using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        // GET: Test
        
        public ActionResult Test1()
        {
            return View();
        }
        [Authorize(Roles = "_test_idtool_admin")]
        public ActionResult Test2()
        {
            return View();
        }
        
            [Authorize(Roles = "access_jobWorker_user")]
        public ActionResult Test3()
        {
            return View();
        }
        [Authorize(Roles = "access_computer_pc00008929_adm")]
        public ActionResult Test4()
        {
            return View();
        }
    }
}