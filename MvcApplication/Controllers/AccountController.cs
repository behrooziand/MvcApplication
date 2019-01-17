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
            //DirectoryEntry entry = new DirectoryEntry("LDAP://SR010010.medi.local:389/DC=medi,DC=Local");
            //DirectorySearcher mySearcher = new DirectorySearcher(entry);
            //mySearcher.Filter = "(&(objectClass=user)(|(cn=" + model.UserName + ")(sAMAccountName=" + model.UserName + ")))";
            //mySearcher.PropertiesToLoad.Add("memberOf");
            //SearchResult result = mySearcher.FindOne();
            //var u = 

            DirectoryEntry entry = new DirectoryEntry("LDAP://SR010010.medi.local:389/DC=medi,DC=Local");
            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(SAMAccountName=" + model.UserName + ")";
            search.PropertiesToLoad.Add("distinguishedName");
            search.PropertiesToLoad.Add("displayName");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("CN");
            search.PropertiesToLoad.Add("Title");
            search.PropertiesToLoad.Add("sn");
            search.PropertiesToLoad.Add("givenname");
            search.PropertiesToLoad.Add("telephoneNumber");
            search.PropertiesToLoad.Add("memberOf");

            SearchResult result = search.FindOne();
            
            var u1 = search.FindOne().Properties.Values;


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

    public ActionResult LogOff()
    {
        FormsAuthentication.SignOut();

        return this.RedirectToAction("Login", "Account");
    }
}