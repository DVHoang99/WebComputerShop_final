using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebComputerShop_final.Models;

namespace WebComputerShop_final.Controllers
{
    public class MainProcessController : Controller
    {
        // GET: SignInAndSignUp
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(ClassSignUp SignUp)
        {

            ComputerShopEntities data = new ComputerShopEntities();


            int a = data.Users.Where(x => x.UserName.Equals(SignUp.userName)).Count();
            if (a == 0)
            {
                User user = new User();

                user.UserName = SignUp.userName;
                user.PassWord = SignUp.passWord;
                user.Address = SignUp.address;
                user.Phone = SignUp.Phone;
                user.role = "Member";
                if (SignUp.rePassWord != SignUp.passWord)
                {
                    ViewBag.a = "Mật Khẩu và Nhập Lại Mật Khẩu Không Đúng";
                    return View();
                }
                else
                {
                    ViewBag.a = "Đăng kí thành công";
                    data.Users.Add(user);
                    data.SaveChanges();
                    return View();
                }
            }
            else
            {
                ViewBag.b = "Tên đăng nhập đã tồn tại";
                return View();
            }
        }
        [HttpPost]
        public ActionResult SignIn(ClassSignIn SignIn)
        {
            ComputerShopEntities data = new ComputerShopEntities();
            var b = data.Users.Where(x => x.UserName.Equals(SignIn.UserName) && x.PassWord.Equals(SignIn.PassWord)).FirstOrDefault();
            if (b != null)
            {
                if (b.role == "Member")
                {
                    Session["SignIn"] = b;
                    return RedirectToAction("Index", "MainProcess");
                }
                else
                {
                    Session["SignIn"] = b;
                    return RedirectToAction("IndexAdmin", "MainProcess");
                }
            }
            else
            {
                ViewBag.d = "sai ten tai khoan mat khau";
            }
            return View();

        }
        // GET: MainProcess
        public ActionResult Index()
        {
        
        ComputerShopEntities data = new ComputerShopEntities();

          var  a = data.InfoProducts;

         return View(a);

        }
        public ActionResult IndexAdmin()
        {
            if (Session["SignIn"] == null)
            {
                return RedirectToAction("SignUp", "MainProcess");
            }else
            {

                User kq = (User)Session["SignIn"];
                if (kq.role == "admin")
                {
                    ComputerShopEntities data = new ComputerShopEntities();

                    var a = from x in data.InfoProducts select x;

                    return View(a);
                }
                    
                else
                {
                    {
                        return RedirectToAction("ThongBao", new { tenaction = "Contact" });
                    }
                }
            }
        }

        public ActionResult Product(ClassProduct b)
        {

            if (Session["SignIn"] == null)

                return RedirectToAction("SignUp", "MainProcess");
            else
            {
                User kq = (User)Session["SignIn"];

                if (kq.role == "admin")
                {
                    ComputerShopEntities data = new ComputerShopEntities();

                    var a = from x in data.InfoProducts select x;
                    return View(a);
                }
                else
                {
                    return RedirectToAction("ThongBao", new { tenaction = "Contact" });
                }
            }

        }
        public ActionResult ThongBao(string tenaction)
        {
            ViewBag.tenaction = tenaction;
            return View();
        }
        public ActionResult AddProduct()
        {

            if (Session["SignIn"] == null)

                return RedirectToAction("SignUp", "MainProcess");
            else
            {
                 User b = (User)Session["SignIn"];

                if (b.role == "admin")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("ThongBao", new { tenaction = "Contact" });
                }
            }
        }
        [HttpPost]
        public ActionResult AddProduct(ClassProduct a, Image imageModel, HttpPostedFileBase ImageFile)
        {
            int c = 0;
            if(a.idProduct =="laptop")
            {
                c = 2;
            }else
            {
                c = 1;
            }
                ComputerShopEntities data = new ComputerShopEntities();
                InfoProduct b = new InfoProduct();
                b.Name = a.Name;
                b.Price = a.price;
                b.Description = a.description;
                b.idProductType = c;
                data.InfoProducts.Add(b);
                data.SaveChanges();
                imageModel.idProduct = b.Id;
                imageModel.Title = b.Name;
                string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                string extension = Path.GetExtension(ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                imageModel.ImagePath = fileName;
                fileName = Path.Combine(Server.MapPath("~/img/"), fileName);
                ImageFile.SaveAs(fileName);
                using (ComputerShopEntities db = new ComputerShopEntities())
                {
                    db.Images.Add(imageModel);
                    db.SaveChanges();
                ViewBag.a = "Thêm sản phẩm thành cống";
                }
                ModelState.Clear();
                return View();
        }

        public ActionResult AddToCart(string name, int amount, float TotalPrice)
        {
            try
            {
                if(Session["SignIn"] != null)
                {
                    User b = (User)Session["SignIn"];

                    if (b.role == "admin" || b.role == "Member")
                    {
                        ComputerShopEntities data = new ComputerShopEntities();
                        cart c = new cart();
                        c.Name = name;
                        c.Amount = amount;
                        c.TotalPrice = TotalPrice;
                        c.Name = b.UserName;
                        c.Phone = b.Phone;
                        c.Address = b.Address;
                        data.carts.Add(c);
                        data.SaveChanges();
                        return Json(1);
                    }
                    else
                    {
                        return RedirectToAction("ThongBao", new { tenaction = "Contact" });
                    }
                }
                else
                {
                    return Json(0);
                }
                

            } catch (Exception ex)
            {
                return Json(0);
            }

        }

        public ActionResult Edit(int id)
        {
            if (Session["SignIn"] == null)
            {
                return RedirectToAction("SignUp", "MainProcess");
            }    
            else
            {
                User b = (User)Session["SignIn"];

                if (b.role == "admin")
                {
                    ComputerShopEntities data = new ComputerShopEntities();
                    var ed = data.InfoProducts.Where(x => x.Id == id).FirstOrDefault();
                    return View(ed);
                }
                else
                {
                    return RedirectToAction("ThongBao", new { tenaction = "Contact" });
                }
            }
        }
        [HttpPost]
        public ActionResult Edit(ClassProduct a, Image imageModel, HttpPostedFileBase ImageFile)
        {
            ComputerShopEntities data = new ComputerShopEntities();
            var u = data.InfoProducts.Where(x => x.Id == a.id).FirstOrDefault();
            var y = data.Images.Where(k => k.idProduct == a.id).FirstOrDefault();
            if(u !=null)
            {
                if (y != null)
                {
                    data.Images.Remove(y);
                }
                
                data.InfoProducts.Remove(u);
                data.SaveChanges();
            }
            int c = 0;
            if (a.idProduct == "laptop")
            {
                c = 2;
            }
            else
            {
                c = 1;
            }

            InfoProduct b = new InfoProduct();
            b.Name = a.Name;
            b.Price = a.price;
            b.Description = a.description;
            b.idProductType = c;
            data.InfoProducts.Add(b);
            data.SaveChanges();
            imageModel.idProduct = b.Id;
            imageModel.Title = b.Name;
            string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
            string extension = Path.GetExtension(ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.ImagePath = fileName;
            fileName = Path.Combine(Server.MapPath("~/img/"), fileName);
            ImageFile.SaveAs(fileName);
            using (ComputerShopEntities db = new ComputerShopEntities())
            {
                db.Images.Add(imageModel);
                db.SaveChanges();
                ViewBag.a = "Sửa thành công";
            }
            
            ModelState.Clear();
            return View();
        }
    

        public ActionResult Delete(int id)
        {
            if (Session["SignIn"] == null)

                return RedirectToAction("SignUp", "MainProcess");
            else
            {
                User b = (User)Session["SignIn"];

                if (b.role == "admin")
                {
                    ComputerShopEntities data = new ComputerShopEntities();
                    var u = data.InfoProducts.Where(x => x.Id == id).FirstOrDefault();
                    var y = data.Images.Where(k => k.idProduct == id).FirstOrDefault();
                    if (u != null)
                    {
                        if (y != null)
                        {
                            data.Images.Remove(y);
                        }

                        data.InfoProducts.Remove(u);
                        data.SaveChanges();
                    }
                    return RedirectToAction ("IndexAdmin", "MainProcess");
                }
                else
                {
                    return RedirectToAction("ThongBao", new { tenaction = "Contact" });
                }
            }
        }

        public ActionResult Kiemtra()
        {
            if (Session["SignIn"] == null)
            {
                return RedirectToAction("SignUp", "MainProcess");
            }
            else
            {
                return RedirectToAction("Index", "MainProcess");
            }

            
        }
        public ActionResult LogInLogOut()
        {
            if (Session["SignIn"] == null)
            {
                return RedirectToAction("SignUp", "MainProcess");
            }
            else
            {
                Session["SignIn"] = null;
                return RedirectToAction("Index", "MainProcess");
            }


        }

    }
   
}