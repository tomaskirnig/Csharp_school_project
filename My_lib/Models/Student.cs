using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace My_lib.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Student { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Personal_Identification_Number { get; set; }
        public string Birth_Place { get; set; }
        public string Birth_Date { get; set; }
        public string Address { get; set; }

        public Student() { }

        public Student(int id, string firstName, string lastName, string personalIdentificationNumber, string birthPlace, string birthDate, string address)
        {
            Id_Student = id;
            First_Name = firstName;
            Last_Name = lastName;
            Personal_Identification_Number = personalIdentificationNumber;
            Birth_Place = birthPlace;
            Birth_Date = birthDate;
            Address = address;
        }
    }
}
