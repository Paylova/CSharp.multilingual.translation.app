using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using sozluk.Models.entity;

namespace sozluk.Controllers
{
    public class UserController : Controller
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
                List<User> values = db.User.ToList();
                return View(values);
            }
            return RedirectToAction("Login", "Authentication");
        }
        public ActionResult Delete(int id)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var usr = db.User.Find(id);
                if (usr.IsDeleted == false)
                {
                    usr.IsDeleted = true;
                    usr.IsActive = false;
                    SendMail(usr.Mail, "Üyelik Durumu Hakkında Bilgilendirme", "<span>Lova Sözlük üyeliğiniz <b>sonlandırılmıştır.</b> </span>");
                }
                else if (usr.IsDeleted == true)
                {
                    usr.IsDeleted = false;
                    usr.IsActive = true;
                    SendMail(usr.Mail, "Üyelik Durumu Hakkında Bilgilendirme", "<span>Lova Sözlük üyeliğiniz <b>tekrar kullanıma açılmıştır.</b> </span>");
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
                var usr = db.User.Find(id);
                if (usr.IsActive == false && usr.IsDeleted == false)
                {
                    usr.IsActive = true;
                    SendMail(usr.Mail, "Üyelik Durumu Hakkında Bilgilendirme", "<span>Lova Sözlük üyeliğiniz <b>kullanıma açılmıştır.</b> </span>");
                }
                else if (usr.IsActive == true)
                {
                    usr.IsActive = false;
                    SendMail(usr.Mail, "Üyelik Durumu Hakkında Bilgilendirme", "<span>Lova Sözlük üyeliğiniz <b>askıya alınmıştır.</b> </span>");
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Authentication");
        }

        public ActionResult Admin(int id)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var usr = db.User.Find(id);
                if (usr.IsAdmin == false)
                {
                    usr.IsAdmin = true;
                    SendMail(usr.Mail, "Üyelik Rolü Hakkında Bilgilendirme", "<span>Lova Sözlük'ün efsane admin kadrosuna <b>hoşgeldin!</b> </span>");
                }
                else if (usr.IsAdmin == true)
                {
                    usr.IsAdmin = false;
                    SendMail(usr.Mail, "Üyelik Rolü Hakkında Bilgilendirme", "<span><span>Lova Sözlük adminlik görevinize <b>son verilmiştir.</b></span>");
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Authentication");
        }

        public ActionResult UserShow(int id)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var usr = db.User.Find(id);

                return View("UserShow", usr);
            }
            return RedirectToAction("Login", "Authentication");
        }

        public ActionResult Update(User p)
        {
            if (Session["LoggedCustomer"] != null)
            {
                var usr = db.User.Find(p.Id);
                usr.Username = p.Username;
                usr.Password = p.Password;
                usr.Mail = p.Mail;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Authentication");
        }
        public void SendMail(string toMailAddress, string subject, string body)
        {
            SmtpClient client = new SmtpClient("mail.lova.domain.com", 587);
            MailAddress fromAddress = new MailAddress("noreply@lovaceviri.com", "Lova Çeviri");
            MailAddress toAddress = new MailAddress(toMailAddress);
            MailMessage msg = new MailMessage(fromAddress, toAddress);
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;
            string usrname = Session["LoggedCustomer"].ToString();
            string usrmail = (from i in db.User
                              where i.Username.Equals(usrname)
                              select i.Mail).SingleOrDefault().ToString();
            msg.CC.Add(usrmail);
            List<string> admins = (from i in db.User
                                   where i.IsAdmin
                                   select i.Mail).ToList();
            foreach (var item in admins)
            {
                msg.Bcc.Add(item);
            }
            msg.ReplyToList.Add("lovacontact@lova.com");
            NetworkCredential userInfo = new NetworkCredential("lova_ceviri@hotmail.com", "loVa123!");
            client.Credentials = userInfo;
            //try
            //{
            //    client.Send(msg);
            //}
            //catch (OverflowException ex)
            //{
            //    throw new Exception("Mail gönderirken bir hata oluştu", ex);
            //}
            //catch (ArgumentNullException ex)
            //{
            //    throw new Exception("Mail gönderirken bir hata oluştu", ex);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Mail gönderirken bir hata oluştu", ex);
            //}
            //finally
            //{

            //}
        }
    }
}