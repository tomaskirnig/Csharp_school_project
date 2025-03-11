using System.ComponentModel.DataAnnotations;

namespace ASP.NET.Models
{
    public class FormStudentModel
    {
        public int Id_Student { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name can only contain letters.")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name can only contain letters.")]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "Personal Identification Number is required.")]
        [RegularExpression(@"^\d{6}/\d{4}$", ErrorMessage = "Personal Identification Number must be in the format xxxxxx/xxxx.")]
        public string Personal_Identification_Number { get; set; }

        public string Birth_Place { get; set; }

        [Required(ErrorMessage = "Birth Date is required.")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public string Birth_Date { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
    }
}
