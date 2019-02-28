using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DyBlog.Models;
using System.Web.Helpers;

namespace DyBlog.Controllers
{
    public class AdminUyeController : Controller
    {
        private DyBlogDB db = new DyBlogDB();

        // GET: AdminUye
        public ActionResult Index()
        {
            var uyes = db.Uyes.Include(u => u.Yetki);
            return View(uyes.OrderByDescending(u=>u.UyeId).ToList());
        }

        // GET: AdminUye/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyes.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        // GET: AdminUye/Create
        public ActionResult Create()
        {
            ViewBag.YetkiId = new SelectList(db.Yetkis, "YetkiId", "Yetki1");
            return View();
        }

        // POST: AdminUye/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UyeId,KullaniciAdi,Email,Sifre,AdSoyad,YetkiId")] Uye uye,string Sifre)
        {
            var md5pass = Sifre;
            if (ModelState.IsValid)
            {
                uye.Sifre = Crypto.Hash(md5pass, "MD5");
                db.Uyes.Add(uye);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.YetkiId = new SelectList(db.Yetkis, "YetkiId", "Yetki1", uye.YetkiId);
            return View(uye);
        }

        // GET: AdminUye/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyes.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            ViewBag.YetkiId = new SelectList(db.Yetkis, "YetkiId", "Yetki1", uye.YetkiId);
            return View(uye);
        }

        // POST: AdminUye/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UyeId,KullaniciAdi,Email,Sifre,AdSoyad,YetkiId")] Uye uye, string Sifre)
        {
            if (ModelState.IsValid)
            {
                var md5pass = Sifre;
                uye.Sifre = Crypto.Hash(md5pass, "MD5");
                db.Entry(uye).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.YetkiId = new SelectList(db.Yetkis, "YetkiId", "Yetki1", uye.YetkiId);
            return View(uye);
        }

        // GET: AdminUye/Delete/5
        public ActionResult Delete(int id)
        {
            Uye item = db.Uyes.Where(x => x.UyeId == id).FirstOrDefault();
            if (item != null)
            {
                if (item == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(item.Foto)))
                {
                    System.IO.File.Delete(Server.MapPath(item.Foto));
                }
                foreach (var i in item.Yorums.ToList())
                {
                    db.Yorums.Remove(i);
                }
                foreach (var i in item.Makales.ToList())
                {
                    db.Makales.Remove(i);
                }
               
                db.Uyes.Remove(item);
                db.SaveChanges();
                TempData["Message"] = Alert("Üye silindi", true);
            }
            else
            {
                TempData["Message"] = Alert("Hata oluştu! Üye silinemedi.", false);
            }
            return RedirectToAction("Index", "AdminUye");


        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public string Alert(string message, bool? type = null)
        {
            string tip;
            switch (type)
            {
                case false: tip = "danger"; break;
                case true: tip = "success"; break;
                default:
                    tip = "info";
                    break;
            }
            string msg = "<div class='alert alert-" + tip + " alert-dismissible'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>×</button>" + message + "</div>";
            return msg;
        }
    }
}
