using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_lib.Models
{
    public class Highschool
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_School { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone_Number { get; set; }
        public string Email { get; set; }
        public string City { get; set; }

        public Highschool() { }

        public Highschool(int id, string name, string address, string phoneNumber, string email, string city)
        {
            Id_School = id;
            Name = name;
            Address = address;
            Phone_Number = phoneNumber;
            Email = email;
            City = city;
        }
    }
}
