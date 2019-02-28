using DyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DyBlog.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        DyBlogDB db = new DyBlogDB();
        public ActionResult Index()
        {
            ViewBag.makaleSayisi = db.Makales.Count();
            ViewBag.yorumSayisi = db.Yorums.Count();
            ViewBag.kategoriSayisi = db.Kategoris.Count();
            ViewBag.uyeSayisi = db.Uyes.Count();
            ViewBag.mesajSayisi = db.GeriBildirims.ToList().Count();
            return View();
        }
        public ActionResult GelenTumMesaj()
        {
            return View(db.GeriBildirims.ToList());
        }
        public ActionResult GelenMesajList(int id)
        {
            return View(db.GeriBildirims.Where(g=>g.id==id).ToList());
        }
        public ActionResult GelenDelete(int id)
        {
            GeriBildirim item = db.GeriBildirims.Where(x => x.id == id).FirstOrDefault();
            if (item != null)
            {
                if (item == null)
                {
                    return HttpNotFound();
                }
                db.GeriBildirims.Remove(item);
                db.SaveChanges();
                TempData["Message"] = Alert("Gelen Mesaj Bilgileriniz silindi", true);
            }
            else
            {
                TempData["Message"] = Alert("Hata oluştu! Gelen Mesaj Bilgileri silinemedi.", false);
            }
            return RedirectToAction("Index");


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