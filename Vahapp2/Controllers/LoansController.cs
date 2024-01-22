using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vahapp2.Data;
using Vahapp2.Models;



namespace Vahapp2.Controllers
{
    public class LoansController : Controller
    {
        private VahappEntities db = new VahappEntities();

        //GET: Loans
        public ActionResult Index()
        {
            
            ViewBag.Notif = db.Notifications.FirstOrDefault();
            if (Session["BasicUser"] == null && Session["AdminUser"] == null)
            {
                return RedirectToAction("login", "home");
            }
            //Tässä rajataan ulos kaikki muut paitsi sisäänkirjautuneet
            else
            {
                var loans = db.Loans.Include(l => l.Articles).Include(l => l.Users);
                return View(loans.ToList());
            }
        }



        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loans loans = db.Loans.Find(id);
            if (loans == null)
            {
                return HttpNotFound();
            }
            return View(loans);
        }
        //Dropdownin valinta testausta----------------------------------------------------
        public JsonResult selection(int? id)
        {
            var articles = db.Articles.Where(a => a.Categories.CategoryID == id);

            List<ArticleData> list = new List<ArticleData>();

            foreach (Articles a in articles)
            {
                ArticleData ad = new ArticleData();
                ad.ArticleID = a.ArticleID;
                ad.ArticleName = a.ArticleName;
                ad.Status = a.Status;
                if (a.Status == "Available") { 
                list.Add(ad);
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //Testaus loppuu--------------------------------------------------------------------

        
        // GET: Loans/Create
        public ActionResult Create()
        {
            ViewBag.ArticleID = new SelectList(db.Articles, "ArticleID", "ArticleName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoanID,UserID,ArticleID,ArticleName,CategoryName,Loandate,Returndate,Duedate")] Loans loans)
        {
            if (ModelState.IsValid)
            {
                db.Loans.Add(loans);
                Articles article = db.Articles.Find(loans.ArticleID);
                article.Status = "OnLoan";

                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticleID = new SelectList(db.Articles, "ArticleID", "SerialNumber", loans.ArticleID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Password", loans.UserID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", loans.Articles.CategoryID);
            return View(loans);
        }
        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loans loans = db.Loans.Find(id);
            if (loans == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleID = new SelectList(db.Articles, "ArticleID", "ArticleName", loans.ArticleID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", loans.UserID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", loans.Articles.CategoryID);
            return View(loans);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanID,UserID,ArticleID,Loandate,Returndate,Duedate")] Loans loans)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loans).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleID = new SelectList(db.Articles, "ArticleID", "Name", loans.ArticleID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Password", loans.UserID);
            return View(loans);
        }

        // GET: Loans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loans loans = db.Loans.Find(id);
            if (loans == null)
            {
                return HttpNotFound();
            }
            return View(loans);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loans loans = db.Loans.Find(id);
            Articles article = db.Articles.Find(loans.ArticleID);
            article.Status = "Available";
            db.Entry(article).State = EntityState.Modified;
            db.Loans.Remove(loans);
            db.SaveChanges();

            
            return RedirectToAction("Index");
        }

        public ActionResult UserLoans()

        {
            string username = User.Identity.Name;

            List<Loans> loans = db.Loans.Where(l => l.Users.Name == username ).ToList();
            return View(loans);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
