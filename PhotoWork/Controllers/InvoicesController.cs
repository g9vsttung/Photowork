using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class InvoicesController : Controller
    {
        private PhotoWorkEntities db = new PhotoWorkEntities();
        string con = ConfigurationManager.ConnectionStrings["strConnection"].ConnectionString;

        // GET: Invoices
        public ActionResult Index()
        {
            var invoices = db.Invoices.Include(i => i.Client).Include(i => i.Service);
            return View(invoices.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(db.Clients, "Username", "LinkSocialmedia");
            ViewBag.ServiceID = new SelectList(db.Services, "ID", "ServiceName");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PaymentMethod,Contract,ServiceID,ClientID,DateEnd,DateStart,process,RealitySubmitDate,ContentFeedback,SubmitDate,Rating,FileReportLocation")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "Username", "LinkSocialmedia", invoice.ClientID);
            ViewBag.ServiceID = new SelectList(db.Services, "ID", "ServiceName", invoice.ServiceID);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(db.Clients, "Username", "LinkSocialmedia", invoice.ClientID);
            ViewBag.ServiceID = new SelectList(db.Services, "ID", "ServiceName", invoice.ServiceID);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PaymentMethod,Contract,ServiceID,ClientID,DateEnd,DateStart,process,RealitySubmitDate,ContentFeedback,SubmitDate,Rating,FileReportLocation")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(db.Clients, "Username", "LinkSocialmedia", invoice.ClientID);
            ViewBag.ServiceID = new SelectList(db.Services, "ID", "ServiceName", invoice.ServiceID);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
        public ActionResult DeleteInvoice()
        {
            string id = Request.Form["txtID"];
            SqlConnection connection = new SqlConnection(con);
            string SQL = "delete from Invoice where id=@id";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Services", "Clients");
        }
        public ActionResult DeleteInvoiceByPhoto()
        {
            string id = Request.Form["txtID"];
            SqlConnection connection = new SqlConnection(con);
            string SQL = "delete from Invoice where id=@id";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Requirement", "Photographers");
        }
    }
}
