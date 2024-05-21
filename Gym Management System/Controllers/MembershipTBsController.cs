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
    public class MembershipTBsController : Controller
    {
        private GYMDBEntities4 db = new GYMDBEntities4();

        // GET: MembershipTBs
        public ActionResult Index()
        {
            return View(db.MembershipTBs.ToList());
        }
        [HttpGet]
        public async Task<ActionResult> Index(string search)
        {
            var data = from x in db.MembershipTBs select x;
            if (!String.IsNullOrEmpty(search))
            {
                data = data.Where(x => x.MembershipPlan.Contains(search));
            }
            return View(await data.AsNoTracking().ToListAsync());
        }
        // GET: MembershipTBs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipTB membershipTB = db.MembershipTBs.Find(id);
            if (membershipTB == null)
            {
                return HttpNotFound();
            }
            return View(membershipTB);
        }
        // GET: MembershipTBs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MembershipTBs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MembershipPlan,Price,Days,Type")] MembershipTB membershipTB)
        {
            if (ModelState.IsValid)
            {
                db.MembershipTBs.Add(membershipTB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(membershipTB);
        }

        // GET: MembershipTBs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipTB membershipTB = db.MembershipTBs.Find(id);
            if (membershipTB == null)
            {
                return HttpNotFound();
            }
            return View(membershipTB);
        }

        // POST: MembershipTBs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MembershipPlan,Price,Days,Type")] MembershipTB membershipTB)
        {
            if (ModelState.IsValid)
            {
                db.Entry(membershipTB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(membershipTB);
        }

        // GET: MembershipTBs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MembershipTB membershipTB = db.MembershipTBs.Find(id);
            if (membershipTB == null)
            {
                return HttpNotFound();
            }
            return View(membershipTB);
        }

        // POST: MembershipTBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MembershipTB membershipTB = db.MembershipTBs.Find(id);
            db.MembershipTBs.Remove(membershipTB);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
