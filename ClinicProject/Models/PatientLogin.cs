using System.ComponentModel.DataAnnotations;

namespace ClinicProject.Models
{
    public class PatientLogin
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Patient_Password { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string Patient_Email { get; set; }

        public bool Patient_RememberMe { get; set; }
    }
}

