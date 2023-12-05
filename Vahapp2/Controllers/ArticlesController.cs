using System.Web;
using System.Web.Mvc;
using Vahapp2.Models;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System;

namespace Vahapp2.Controllers
{
    public class ArticlesController : Controller
    {
        private VahappEntities db = new VahappEntities();
        // GET: Articles
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.Categories);
            return View(articles.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");

            return View();
        }

        // POST: Asiakkaat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleID,CategoryID,SerialNumber,ArticleName,Price,Image,Purchasedate,Status,Warranty")] Articles article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryID", article.CategoryID);

            return View(article);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articles article = db.Articles.Find(id);
            //if (article == null)
            //{
            //    return HttpNotFound();
            //}
            return View(article);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Articles article = db.Articles.Find(id);
                db.Articles.Remove(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            catch (Exception)
            {

                return View("Error");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articles article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(article);
        }

        // POST: Asiakkaat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleID,CategoryID,SerialNumber,ArticleName,Price,Image,Purchasedate,Status,Warranty")] Articles article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryID", article.CategoryID);

            return View(article);
        }
    }
}