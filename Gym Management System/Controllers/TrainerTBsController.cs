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
    public class TrainerTBsController : Controller
    {
        private GYMDBEntities4 db = new GYMDBEntities4();

        // GET: TrainerTBs
        public ActionResult Index()
        {
            return View(db.TrainerTBs.ToList());
        }
        [HttpGet]
        public async Task<ActionResult> Index(string search)
        {
            var data = from x in db.TrainerTBs select x;
            if (!String.IsNullOrEmpty(search))
            {
                data = data.Where(x => x.Name.Contains(search));
            }
            return View(await data.AsNoTracking().ToListAsync());
        }
        // GET: TrainerTBs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainerTB trainerTB = db.TrainerTBs.Find(id);
            if (trainerTB == null)
            {
                return HttpNotFound();
            }
            return View(trainerTB);
        }

        // GET: TrainerTBs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainerTBs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Phone,Email,JoinDate,DOB,Address,Experience")] TrainerTB trainerTB)
        {
            if (ModelState.IsValid)
            {
                db.TrainerTBs.Add(trainerTB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trainerTB);
        }

        // GET: TrainerTBs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainerTB trainerTB = db.TrainerTBs.Find(id);
            if (trainerTB == null)
            {
                return HttpNotFound();
            }
            return View(trainerTB);
        }

        // POST: TrainerTBs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Phone,Email,JoinDate,DOB,Address,Experience")] TrainerTB trainerTB)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainerTB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainerTB);
        }

        // GET: TrainerTBs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainerTB trainerTB = db.TrainerTBs.Find(id);
            if (trainerTB == null)
            {
                return HttpNotFound();
            }
            return View(trainerTB);
        }

        // POST: TrainerTBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainerTB trainerTB = db.TrainerTBs.Find(id);
            db.TrainerTBs.Remove(trainerTB);
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
