using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using sozluk.Models.entity;

namespace sozluk.Controllers
{
    public class HomeController : Controller
    {
        MultilingualDictionaryEntities db = new MultilingualDictionaryEntities();
        public ActionResult Search()
        {
            if (Session["LoggedCustomer"] != null)
            {
                List<SelectListItem> values = (from i in db.Language.ToList()
                                               where i.IsActive
                                               select new SelectListItem
                                               {
                                                   Text = i.LanguageName,
                                                   Value = i.Id.ToString()
                                               }).ToList();
                ViewBag.vls = values;

                if (TempData["x"] == null)
                {
                    ViewBag.tempcontrol = null;
                }
                else
                {
                    ViewBag.tempcontrol = "notnull";
                    if (TempData["x"].ToString() == "null")
                    {
                        ViewBag.findControl = null;
                    }
                    else
                    {
                        ViewBag.findControl = TempData["x"];
                    }
                }
                return View();
            }
            return RedirectToAction("Login", "Authentication");
        }
        [HttpPost]
        public ActionResult Search(Vocabulary p)
        {
            List<Vocabulary> values = (from i in db.Vocabulary
                          where i.Language.Id == p.Language.Id && i.Word.Equals(p.Word) && i.IsActive || i.Language.Id == p.Language.Id && i.Translation.Equals(p.Word) && i.IsActive
                          select i).ToList();
            TempData["x"] = "null";
            if (values.Count > 0)
            {
                TempData["x"] = values;
            }
            return RedirectToAction("Search");
        }
        public ActionResult About()
        {
            if (Session["LoggedCustomer"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Authentication");
        }
        public ActionResult Contact()
        {
            if (Session["LoggedCustomer"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Authentication");
        }
        public ActionResult AdminPanel()
        {
            if (Session["LoggedCustomer"] != null)
            {
                if (Session["isAdmin"].ToString() == "user")
                {
                    return RedirectToAction("Search", "Home");
                }
                return View();
            }
            return RedirectToAction("Login", "Authentication");
        }
    }
}