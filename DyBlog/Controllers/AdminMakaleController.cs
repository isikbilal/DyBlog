using DyBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace DyBlog.Controllers
{
    public class AdminMakaleController : Controller
    {
        DyBlogDB db = new DyBlogDB();
        // GET: AdminMakale
        public ActionResult Index(int page = 1)
        {
            var makales = db.Makales.OrderByDescending(m => m.MakaleId).ToPagedList(page, 5);
            return View(makales);
        }

        // GET: AdminMakale/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi");
            return View();
        }

        // POST: AdminMakale/Create
        [HttpPost]
        public ActionResult Create(Makale makale,string etiketler,HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/MakaleFoto/" + newfoto);
                    makale.Foto = "/Uploads/MakaleFoto/" + newfoto;
                }
                else
                {
                    makale.Foto = "/Uploads/MakaleFoto/default.jpg";
                }
                if (etiketler != null)
                {
                    string[] etiketDizi = etiketler.Split(',');
                    foreach (var i in etiketDizi)
                    {
                        var yeniEtiket = new Etiket { EtiketAdi = i };
                        db.Etikets.Add(yeniEtiket);
                        makale.Etikets.Add(yeniEtiket);
                    }
                }
                makale.UyeId = Convert.ToInt32(Session["uyeid"]);
                makale.Okuma = 1;
                makale.Tarih=DateTime.Now;
                db.Makales.Add(makale);
                db.SaveChanges();

                return RedirectToAction("Index");


            }

            return View(makale);

        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int id)
        {

            var makale = db.Makales.Where(x => x.MakaleId == id).SingleOrDefault();
            if (makale==null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi",makale.KategoriId);
            return View(makale);
        }

        // POST: AdminMakale/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Makale makale, HttpPostedFileBase Foto)
        {
            try
            {
                var makales = db.Makales.Where(x => x.MakaleId == id).SingleOrDefault();
                if (Foto!=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(makales.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(makales.Foto));
                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/MakaleFoto/" + newfoto);
                    makales.Foto = "/Uploads/MakaleFoto/" + newfoto;
                
                }
                makales.Baslik = makale.Baslik;
                makales.Icerik = makale.Icerik;
                makales.KategoriId = makale.KategoriId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.KategoriId = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi", makale.KategoriId);
                return View(makale);
            }
        }
     
        // POST: AdminMakale/Delete/5
        public ActionResult Delete(int id)
        {
            Makale item = db.Makales.Where(x => x.MakaleId == id).FirstOrDefault();
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
                foreach (var i in item.Etikets.ToList())
                {
                    db.Etikets.Remove(i);
                }
                db.Makales.Remove(item);
                db.SaveChanges();
                TempData["Message"] = Alert("Makale silindi", true);
            }
            else
            {
                TempData["Message"] = Alert("Hata oluştu! Makale silinemedi.", false);
            }
            return RedirectToAction("Index","AdminMakale");
            
            
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
