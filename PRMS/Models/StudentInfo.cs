using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PRMS.Models
{
    public class StudentInfo
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string fathers_name { get; set; }
        public string mothers_name { get; set; }

        public int StudentId { get; set; }

        public int reg { get; set; }
        public string regularity { get; set; }
        public string hall { get; set; }
        public string sex { get; set; }
        public string nationality { get; set; }
        public string religion { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string blood { get; set; }


        public StudentInfo(string name, string fathers_name, string mothers_name, int id, int reg,
            string regularity, string hall, string sex, string nationality, string religion,
            string phone, string email, string blood)
        {
            this.name = name;
            this.fathers_name = fathers_name;
            this.mothers_name = mothers_name;
            this.StudentId = id;
            this.reg = reg;
            this.regularity = regularity;
            this.hall = hall;
            this.sex = sex;
            this.nationality = nationality;
            this.religion = religion;
            this.phone = phone;
            this.email = email;
            this.blood = blood;

        }
        public StudentInfo()
        {

        }


    }
}