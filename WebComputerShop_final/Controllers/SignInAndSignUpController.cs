using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebComputerShop_final.Models;

namespace WebComputerShop_final.Controllers
{
    public class SignInAndSignUpController : Controller
    {
        // GET: SignInAndSignUp
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(ClassSignUp SignUp, ClassSignIn SignIn)
        {
           
            ComputerShopEntities data = new ComputerShopEntities();
            

                int a = data.Users.Where(x => x.UserName.Equals(SignUp.userName)).Count();
                if (a == 0)
                {
                    User user = new User();

                    user.UserName = SignUp.userName;
                    user.PassWord = SignUp.passWord;
                    user.Address = SignUp.address;
                    user.Position = "Member";
                    user.Startus = 1;
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

        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(ClassSignIn a)
        {
            ComputerShopEntities data = new ComputerShopEntities();
            int b = data.Users.Where(x => x.UserName.Equals(a.UserName) && x.PassWord.Equals(a.PassWord)).Count();
            if(b == 1)
            {
                return RedirectToAction("Index", "MainProcess");
            }
            else
            {
                ViewBag.a = "Sai Ten Tai Khoan Mat Khau";
                return View();
            }
            
        }
    }

}