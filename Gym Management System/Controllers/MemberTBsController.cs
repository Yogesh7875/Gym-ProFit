using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Gym_Management_System.Models;

namespace Gym_Management_System.Controllers
{
    public class MemberTBsController : Controller
    {
        private const string V = "ID,Name,Phone,Email,JoiningDate,DOB,Address,Gender";
        private GYMDBEntities4 db = new GYMDBEntities4();

        // GET: MemberTBs
        [Route("MemberTBs/Index")]
        public ActionResult Index(string searchString, string gender)
        {
            if (searchString!=null)
            {
                var filteredTransactions = db.TransactionTBs.Include(t => t.MemberTB)
    .Where(t => t.MemberTB.Name.Contains(searchString))
    .ToList();
                int memberCount = db.MemberTBs.Where(m => m.Name.Contains(searchString)).Count();
                ViewData["tCount"] = memberCount;
                ViewBag.Transactions = filteredTransactions;
                return View();
            }
            else if(gender!=null)
            {
                var filteredTransactions = db.TransactionTBs.Include(t => t.MemberTB)
                .Where(t => gender == null || t.MemberTB.Gender == gender)
                .ToList();

                int memberCount = db.MemberTBs.Where(m => gender == null || m.Gender == gender).Count();

                ViewData["tCount"] = memberCount;
                ViewBag.Transactions = filteredTransactions;
                ViewBag.SelectedGender = gender;

                return View();
            }
            else
            {
                var transactions = db.TransactionTBs.Include(t => t.MemberTB).ToList();
                var memberModel = db.MemberTBs.ToList();

                ViewBag.Transactions = transactions;
                ViewBag.Members = memberModel;
                ViewData["tCount"] = db.MemberTBs.Count();
                return View();
            }
        }

        // GET: MemberTBs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberTB memberTB = db.MemberTBs.Find(id);
            if (memberTB == null)
            {
                return HttpNotFound();
            }
            return View(memberTB);
        }

        // GET: MemberTBs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberTBs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = V)] MemberTB memberTB)
        {
            if (ModelState.IsValid)
            {
                db.MemberTBs.Add(memberTB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(memberTB);
        }

        // GET: MemberTBs/Edit/5
        public ActionResult Edit(int? id,int? mid)
        {
            if (id == null && mid==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberTB memberTB = db.MemberTBs.Find(id);
            if (memberTB == null)
            {
                return HttpNotFound();
            }
            return View(memberTB);
        }
    }
}
