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
    public class AdminHakkimdaController : Controller
    {
        private DyBlogDB db = new DyBlogDB();

        // GET: AdminHakkimda
        public ActionResult Index()
        {
            return View(db.Hakkimdas.ToList());
        }

        // GET: AdminHakkimda/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hakkimda hakkimda = db.Hakkimdas.Find(id);
            if (hakkimda == null)
            {
                return HttpNotFound();
            }
            return View(hakkimda);
        }

        // GET: AdminHakkimda/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminHakkimda/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "hak_id,icerik")] Hakkimda hakkimda)
        {
            if (ModelState.IsValid)
            {
                db.Hakkimdas.Add(hakkimda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hakkimda);
        }

        // GET: AdminHakkimda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hakkimda hakkimda = db.Hakkimdas.Find(id);
            if (hakkimda == null)
            {
                return HttpNotFound();
            }
            return View(hakkimda);
        }

        // POST: AdminHakkimda/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "hak_id,icerik")] Hakkimda hakkimda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hakkimda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hakkimda);
        }

        // GET: AdminHakkimda/Delete/5
        public ActionResult Delete(int id)
        {
            Hakkimda item = db.Hakkimdas.Where(x => x.hak_id == id).FirstOrDefault();
            if (item != null)
            {
                if (item == null)
                {
                    return HttpNotFound();
                }
                db.Hakkimdas.Remove(item);
                db.SaveChanges();
                TempData["Message"] = Alert("Hakkımızdaki Bilgiler silindi", true);
            }
            else
            {
                TempData["Message"] = Alert("Hata oluştu! Hakkımızdaki Bilgiler silinemedi.", false);
            }
            return RedirectToAction("Index", "AdminHakkimda");


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
