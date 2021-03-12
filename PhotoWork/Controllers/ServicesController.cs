using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;

using System.Diagnostics;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoWork.Models;

namespace PhotoWork.Controllers
{
    public class ServicesController : Controller
    {
        private PhotoWorkEntities db = new PhotoWorkEntities();
        string con = @"server=SE140240\SQLEXPRESS;database=PhotoWork;uid=sa;pwd=123456";
        // GET: Services
        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            if (Session["ROLE"] == null)
            {
                var services = db.Services.Include(s => s.Photographer);
                return View(services.ToList());
            }
        
            string Role = Session["ROLE"].ToString().ToLower();

            if (Role == "photographer")
            {
                return RedirectToAction("", "Photographers");
            }
            else if (Role == "client")
            {
                return RedirectToAction("", "Client");
            }
            else if (Role == "admin")
            {
                return RedirectToAction("ErrorAction", "Services");
            }
            return RedirectToAction("Details", "Services");

        }
        public ActionResult ErrorAction()
        {
            ViewBag.ERROR = "Bạn không đủ quyền truy cập";
            return View();
        }

        // GET: Services/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            Skill skill = db.Skills.Find(id);
            ViewBag.Skill = skill.name.ToString();
            string categoryId = skill.CategoryID.ToString();
            ViewBag.Category = db.Categories.Where(c => c.CategoryID == categoryId)
                                                .FirstOrDefault<Category>().name.ToString();
            Debug.WriteLine(skill.name.ToString());
            Debug.WriteLine("Hello");
            return View(service);
        }
        // GET: Services/Create
        public ActionResult Create()
        {
            ViewBag.PhotographerID = new SelectList(db.Photographers, "Username");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string returnUrl)
        {
            string id = Request.Form["txtId"];
            string name = Request.Form["txtName"];
            string des = Request.Form["txtDes"];

                db.Services.SqlQuery("insert  Service(id,ServiceName,Description,isAvaiable,CreateDate,isDelete,PhotographerID) values(@id,@name,@des,1,@createDate,0,@photo)", new SqlParameter("@id", id),new SqlParameter("@name",name), new SqlParameter("@des", des), new SqlParameter("@photo", Session["USERNAME"].ToString()));
                
                return RedirectToAction("Index");
            


        }
        public ActionResult GoToEdit(string id)
        {
            TempData["photoId"] = id;
            return View("Edit", "Services","");
        }

        // GET: Services/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ServiceName,Description,isAvaiable,CreateDate,isDelete,deleteDate,PhotographerID,Rating")] Service service)
        {
            
            string name = Request.Form["txtName"];  
            string des = Request.Form["txtDes"];
            bool avai = Boolean.Parse(Request.Form["cbIsAvaiable"]);
            SqlConnection connection = new SqlConnection(con);
            string SQL = "update Service set ServiceName=@name, Description=@des,isavaiable=@avai where id=@id";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@des", des);
            command.Parameters.AddWithValue("@avai", avai);
            command.Parameters.AddWithValue("@id", service.ID);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
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
