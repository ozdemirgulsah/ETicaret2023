﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ETicaret2023.Models;
using Newtonsoft.Json;

namespace ETicaret2023.Controllers
{
    public class UrunlerController : Controller
    {
        private ETicaretEntities db = new ETicaretEntities();
        HttpClient client=new HttpClient();

        // GET: Urunler
        public ActionResult Index()
        {

            List<Urunler> urunler = null;
            client.BaseAddress = new Uri("https://localhost:44337/api/");
            var response = client.GetAsync("Urun");

            response.Wait();

            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync();
                data.Wait();
               urunler = JsonConvert.DeserializeObject<List<Urunler>>(data.Result);

            }
            for (int i = 0; i < urunler.Count; i++)
            {
                urunler[i].Kategoriler = db.Kategoriler.Find(urunler[i].KategoriID);
            }
            return View(urunler);
            //    var urunler = db.Urunler.Include(u => u.Kategoriler);
            //    return View(urunler.ToList());
            //
        }

        // GET: Urunler/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = UrunBul(id);

            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        private Urunler UrunBul(int? id)
        {
            Urunler urunler = null;
            client.BaseAddress = new Uri("https://localhost:44337/api/");
            var response = client.GetAsync("Urun/" + id.ToString());
            response.Wait();
            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsAsync<Urunler>();
                data.Wait();
                urunler = data.Result;
            }

            return urunler;
        }

        // GET: Urunler/Create
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi");
            ViewBag.UrunID = new SelectList(db.SiparisDetay, "SiparisDetayID", "SiparisDetayID");
            return View();
        }

        // POST: Urunler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Urunler urunler, HttpPostedFileBase urunResim)
        {
            if (ModelState.IsValid)
            {
                client.BaseAddress = new Uri("https://localhost:44337/api/");
                var response = HttpClientExtensions.PostAsJsonAsync<Urunler>(client, "Urun", urunler);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsAsync<Urunler>();
                    data.Wait();
                    Urunler urun = data.Result;
                    if (urunResim != null)
                    {
                        string dosya = Path.Combine(Server.MapPath("~/image/"), urunler.UrunID + ".jpg");
                        urunResim.SaveAs(dosya);
                    }
                    return RedirectToAction("Index");
                }
                return View(urunler);
            }
            return View(urunler);
        }

                // GET: Urunler/Edit/5
                public ActionResult Edit(int? id)
                {
                   if (id == null)
                   {
                          return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                   }
                     Urunler urunler =UrunBul(id);
                    if (urunler == null)
                    {
                         return HttpNotFound();
                    }
                      ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi", urunler.KategoriID);
                      return View(urunler);
                }






        // POST: Urunler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
             public ActionResult Edit(Urunler urunler, HttpPostedFileBase urunResim)
             {

            //if (ModelState.IsValid)
            //{
            //    client.BaseAddress = new Uri("https://localhost:44337/api/");
            //    var response = client.PutAsJsonAsync<Urunler>("Urunler", urunler);
            //    response.Wait();

            //    var result = response.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        return RedirectToAction("Index");
            //    }
            //    //db.Entry(urunler).State = EntityState.Modified;
            //    //db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(urunler);

                 if (ModelState.IsValid)
                 {
                   db.Entry(urunler).State = EntityState.Modified;
                   db.SaveChanges();
                    if (urunResim != null)
                    {
                    string dosya = Path.Combine(Server.MapPath("~/image/"),
                        urunler.UrunID + ".jpeg");
                    urunResim.SaveAs(dosya);
                    }
                            return RedirectToAction("Index");
                 }
                      ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi", urunler.KategoriID);
                      return View(urunler);

             }



        // GET: Urunler/Delete/5
        public ActionResult Delete(int? id)
        {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Urunler urunler = UrunBul(id);
                if (urunler == null)
                {
                    return HttpNotFound();
                }
                return View(urunler);
        }

            // POST: Urunler/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                Urunler urunler = UrunBul(id);
                db.Urunler.Remove(urunler);
                db.SaveChanges();
                string dosya = Path.Combine(Server.MapPath("~/image/"),urunler.UrunID+ ".jpg");
                FileInfo fi = new FileInfo(dosya);
                if (fi.Exists)
                {
                    fi.Delete();
                }
                return RedirectToAction("Index");
            }
     }

}