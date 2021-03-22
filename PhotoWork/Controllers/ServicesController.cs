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
using System.Text;
using System.Globalization;

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
                return RedirectToAction("Details", "Services", new { id = id });
            }
        }
        public ActionResult ErrorAction()
        {
            ViewBag.ERROR = "Bạn không đủ quyền truy cập";
            return View();
        }
        //POST SEARCH
        [HttpPost]
        public ActionResult Search()
        {
            string searchBy = Request.Form["searchBy"];
            string content = Request.Form["content"];
            if (searchBy == "Photographer")
            {
                return RedirectToAction("SearchByPhotographer", new { content = content });
            }
            else if (searchBy == "Service")
            {
                return RedirectToAction("SearchByService", new { content = content });
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public ActionResult SearchByPhotographer(string content)
        {
            if (content == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            SqlConnection connection = new SqlConnection(con);
            string SQL = "select FullName,Bio,Avatar,Username ,phoneNumber,TotalProjectDone "+
                            "from AuthenticatedUser A, Photographer p "+
                            "where dbo.ufn_removeMark(FullName) like @content and Username = Email";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@content", "%" + nonAccentVietnamese(content) + "%");
            connection.Open();
            SqlDataReader rd = command.ExecuteReader();
            List<Photographer> list = new List<Photographer>();
            while (rd.Read())
            {
                list.Add(new Photographer()
                {
                    Username = rd["Username"].ToString(),
                    Bio = rd["Bio"].ToString(),
                    Avatar = rd["Avatar"].ToString(),
                    FullName = rd["FullName"].ToString(),
                    phoneNumber = rd["phoneNumber"].ToString(),
                    TotalProjectDone =int.Parse(rd["TotalProjectDone"].ToString()),
                });
            }
            connection.Close();
            return View(list);
        }
        private string nonAccentVietnamese(string str)
        {
            var normalizedString = str.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public ActionResult SearchByService(string content)
        {
            if (content == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            SqlConnection connection = new SqlConnection(con);
            string SQL = "select s.ID,serviceName, s.Description,PhotographerID,FullName,rating, s.startingPrice " +
                            "from service s, ServiceSkill ss, AuthenticatedUser u " +
                            "where ss.ServiceID = s.ID and u.Email = s.PhotographerID and iSdelete = 0 and isBanned = 0  and dbo.ufn_removeMark(serviceName) like  @content";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@content", "%" + nonAccentVietnamese(content) + "%");
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
                    StartingPrice = decimal.Parse(rd["startingPrice"].ToString())

                });
            }
            connection.Close();

            return View(list);
        }
        // GET: Services/Details/5
       
        public ActionResult Details(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            SqlConnection connection = new SqlConnection(con);
            string SQL = "select s.ID,serviceName, s.Description,PhotographerID,FullName,rating, startingPrice " +
                    "from service s, ServiceSkill ss, AuthenticatedUser u " +
                    "where ss.ServiceID = s.ID and ss.SkillID = @id and u.Email = s.PhotographerID and iSdelete = 0 and isBanned = 0";
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
                    Rating = double.Parse(rd["Rating"].ToString()),
                    FullName = rd["FullName"].ToString(),
                    PhotographerID = rd["PhotographerID"].ToString(),
                   StartingPrice = Decimal.Parse(rd["startingPrice"].ToString())

                });
            }
            connection.Close();
            Skill skill = db.Skills.Find(id);
            Session.Add("LIST", list);
            ViewBag.Skill = skill.name;
            ViewBag.Category = db.Categories.Where(c => c.CategoryID == skill.CategoryID)
                                               .FirstOrDefault<Category>().name.ToString();

            return View(list);
        }
        // GET: Services/Create
        public ActionResult Create()
        {

            return View();
        }
        //POST : Services/Booking
        //To book a service
        [HttpPost]
        [Authorize(Roles = "Client")]
        public ActionResult Booking(string id)
        {
          
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //Create new Invoice -> set process is 'Waiting'
          
            
            Invoice i = new Invoice()
            {
                ID = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(),
                ServiceID = id,
                ClientID = Session["USERNAME"].ToString(),
                process = "Waiting",
                DateStart = DateTime.Parse(Request.Form["timeStart"]),
                Contract= Request.Form["requirement"],
            };
            Debug.WriteLine("hi");
            db.Invoices.Add(i);
            db.SaveChanges();
            return RedirectToAction("Index","Clients");
        }
        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Service service)
        {
            DateTime dt = DateTime.Now;
            string id = dt.ToString("ddMMyyyyffffssmmhh");

            string name = service.ServiceName;
            string des = service.Description;
            //db.Services.SqlQuery("insert  Service(id,ServiceName,Description,isAvaiable,CreateDate,isDelete,PhotographerID) values(@id,@name,@des,1,@createDate,0,@photo)", new SqlParameter("@id", id), new SqlParameter("@name", name), new SqlParameter("@des", des), new SqlParameter("@photo", Session["USERNAME"].ToString()));
            SqlConnection connection = new SqlConnection(con);
            string SQL = "insert  Service(id,ServiceName,Description,isAvaiable,CreateDate,isDelete,PhotographerID,Rating) values(@id,@name,@des,1,@createDate,0,@photo,0)";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@des", des);
            command.Parameters.AddWithValue("@photo", Session["USERNAME"].ToString());
            command.Parameters.AddWithValue("@id", id);
            DateTime date = DateTime.Now;
            string now = date.ToString("yyyy-MM-dd");
            command.Parameters.AddWithValue("@createDate", now);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Index", "Photographers");

        }
        public ActionResult GoToEdit(string id)
        {
            TempData["photoId"] = id;
            return View("Edit", "Services", "");
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
        public ActionResult Edit(Service service)
        {

            string name = service.ServiceName;
            string des = service.Description;
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

            return RedirectToAction("Index", "Photographers");
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
            return RedirectToAction("Index", "Photographers");
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
