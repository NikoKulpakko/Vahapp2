using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vahapp2.Models;

namespace Vahapp2.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            if (Session["BasicUser"] == null)
            {
                ViewBag.LoggedStatus = "Signed out";
            }
            else ViewBag.LoggedStatus = "Signed in";
            return View();
        }

        public ActionResult About()
        {
            
            ViewBag.Message = "Your application description page.";

            return View();
            
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Authorize(Users LoginModel)
        {
            VahappEntities db = new VahappEntities();

            var LoggedUser = db.Users.SingleOrDefault(x => x.Email == LoginModel.Email && x.Password == LoginModel.Password);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Successfull login";
                ViewBag.LoggedStatus = "Signed in";
                Session["BasicUser"] = LoggedUser.Name;
                return RedirectToAction("Index", "Home");
            }

            else 
            {
                ViewBag.LoginMessage = "Login Unsuccessfull";
                ViewBag.LoggedStatus = "Logged out";
                LoginModel.LoginErrorMessage = "Unknown user or password";
                return View("Login", LoginModel);
            }
        }

        public ActionResult AdmAuthorize(tblAdmins LoginModel)
        {
            VahappEntities db = new VahappEntities();

            var LoggedAdmin = db.tblAdmins.SingleOrDefault(x => x.AdminEmail == LoginModel.AdminEmail && x.AdminPass == LoginModel.AdminPass);
            if (LoggedAdmin != null)
            {
                ViewBag.LoginMessage = "Successfull login";
                ViewBag.LoggedStatus = "Signed in";
                Session["AdminUser"] = LoggedAdmin.AdminName;
                return RedirectToAction("Index", "Home");
            }

            else
            {
                ViewBag.LoginMessage = "Login Unsuccessfull";
                ViewBag.LoggedStatus = "Logged out";
                LoginModel.LoginErrorMessage = "Unknown user or password";
                return View("Login", LoginModel);
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Logged out";
            return RedirectToAction("Index", "Home");
        }

    }
}