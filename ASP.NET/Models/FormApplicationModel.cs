using System.ComponentModel.DataAnnotations;

namespace ASP.NET.Models
{
    public class FormApplicationModel
    {
        [Required]
        public int Id_Applications { get; set; }

        [Required(ErrorMessage = "School ID is required.")]
        public int Id_School { get; set; }

        [Required(ErrorMessage = "Specialization 1 is required.")]
        public int Id_Specialization_1 { get; set; }

        public int Id_Specialization_2 { get; set; }

        public int Id_Specialization_3 { get; set; }

        [Required(ErrorMessage = "Student is required.")]
        public int Id_Student { get; set; }

        public string DateOfCreation { get; set; }
    }
}
