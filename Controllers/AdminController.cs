using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLLaptop.Models;
using System.IO;

namespace QLLaptop.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        SanPhamDBDataContext data = new SanPhamDBDataContext();

        [HttpGet]
        public ActionResult Login_Admin()
        {
            return View();
        }
        public ActionResult Login_Admin(FormCollection collection)
        {
            var IDx = collection["ID"];
            var Passwordx = collection["Password"];
            ADMIN ad = data.ADMINs.SingleOrDefault(n => n.UserAdmin == IDx && n.PassAdmin == Passwordx);
            if (ad != null)
            {
                Session["TaikhoanAdmin"] = ad;
                return RedirectToAction("Index_Admin", "Admin");
            }
            else
                ViewBag.Thongbao = "ID or Password is incorrect";
            return View();
        }
        public ActionResult Index_Admin()
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            return View();
        }
        public ActionResult LabelAdmin()
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            return PartialView();
        }
        public ActionResult Logout()
        {
            Session["TaikhoanAdmin"] = null;
            return RedirectToAction("Login_Admin", "Admin");
        }
        public ActionResult Table_SP()
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            var sanpham = from sp in data.SANPHAMs select sp;
            return View(sanpham);
        }
        public ActionResult Details(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            var sanpham = from sp in data.SANPHAMs where sp.MaSP == id select sp;
            return View(sanpham.Single());
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            var sanpham = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            return View(sanpham);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult XoaSPDaChon(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            SANPHAM sanpham = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SANPHAMs.DeleteOnSubmit(sanpham);
            data.SubmitChanges();
            return RedirectToAction("Table_SP", "Admin");
        }
        [HttpGet]
        public ActionResult SuaSP(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            ViewBag.MaTH = new SelectList(data.THUONGHIEUs.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            var sanpham = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            return View(sanpham);
        }
        [HttpPost, ActionName("SuaSP")]
        public ActionResult SuaSP(SANPHAM sanpham, FormCollection collect)
        {
           
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            var id = int.Parse(collect["idid"]);
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            sp.TenSP = sanpham.TenSP;
            sp.Soluongton = sanpham.Soluongton;
            sp.Mota = sanpham.Mota;
            sp.MaTH = sanpham.MaTH;
            sp.Giaban = sanpham.Giaban;
            sp.Anhbia = sanpham.Anhbia;
            ViewBag.MaTH = new SelectList(data.THUONGHIEUs.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            UpdateModel(sp);
            data.SubmitChanges();
            return RedirectToAction("Table_SP");

        }



        public ActionResult Table_KH()
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            var khachhang = from kh in data.KHACHHANGs select kh;
            return View(khachhang);
        }
        [HttpGet]
        public ActionResult XoaKH(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            var khachhang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            return View(khachhang);
        }
        [HttpPost, ActionName("XoaKH")]
        public ActionResult XoaKHDaChon(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            KHACHHANG khachhang = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KHACHHANGs.DeleteOnSubmit(khachhang);
            data.SubmitChanges();
            return RedirectToAction("Table_KH", "Admin");
        }
        public ActionResult Table_TH()
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            var thuonghieu = from th in data.THUONGHIEUs select th;
            return View(thuonghieu);
        }
        [HttpGet]
        public ActionResult XoaTH(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            var thuonghieu = data.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            return View(thuonghieu);
        }
        [HttpPost, ActionName("XoaTH")]
        public ActionResult XoaTHDaChon(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            THUONGHIEU thuonghieu = data.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.THUONGHIEUs.DeleteOnSubmit(thuonghieu);
            data.SubmitChanges();
            return RedirectToAction("Table_TH", "Admin");
        }
        [HttpGet]
        public ActionResult ThemmoiSP()
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            ViewBag.MaTH = new SelectList(data.THUONGHIEUs.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            return View();
        }

        public void ThuongHieu(int? selected = null)
        {
            ViewBag.MaTH = new SelectList(data.THUONGHIEUs.ToList(), "MaTH", "TenTH", selected);
        }
        [HttpPost]
        public ActionResult ThemmoiSP(SANPHAM sanpham, HttpPostedFileBase nhinhanh)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            // ViewBag.MaCD = new SelectList(data.THUONGHIEUx.ToList().OrderBy(n => n.TenTH), "MaCD", "TenThuongHieu");
            if (nhinhanh == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn bảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(nhinhanh.FileName);
                    var path = Path.Combine(Server.MapPath("~/123/Hinh"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại!";
                    }
                    else
                    {
                        nhinhanh.SaveAs(path);
                    }
                    sanpham.Anhbia = fileName;
                    data.SANPHAMs.InsertOnSubmit(sanpham);
                    data.SubmitChanges();
                    return RedirectToAction("Table_SP");
                }
                ThuongHieu();
                return View();


            }
        }
        [HttpGet]
        public ActionResult ThemmoiTH()
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }
            return View();

        }
        [HttpPost]
        public ActionResult ThemmoiTH(THUONGHIEU thuonghieu, FormCollection fom)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login_Admin", "Admin");
            }

            var ntensp = fom["ntensp"];
            var nthuonghieu = fom["nthuonghieu"];

            THUONGHIEU test = data.THUONGHIEUs.SingleOrDefault(n => n.TenTH.Contains(ntensp));
            if (test != null)
            {
                ViewBag.Thongbao = "Thương Hiệu Đã Tồn Tại";

            }

            else
            {

                thuonghieu.TenTH = ntensp;

                data.THUONGHIEUs.InsertOnSubmit(thuonghieu);
                data.SubmitChanges();

                return RedirectToAction("Table_TH");
            }


            return View();

        }
        [HttpGet]
        public ActionResult SuaTH(int id)
        {
            var th = data.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            return View(th);
        }


        [HttpPost, ActionName("SuaTH")]
        public ActionResult SuaTP(int id, FormCollection collect)
        {
            var msth = id;
            var tenth = collect["ntenth"];
            var tenthuonghieu = collect["nthuonghieu"];


            THUONGHIEU th = data.THUONGHIEUs.SingleOrDefault(n => n.MaTH == msth);
            th.TenTH = tenth;

            th.MaTH = msth;
            UpdateModel(th);
            data.SubmitChanges();
            return RedirectToAction("Table_TH");



        }
    }
}