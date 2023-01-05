using ETicaret2023.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ETicaret2023.Controllers
{
    public class SepetController : Controller
    {
        ETicaretEntities db =new ETicaretEntities();
        // GET: Sepet
        public ActionResult Index()
        {
            string kulID = User.Identity.GetUserId();
            return View(db.Sepet.Where(x=>x.KullaniciID==kulID).ToList());
        }
          public ActionResult SepeteEkle(int UrunID,int adet)
        {
            string kulID=User.Identity.GetUserId();//login olan kullanıcın idsini almak için kullanılıyo bu Getuser login olan kulanıcını idsini getir

            Sepet sepettekiurun=db.Sepet.FirstOrDefault(x=>x.UrunID== UrunID && x.KullaniciID==kulID);//kayıt var mı yok mu bakcaz 

            Urunler urun=db.Urunler.Find(UrunID);

            if (sepettekiurun==null)
            {
                Sepet yeniurun=new Sepet()
                { 
                    KullaniciID=kulID,
                    UrunID= UrunID, 
                    Adet=adet,
                    ToplamTutar=urun.UrunFiyati*adet
                };
                db.Sepet.Add(yeniurun);
                
            }
            else
            {
                sepettekiurun.Adet=sepettekiurun.Adet+adet;
                sepettekiurun.ToplamTutar=sepettekiurun.Adet*urun.UrunFiyati;
                
            }
            db.SaveChanges();
            return RedirectToAction("Index");
           }

        public ActionResult SepetGuncelle(int? id, int adet)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Sepetin IDsi var mı kontrol et 
            Sepet sepet = db.Sepet.Find(id);
            //varsa o sepeti bul
            if (sepet == null)
            {
                return HttpNotFound();
            }
            //Sepetıdsi yoksa hata ver
            Urunler urun = db.Urunler.Find(sepet.UrunID);
            //yoksa ürünleri bul
            sepet.Adet = adet;

            sepet.ToplamTutar = sepet.Adet * urun.UrunFiyati;
            //sepetteki adeti ürün fiyatı ile çarp
            db.SaveChanges();
            //değişiklikleri kaydet


            return RedirectToAction("Index");
            //indexe yönlendir

        }

        public ActionResult Delete(int id)
        {
            Sepet sepet = db.Sepet.Find(id);//sepeti bul ,sil, değişiklikleri kaydet, indexe yönlendir
            db.Sepet.Remove(sepet);
            db.SaveChanges();
            return RedirectToAction("Index");   

        }


    }
}