using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoWork.Models;

namespace PhotoWork.Controllers
{
    public class AuthenticatedUsersController : Controller
    {
        private PhotoWorkEntities db = new PhotoWorkEntities();

        // GET: AuthenticatedUsers
        public ActionResult Index()
        {
            var authenticatedUsers = db.AuthenticatedUsers.Include(a => a.Admin).Include(a => a.Client).Include(a => a.Photographer);
            return View(authenticatedUsers.ToList());
        }

        // GET: AuthenticatedUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthenticatedUser authenticatedUser = db.AuthenticatedUsers.Find(id);
            if (authenticatedUser == null)
            {
                return HttpNotFound();
            }
            return View(authenticatedUser);
        }

        // GET: AuthenticatedUsers/Create
        public ActionResult Create()
        {
            ViewBag.Email = new SelectList(db.Admins, "Username", "logFileLocation");
            ViewBag.Email = new SelectList(db.Clients, "Username", "LinkSocialmedia");
            ViewBag.Email = new SelectList(db.Photographers, "Username", "LinkProject");
            return View();
        }

        // POST: AuthenticatedUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,passwords,Role,phoneNumber,FullName,isActive,Avatar,isBanned")] AuthenticatedUser authenticatedUser)
        {
            if (ModelState.IsValid)
            {
                db.AuthenticatedUsers.Add(authenticatedUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Email = new SelectList(db.Admins, "Username", "logFileLocation", authenticatedUser.Email);
            ViewBag.Email = new SelectList(db.Clients, "Username", "LinkSocialmedia", authenticatedUser.Email);
            ViewBag.Email = new SelectList(db.Photographers, "Username", "LinkProject", authenticatedUser.Email);
            return View(authenticatedUser);
        }

        // GET: AuthenticatedUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthenticatedUser authenticatedUser = db.AuthenticatedUsers.Find(id);
            if (authenticatedUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.Email = new SelectList(db.Admins, "Username", "logFileLocation", authenticatedUser.Email);
            ViewBag.Email = new SelectList(db.Clients, "Username", "LinkSocialmedia", authenticatedUser.Email);
            ViewBag.Email = new SelectList(db.Photographers, "Username", "LinkProject", authenticatedUser.Email);
            return View(authenticatedUser);
        }

        // POST: AuthenticatedUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,passwords,Role,phoneNumber,FullName,isActive,Avatar,isBanned")] AuthenticatedUser authenticatedUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(authenticatedUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Email = new SelectList(db.Admins, "Username", "logFileLocation", authenticatedUser.Email);
            ViewBag.Email = new SelectList(db.Clients, "Username", "LinkSocialmedia", authenticatedUser.Email);
            ViewBag.Email = new SelectList(db.Photographers, "Username", "LinkProject", authenticatedUser.Email);
            return View(authenticatedUser);
        }

        // GET: AuthenticatedUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthenticatedUser authenticatedUser = db.AuthenticatedUsers.Find(id);
            if (authenticatedUser == null)
            {
                return HttpNotFound();
            }
            return View(authenticatedUser);
        }

        // POST: AuthenticatedUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AuthenticatedUser authenticatedUser = db.AuthenticatedUsers.Find(id);
            db.AuthenticatedUsers.Remove(authenticatedUser);
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
