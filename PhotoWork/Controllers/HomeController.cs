using PhotoWork.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace PhotoWork.Controllers
{
    public class HomeController : Controller
    {
        /* [Authorize(Roles = "Admin")]*/
        public ActionResult Index()
        {
            return View();
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
            PhotoWorkEntities db = new PhotoWorkEntities();
            var dataItem = db.AuthenticatedUsers.Where(x => x.Email == Email && x.passwords == Password).FirstOrDefault();
            if (dataItem != null)
            {
                Session["Account_ERR"] = "";
                FormsAuthentication.SetAuthCookie(dataItem.Email, false);  //set Cookie 
                Session["ROLE"] = dataItem.Role;  //set session 
                Session["USERNAME"] = Email;
                
                if (dataItem.Role.ToLower() == "admin")
                {
                    return RedirectToAction("Index", "Admins");
                }
                else if (dataItem.Role.ToLower() == "photographer")
                {
                    return RedirectToAction("Index", "Photographers");
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

            Session["Account_ERR"]  = "Sai mật khẩu hoặc email" ;

            return RedirectToAction("Index", "Home");

        }


        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

    }
}