using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLLaptop.Models;

namespace QLLaptop.Models
{
    public class GH
    {
        SanPhamDBDataContext data = new SanPhamDBDataContext();
        public int iMaLT { set; get; }
        public string sTenLT { set; get; }
        public string sAnhbia { set; get; }
        public double dDongia { set; get; }
        public int iSoluong { set; get; }
        public double dThanhtien
        {
            get {return iSoluong*dDongia;}
        }
        public GH(int MaLT)
        {
            iMaLT = MaLT;
            SANPHAM Laptop = data.SANPHAMs.Single(n => n.MaSP == iMaLT);
            sTenLT = Laptop.TenSP;
            sAnhbia = Laptop.Anhbia;
            dDongia = double.Parse(Laptop.Giaban.ToString());
            iSoluong = 1;
        }
    }
}