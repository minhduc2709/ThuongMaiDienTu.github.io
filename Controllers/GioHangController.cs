using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLLaptop.Models;

namespace QLLaptop.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/
        SanPhamDBDataContext data = new SanPhamDBDataContext();

        public List<GH>takeGH()
        {
            List<GH> lstGioHang = Session["GioHang"] as List<GH>;
            if(lstGioHang == null)
            {
                lstGioHang = new List<GH>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int iMaLT,string strURL)
        {
            List<GH> lstGioHang = takeGH();
            GH sanpham = lstGioHang.Find(n => n.iMaLT == iMaLT);
            if(sanpham == null)
            {
                sanpham = new GH(iMaLT);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        public int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GH> lstGioHang = Session["GioHang"] as List<GH>;
            if(lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<GH> lstGioHang = Session["GioHang"] as List<GH>;
            if(lstGioHang != null)
            {
                iTongTien = lstGioHang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        public ActionResult GioHang()
        {
            List<GH> lstGioHang = takeGH();
            if(lstGioHang.Count == 0)
            {
                //return RedirectToAction("Index", "MyWeb");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult XoaGioHang(int iMaSP)
        {
            List<GH> lstGioHang = takeGH();
            GH sanpham = lstGioHang.SingleOrDefault(n => n.iMaLT == iMaSP);
            if(sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iMaLT == iMaSP);
                return RedirectToAction("GioHang");
            }
            if(lstGioHang.Count == 0)
            {
                //return RedirectToAction("Index", "MyWeb");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult Update(int iMaSP,FormCollection f)
        {
            List<GH> lstGioHang = takeGH();
            GH sanpham = lstGioHang.SingleOrDefault(n => n.iMaLT == iMaSP);
            if(sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult DeleteAll()
        {
            List<GH> lstGioHang = takeGH();
            lstGioHang.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult Order()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "Sign");
            }
            if (TongSoLuong() == 0)
            {
                return RedirectToAction("GioHang");
            }
            List<GH> lstGioHang = takeGH();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult Order(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<GH> gh = takeGH();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = string.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach(var item in gh)
            {
                CHITIETDONHANG ctdh = new CHITIETDONHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaLT = item.iMaLT;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CHITIETDONHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            //Session["GioHang"] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");
        }
        public ActionResult Xacnhandonhang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "Sign");
            }
            if (TongSoLuong() == 0)
            {
                return RedirectToAction("GioHang");
            }
            List<GH> lstGioHang = takeGH();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            Session["GioHang"] = null;
            return View();

        }
    }
}