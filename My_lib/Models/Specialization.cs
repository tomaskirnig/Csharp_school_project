using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace My_lib.Models
{
    public class Specialization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Specialization { get; set; }
        public int Id_School { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Number_Of_Available_Positions { get; set; }
        public int Duration_Of_Specialization { get; set; }
        public string Language { get; set; }
        public string Form_Of_Study { get; set; }

        public Specialization() { }

        public Specialization(int id, int idSchool, string name, string description, int numberOfAvailablePositions, int durationOfSpecialization, string language, string formOfStudy)
        {
            Id_Specialization = id;
            Id_School = idSchool;
            Name = name;
            Description = description;
            Number_Of_Available_Positions = numberOfAvailablePositions;
            Duration_Of_Specialization = durationOfSpecialization;
            Language = language;
            Form_Of_Study = formOfStudy;
        }
    }
}
