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
        string ConnectionString = @"server=localhost;database=PhotoWork;uid=linhtnl;pwd=123";



        // GET: Services
        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            Session["SERVICE_ID"] = id;
            if (Session["ROLE"] == null)
            {
                return RedirectToAction("Details", "Services");
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
            else
            {
               return  RedirectToAction("Details", "Services");
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
            
            if (id == null) id = Session["SERVICE_ID"].ToString();
            SqlConnection connection = new SqlConnection(ConnectionString);
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
            ViewBag.PhotographerID = new SelectList(db.Photographers, "Username", "LinkProject", service.PhotographerID);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ServiceName,Description,isAvaiable,CreateDate,isDelete,deleteDate,PhotographerID,Rating")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhotographerID = new SelectList(db.Photographers, "Username", "LinkProject", service.PhotographerID);
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
