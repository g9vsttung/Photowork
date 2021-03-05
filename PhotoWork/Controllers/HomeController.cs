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
    
       
       [AllowAnonymous]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(AuthenticatedUser model, string returnUrl)
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
            PhotoWorkEntities db = new PhotoWorkEntities();
            var dataItem = db.AuthenticatedUsers.Where(x => x.Email == model.Email && x.passwords == model.passwords).FirstOrDefault();           
            if (dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(dataItem.Email, false);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            else
            {
                ModelState.AddModelError("", "Invalid user/pass");
                return View();
            }
        }
      

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

    }
}