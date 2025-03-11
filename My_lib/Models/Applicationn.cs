using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_lib.Models
{
    public class Applicationn   //two nn because of the name conflict with the Application class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Applications { get; set; }
        public int Id_School { get; set; }
        public int Id_Specialization_1 { get; set; }
        public int Id_Specialization_2 { get; set; }
        public int Id_Specialization_3 { get; set; }
        public int Id_Student { get; set; }
        public string DateOfCreation { get; set; }

        public Applicationn() { }

        public Applicationn(int id, int idSchool, int idSpecialization1, int idSpecialization2, int idSpecialization3, int idStudent, string dateOfCreation)
        {
            Id_Applications = id;
            Id_School = idSchool;
            Id_Specialization_1 = idSpecialization1;
            Id_Specialization_2 = idSpecialization2;
            Id_Specialization_3 = idSpecialization3;
            Id_Student = idStudent;
            DateOfCreation = dateOfCreation;
        }
    }
}
