using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLLaptop.Models;

namespace QLLaptop.Controllers
{
    public class SignController : Controller
    {
        //
        // GET: /Sign/
        SanPhamDBDataContext data = new SanPhamDBDataContext();
        
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(FormCollection collection,KHACHHANG kh)
        {
            var Usernamex = collection["Username"];
            var IDx = collection["ID"];
            var Passwordx = collection["Password"];
            var Emailx = collection["Email"];
            var Addressx = collection["Address"];
            var Phonex = collection["Phone"];
            kh.HoTen = Usernamex;
            kh.Taikhoan = IDx;
            kh.Matkhau = Passwordx;
            kh.Email = Emailx;
            kh.DiachiKH = Addressx;
            kh.DienthoaiKH = Phonex;
            data.KHACHHANGs.InsertOnSubmit(kh);
            data.SubmitChanges();
            return RedirectToAction("Login");
            //return this.SignUp();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var IDx = collection["ID"];
            var Passwordx = collection["Password"];
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == IDx && n.Matkhau == Passwordx);
            if (kh != null)
            {
                Session["Taikhoan"] = kh;
                return RedirectToAction("Index", "MyWeb");
            }
            else
                ViewBag.Thongbao = "ID or Password is incorrect";
            return View();
        }
        public ActionResult TH()
        {
            var Thuonghieu = from th in data.THUONGHIEUs select th;
            return PartialView(Thuonghieu);
        }
        public ActionResult LabelUser()
        {
            return PartialView();
        }
        public ActionResult Logout()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Index", "MyWeb");
        }
    }
}