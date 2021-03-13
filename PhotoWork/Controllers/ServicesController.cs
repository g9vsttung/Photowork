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
using System.Configuration;

namespace PhotoWork.Controllers
{
    public class ServicesController : Controller
    {
        private PhotoWorkEntities db = new PhotoWorkEntities();
        string con = ConfigurationManager.ConnectionStrings["strConnection"].ConnectionString;
        // GET: Services
        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            Session["SERVICE_ID"] = id;
            if (Session["ROLE"] == null)
            {
                return RedirectToAction("Details", "Services", new { id = id });
            }

            string Role = Session["ROLE"].ToString().ToLower();          
           if (Role == "admin" || Role == "photographer")
            {
                return RedirectToAction("ErrorAction", "Services");
            }
            else
            {
                return RedirectToAction("Details", "Services", new { id=id});
            }            
        }
        public ActionResult ErrorAction()
        {
            ViewBag.ERROR = "Bạn không đủ quyền truy cập";
            return View();
        }

        // GET: Services/Details/5

        public ActionResult Details(string id)
        {
            
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            SqlConnection connection = new SqlConnection(con);
            string SQL = "select s.ID,serviceName, s.Description,PhotographerID,FullName,rating, startingPrice"+
                            " from service s, ServiceSkill ss, AuthenticatedUser u, PackageDetail p "+
                            "where ss.ServiceID = s.ID and ss.SkillID = @id and u.Email = s.PhotographerID and iSdelete = 0 and isBanned = 0 and p.PackageID = 1 and p.ServiceID = s.ID";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@id", id);
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
                    Rating = int.Parse(rd["Rating"].ToString()),
                    FullName = rd["FullName"].ToString(),
                    PhotographerID = rd["PhotographerID"].ToString(),
                    StartingPrice = double.Parse(rd["startingPrice"].ToString())

                });
            }
            connection.Close();
            Skill skill = db.Skills.Find(id);
            Session.Add("LIST", list);         
            ViewBag.Skill = skill.name;
            ViewBag.Category = db.Categories.Where(c => c.CategoryID == skill.CategoryID)
                                               .FirstOrDefault<Category>().name.ToString();
           
            return View(list.ToList());
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
            db.Services.SqlQuery("insert  Service(id,ServiceName,Description,isAvaiable,CreateDate,isDelete,PhotographerID) values(@id,@name,@des,1,@createDate,0,@photo)", new SqlParameter("@id", id), new SqlParameter("@name", name), new SqlParameter("@des", des), new SqlParameter("@photo", Session["USERNAME"].ToString()));
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
