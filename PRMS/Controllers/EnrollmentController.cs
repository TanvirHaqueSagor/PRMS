using OfficeOpenXml;
using PRMS.Models;
using PRMS.Models.CSE;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRMS.Controllers
{
    public class EnrollmentController : Controller
    {
        private CseDbContext csedb = new CseDbContext();
        private ProjectDB db = new ProjectDB();
        //
        // GET: /Enrollment/
        public ActionResult Index()
        {

            ViewBag.faculties = db.Faculty.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Index(String faculty, int semester)
        {

            String fileName = faculty + "_Enroll_" + semester + ".xlsx";

            Server.MapPath("~/App_Data/CSE/" + fileName);

            return RedirectToAction("UploadEnrollment", "Enrollment", new { faculty = faculty, semester = semester });
        }



        public ActionResult Download(String faculty, int semester)
        {



            ViewBag.faculties = db.Faculty.ToList();
            return View();
        }


        public ActionResult UploadEnrollment(String faculty, int semester)
        {

            String sem = SemesterName(semester);


            ViewBag.semester = semester;
            ViewBag.faculty = faculty;
            return View();
        }


        [HttpPost]
        public ActionResult UploadEnrollment(String faculty, int semester, HttpPostedFileBase postedFile)
        {
            String fileName;
            if (faculty != null && postedFile != null)
            {

                if ((postedFile.ContentLength > 0) && !string.IsNullOrEmpty(postedFile.FileName))
                {
                    //string fileContentType = Path.GetExtension(file.FileName).ToLower();
                    fileName = Path.GetFileName(postedFile.FileName);

                    if (fileName.ToLower().Contains(".xls") || fileName.ToLower().Contains(".xlsx"))
                    {

                        byte[] fileBytes = new byte[postedFile.ContentLength];

                        var data = postedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(postedFile.ContentLength));
                        using (var package = new ExcelPackage(postedFile.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            String property;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                CurrentSemester cseCurrentSemester = new CurrentSemester();
                                CurrentSemester currentSemesterTemp = new CurrentSemester();
                              //  cseCurrentSemester = null;
                                currentSemesterTemp = null;

                                JanCseEnrolment janCseEnroll = new JanCseEnrolment();
                                JuneCseEnrollment juneCseEnroll = new JuneCseEnrollment();
                               // janCseEnroll = null;
                               // juneCseEnroll = null;
                                JuneCseEnrollment juneTemp = new JuneCseEnrollment();
                                JanCseEnrolment janTemp = new JanCseEnrolment();
                                juneTemp = null;
                                janTemp = null;

                                for (int i = 1; i <= noOfCol; i++)
                                {
                                    property = workSheet.Cells[1, i].Value.ToString();
                                    if (semester % 2 == 0)
                                    {
                                        if (i == 1 || i == 4)
                                        {
                                            juneCseEnroll.GetType().GetProperty(property).SetValue(juneCseEnroll, workSheet.Cells[rowIterator, i].Value, null);
                                        }
                                        else if (i == 2 || i == 3 || i == 5)
                                        {
                                            if (i == 2)
                                            {
                                                int sid = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value);
                                                int seme = Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value);
                                                currentSemesterTemp = csedb.CurrentSemester.SingleOrDefault(user => user.StudentId == sid && user.Semester == seme);
                                                if (currentSemesterTemp ==null)
                                                {
                                                    cseCurrentSemester.StudentId = sid;
                                                    cseCurrentSemester.Semester = seme;
                                                    csedb.CurrentSemester.Add(cseCurrentSemester);
                                                    csedb.SaveChanges();
                                                }
                                            }
                                            else if (i == 5)
                                            {
                                                if (currentSemesterTemp!=null)
                                                {
                                                    int sid = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value);
                                                    int seme = Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value);
                                                    juneTemp = csedb.JuneCseEnrollment.SingleOrDefault(user => user.StudentId == sid && user.Semester == seme);

                                                    juneCseEnroll = juneTemp;
                                                    currentSemesterTemp = null;
                                                     
                                                }

                                            }

                                            juneCseEnroll.GetType().GetProperty(property).SetValue(juneCseEnroll, Convert.ToInt32(workSheet.Cells[rowIterator, i].Value), null);
                                        }
                                        else
                                        {
                                            juneCseEnroll.GetType().GetProperty(property).SetValue(juneCseEnroll, Convert.ToBoolean(workSheet.Cells[rowIterator, i].Value), null);
                                        }
                                 }
                                    else
                                    {

                                        if (i == 1 || i == 4)
                                        {

                                            janCseEnroll.GetType().GetProperty(property).SetValue(janCseEnroll, workSheet.Cells[rowIterator, i].Value, null);
                                        }
                                        else if (i == 2 || i == 3 || i == 5)
                                        {
                                            if (i == 2)
                                            {
                                                int sid = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value);
                                                int seme = Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value);
                                                currentSemesterTemp = csedb.CurrentSemester.SingleOrDefault(user => user.StudentId == sid && user.Semester == seme);
                                                if (currentSemesterTemp==null )
                                                {
                                                    cseCurrentSemester.StudentId = sid;
                                                    cseCurrentSemester.Semester = seme;
                                                    csedb.CurrentSemester.Add(cseCurrentSemester);
                                                    csedb.SaveChanges();
                                                }
                                            }
                                            else if (i == 5)
                                            {
                                                if (currentSemesterTemp!=null )
                                                {
                                                    int sid = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value);
                                                    int seme = Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value);
                                                    janTemp = csedb.JanCseEnrolment.SingleOrDefault(user => user.StudentId == sid && user.Semester == seme);

                                                    janCseEnroll = janTemp;
                                                    currentSemesterTemp = null;
                                                }

                                            }

                                            janCseEnroll.GetType().GetProperty(property).SetValue(janCseEnroll, Convert.ToInt32(workSheet.Cells[rowIterator, i].Value), null);
                                        }
                                        else
                                        {
                                            janCseEnroll.GetType().GetProperty(property).SetValue(janCseEnroll, Convert.ToBoolean(workSheet.Cells[rowIterator, i].Value), null);
                                        }


                                    }

                                }

                                if (semester % 2 == 0)
                                {
                                    if (juneTemp != null )
                                    {
                                        
                                            csedb.Entry(juneTemp).State = EntityState.Modified;
                                            csedb.SaveChanges();
                                        
                                    }
                                    else
                                    {
                                        csedb.JuneCseEnrollment.Add(juneCseEnroll);
                                        csedb.SaveChanges();
                                    }

                                }
                                else
                                {

                                    if (janTemp != null )
                                    {
                                         
                                            csedb.Entry(janTemp).State = EntityState.Modified;
                                            csedb.SaveChanges();
                                       
                                    }
                                    else
                                    {
                                        csedb.JanCseEnrolment.Add(janCseEnroll);
                                        csedb.SaveChanges();
                                    }

                                }

                                
                            }





                        }

                    }
                    else
                    {
                        //format not supported
                        ViewBag.Message = "File formate not supported. Only .xls or .xlsx is required.";
                    }
                }

            }
            else
            {
                ViewBag.Message = "Required field(s) missing.";
            }


            ViewBag.semester = semester;
            ViewBag.faculty = faculty;
            return View();
        }

        private static string SemesterName(int semester)
        {
            String sem = semester.ToString();

            switch (semester)
            {
                case 1:
                    sem += "st ";
                    break;
                case 2:
                    sem += "nd ";
                    break;
                case 3:
                    sem += "rd ";
                    break;
                case 4:
                    sem += "th ";
                    break;
                case 5:
                    sem += "th ";
                    break;
                case 6:
                    sem += "th ";
                    break;
                case 7:
                    sem += "th ";
                    break;
                case 8:
                    sem += "th ";
                    break;


            }
            return sem;
        }



    }
}