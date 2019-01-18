using MvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Profile()
        {
            // set up domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
            UserProfile userview = new UserProfile()
            {
                UserName=(user.SamAccountName!=null)?user.SamAccountName.ToString():"",
                DisplayName=(user.DisplayName != null) ?user.DisplayName.ToString():"",
                EmployeeId = (user.EmployeeId != null) ? user.EmployeeId.ToString():"",
                FirstName = (user.GivenName != null) ? user.GivenName.ToString():"",
                LastName = (user.Surname != null) ? user.Surname.ToString():"",
                Email = (user.EmailAddress != null) ? user.EmailAddress.ToString():"",
                PhoneNumber = (user.VoiceTelephoneNumber != null) ? user.VoiceTelephoneNumber.ToString():"",
                Groups =  FondGroups(User.Identity.Name).ToList()
        };
            return View(userview);
        }
        [Authorize(Roles = "access_computer_pc00008929_adm")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public List<string> FondGroups(string username)
        {
            List<string> Groups = new List<string>();

            //initialize the directory entry object 
            DirectoryEntry dirEntry = new DirectoryEntry(ConfigurationManager.AppSettings["MyDomain"]);

            //directory searcher
            DirectorySearcher dirSearcher = new DirectorySearcher(dirEntry);

            //enter the filter
            dirSearcher.Filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", username);

            //get the member of properties for the search result
            dirSearcher.PropertiesToLoad.Add("memberOf");
            int propCount;
            SearchResult dirSearchResults = dirSearcher.FindOne();
            propCount = dirSearchResults.Properties["memberOf"].Count;
            string dn;
            int equalsIndex;
            int commaIndex;
            for (int i = 0; i <= propCount - 1; i++)
            {
                dn = dirSearchResults.Properties["memberOf"][i].ToString();

                equalsIndex = dn.IndexOf("=", 1);
                commaIndex = dn.IndexOf(",", 1);
                if (equalsIndex == -1)
                {
                    return null;
                }
                if (!Groups.Contains(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1)))
                {
                    Groups.Add(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                }
            }

            return Groups;
        }
    }
}