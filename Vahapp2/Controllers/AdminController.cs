using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vahapp2.Models;

namespace Vahapp2.Controllers
{
    public class AdminController : Controller
    {
        private VahappEntities db = new VahappEntities();
        // GET: Admin

        //[HttpGet]
        //[HandleError]

        //public ActionResult Login()
        //{
        //    return View();
        //}

        [HttpGet]
        [HandleError]
        public ActionResult Login(tblAdmins admin)
        {
            var adm = db.tblAdmins.SingleOrDefault(a => a.AdminEmail == admin.AdminEmail && a.AdminPass == admin.AdminPass);
            if (adm != null)
            {
                int id = adm.AdminId;
                Session["adminId"] = adm.AdminId;
                return RedirectToAction("Index");
            }
            else if (admin.AdminEmail == null && admin.AdminPass == null)
            {
                return View();
            }
            ViewBag.Message = "User name and password are not matching";
            return View();
        }

        [HandleError]
        public ActionResult Logout()
        {
            Session.Remove("adminId");
            return RedirectToAction("Index", "Home");
        }
 
    }
}
