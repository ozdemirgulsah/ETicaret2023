using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using WebApiProje.Models;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace WebApiProje.Controllers
{
    public class KategoriController : ApiController
    {

        E_TicaretEntities db= new E_TicaretEntities();
        //get listeleme //host  //put update metodu //delete sil metodu

        [HttpGet]
        public List<Kategoriler> Get()
        {
         
          

            db.Configuration.ProxyCreationEnabled=false;
            List<Kategoriler> liste=db.Kategoriler.ToList();
            return liste;

        }
            [HttpGet]
        public IHttpActionResult Get(int id) //Cevap dönsün diye bu metodu kullanıyoruz
            {
            Kategoriler kategori= db.Kategoriler.Find(id);
            Kategori kat=new Kategori()
            {
                Id=kategori.KategoriID,
                name=kategori.KategoriAdi
            };
          
            return Ok(kat);
            
            }
        [System.Web.Http.HttpPost]
        public IHttpActionResult Post([FromBody]Kategoriler kategori)
            {
            db.Kategoriler.Add(kategori);
            db.SaveChanges();
            return Ok();

          
                
            }
       

        
    }
}
