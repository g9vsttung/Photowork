using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoWork.Models;

namespace PhotoWork.Controllers
{
    public class ClientsController : Controller
    {
        private PhotoWorkEntities db = new PhotoWorkEntities();
        string con = ConfigurationManager.ConnectionStrings["strConnection"].ConnectionString;
        // GET: Clients
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewProfile()
        {
            string id = Session["USERNAME"].ToString();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Debug.WriteLine(id);
            Client client = db.Clients.Find(id);
            client.AuthenticatedUser = db.AuthenticatedUsers.Find(id);
            return View(client);
        }
        // GET: Clients/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.Admins, "Username", "logFileLocation");
            ViewBag.Username = new SelectList(db.AuthenticatedUsers, "Email", "passwords");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,timeReported,LinkSocialmedia,updateDate,AdminID")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminID = new SelectList(db.Admins, "Username", "logFileLocation", client.AdminID);
            ViewBag.Username = new SelectList(db.AuthenticatedUsers, "Email", "passwords", client.Username);
            return View(client);
        }

        // GET: Clients/EditProfile
        public ActionResult Edit()
        {

            
            if ( Session["USERNAME"] == null)
            {
                return RedirectToAction("Index");
            }
            string id = Session["USERNAME"].ToString();
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            client.AuthenticatedUser = db.AuthenticatedUsers.Find(id);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEdit()
        {
            

          
          
            if(Session["USERNAME"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
           if (ModelState.IsValid)
            {
                SqlConnection connection = new SqlConnection(con);
                //Update AuthenticatedUser : FullName, PhoneNumber
                string SQL = "Update AuthenticatedUser set FullName=@name ,PhoneNumber=@phone where Email=@email";
                SqlCommand command = new SqlCommand(SQL, connection);
                command.Parameters.AddWithValue("@name", Request.Form["AuthenticatedUser.FullName"].Trim());
                command.Parameters.AddWithValue("@phone", Request.Form["AuthenticatedUser.phoneNumber"].Trim());
                command.Parameters.AddWithValue("@email", Session["USERNAME"].ToString());
                connection.Open();
                command.ExecuteNonQuery();
                //Update Client: LinkSocialMedia
                SQL = "Update Client set LinkSocialMedia = @link where username = @username";
                command.Parameters.AddWithValue("@link", Request.Form["LinkSocialmedia"]);
                command.Parameters.AddWithValue("@username", Session["USERNAME"].ToString());
                command.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToAction("ViewProfile");
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
