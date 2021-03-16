using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoWork.DTO;
using PhotoWork.Models;

namespace PhotoWork.Controllers
{
    public class PhotographersController : Controller
    {
        string con = ConfigurationManager.ConnectionStrings["strConnection"].ConnectionString;
        private PhotoWorkEntities db = new PhotoWorkEntities();
        // GET: Photographers
        public ActionResult Index()
        {
            
            SqlConnection connection = new SqlConnection(con);
            string SQL = "select * from Service where PhotographerID=@id";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@id", Session["USERNAME"].ToString());
            connection.Open();
            SqlDataReader rd = command.ExecuteReader();
            List<Service> list = new List<Service>();
            while (rd.Read())
            {
                list.Add(new Service()
                {
                    ID = rd["ID"].ToString(),
                    ServiceName = rd["ServiceName"].ToString(),
                    Description = rd["Description"].ToString(),
                    isAvaiable = Boolean.Parse(rd["isAvaiable"].ToString()),
                    isDelete = Boolean.Parse(rd["isDelete"].ToString()),
                    CreateDate = DateTime.Parse(rd["CreateDate"].ToString()),
                    Rating = double.Parse(rd["Rating"].ToString())
                    
                });
            }
            connection.Close();
            // "select * from Service where PhotographerID=@id", new SqlParameter("@id", Session["USERNAME"])).ToList<Service>()
            Session.Add("LIST", list);

            return View(list);
        }
        public ActionResult ViewProfile()
        {
            string id = Session["USERNAME"].ToString();
  
            Photographer pho = db.Photographers.Find(id);
            pho.AuthenticatedUser = db.AuthenticatedUsers.Find(id);
            return View(pho);
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
       /* public ActionResult ViewProfile(string id)
        {
            PhotographerProfile profile = new PhotographerProfile();
            AuthenticatedUser photographer = db.AuthenticatedUsers.Find(Session["USERNAME"].ToString());
            SqlConnection connection = new SqlConnection(con);
            connection.Open();
            string SQL = "select Username,phoneNumber,FullName,TotalProjectDone,Bio,LinkProject, LinkSocialMedia,CurrentMoney from Photographer p, AuthenticatedUser a  where a.email = p.username and p.username = @id";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@id", Session["USERNAME"].ToString());
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                profile = new PhotographerProfile(reader["Username"].ToString(),reader["phoneNumber"].ToString(),reader["FullName"].ToString(),int.Parse(reader["TotalProjectDone"].ToString()),reader["Bio"].ToString(),reader["LinkProject"].ToString(),reader["LinkSocialMedia"].ToString(),float.Parse(reader["CurrentMoney"].ToString()));
            }
            connection.Close();
            return View(photographer);
        }
*/
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
