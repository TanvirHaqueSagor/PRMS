using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRMS.Models
{
    public class StudentInfo
    {
        //student information for create session
        public string name { get; set; }
        public string fathers_name { get; set; }
        public string mothers_name { get; set; }
        public int id { get; set; }
        public int reg { get; set; }
        public string regularity { get; set; }
        public string hall { get; set; }
        public string sex { get; set; }
        public string nationality { get; set; }
        public string religion { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string blood { get; set; }
    }
}