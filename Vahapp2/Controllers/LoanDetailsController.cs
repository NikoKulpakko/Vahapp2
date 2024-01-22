using System.Web;
using System.Web.Mvc;
using Vahapp2.Models;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace Vahapp2.Controllers
{
    public class LoanDetailsController : Controller
    {
        private VahappEntities db = new VahappEntities();
        // GET: LoanDetails
        public ActionResult Index()
        {

            var loandetails = db.LoanDetails.Include(a => a.Loans).Include(a => a.Users);

            return View(loandetails.ToList());

        }

        public ActionResult Notifications(int? id)
        {
            
            if (Session["AdminUser"] != null)
            {

                var ld = db.Loans.Find(id);
            var notif = db.Notifications.FirstOrDefault();
            string resultString = Encoding.UTF8.GetString(notif.SsHash);
            MailAddress to = new MailAddress(ld.Users.Email);

            MailAddress from = new MailAddress(notif.Sapo);

            MailMessage email = new MailMessage(from, to);

            email.Subject = notif.Otsikko;
            email.Body = notif.Viesti;
                
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587; 
            smtp.Credentials = new NetworkCredential(notif.Sapo, resultString);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            try
            {
                
                smtp.Send(email);
                    
                return RedirectToAction("Index", "Loans");
            }
            catch (Exception)
            {

                return View("NotifError");
            }
            }
            else return RedirectToAction("login", "home");

        }

        public ActionResult SetNotifications()
        {
            ViewBag.Notif = db.Notifications.FirstOrDefault();
            if (Session["AdminUser"] != null)
            {
                return View();
            }
            else return RedirectToAction("login", "home");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetNotifications([Bind(Include = "NotifID,Sapo,SsHash,Otsikko,Viesti,ExtraField")] Notifications not)
        {
            byte[] Bytes = Encoding.UTF8.GetBytes(not.ExtraField);
            if (ModelState.IsValid)
            {
                not.SsHash = Bytes;
                not.ExtraField = null;
                var removal = db.Notifications.FirstOrDefault();
                if(removal != null)
                {
                    db.Notifications.Remove(removal);
                }
                
                db.Notifications.Add(not);
                db.SaveChanges();
                return RedirectToAction("Index", "Loans");
            }

            return View();
        }

        public ActionResult DeleteNotifications(int? id)
        {
            Notifications not = db.Notifications.FirstOrDefault();

            if (Session["AdminUser"] != null)
            {
                return View(not);
            }
            else return RedirectToAction("login", "home");
            
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteNotifications()
        {
            Notifications not = db.Notifications.FirstOrDefault();
            if (ModelState.IsValid)
            {
                
                
                if (not != null)
                {
                    db.Notifications.Remove(not);
                }

               
                db.SaveChanges();
                return RedirectToAction("Index", "Loans");
            }

            return View();
        }
    }
}