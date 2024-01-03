using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Vahapp2.Models;

namespace Vahapp2.Controllers
{
    public class DropdownController : ApiController
    {
        private VahappEntities db = new VahappEntities();

        [System.Web.Http.HttpGet]
        public List<Articles> getArticles(int categoryID) {

            var articles = db.Articles.Where(a => a.CategoryID == categoryID).ToList();
            return articles;
        }
    }
}
