using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CyberFortress.Models;

namespace CyberFortress.Controllers
{
    public class SoftwaresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Softwares.ToList());
        }

        public ActionResult Download(int id)
        {
            Software software = db.Softwares.Find(id);

            if(software == null)
            {
                return Content("Error");
            }

            string softwareName = software.SoftwareName;

            string filePath = Server.MapPath("~/Software/" + softwareName);
            if (System.IO.File.Exists(filePath))
            {
                return File(filePath, "application/octet-stream", softwareName);
            }
            else
            {
                return HttpNotFound();
            }
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
