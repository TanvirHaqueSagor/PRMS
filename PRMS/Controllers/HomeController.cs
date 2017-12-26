using PSTU_RESULT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRMS.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index(string usertype,string username, string password)
        {
            if (username != null && password != null)
            {

                if (usertype.Equals("admin"))
                { 
           
               
               string email = "tanvir@gmail.com";
               Login aLogin = new Login();
               Boolean ch = aLogin.LoginCheck(username, password);
                    if (ch == true)
                    {
                       
                        return RedirectToAction("Index", "Admin",new{ username=username,email=email});
                    }
               
               }
                else if (usertype.Equals("teacher"))
                {
                    ViewBag.username = username;
                    ViewBag.password = password;
                    Login aLogin = new Login();
                    Boolean ch = aLogin.LoginCheck(username, password);
                    if (ch == true)
                    {

                        return View();
                    }

                }

                }

            return View();
        }

     
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}