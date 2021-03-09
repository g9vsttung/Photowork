using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoWork.Models;

namespace PhotoWork.Controllers
{
    public class PhotographersController : Controller
    {
        private PhotoWorkEntities db = new PhotoWorkEntities();

        // GET: Photographers
        public ActionResult Index()
        { List<Service> list = db.Services.SqlQuery("select * from Service where PhotographerID=@id", new SqlParameter("@id", Session["USERNAME"])).ToList<Service>();
            Session.Add("LIST", list);
            return View(list);
        }

        // GET: Photographers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photographer photographer = db.Photographers.Find(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(photographer);
        }

        // GET: Photographers/Create
        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.Admins, "Username", "logFileLocation");
            ViewBag.Username = new SelectList(db.AuthenticatedUsers, "Email", "passwords");
            return View();
        }

        // POST: Photographers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,TotalProjectDone,isAvaiable,LinkProject,Bio,LinkSocialMedia,updateDate,CurrentMoney,AdminID")] Photographer photographer)
        {
            if (ModelState.IsValid)
            {
                db.Photographers.Add(photographer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminID = new SelectList(db.Admins, "Username", "logFileLocation", photographer.AdminID);
            ViewBag.Username = new SelectList(db.AuthenticatedUsers, "Email", "passwords", photographer.Username);
            return View(photographer);
        }

        // GET: Photographers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photographer photographer = db.Photographers.Find(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.Admins, "Username", "logFileLocation", photographer.AdminID);
            ViewBag.Username = new SelectList(db.AuthenticatedUsers, "Email", "passwords", photographer.Username);
            return View(photographer);
        }

        // POST: Photographers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,TotalProjectDone,isAvaiable,LinkProject,Bio,LinkSocialMedia,updateDate,CurrentMoney,AdminID")] Photographer photographer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photographer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminID = new SelectList(db.Admins, "Username", "logFileLocation", photographer.AdminID);
            ViewBag.Username = new SelectList(db.AuthenticatedUsers, "Email", "passwords", photographer.Username);
            return View(photographer);
        }

        // GET: Photographers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photographer photographer = db.Photographers.Find(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(photographer);
        }

        // POST: Photographers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Photographer photographer = db.Photographers.Find(id);
            db.Photographers.Remove(photographer);
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
