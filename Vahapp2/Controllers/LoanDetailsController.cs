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

        public ActionResult Notifications()
        {
            var loandetails = db.LoanDetails.Include(a => a.Loans).Include(a => a.Users);
            MailAddress to = new MailAddress("ReceiversAddress");

            MailAddress from = new MailAddress("SenderAddress");

            MailMessage email = new MailMessage(from, to);

            email.Subject = "Article Unreturned";
            email.Body = "Please return the Article or contact the administrator to postpone duedate";

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587; // on SSL
            smtp.Credentials = new NetworkCredential("SenderAddress", "apppasswordhere");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            try
            {
                /* Send method called below is what will send off our email 
                 * unless an exception is thrown.
                 */
                smtp.Send(email);
                return RedirectToAction("Index","Loans");
            }
            catch (Exception)
            {

                return View("Error");
            }

        }
    }
}