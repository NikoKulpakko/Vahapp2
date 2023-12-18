using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vahapp2.Models;

namespace Vahapp2.Controllers
{
    public class LoansController : Controller
    {
        private VahappEntities db = new VahappEntities();

        //GET: Loans
        public ActionResult Index()
        {
            var loans = db.Loans.Include(l => l.Articles).Include(l => l.Users).Include(l => l.Articles.Categories);
            return View(loans.ToList());
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

        // GET: Loans/Create
        public ActionResult Create()
            
        
        //Dropdownien täytöt
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
        
        public ActionResult Create([Bind(Include = "LoanID,Name,UserID,ArticleID,CategoryID,CategoryName,Categories,Loandate,Returndate,Duedate,ArticleName")] Loans loans)
        {
            

            if (ModelState.IsValid)
            {
                
                db.Loans.Add(loans);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //Poistettu Name

            
            ViewBag.ArticleID = new SelectList(db.Articles, "ArticleID", "ArticleName", loans.ArticleID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Name", loans.UserID);
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
            ViewBag.ArticleID = new SelectList(db.Articles, "ArticleID", "Name", loans.ArticleID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Password", loans.UserID);
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
            db.Loans.Remove(loans);
            db.SaveChanges();
            return RedirectToAction("Index");
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
