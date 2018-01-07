using PRMS.Models;
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
            try { 
           username= username.Replace(" ", String.Empty);
             password=   password.Replace(" ", String.Empty);
            }
            catch (NullReferenceException e)
            {

            }

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
                    Teacher teacher= aLogin.TeacherLoginCheck(username);
                    if (teacher.PasswordChanged == true) { 
                        password = new EncryptionDectryption().Encryptdata(password);
                    }

                    if (teacher.Email ==username && teacher.Password==password)
                    {
                        HttpContext.Session["teacher"] = teacher;
                        return RedirectToAction("Index", "Teacher");
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