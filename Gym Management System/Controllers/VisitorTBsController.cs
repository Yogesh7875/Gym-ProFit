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
    public class VisitorTBsController : Controller
    {
        private GYMDBEntities4 db = new GYMDBEntities4();

        // GET: VisitorTBs
        public ActionResult Index()
        {
            return View(db.VisitorTBs.ToList());
        }
        [HttpGet]
        public async Task<ActionResult> Index(string search)
        {
            var data = from x in db.VisitorTBs select x;
            if (!String.IsNullOrEmpty(search))
            {
                data = data.Where(x => x.name.Contains(search));
            }
            return View(await data.AsNoTracking().ToListAsync());
        }
        // GET: VisitorTBs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitorTB visitorTB = db.VisitorTBs.Find(id);
            if (visitorTB == null)
            {
                return HttpNotFound();
            }
            return View(visitorTB);
        }

        // GET: VisitorTBs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VisitorTBs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,name,phone,email,visitDate,DOB,address,gender,enquiryMode")] VisitorTB visitorTB)
        {
            if (ModelState.IsValid)
            {
                db.VisitorTBs.Add(visitorTB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(visitorTB);
        }

        // GET: VisitorTBs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitorTB visitorTB = db.VisitorTBs.Find(id);
            if (visitorTB == null)
            {
                return HttpNotFound();
            }
            return View(visitorTB);
        }

        // POST: VisitorTBs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,name,phone,email,visitDate,DOB,address,gender,enquiryMode")] VisitorTB visitorTB)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visitorTB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(visitorTB);
        }

        // GET: VisitorTBs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitorTB visitorTB = db.VisitorTBs.Find(id);
            if (visitorTB == null)
            {
                return HttpNotFound();
            }
            return View(visitorTB);
        }

        // POST: VisitorTBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VisitorTB visitorTB = db.VisitorTBs.Find(id);
            db.VisitorTBs.Remove(visitorTB);
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
