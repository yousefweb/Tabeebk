using System.ComponentModel.DataAnnotations;

namespace ClinicProject.Models
{
    public class DoctorLogIn
    {

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Doctor_Password { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string Doctor_Email { get; set; }

        public bool Doctor_RememberMe { get; set; }

    }
}
