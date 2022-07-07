using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sozluk.Models.entity;

namespace sozluk.Controllers
{
    public class LanguageController : Controller
    {
        MultilingualDictionaryEntities db = new MultilingualDictionaryEntities();
        public ActionResult Index()
        {
            if (Session["LoggedCustomer"] != null)
            {
                if (Session["isAdmin"].ToString() == "user")
                {
                    return RedirectToAction("Search", "Home");
                }
                List<Language> values = db.Language.ToList();
                return View(values);
            }
            return RedirectToAction("Login", "Authentication");
        }
       
        public ActionResult Add()
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
        [HttpPost]
        public ActionResult Add(Language p)
        {
            if (Session["LoggedCustomer"] != null)
            {
                db.Language.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Authentication");
        }
        public ActionResult Delete(int id)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var lang = db.Language.Find(id);
                if (lang.IsDeleted == false)
                {
                    lang.IsDeleted = true;
                    lang.IsActive = false;
                }
                else if (lang.IsDeleted == true)
                {
                    lang.IsDeleted = false;
                    lang.IsActive = true;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Authentication");
        }
        public ActionResult Active(int id)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var lang = db.Language.Find(id);
                if (lang.IsActive == false && lang.IsDeleted == false)
                {
                    lang.IsActive = true;
                }
                else if (lang.IsActive == true)
                {
                    lang.IsActive = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Authentication");
        }
        public ActionResult LanguageShow(int id)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var lang = db.Language.Find(id);
                return View("LanguageShow", lang);
            }
            return RedirectToAction("Login", "Authentication");
        }
        public ActionResult Update(Language p)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var lang = db.Language.Find(p.Id);
                lang.LanguageName = p.LanguageName;
                lang.IsActive = p.IsActive;
                lang.IsDeleted = p.IsDeleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Authentication");
        }
    }
}
