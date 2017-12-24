using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
namespace PSTU_RESULT.Models
{
    public class Login
    {





        string connectionString = WebConfigurationManager.ConnectionStrings["LoginDbConnectionString"].ConnectionString;

        public Boolean LoginCheck(string username, string password)
        {
            string query = "Select * from [Login].[dbo].[adminInfo] where Username = '" + username + "' and Password = '" + password + "'";

            try
            {
                SqlConnection aSqlConnection = new SqlConnection(connectionString);
                aSqlConnection.Open();
                SqlCommand aCommand = new SqlCommand(query, aSqlConnection);

                SqlDataReader aReader = aCommand.ExecuteReader();

                if (aReader.Read())
                {
                    aSqlConnection.Close();
                    return true;
                }

            }
            catch (Exception e)
            {


            }

           // aSqlConnection.Close();
            return false;

        }
    }
}