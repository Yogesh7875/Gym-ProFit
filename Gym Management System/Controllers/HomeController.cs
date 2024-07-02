using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gym_Management_System.Models;
using System.Data;
using System.Data.Entity;
using System.Net;
using WhatsAppApi;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Gym_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private GYMDBEntities4 db = new GYMDBEntities4();
        public ActionResult Index()
        {
            var visitor = db.VisitorTBs.Any() ? db.VisitorTBs.Count() : 0;
            var membershipCount = db.MembershipTBs.Any() ? Convert.ToInt32(db.MembershipTBs.Count()) : 0;
            var memberCount = db.MemberTBs.Any() ? Convert.ToInt32(db.MemberTBs.Count()) : 0;
            var trainerCount = db.TrainerTBs.Any() ? Convert.ToInt32(db.TrainerTBs.Count()) : 0;
            var transactionCount = db.TransactionTBs.Any() ? Convert.ToInt32(db.TransactionTBs.Count()) : 0;
            var transactions = db.TransactionTBs.Include(t => t.MemberTB)
                    .Where(t => t.MembershipEndDate <= DateTime.Today)
                    .ToList();
            var totalPaidAmount = transactions.Count();

            ShowData s = new ShowData()
            {
                vData = visitor,
                mData = membershipCount,
                memberData = memberCount,
                trainerData = trainerCount,
                tranData = transactionCount,
                totalAmount = totalPaidAmount
            };

            return View(s);
        }

        // GET: Auth/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "password")
            {
                // Authentication successful for admin
                Session["UserRole"] = "Admin";
                return RedirectToAction("Index", "Home");
            }
            else if (username == "Account" && password == "1234")
            {
                // Authentication successful for account
                Session["UserRole"] = "Account";
                return RedirectToAction("Index", "Home");
            }

            // Authentication failed
            ViewBag.Message = "Invalid login attempt";
            return View();
        }





        public ActionResult MembershipEnd(string searchString, string gender)
        {
            DateTime today = DateTime.Today;

            // Filter by searchString
            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredTransactions = db.TransactionTBs.Include(t => t.MemberTB)
                    .Where(t => t.MemberTB.Name.Contains(searchString) &&
                                t.MembershipEndDate <= today)
                    .ToList();

                int memberCount = filteredTransactions.Select(t => t.MemberTB).Distinct().Count();

                ViewData["tCount"] = memberCount;
                ViewBag.Transactions = filteredTransactions;
                return View();
            }
            // Filter by gender
            else if (!string.IsNullOrEmpty(gender))
            {
                var filteredTransactions = db.TransactionTBs.Include(t => t.MemberTB)
                    .Where(t => t.MemberTB.Gender == gender &&
                                t.MembershipEndDate <= today)
                    .ToList();

                int memberCount = filteredTransactions.Select(t => t.MemberTB).Distinct().Count();

                ViewData["tCount"] = memberCount;
                ViewBag.Transactions = filteredTransactions;
                ViewBag.SelectedGender = gender;

                return View();
            }
            // No filters
            else
            {
                var transactions = db.TransactionTBs.Include(t => t.MemberTB)
                    .Where(t => t.MembershipEndDate <= today)
                    .ToList();

                var memberModel = transactions.Select(t => t.MemberTB).Distinct().ToList();

                ViewBag.Transactions = transactions;
                ViewBag.Members = memberModel;
                ViewData["tCount"] = memberModel.Count;

                return View();
            }
        }



        public ActionResult UpdateMembership(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionTB transac = db.TransactionTBs.Find(id);
            if (transac == null)
            {
                return HttpNotFound();
            }
            IEnumerable<MembershipTB> mDta = db.MembershipTBs.ToList();
            var data = mDta.ToList();
            IEnumerable<TrainerTB> tData = db.TrainerTBs.ToList();
            var tD = tData.ToList();
            ViewBag.trainerSelect = new SelectList(tD, "Name", "Name");
            ViewBag.MembershipPlans = new SelectList(data, "MembershipPlan", "MembershipPlan");
            return View(transac);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMembership([Bind(Include = "ID, Membership, OfferPrice, PaidAmount, DueAmount, DueDate, M_ID, Trainer, PaymentMode, PaymentDate, Days, MembershipEndDate")] TransactionTB transaction)
        {
            if (ModelState.IsValid)
            {
                // Calculate DueAmount
                transaction.DueAmount = transaction.OfferPrice - transaction.PaidAmount;

                // Update the transaction object in the database context
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("MembershipEnd"); // Redirect to a suitable action after successful update
            }

            // Optionally, you may want to repopulate ViewBag properties like MembershipPlans and TrainerSelect here if needed
            return View(transaction);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult WhatsApp(int id)
        {
            MemberTB memberTB = db.MemberTBs.Find(id);
            if (memberTB.Email != null)
            {
                if (IsValidEmail(memberTB.Email))
                {
                    try
                    {
                        MailMessage mail = new MailMessage
                        {
                            From = new MailAddress("yogeshharal787@gmail.com")
                        };
                        mail.To.Add(memberTB.Email);
                        mail.Subject = "Membership Renewal Reminder";
                        mail.Body = "Dear " + memberTB.Name + ",<br><br>" +
                                    "We wanted to let you know that your gym membership has expired. Please renew it at your earliest convenience to continue enjoying our facilities.<br><br>" +
                                    "Best regards,<br>GYMProFit" + "<br><br><br>" +
                                    "प्रिय " + memberTB.Name + ",<br><br>" +
                                    "आम्ही आपल्याला कळवू इच्छितो की आपले जिम सदस्यत्व संपले आहे. कृपया लवकरात लवकर नूतनीकरण करा जेणेकरून आपल्याला आमच्या सुविधांचा लाभ घेता येईल.<br><br>" +
                                    "सधन्यवाद,<br>GYMProFit";
                        mail.IsBodyHtml = true;

                        SmtpClient smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential("yogeshharal787@gmail.com", "lmll dlio fxjt xiuw"),
                            EnableSsl = true
                        };

                        smtp.Send(mail);
                        TempData["Message"] = "Email sent successfully!";
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = "Error sending email: " + ex.Message;
                    }
                }
                else
                {
                    TempData["Message"] = "Error: Invalid email address.";
                }
            }
            else
            {
                TempData["Message"] = "Error: No email address provided.";
            }

            return RedirectToAction("MembershipEnd", "Home");
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}