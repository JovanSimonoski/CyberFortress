using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using CyberFortress.Models;
using Microsoft.AspNet.Identity;

namespace CyberFortress.Controllers
{
    [Authorize]
    public class StoredFilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const double maxTotalSize = 150.0;
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            if (db.StoredFiles.Where(m => m.UserId.Equals(currentUserId)).Count() > 0)
            {
                int totalSizeOfFiles = db.StoredFiles.Where(m => m.UserId.Equals(currentUserId)).Sum(m => m.StoredFileSize);
                ViewBag.TotalFilesSize = ((decimal)totalSizeOfFiles) / 1000;
            }
            else
            {
                ViewBag.TotalFilesSize = 0;
            }
            ViewBag.MaxTotalSize = maxTotalSize;
            string userId = User.Identity.GetUserId();
            return View(db.StoredFiles.Where(m => m.UserId.Equals(userId)).ToList());
        }

        public ActionResult Upload()
        {
            var currentUserId = User.Identity.GetUserId();
            double totalSizeOfFiles = 0;
            if (db.StoredFiles.Where(m => m.UserId.Equals(currentUserId)).Count() > 0)
            {
                totalSizeOfFiles = db.StoredFiles.Where(m => m.UserId.Equals(currentUserId)).Sum(m => m.StoredFileSize);
            }
            ViewBag.TotalFilesSize = ((decimal)totalSizeOfFiles) / 1000;
            ViewBag.MaxTotalSize = maxTotalSize;
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var currentUserId = User.Identity.GetUserId();
            double totalSizeOfFiles = 0;
            if (db.StoredFiles.Where(m => m.UserId.Equals(currentUserId)).Count() > 0)
            {
                totalSizeOfFiles = db.StoredFiles.Where(m => m.UserId.Equals(currentUserId)).Sum(m => m.StoredFileSize);
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
                var userId = User.Identity.GetUserId();
                var path = Path.Combine(Server.MapPath("~/Safe/" + userId), fileName);
                var fileSize = file.ContentLength;
                byte[] encryptionKey;
                byte[] IV;

                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                using (var aesAlg = Aes.Create())
                {
                    encryptionKey = aesAlg.Key;
                    IV = aesAlg.IV;
                }

                EncryptAndSaveFile(file.InputStream, path, encryptionKey, IV);

                var storedFile = new StoredFile
                {
                    StoredFileName = fileName,
                    StoredFilePath = path,
                    UserId = userId,
                    StoredFileSize = fileSize,
                    StoredFileEncryptionKey = encryptionKey,
                    StoredFileIV = IV
                };

                db.StoredFiles.Add(storedFile);
                db.SaveChanges();

                ViewBag.Message = "File uploaded and encrypted successfully";
            }
            else
            {
                ViewBag.Message = "No file uploaded";
            }
            ViewBag.TotalFilesSize = totalSizeOfFiles;
            ViewBag.MaxTotalSize = maxTotalSize;
            return View("Upload");
        }

        private void EncryptAndSaveFile(Stream inputStream, string outputPath, byte[] encryptionKey, byte[] IV)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = encryptionKey;
                aesAlg.IV = IV;

                using (var outputFile = new FileStream(outputPath, FileMode.Create))
                using (var cryptoStream = new CryptoStream(outputFile, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    inputStream.CopyTo(cryptoStream);
                }
            }
        }

        public ActionResult Download(int? id)
        {
            var userId = User.Identity.GetUserId();
            StoredFile file = db.StoredFiles.Where(m => m.Id == id).FirstOrDefault();

            if(file == null)
            {
                return Content("Invalid file.");
            }

            if(userId != file.UserId)
            {
                return Content("Forbidden operation.");
            }

            var fileName = file.StoredFileName;
            var filePath = Path.Combine(Server.MapPath("~/Safe/" + userId), fileName);

            byte[] encryptionKey = file.StoredFileEncryptionKey;
            byte[] IV = file.StoredFileIV;

            return DecryptAndDownloadFile(filePath, fileName, encryptionKey, IV);
        }
        private FileStreamResult DecryptAndDownloadFile(string filePath, string fileName, byte[] encryptionKey, byte[] IV)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = encryptionKey;
                aesAlg.IV = IV;

                using (var inputFile = new FileStream(filePath, FileMode.Open))
                using (var cryptoStream = new CryptoStream(inputFile, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
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
            StoredFile storedFile = db.StoredFiles.Find(id);
            if(storedFile == null)
            {
                return Content("Invalid file.");
            }

            db.StoredFiles.Remove(storedFile);
            db.SaveChanges();

            var filePath = storedFile.StoredFilePath;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                ViewBag.Message = "File deleted successfully";
            }

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
