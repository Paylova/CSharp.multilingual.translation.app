using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sozluk.Models.entity;
namespace sozluk.Controllers
{
    public class VocabularyController : Controller
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
                List<Vocabulary> values = db.Vocabulary.ToList();
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
                List<SelectListItem> values = (from i in db.Language.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = i.LanguageName,
                                                   Value = i.Id.ToString()
                                               }).ToList();
                ViewBag.vls = values;
                return View();
            }
            return RedirectToAction("Login", "Authentication");
        }
        [HttpPost]
        public ActionResult Add(Vocabulary p)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var lang = db.Language.Where(m => m.Id == p.Language.Id).FirstOrDefault();
                p.Language = lang;
                db.Vocabulary.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Authentication");
        }
        public ActionResult Delete(int id)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var voca = db.Vocabulary.Find(id);
                if (voca.IsDeleted == false)
                {
                    voca.IsDeleted = true;
                    voca.IsActive = false;
                }
                else if (voca.IsDeleted == true)
                {
                    voca.IsDeleted = false;
                    voca.IsActive = true;
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
                var voca = db.Vocabulary.Find(id);
                if (voca.IsActive == false && voca.IsDeleted == false)
                {
                    voca.IsActive = true;
                }
                else if (voca.IsActive == true)
                {
                    voca.IsActive = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Authentication");
        }

        public ActionResult VocabularyShow(int id)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var voca = db.Vocabulary.Find(id);
                List<SelectListItem> values = (from i in db.Language.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = i.LanguageName,
                                                   Value = i.Id.ToString()
                                               }).ToList();
                ViewBag.vls = values;
                return View("VocabularyShow", voca);
            }
            return RedirectToAction("Login", "Authentication");
        }
        public ActionResult Update(Vocabulary p)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var voca = db.Vocabulary.Find(p.Id);
                voca.Word = p.Word;
                voca.Translation = p.Translation;
                voca.IsActive = p.IsActive;
                voca.IsDeleted = p.IsDeleted;
                var lang = db.Language.Where(m => m.Id == p.Language.Id).FirstOrDefault();
                voca.LanguageID = lang.Id;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Authentication");
        }
    }
}
