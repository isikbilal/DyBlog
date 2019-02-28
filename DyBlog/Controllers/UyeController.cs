using DyBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace DyBlog.Controllers
{
    public class UyeController : Controller
    {
        // GET: Uye
        DyBlogDB db = new DyBlogDB();
        public ActionResult Index(int id)
        {
     
            var uye = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["uyeid"])!=uye.UyeId)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        public ActionResult Login()
        {
        
            return View();
        }
        [HttpPost]
        public ActionResult Login(Uye uye,string Sifre)
        {

            var md5pass = Crypto.Hash(Sifre, "MD5");
                
            var login = db.Uyes.Where(u => u.KullaniciAdi == uye.KullaniciAdi).SingleOrDefault();
            if (login.KullaniciAdi==uye.KullaniciAdi && login.Sifre==md5pass)
            {
               
                Session["AdSoyad"] = login.AdSoyad;
                Session["kullaniciAdi"] = login.KullaniciAdi;
                Session["uyeid"] = login.UyeId;
                Session["YetkiId"] = login.YetkiId;
                return RedirectToAction("Index", "Home");
            }
            TempData["Message"] = Alert("Hatalı giriş yaptınız. Lütfen Kullanıcı Adı veya Şifrenizi Kontrol Ediniz!", false);
            return View();
        }
       
        public ActionResult Logout()
        {
            
            Session["uyeid"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Uye uye,string Sifre,HttpPostedFileBase Foto)
        {
            var md5pass = Sifre;
            if (ModelState.IsValid)
            {
                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UyeFoto/" + newfoto);
                    uye.Foto = "/Uploads/UyeFoto/" + newfoto;
                    uye.YetkiId = 2;
                    uye.Sifre = Crypto.Hash(md5pass,"MD5");
                    db.Uyes.Add(uye);
                    db.SaveChanges();
                    Session["uyeid"] = uye.UyeId;
                    Session["kullaniciAdi"] = uye.KullaniciAdi;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    uye.Foto = "/Uploads/UyeFoto/user1.jpg";
                    uye.YetkiId = 2;
                    uye.Sifre = Crypto.Hash(md5pass, "MD5");
                    db.Uyes.Add(uye);
                    db.SaveChanges();
                    Session["uyeid"] = uye.UyeId;
                    Session["kullaniciAdi"] = uye.KullaniciAdi;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(uye);
        }

        public ActionResult Edit(int id)
        {
            var uye = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();
            if (Convert.ToInt32(Session["uyeid"]) != uye.UyeId)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        [HttpPost]
        public ActionResult Edit(Uye uye,string Sifre,int id,HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                var md5pass = Sifre;

                var uyes = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();
                if (Foto!=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(uyes.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(uyes.Foto));
                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/UyeFoto/" + newfoto);
                    uyes.Foto = "/Uploads/UyeFoto/" + newfoto;
                }
                uyes.AdSoyad = uye.AdSoyad;
                uyes.Email = uye.Email;
                uyes.KullaniciAdi = uye.KullaniciAdi;
                uyes.Sifre =Crypto.Hash(md5pass, "MD5");
                db.SaveChanges();
                Session["kullaniciAdi"] = uye.KullaniciAdi;
                return RedirectToAction("Index","Home",new { id=uyes.UyeId});
            }
            return View();
        }

        public ActionResult UyeProfil(int id)
        {
            var uye = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();
            return View(uye);
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