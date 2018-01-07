using PRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;

namespace PRMS.Controllers
{
    public class TeacherController : Controller
    {
        //
        // GET: /Teacher/

        private ProjectDB db = new ProjectDB(); 
        public ActionResult Index( )
        {

         //   ViewBag.Name = teacher.Name;
          //  ViewBag.Email = teacher.Email;   
            return View();
        }

	    public ActionResult History(int? id){

            Teacher teacher = db.Teacher.Find(id);
            
            return View(teacher);

}
        public ActionResult ChangePassword(int? id)
        {


            Teacher teacher = db.Teacher.Find(id);

            return View(teacher);
        

        }

          [HttpPost]
        public ActionResult ChangePassword(int? TeacherID, String oldpassword, String newpassword, String retypenewpassword)
        {


            Teacher teacher = db.Teacher.Find(TeacherID);

            if (newpassword.Equals(retypenewpassword))
            {

                if (teacher.PasswordChanged == true)
                {
                    oldpassword = new EncryptionDectryption().Encryptdata(oldpassword);
                }

                if(teacher.Password.Equals(oldpassword)){
                    teacher.Password = new EncryptionDectryption().Encryptdata(newpassword);
                    teacher.PasswordChanged = true;
                    db.Entry(teacher).State = EntityState.Modified;
                    db.SaveChanges();


                    ViewBag.Message = "Successfully password changed";
                    return RedirectToAction("index", "Teacher");
                }
                else
                {
                    ViewBag.Message = "Invalid Old Password..!";
                    
                    return View(teacher);
                }


            }
            else
            {
                ViewBag.Message = "Miss Match new Password..!";
                return View(teacher);
            }

              

        }

          public ActionResult UploadMark()
          {

              //   ViewBag.Name = teacher.Name;
              //  ViewBag.Email = teacher.Email; 
              ViewBag.faculties = db.Faculty.ToList();
              return View();
          }
          public ActionResult ChangeMark()
          {

              //   ViewBag.Name = teacher.Name;
              //  ViewBag.Email = teacher.Email;   
              return View();
          }
    }
}