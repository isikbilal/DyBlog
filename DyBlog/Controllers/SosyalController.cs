using DyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DyBlog.Controllers
{
    public class SosyalController : Controller
    {
        // GET: Sosyal
        DyBlogDB db = new DyBlogDB();
        public ActionResult CreateSocial()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "Facebook", Value = "facebook"},
                new SelectListItem{Text = "Twitter", Value = "twitter"},
                new SelectListItem{Text = "Linkedin", Value = "linkedin"},
                new SelectListItem{Text = "Google+", Value = "gplus"},
                new SelectListItem{Text = "Pinterest", Value = "pinterest"}

            };
            ViewBag.Socials = new SelectList(items, "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult CreateSocial(SosyalMedya newitem)
        {
            db.SosyalMedyas.Add(newitem);
            db.SaveChanges();
            return RedirectToAction("Socials", "Sosyal");
        }

        public ActionResult EditSocial(int id)
        {
            SosyalMedya item = db.SosyalMedyas.Where(x => x.sosId == id).FirstOrDefault();
            if (item == null)
            {
                return RedirectToAction("Socials", "Sosyal");
            }
            return View(item);
        }

        public ActionResult UpdateSocial(SosyalMedya newitem)
        {
            SosyalMedya item = db.SosyalMedyas.Where(x => x.sosId == newitem.sosId).FirstOrDefault();
            if (item != null)
            {
                item.Adi = newitem.Adi;
                item.UrlAdres = newitem.UrlAdres;
                db.SaveChanges();
                TempData["Message"] = Alert("Sosyal hesap güncellendi.", true);
            }
            else
            {
                TempData["Message"] = Alert("Hata oluştu! Sosya hesap güncellenemedi.", false);
            }
            return RedirectToAction("Socials", "Sosyal");
        }

        public ActionResult DeleteSocial(int id)
        {
            SosyalMedya item = db.SosyalMedyas.Where(x => x.sosId == id).FirstOrDefault();
            if (item != null)
            {
                db.SosyalMedyas.Remove(item);
                db.SaveChanges();
                TempData["Message"] = Alert("Sosyal hesap silindi", true);
            }
            else
            {
                TempData["Message"] = Alert("Hata oluştu! Sosyal hesap silinemedi.", false);
            }
            return RedirectToAction("Socials", "Sosyal");
        }

        public ActionResult Socials()
        {
            List<SosyalMedya> socials = db.SosyalMedyas.ToList();
            return View(socials);
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