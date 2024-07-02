using Gym_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Gym_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        private SaveData objSaveData;
        private GYMDBEntities4 db = new GYMDBEntities4();
        public DashboardController()
        {
            objSaveData = new SaveData();
        }
        // GET: Dashboad
        public ActionResult Dues()
        {
            var transactions = db.TransactionTBs
    .Where(t => t.DueAmount != null && t.DueAmount != 0)
    .Include(t => t.MemberTB)
    .ToList();
            var memberModel = db.MemberTBs.ToList();
            ViewBag.Transactions = transactions;
            ViewBag.Members = memberModel;
            return View();
        }
        public ActionResult Payment()
        {
            IEnumerable<MembershipTB> mDta = db.MembershipTBs.ToList();
            var data = mDta.ToList();
            IEnumerable<TrainerTB> tData = db.TrainerTBs.ToList();
            var tD = tData.ToList();
            ViewBag.trainerSelect = new SelectList(tD, "Name", "Name");
            ViewBag.MembershipPlans = new SelectList(data, "MembershipPlan", "MembershipPlan");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Create(CombineView combineView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objSaveData.insertData(combineView);
                    TempData["msg"] = "Records Add Successfuly.";
                    return RedirectToAction("Index", "MemberTBs");
                }
                else
                {
                    IEnumerable<MembershipTB> mDta = db.MembershipTBs.ToList();
                    var data = mDta.ToList();
                    IEnumerable<TrainerTB> tData = db.TrainerTBs.ToList();
                    var tD = tData.ToList();
                    ViewBag.trainerSelect = new SelectList(tD, "Name", "Name");
                    ViewBag.MembershipPlans = new SelectList(data, "MembershipPlan", "MembershipPlan");
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Records Add Failed." + ex.Message;
                return RedirectToAction("Payment");
            }
        }
        public ActionResult Transaction(string payment)
        {
            if (payment == "Cash")
            {
                var transactions = db.TransactionTBs
                     .Include(t => t.MemberTB)
                     .Where(t => t.PaymentMode == "Cash")
                     .ToList();

                var memberModel = db.MemberTBs.ToList();

                ViewBag.Transactions = transactions;
                ViewBag.Members = memberModel;

                return View();
            }
            else if (payment == "UPI")
            {
                var transactions = db.TransactionTBs
                     .Include(t => t.MemberTB)
                     .Where(t => t.PaymentMode == "UPI")
                     .ToList();

                var memberModel = db.MemberTBs.ToList();

                ViewBag.Transactions = transactions;
                ViewBag.Members = memberModel;

                return View();
            }
            else if (payment == "Google Pay")
            {
                var transactions = db.TransactionTBs
                     .Include(t => t.MemberTB)
                     .Where(t => t.PaymentMode == "Google Pay")
                     .ToList();

                var memberModel = db.MemberTBs.ToList();

                ViewBag.Transactions = transactions;
                ViewBag.Members = memberModel;

                return View();
            }
            else if (payment == "Phone Pay")
            {
                var transactions = db.TransactionTBs
                     .Include(t => t.MemberTB)
                     .Where(t => t.PaymentMode == "Phone Pay")
                     .ToList();

                var memberModel = db.MemberTBs.ToList();

                ViewBag.Transactions = transactions;
                ViewBag.Members = memberModel;

                return View();
            }
            else
            {
                var transactions = db.TransactionTBs
            .Include(t => t.MemberTB).ToList();
                var memberModel = db.MemberTBs.ToList();
                ViewBag.Transactions = transactions;
                ViewBag.Members = memberModel;
                return View();
            }
        }
        public ActionResult UpdatePayment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionTB transaction = db.TransactionTBs.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }
        [HttpPost]
        public ActionResult UpdateData(int id, int remAmount, int paidAmounts, int dueAmount, DateTime paydate, string mode)
        {
            var existingTransactionTB = db.TransactionTBs.Find(id);
            if (existingTransactionTB == null)
            {
                return HttpNotFound();
            }
            existingTransactionTB.PaidAmount = Convert.ToInt32(paidAmounts + dueAmount);
            existingTransactionTB.DueAmount = Convert.ToInt32(remAmount - dueAmount);
            existingTransactionTB.PaymentDate = paydate;
            existingTransactionTB.PaymentMode = mode;
            db.Entry(existingTransactionTB).Property(x => x.PaidAmount).IsModified = true;
            db.Entry(existingTransactionTB).Property(x => x.DueAmount).IsModified = true;
            db.Entry(existingTransactionTB).Property(x => x.PaymentDate).IsModified = true;
            db.Entry(existingTransactionTB).Property(x => x.PaymentMode).IsModified = true;
            db.SaveChanges();
            return RedirectToAction("Dues");
        }
        public ActionResult UpdateMember(int? id, int? mid)
        {
            IEnumerable<MembershipTB> mDta = db.MembershipTBs.ToList();
            var data = mDta.ToList();
            IEnumerable<TrainerTB> tData = db.TrainerTBs.ToList();
            var tD = tData.ToList();
            ViewBag.trainerSelect = new SelectList(tD, "Name", "Name");
            ViewBag.MembershipPlans = new SelectList(data, "MembershipPlan", "MembershipPlan");
            var combineData = new CombineView
            {
                member = db.MemberTBs.Find(id),
                transaction = db.TransactionTBs.Find(mid)
            };
            return View(combineData);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult UpdateMemberData(CombineView combineView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objSaveData.UpdateData(combineView);
                    TempData["msg"] = "Records Add Successfuly.";
                    return RedirectToAction("Index", "MemberTBs");
                }
                else
                {
                    IEnumerable<MembershipTB> mDta = db.MembershipTBs.ToList();
                    var data = mDta.ToList();
                    IEnumerable<TrainerTB> tData = db.TrainerTBs.ToList();
                    var tD = tData.ToList();
                    ViewBag.trainerSelect = new SelectList(tD, "Name", "Name");
                    ViewBag.MembershipPlans = new SelectList(data, "MembershipPlan", "MembershipPlan");
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Records Add Failed." + ex.Message;
                return RedirectToAction("Payment");
            }
        }
        public ActionResult DeleteMember(int? id, int? mid)
        {
            if (id == null && mid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var combineData = new CombineView
            {
                member = db.MemberTBs.Find(id),
                transaction = db.TransactionTBs.Find(mid)
            };
            if (combineData == null)
            {
                return HttpNotFound();
            }
            return View(combineData);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult DeleteConfirmed(int id, int mid)
        {
            MemberTB memberTB = db.MemberTBs.Find(id);
            TransactionTB transactionTB = db.TransactionTBs.Find(mid);

            db.TransactionTBs.Remove(transactionTB);
            db.MemberTBs.Remove(memberTB);

            db.SaveChanges();
            return RedirectToAction("Index", "MemberTBs");
        }

        public ActionResult ConvertVisitor(int? id)
        {
            CombineView model = new CombineView();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitorTB visitor = db.VisitorTBs.Find(id);
            if (visitor == null)
            {
                return HttpNotFound();
            }
            model.member = new MemberTB();
            model.member.Name = visitor.name;
            model.member.Phone = visitor.phone;
            model.member.Email = visitor.email;
            model.member.DOB = visitor.DOB;
            model.member.Address = visitor.address;
            model.member.Gender = visitor.gender;
            model.member.ID = visitor.ID;
            IEnumerable<MembershipTB> mDta = db.MembershipTBs.ToList();
            var data = mDta.ToList();
            IEnumerable<TrainerTB> tData = db.TrainerTBs.ToList();
            var tD = tData.ToList();
            ViewBag.trainerSelect = new SelectList(tD, "Name", "Name");
            ViewBag.MembershipPlans = new SelectList(data, "MembershipPlan", "MembershipPlan");
            return View(model);
        }
        public ActionResult ConvertToMember(CombineView combineView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objSaveData.convertToMember(combineView);
                    TempData["msg"] = "Records Add Successfuly.";
                    return RedirectToAction("Index", "MemberTBs");
                }
                else
                {
                    IEnumerable<MembershipTB> mDta = db.MembershipTBs.ToList();
                    var data = mDta.ToList();
                    IEnumerable<TrainerTB> tData = db.TrainerTBs.ToList();
                    var tD = tData.ToList();
                    ViewBag.trainerSelect = new SelectList(tD, "Name", "Name");
                    ViewBag.MembershipPlans = new SelectList(data, "MembershipPlan", "MembershipPlan");
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Records Add Failed." + ex.Message;
                return RedirectToAction("Index", "VisitorTBs");
            }
        }

        public ActionResult printBill(int? id, int? mid)
        {
            var combineData = new CombineView
            {
                member = db.MemberTBs.Find(id),
                transaction = db.TransactionTBs.Find(mid)
            };
            return View(combineData);
        }
    }
}