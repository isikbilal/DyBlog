using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DyBlog.Models;

namespace DyBlog.Controllers
{
    public class AdminKategoriController : Controller
    {
        private DyBlogDB db = new DyBlogDB();

        // GET: AdminKategori
        public ActionResult Index()
        {
            return View(db.Kategoris.ToList());
        }
        // GET: AdminKategori/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminKategori/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KategoriId,KategoriAdi")] Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                db.Kategoris.Add(kategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kategori);
        }

        // GET: AdminKategori/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = db.Kategoris.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // POST: AdminKategori/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KategoriId,KategoriAdi")] Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kategori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategori);
        }

        // GET: AdminKategori/Delete/5
        public ActionResult Delete(int id)
        {
            Kategori item = db.Kategoris.Where(x => x.KategoriId == id).FirstOrDefault();
            if (item != null)
            {
                if (item == null)
                {
                    return HttpNotFound();
                }
                

                foreach (var i in item.Makales.ToList())
                {
                    db.Makales.Remove(i);
                }
               
                db.Kategoris.Remove(item);
                db.SaveChanges();
                TempData["Message"] = Alert("Kategori silindi", true);
            }
            else
            {
                TempData["Message"] = Alert("Hata oluştu! Kategori silinemedi.", false);
            }
            return RedirectToAction("Index", "AdminKategori");


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
