using System.Web;
using System.Web.Mvc;
using Vahapp2.Models;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Vahapp2.Controllers
{
    public class ArticlesController : Controller
    {
        private VahappEntities db = new VahappEntities();
        // GET: Articles
        public ActionResult Index()
        {
            if (Session["BasicUser"] == null && Session["AdminUser"] == null)
            {
                return RedirectToAction("login", "home");
            }
            //Tässä rajataan ulos kaikki muut paitsi sisäänkirjautuneet
            else
            {
                var articles = db.Articles.Include(a => a.Categories);
                return View(articles.ToList());
            }
        }

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            //alla string tyyppinen lista statuksen arvoiksi
            List<string> statusList = new List<string>
            {
                "Lainattavissa",
                "OnLoan",
                "Broken"
            };
            //Alla Statuksen Dropdownin koodi käyttää aikaisemmin luotua listaa
            ViewBag.Status = new SelectList(statusList);
            return View();
        }

        // POST: Asiakkaat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleID,CategoryID,SerialNumber,ArticleName,Price,Image,Purchasedate,Status,Warranty,Info,Photopath")] Articles article)
        {
            //Alla string tyyppinen lista statuksen arvoksi
            List<string> statusList = new List<string>
            {
                "Lainattavissa",
                "OnLoan",
                "Broken"
            };
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryID", article.CategoryID);
            //Alla Statuksen Dropdownin controlleri koodi käyttää aikaisemmin luotua listaa
            ViewBag.Status = new SelectList(statusList, "Status", "Status", article.Status);
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

                string fullPath = Request.MapPath("~/Content/Images/" + article.Photopath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                //code above checks does the article have an image file in Images folder and deletes it
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

            List<string> statusList = new List<string>
            {
                "Lainattavissa",
                "OnLoan",
                "Broken"
            };

            ViewBag.Status = new SelectList(statusList);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(article);
        }

        // POST: Asiakkaat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleID,CategoryID,SerialNumber,ArticleName,Price,Image,Purchasedate,Status,Warranty,Info,Photopath")] Articles article)
        {
            List<string> statusList = new List<string>
            {
                "Lainattavissa",
                "OnLoan",
                "Broken"
            };
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;



                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryID", article.CategoryID);
            ViewBag.Status = new SelectList(statusList, "Status", "Status", article.Status);
            return View(article);
        }

        [HttpGet]
        public ActionResult InsertPicture(int? id)
        {


            Articles article = db.Articles.Find(id);


            return View(article);
        }

        [HttpPost]
        public ActionResult InsertPicture([Bind(Include = "ArticleID,CategoryID,SerialNumber,ArticleName,Price,Image,Purchasedate,Status,Warranty,Photopath,Info,postedFile")] Articles article)
        {
            string id = article.ArticleID.ToString();

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase postedFile = Request.Files["postedFile"];
                string path = Server.MapPath("~/Content/Images/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //Above if Images folder does not exist program creates one
                //Below if article already has image it will be deleted
                string fullPath = Request.MapPath("~/Content/Images/" + article.Photopath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);

                }
                //Below saves image file in Images folder and file is named based on articleID and filetype ex .jpg
                postedFile.SaveAs(path + id + "." + Path.GetFileName(postedFile.ContentType));
                string polku = id + "." + Path.GetFileName(postedFile.ContentType);

                //byte[] buffer = new byte[postedFile.InputStream.Length];
                //postedFile.InputStream.Read(buffer, 0, (int)postedFile.InputStream.Length);
                // commented code above can be used downloading images as bytes in database.
                // in this case you have to replace article.Photopath = polku
                //by article.Image = buffer;
                // it seemed to slow down the program,but
                // left here just in case the Image file solution does not work in Azure
                db.Entry(article).State = EntityState.Modified;



                article.Photopath = polku;


                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {

            Articles article = db.Articles.Find(id);

            return View(article);
        }

        public ActionResult UserArticles()
        {
            return View("~/Views/Articles/UserArticles.cshtml");
        }
    }
}