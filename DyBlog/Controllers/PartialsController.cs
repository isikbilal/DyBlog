using DyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DyBlog.Controllers
{
    public class PartialsController : Controller
    {
        DyBlogDB db = new DyBlogDB();
        // GET: Partials
        public ActionResult Kategori()
        {
            return View(db.Kategoris.ToList());
        }
        public ActionResult PopulerMakaleler()
        {

            return View(db.Makales.OrderByDescending(m => m.Okuma).Take(3));
        }
        public ActionResult SonGonderi()
        {
            return View(db.Makales.OrderByDescending(m => m.MakaleId).ToList().Take(3));
        }
        public ActionResult KategoriList()
        {
            var kat = db.Kategoris.ToList();
            return View(kat);
        }
        public ActionResult SosyalMedya()
        {
            var sos = db.SosyalMedyas.ToList();
            return View(sos);
        }
        public ActionResult Hakkimda()
        {
            var hak = db.Hakkimdas.ToList();
            return View(hak);
        }
        public ActionResult GelenMesaj()
        {
            return View(db.GeriBildirims.OrderByDescending(m => m.id).Take(3));
        }
        public ActionResult MesajBulten()
        {
            return View();
        }
    }
}