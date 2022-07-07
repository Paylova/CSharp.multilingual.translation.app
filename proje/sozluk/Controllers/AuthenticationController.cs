using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using sozluk.Models.entity;
namespace sozluk.Controllers
{
    public class AuthenticationController : Controller
    {
        MultilingualDictionaryEntities db = new MultilingualDictionaryEntities();
        public ActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(User p, string pw)
        {
            p.Password = PasswordHash(pw);
            db.User.Add(p);
            db.SaveChanges();
            return RedirectToAction("SuccesfulCreateAccount");
        }
        public ActionResult SuccesfulCreateAccount()
        {
            return View();
        }
        public ActionResult Login()
        {
            if (Session["LoggedCustomer"] != null)
            {
                return RedirectToAction("Search", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(User p, string pw)
        {
            p.Password = PasswordHash(pw);
            var usr = (from i in db.User
                       where i.Username.Equals(p.Username) && i.Password.Equals(p.Password) && i.IsActive
                       select i).SingleOrDefault();
            if (usr != null)
            {
                Session.Add("LoggedCustomer", usr.Username);
                if (usr.IsAdmin)
                {
                    Session.Add("isAdmin", "admin");
                    return RedirectToAction("AdminPanel", "Home");
                }
                else
                {
                    Session.Add("isAdmin", "user");
                    return RedirectToAction("Search", "Home");
                }
            }
            else
                ViewBag.message = "Hatalı kullanıcı adı veya şifre";
            return View("Login");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
        public static byte[] PasswordHash(string pw)
        {
            SHA256Managed managed = new SHA256Managed();
            byte[] readytohash = UTF32Encoding.ASCII.GetBytes(pw);
            byte[] hashedByte = managed.ComputeHash(readytohash);
            return hashedByte;
        }
    }
}
