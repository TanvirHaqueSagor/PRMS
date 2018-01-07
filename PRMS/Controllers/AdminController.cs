using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml;
using System.Configuration;
using System.Data.SqlClient;
using PRMS.Models;
using System.Web.Configuration;
using System.Net;
using System.Data.Entity;

namespace PRMS.Controllers
{
    public class AdminController : Controller
    {

        private ProjectDB db = new ProjectDB(); 
        
        //
        // GET: /Admin/
        public ActionResult Index(string username,string email)
        {
            ViewBag.username = username;
            ViewBag.email = email;
            return View();
        }
        public ActionResult AddTeacher(string fullanme, string emailid, string mobile, string faculty, string department)
        {                                  //     [Bind(Include="TeacherID,Name,Email,Mobile,Faculty,Department")] Teacher teacher
              
            ViewBag.faculties = db.Faculty.ToList();




                if(fullanme!=null & emailid!=null & mobile!=null & faculty !=null & department !=null ){

                    Teacher teacher = new Teacher(fullanme, emailid, mobile, faculty, department, new FacultyDepartmentInfo().createRandomPassword());
                    db.Teacher.Add(teacher);
                    db.SaveChanges();

                }




            return View();

        }
        public ActionResult TeachersList()
        {
               return View(db.Teacher.ToList());

        }
        public ActionResult Edit(int? id)
        {

          //  return View("~/Views/Admin/EditTeacher.cshtml");
           return RedirectToAction("EditTeacher", "Admin", new { ind = id });

        }
        public ActionResult EditTeacher(int? ind)
        {
            if (ind == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teacher.Find(ind);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            FacultyDepartmentInfo facultyDeptInfo = new FacultyDepartmentInfo();
            ViewBag.faculties = facultyDeptInfo.getFaculty();
            return View(teacher);

        }
        [HttpPost]
        public ActionResult EditTeacher([Bind(Include = "TeacherID,Name,Email,Mobile,Faculty,Department,Password")] Teacher teacher)
        {
            Teacher tcr = db.Teacher.Find(teacher.TeacherID);
            db.Entry(tcr).State = EntityState.Detached;
                
                  if(tcr.PasswordChanged==true){
                      EncryptionDectryption sc = new EncryptionDectryption();
                       teacher.Password = sc.Encryptdata(teacher.Password);
                       teacher.PasswordChanged = true;
                  }


           if (teacher.Password.Equals(tcr.Password)) { 
           
            if (ModelState.IsValid)
            {                                        
                   db.Entry(teacher).State = EntityState.Modified;
                   db.SaveChanges();
            }
            
            return RedirectToAction("TeachersList", "Admin");
                  }
                  else
                  {
                      ViewBag.Message = "Please Put the correct password";
                      return View();
                  }

        }
        public ActionResult Delete(int? id)
        {
            Teacher teacher = db.Teacher.Find(id);
            db.Teacher.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("TeachersList", "Admin");
        }
        public ActionResult Detail(int? id)
        {

            //  return View("~/Views/Admin/EditTeacher.cshtml");
            return RedirectToAction("TeacherDetail", "Admin", new { ind = id });

        }
        public ActionResult TeacherDetail(int? ind)
        {
            if (ind == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teacher.Find(ind);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }
        public ActionResult AddSemesters()
        {
            return View();

        }
        public ActionResult AddSession(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["postedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                      

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var info = new StudentInfo();
                            info.name = workSheet.Cells[rowIterator, 1].Value.ToString();
                            info.fathers_name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            info.mothers_name = workSheet.Cells[rowIterator, 3].Value.ToString();
                            info.StudentId = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                            info.reg = Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value);
                            info.regularity = workSheet.Cells[rowIterator, 6].Value.ToString();
                            info.hall = workSheet.Cells[rowIterator, 7].Value.ToString();
                            info.sex = workSheet.Cells[rowIterator, 8].Value.ToString();
                            info.nationality = workSheet.Cells[rowIterator, 9].Value.ToString();
                            info.religion = workSheet.Cells[rowIterator, 10].Value.ToString();
                            info.phone = workSheet.Cells[rowIterator, 11].Value.ToString();
                            info.email = workSheet.Cells[rowIterator, 12].Value.ToString();
                            info.blood = workSheet.Cells[rowIterator, 13].Value.ToString();


                          db.StudentInfo.Add(info);
                           db.SaveChanges();


                            ViewBag.Message = "Create Session Successfully";
                        }
                        
                    }
                }
            }
            return View();

        }
        public ActionResult ManageSession()
        {
            return View();

        }
        public JsonResult GetDepartment(string faculty)
        {



            List<Department> dept = db.Department.Where(b => b.Faculty == faculty).ToList(); 
          //  db.Faculty.ToList();

            List<String> departments = new List<String>();

            foreach (Department dp in dept){
                
                    departments.Add(dp.ShortForm);
                
            }


            return Json(departments, JsonRequestBehavior.AllowGet);
        }

        
    }
}