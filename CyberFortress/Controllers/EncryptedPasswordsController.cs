using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CyberFortress.Models;
using Microsoft.AspNet.Identity;

namespace CyberFortress.Controllers
{
    [Authorize]
    public class EncryptedPasswordsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            return View(db.EncryptedPasswords.Where(m => m.UserId.Equals(currentUserId)).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Website,Username,EncryptedData")] EncryptedPassword encryptedPassword)
        {
            if (ModelState.IsValid)
            {
                encryptedPassword.UserId = User.Identity.GetUserId();
                db.EncryptedPasswords.Add(encryptedPassword);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(encryptedPassword);
        }

        public ActionResult Delete(int id)
        {
            EncryptedPassword encryptedPassword = db.EncryptedPasswords.Find(id);
            if(!encryptedPassword.UserId.Equals(User.Identity.GetUserId()))
            {
                return Content("Forbidden action");
            }
            db.EncryptedPasswords.Remove(encryptedPassword);
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
