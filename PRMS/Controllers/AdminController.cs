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

                        string constr = ConfigurationManager.ConnectionStrings["info"].ConnectionString;
                        SqlConnection con = new SqlConnection(constr);
                        con.Open();

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var info = new StudentInfo();
                            info.name = workSheet.Cells[rowIterator, 1].Value.ToString();
                            info.fathers_name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            info.mothers_name = workSheet.Cells[rowIterator, 3].Value.ToString();
                            info.id = Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value);
                            info.reg = Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value);
                            info.regularity = workSheet.Cells[rowIterator, 6].Value.ToString();
                            info.hall = workSheet.Cells[rowIterator, 7].Value.ToString();
                            info.sex = workSheet.Cells[rowIterator, 8].Value.ToString();
                            info.nationality = workSheet.Cells[rowIterator, 9].Value.ToString();
                            info.religion = workSheet.Cells[rowIterator, 10].Value.ToString();
                            info.phone = workSheet.Cells[rowIterator, 11].Value.ToString();
                            info.email = workSheet.Cells[rowIterator, 12].Value.ToString();
                            info.blood = workSheet.Cells[rowIterator, 13].Value.ToString();

                            SqlCommand stu_info = new SqlCommand("INSERT INTO stu_info VALUES(@name, @fathers_name, @mothers_name, @id, @reg, @regularity, @hall, @sex, @nationality, @religion, @phone, @email, @blood)", con);

                            stu_info.Parameters.AddWithValue("@name", info.name);
                            stu_info.Parameters.AddWithValue("@fathers_name", info.fathers_name);
                            stu_info.Parameters.AddWithValue("@mothers_name", info.mothers_name);
                            stu_info.Parameters.AddWithValue("@id", info.id);
                            stu_info.Parameters.AddWithValue("@reg", info.reg);
                            stu_info.Parameters.AddWithValue("@regularity", info.regularity);
                            stu_info.Parameters.AddWithValue("@hall", info.hall);
                            stu_info.Parameters.AddWithValue("@sex", info.sex);
                            stu_info.Parameters.AddWithValue("@nationality", info.nationality);
                            stu_info.Parameters.AddWithValue("@religion", info.religion);
                            stu_info.Parameters.AddWithValue("@phone", info.phone);
                            stu_info.Parameters.AddWithValue("@email", info.email);
                            stu_info.Parameters.AddWithValue("@blood", info.blood);

                            stu_info.ExecuteNonQuery();
                            ViewBag.Message = "Create Session Successfully";
                        }
                        con.Close();
                    }
                }
            }
            return View();

        }

        public ActionResult ManageSession()
        {
            return View();

        }
	}
}