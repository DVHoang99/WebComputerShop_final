using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebComputerShop_final.Models;

namespace WebComputerShop_final.Controllers
{
    public class MainProcessController : Controller
    {
        // GET: MainProcess
        public ActionResult Index()
        {
            ComputerShopEntities data = new ComputerShopEntities();

            var a = from x in data.InfoProducts select x;
            return View(a);
        }

        public ActionResult Product(ClassProduct b)
        {
            ComputerShopEntities data = new ComputerShopEntities();

            var a = from x in data.InfoProducts select x;
            return View(a);
        }

        [HttpPost]
        public JsonResult ThemSPJson(ClassProduct a, HttpPostedFileBase fileImg)
        {
            try
            {
                ComputerShopEntities data = new ComputerShopEntities();
                InfoProduct b = new InfoProduct();

                b.Name = a.Name;
                b.Price = a.price;
                b.Description = a.description;
                data.InfoProducts.Add(b);
                //string path = System.IO.Path.Combine(Server.MapPath("~/img/"), fileImg.FileName);
                //fileImg.SaveAs(path);
                //IMG img = new IMG();
                //img.IMGPath = "/Img/" + fileImg.FileName;
                //img.idProduct = b.Id;
                //data.IMGs.Add(img);
                data.SaveChanges();
                return Json(1);
            }
            catch (Exception ex)
            { return Json(0); }

        }

        public ActionResult Delete(ClassProduct a)
        {
            ComputerShopEntities data = new ComputerShopEntities();

            var kq2 = data.InfoProducts.Where(x =>  x.Id.Equals(a.id)).FirstOrDefault();
            var kq1 = data.IMGs.Where(m => m.idProduct == a.id).FirstOrDefault();
            if (kq2 != null)
            {       
                data.IMGs.Remove(kq1);
                data.InfoProducts.Remove(kq2);
            }
            return RedirectToAction("index", "MainProcess");
        }


    }
}