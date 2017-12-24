using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRMS.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index(string username,string email)
        {
            ViewBag.username = username;
            ViewBag.email = email;
            return View();
        }

        public ActionResult AddTeacher()
        {
            return View();

        }


        public ActionResult TeachersList()
        {
            return View();

        }


        public ActionResult AddSemesters()
        {
            return View();

        }
	}
}