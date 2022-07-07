using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using sozluk.Models.entity;
namespace sozluk.Controllers
{
    public class LogoController : Controller
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
                List<Logo> values = db.Logo.ToList();
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
        public ActionResult Add(Logo p, HttpPostedFileBase userFile)
        {
            if (userFile == null || userFile.ContentLength <= 0)
            {
                ViewBag.ErrormMessage = "Dosya yok, uygun değil";
                return View();
            }
            p.LogoImage = new byte[userFile.ContentLength];
            userFile.InputStream.Read(p.LogoImage, 0, userFile.ContentLength);
            db.Logo.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var lg = db.Logo.Find(id);
            db.Logo.Remove(lg);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UpdateLogo(int id)
        {
            var p = db.Logo.Find(id);
            var value = db.Logo.Where(m => m.IsActive == true).FirstOrDefault();
            p.IsActive = true;
            if (value != null)
            {
                value.IsActive = false;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
