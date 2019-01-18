using System.Web.Mvc;
using System.Web.Security;
using System.DirectoryServices;
using MvcApplication.Models;
using System.Security.Principal;
using System.Configuration;
using System.Security;
using Galactic.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Collections.Generic;
using System;
using System.DirectoryServices.AccountManagement;

public class AccountController : Controller
{
    public ActionResult Login()
    {
        return this.View();
    }

    [HttpPost]
    public ActionResult Login(LoginModel model, string returnUrl)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        if (Membership.ValidateUser(model.UserName, model.Password))
        {
            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
            if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return this.Redirect(returnUrl);
            }


            //Findes all groups for user
            var groups = FondGroups(model.UserName);



            ////findes role for user
            //// set up domain context
            //PrincipalContext ctx = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["MyDomain"]);

            //// find a user
            //UserPrincipal user = UserPrincipal.FindByIdentity(ctx, model.UserName);

            //if (user != null)
            //{
            //    // find the roles....
            //    var roles = user.GetAuthorizationGroups();

            //    // enumerate over them
            //    foreach (Principal p in roles)
            //    {
            //        // do something
            //    }
            //}

            //DirectoryEntry entry = new DirectoryEntry(ConfigurationManager.AppSettings["MyDomain"]);
            //DirectorySearcher search = new DirectorySearcher(entry);
            //search.Filter = "(SAMAccountName=" + model.UserName + ")";
            //search.PropertiesToLoad.Add("distinguishedName");
            //search.PropertiesToLoad.Add("displayName");
            //search.PropertiesToLoad.Add("mail");
            //search.PropertiesToLoad.Add("CN");
            //search.PropertiesToLoad.Add("Title");
            //search.PropertiesToLoad.Add("sn");
            //search.PropertiesToLoad.Add("givenname");
            //search.PropertiesToLoad.Add("telephoneNumber");
            //search.PropertiesToLoad.Add("memberOf");

         
            //SearchResult result = search.FindOne();


            return this.RedirectToAction("Index", "Home");
        } 

        this.ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");

        return this.View(model);
    }
    public string GetProperty(SearchResult searchResult,string PropertyName)
    {
        if (searchResult.Properties.Contains(PropertyName))
        {
            return searchResult.Properties[PropertyName][0].ToString();
        }
        else
        {
            return string.Empty;
        }
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
        //int equalsIndex;
        //int commaIndex;
        for (int i = 0; i <= propCount - 1; i++)
        {
            dn = dirSearchResults.Properties["memberOf"][i].ToString();
            Groups.Add(dn);
            //    equalsIndex = dn.IndexOf("=", 1);
            //    commaIndex = dn.IndexOf(",", 1);
            //    if (equalsIndex == -1)
            //    {
            //        return null;
            //    }
            //    if (!Groups.Contains(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1)))
            //    {
            //        Groups.Add(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
            //    }
        }

            return Groups;
    }
    
    public ActionResult LogOff()
    {
        FormsAuthentication.SignOut();

        return this.RedirectToAction("Login", "Account");
    }
}