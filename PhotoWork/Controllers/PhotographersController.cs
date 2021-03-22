using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
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
        public ActionResult Home()
        {
            return View();
        }
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
        public ActionResult Edit()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index");
            }
            string id = Session["USERNAME"].ToString();
            Photographer pho = db.Photographers.Find(id);
            if (pho == null)
            {
                return HttpNotFound();
            }
            pho.AuthenticatedUser = db.AuthenticatedUsers.Find(id);
            return View(pho);


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
        [HttpPost]
        public ActionResult SaveEdit(Photographer photographer)
        {
            if (Session["USERNAME"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            SqlConnection connection = new SqlConnection(con);
            //Update AuthenticatedUser : FullName, PhoneNumber

            string SQL = "Update AuthenticatedUser set FullName=@name ,PhoneNumber=@phone where Email=@email";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@name", Request.Form["AuthenticatedUser.FullName"].Trim());
            command.Parameters.AddWithValue("@phone", Request.Form["AuthenticatedUser.phoneNumber"].Trim());
            command.Parameters.AddWithValue("@email", Session["USERNAME"].ToString());
            connection.Open();
            command.ExecuteNonQuery();
            //Update Photographer:isAvailable,Bio,LinkProject,LinkSocialMedia
            SQL = "Update Photographer set isAvaiable=@isAvailable,Bio=@Bio,LinkSocialMedia=@link, LinkProject=@linkProject where Username=@Username";
            command = new SqlCommand(SQL, connection);

            command.Parameters.AddWithValue("@isAvailable", Request.Form["isReady"]);
            command.Parameters.AddWithValue("@Bio", Request.Form["Bio"].Trim());
            command.Parameters.AddWithValue("@link", Request.Form["LinkSocialMedia"].Trim());
            command.Parameters.AddWithValue("@linkProject", Request.Form["LinkProject"].Trim());
            command.Parameters.AddWithValue("@Username", Session["USERNAME"].ToString());
           
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("ViewProfile");
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
        public ActionResult Requirement(string id)
        {
            List<Invoice> list = new List<Invoice>();
            string SQL = "select I.id,Contract, ClientID, process, DateStart,I.ServiceID,S.ServiceName,S.Description from invoice I, Service S where I.ServiceID=S.ID and PhotographerID = @photo and (process like 'CanceledByClient' or process like 'Waiting' or process like 'Doing') ";
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@photo", Session["USERNAME"].ToString());
            connection.Open();
            SqlDataReader rd = command.ExecuteReader();

            while (rd.Read())
            {
                list.Add(new Invoice()
                {
                    ID = rd["ID"].ToString(),
                    ServiceID = rd["ServiceID"].ToString(),
                    Contract = rd["Contract"].ToString(),
                    ClientID = rd["ClientID"].ToString(),
                    process = rd["process"].ToString(),
                    DateStart = DateTime.Parse(rd["DateStart"].ToString()),
                    ServiceName = rd["ServiceName"].ToString(),
                    Description = rd["Description"].ToString()
                });

            }

            connection.Close();
            return View(list);
        }

        public ActionResult Accept(string id)
        {
            string SQL = "update invoice set process='Doing' where ID=@id";
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Requirement");
        }

        public ActionResult Reject(string id)
        {
            string SQL = "delete from invoice where ID=@id";
            SqlConnection connection = new SqlConnection(con);
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Requirement");
        }
    }
}
