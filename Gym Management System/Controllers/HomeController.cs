using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gym_Management_System.Models;

namespace Gym_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private GYMDBEntities4 db = new GYMDBEntities4();
        public ActionResult Index()
        {
            ShowData s = new ShowData()
            {
                vData = db.VisitorTBs.Count(),
                mData = db.MembershipTBs.Count(),
                memberData = db.MemberTBs.Count(),
                trainerData=db.TrainerTBs.Count(),
                tranData=db.TransactionTBs.Count(),
                totalAmount=db.TransactionTBs.Sum(x=>x.PaidAmount)
            };
            return View(s);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}