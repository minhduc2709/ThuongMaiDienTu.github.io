using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLLaptop.Models;

using PagedList;
using PagedList.Mvc;
using System.Net.Mail;
namespace QLLaptop.Controllers
{
    public class MyWebController : Controller
    {
        //
        // GET: /MyWeb/
        SanPhamDBDataContext data = new SanPhamDBDataContext();

        private List<SANPHAM> take(int count)
        {
            return data.SANPHAMs.Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);
            var Sanpham = take(4);//from sp in data.SANPHAMs select sp;
            return View(Sanpham.ToPagedList(pageNum,pageSize));
        }
        public ActionResult TH()
        {
            var Thuonghieu = from th in data.THUONGHIEUs select th;
            return PartialView(Thuonghieu);
        }
        public ActionResult SPtheoTH(int id,int ? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);
            var Sanpham = from sp in data.SANPHAMs where sp.MaTH == id select sp;
            return View(Sanpham.ToPagedList(pageNum, pageSize));
        }
        public ActionResult CTSP(int id)
        {
            var Sanpham = from sp in data.SANPHAMs where sp.MaSP == id select sp;
            return View(Sanpham.Single());
        }
        public ActionResult THSP(int id)
        {
            var Thuonghieu = from th in data.THUONGHIEUs where th.MaTH == id select th;
            return PartialView(Thuonghieu);
        }
        public ActionResult Map()
        {
            return View();
        }
        public ActionResult SearchSP(string keywords, int? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);
            var Sanpham = from sp in data.SANPHAMs where  sp.TenSP.Contains(keywords) select sp;
            return View(Sanpham.ToPagedList(pageNum, pageSize));
        }


        [HttpGet]
        public ActionResult SendMail()
        {
            return PartialView();
        }
        [HttpPost]

        public ActionResult SendMail(FormCollection collect)
        {
            var HoTen = collect["Name"];
            var EmailLayVe = collect["Email"];
            var TieuDe = collect["Subject"];
            var NoiDung = collect["Message"];
            var Email = new SmtpClient("smtp.gmail.com", 25)
            {
                Credentials = new System.Net.NetworkCredential("dominhtri1996ntl@gmail.com", "cbkgndcszq123456"),
                EnableSsl = true
            };
            var ThongDiep = new MailMessage();
            ThongDiep.From = new MailAddress(EmailLayVe, "Gửi Từ: " + HoTen + "Từ Mail: " + EmailLayVe, System.Text.Encoding.UTF8);
            ThongDiep.To.Add("dominhtri1996_ntl@yahoo.com");
            ThongDiep.Subject = TieuDe;
            ThongDiep.Body = NoiDung;
            Email.Send(ThongDiep);
            ViewData["ThanhCong"] = "Send mail thành công";
            return RedirectToAction("Map");
        }
    }
}