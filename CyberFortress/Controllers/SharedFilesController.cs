using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using CyberFortress.Models;
using Microsoft.AspNet.Identity;

namespace CyberFortress.Controllers
{
    [Authorize]
    public class SharedFilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const double maxTotalSize = 1000.0;

        public ActionResult SharedWithMe()
        {
            string currentUserId = User.Identity.GetUserId();
            return View(db.SharedFiles.Where(m => m.ReceiverUserId.Equals(currentUserId)).ToList());
        }
        public ActionResult SharedByMe()
        {
            var currentUserId = User.Identity.GetUserId();
            if (db.SharedFiles.Where(m => m.SenderUserId.Equals(currentUserId)).Count() > 0)
            {
                int totalSizeOfFiles = db.SharedFiles.Where(m => m.SenderUserId.Equals(currentUserId)).Sum(m => m.SharedFileSize);
                ViewBag.TotalFilesSize = ((decimal)totalSizeOfFiles) / 1000;
            }
            else
            {
                ViewBag.TotalFilesSize = 0;
            }
            ViewBag.MaxTotalSize = maxTotalSize;
            return View(db.SharedFiles.Where(m => m.SenderUserId.Equals(currentUserId)).ToList());
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.Message = "Please select a file";
                return View("Upload");
            }
            var currentUserId = User.Identity.GetUserId();
            double totalSizeOfFiles = 0;
            if (db.SharedFiles.Where(m => m.SenderUserId.Equals(currentUserId)).Count() > 0)
            {
                totalSizeOfFiles = db.SharedFiles.Where(m => m.SenderUserId.Equals(currentUserId)).Sum(m => m.SharedFileSize);
            }
            totalSizeOfFiles += file.ContentLength;
            totalSizeOfFiles /= 1000;
            if (totalSizeOfFiles > maxTotalSize)
            {
                ViewBag.Message = "Reached the total memory capacity of all files";
                return View("Upload");
            }
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var senderUserId = User.Identity.GetUserId();
                var path = Path.Combine(Server.MapPath("~/SharedSafe/" + senderUserId), fileName);
                var fileSize = file.ContentLength;
                byte[] encryptionKey;
                byte[] IV;

                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                using (var aes = Aes.Create())
                {
                    encryptionKey = aes.Key;
                    IV = aes.IV;
                }

                EncryptAndSaveFile(file.InputStream, path, encryptionKey, IV);

                var sharedFile = new SharedFile
                {
                    SharedFileName = fileName,
                    SharedFilePath = path,
                    SenderUserId = senderUserId,
                    SenderUserName = User.Identity.GetUserName(),
                    SharedFileSize = fileSize,
                    SharedFileEncryptionKey = encryptionKey,
                    SharedFileIV = IV
                };

                db.SharedFiles.Add(sharedFile);
                db.SaveChanges();

                ViewBag.Message = "File uploaded and encrypted successfully";

                SharedFileReceiverViewModel model = new SharedFileReceiverViewModel();
                model.SharedFileId = sharedFile.Id;
                return RedirectToAction("AddSharedFileReceiver",model);
            }
            else
            {
                ViewBag.Message = "No file uploaded";
            }
            return View("Upload");
        }

        private void EncryptAndSaveFile(Stream inputStream, string outputPath, byte[] encryptionKey, byte[] IV)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = encryptionKey;
                aes.IV = IV;

                using (var outputFile = new FileStream(outputPath, FileMode.Create))
                using (var cryptoStream = new CryptoStream(outputFile, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    inputStream.CopyTo(cryptoStream);
                }
            }
        }

        public ActionResult AddSharedFileReceiver(SharedFileReceiverViewModel model) { 
            return View(model); 
        }

        [HttpPost]
        public ActionResult AddSharedFileReceiverPost(SharedFileReceiverViewModel model)
        {
            SharedFile file = db.SharedFiles.Find(model.SharedFileId);
            if(file == null)
            {
                return Content("Invalid operaion");
            }

            file.SenderUserId = User.Identity.GetUserId();
            file.SenderUserName = User.Identity.GetUserName();
            file.ReceiverUserName = model.SharedFileReceiverUsername;
            file.ReceiverUserId = db.Users.Where(m => m.Email.Equals(model.SharedFileReceiverUsername)).FirstOrDefault().Id;
            db.SaveChanges();

            return RedirectToAction("SharedByMe");
        }

        public ActionResult Download(int id)
        {
            var userId = User.Identity.GetUserId();
            SharedFile file = db.SharedFiles.Where(m => m.Id == id).FirstOrDefault();

            if (file == null)
            {
                return Content("Invalid file");
            }

            if (userId != file.SenderUserId && userId != file.ReceiverUserId)
            {
                return Content("Forbidden operation");
            }

            var fileName = file.SharedFileName;
            var filePath = Path.Combine(Server.MapPath("~/SharedSafe/" + file.SenderUserId), fileName);

            byte[] encryptionKey = file.SharedFileEncryptionKey;
            byte[] IV = file.SharedFileIV;

            return DecryptAndDownloadFile(filePath, fileName, encryptionKey, IV);
        }

        private FileStreamResult DecryptAndDownloadFile(string filePath, string fileName, byte[] encryptionKey, byte[] IV)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = encryptionKey;
                aes.IV = IV;

                using (var inputFile = new FileStream(filePath, FileMode.Open))
                using (var cryptoStream = new CryptoStream(inputFile, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    HttpContext.Response.Buffer = false;
                    HttpContext.Response.Clear();
                    HttpContext.Response.ClearContent();
                    HttpContext.Response.ClearHeaders();
                    HttpContext.Response.ContentType = "application/octet-stream";
                    HttpContext.Response.AppendHeader("Content-Disposition", $"attachment; filename={fileName}");

                    cryptoStream.CopyTo(HttpContext.Response.OutputStream);

                    HttpContext.Response.End();
                }
            }

            return new FileStreamResult(Stream.Null, "application/octet-stream");
        }

        public ActionResult Delete(int id)
        {
            SharedFile sharedFile = db.SharedFiles.Find(id);
            if (sharedFile == null)
            {
                return Content("Invalid file");
            }

            if(!sharedFile.SenderUserId.Equals(User.Identity.GetUserId()) &&
               !sharedFile.ReceiverUserId.Equals(User.Identity.GetUserId()))
            {
                return Content("Forbidden action");
            }

            db.SharedFiles.Remove(sharedFile);
            db.SaveChanges();

            var filePath = sharedFile.SharedFilePath;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                ViewBag.Message = "File deleted successfully";
            }

            return RedirectToAction("SharedByMe");
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
