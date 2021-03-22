using PhotoWork.DTO;
using PhotoWork.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace PhotoWork.Controllers
{
    public class HomeController : Controller
    {
        /*
         Select * 
        from Service s, Invoice i 
        where s.ID=i.ServiceID and PhotographerID='hi2@gmail.com'
         */
        string con = ConfigurationManager.ConnectionStrings["strConnection"].ConnectionString;
        /* [Authorize(Roles = "Admin")]*/
        public ActionResult Index()
        {
            if (Session["ROLE"] == null)
            {
                return View();
            }

            string Role = Session["ROLE"].ToString().ToLower();

            if (Role == "admin")
            {
                return RedirectToAction("Index", "Admins");
            }
            else if (Role == "photographer")
            {
                return RedirectToAction("Index", "Photographers");
            }
            else if (Role == "client")
            {
                return RedirectToAction("Index", "Clients");
            }
            return View();
        }
        public ActionResult Register()
        {
            AuthenticatedUser au = new AuthenticatedUser();
            return View(au);
        }
            [HttpPost]
        public ActionResult Register(AuthenticatedUser user)
        {


            string email = user.Email;
            string password = user.passwords;
            string rePassword = user.repasswords;
            string role = Request.Form["role"];
            string phone = user.phoneNumber;
            string fullName = user.FullName;

            SqlConnection connection = new SqlConnection(con);
            string SQL = "insert into AuthenticatedUser(email, passwords, role, phoneNumber, fullName,isActive,isBanned,Avatar) values(@email,@pass,@role,@phone,@name,1,0,'1')";
            SqlCommand command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@pass", password);
            command.Parameters.AddWithValue("@role", role);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@name", fullName);
            connection.Open();
            int n = command.ExecuteNonQuery();
            connection.Close();
            if (role.Equals("Client"))
            {
                SQL = "insert into Client(username) values(@email)";
            }
            else
            {
                SQL = "insert into Photographer(username, isAvaiable) values(@email,1)";
            }           
            command = new SqlCommand(SQL, connection);
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            command.ExecuteNonQuery();

            connection.Close();
            return RedirectToAction("Index", "Home");


        }
        [HttpPost]
        public ActionResult Login(string returnUrl)
        {
            /*
             1.Check Account
                - FormsAuthentication.SetAuthCookie(dataItem.Email, false);
                - Nếu không tồn tại thì quay về trang Login và đưa ra msg 
                - Nếu tồn tại:
                    + nếu role là Guest : return RedirectToAction("Index","Guests");
                    + nếu role là Photographer : return RedirectToAction("Index","Photographers");
                    + nếu role là Admin : return RedirectToAction("Index","Admins");
             */
            String Email = Request.Form["email"];
            String Password = Request.Form["password"];
            Session.Remove("Account_ERR");
            PhotoWorkEntities db = new PhotoWorkEntities();
            var dataItem = db.AuthenticatedUsers.Where(x => x.Email == Email && x.passwords == Password).FirstOrDefault();
            if (dataItem != null)
            {

                FormsAuthentication.SetAuthCookie(dataItem.Email, false);  //set Cookie 
                Session["ROLE"] = dataItem.Role;  //set session 


                Session["USERNAME"] = dataItem.Email;

                if (dataItem.Role.ToLower() == "admin")
                {
                    return RedirectToAction("Index", "Admins");
                }
                else if (dataItem.Role.ToLower() == "photographer")
                {
                    return RedirectToAction("Home", "Photographers");
                }
                else if (dataItem.Role.ToLower() == "client")
                {
                    return RedirectToAction("Index", "Clients");
                }
                else
                {
                    Session["Account_ERR"] = "Xin lỗi, bạn không đủ quyền truy cập ";
                    return RedirectToAction("Index", "Home");
                }
            }

            Session["Account_ERR"] = "Sai mật khẩu hoặc email";
            return RedirectToAction("Index", "Home");

        }


        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}