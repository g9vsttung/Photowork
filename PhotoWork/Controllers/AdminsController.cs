using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoWork.Models;

namespace PhotoWork.Controllers
{
    public class AdminsController : Controller
    {

        private PhotoWorkEntities db = new PhotoWorkEntities();

        // GET: Admins
        public ActionResult Index() //Find report (condition: fileLocation not null and process not "Done")
        {
            var reports = db.Invoices.Where(s => (s.FileReportLocation != "" && s.process != "Done"));
            //var admins = db.Admins.Include(a => a.AuthenticatedUser);
            return View(reports.ToList());
        }

        // GET: Admins/Details/5
        public ActionResult Details(string id) //View detail of the report 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice inv = db.Invoices.Find(id);
            inv.PhotographerID = db.Services.Where(s => s.ID == inv.ServiceID).FirstOrDefault().PhotographerID;
            if (inv == null)
            {
                return HttpNotFound();
            }
            return View(inv);
        }
        [HttpPost]
        public ActionResult BanUser(string id)
        {
            Debug.WriteLine(id);
            string selected = Request.Form["user"];
            Debug.WriteLine(selected);
            if(selected == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (selected.Contains(","))
            {
                string[] users = selected.Split(',');
                if(banUserDB(users[0]) && banUserDB(users[1]))
                {
                    TempData["status"] = "Khóa tài khoản thành công";
                    return RedirectToAction("Details", new { id = id});                   
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                if (banUserDB(selected))
                {
                   TempData["status"] = "Khóa tài khoản thành công";
                    return RedirectToAction("Details", new { id = id});                 
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }          
        }
        public ActionResult ViewProfile(string id)
        {
            AuthenticatedUser u = db.AuthenticatedUsers.Find(id);

            return View();
        }
            private bool banUserDB(string email)
        {
            AuthenticatedUser u = db.AuthenticatedUsers.Find(email);
            u.isBanned = true;
            db.Entry(u).State = EntityState.Modified;
            return db.SaveChanges() >0;
        }
        // GET: Admins/Create
        public ActionResult Create()
        {
            ViewBag.Username = new SelectList(db.AuthenticatedUsers, "Email", "passwords");
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,logFileLocation")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Username = new SelectList(db.AuthenticatedUsers, "Email", "passwords", admin.Username);
            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            ViewBag.Username = new SelectList(db.AuthenticatedUsers, "Email", "passwords", admin.Username);
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,logFileLocation")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Username = new SelectList(db.AuthenticatedUsers, "Email", "passwords", admin.Username);
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
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
