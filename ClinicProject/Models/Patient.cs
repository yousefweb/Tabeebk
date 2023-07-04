using ClinicProject.Data.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.Models
{
    public class Patient
    {
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [Display(Name="First Name")]
        public string Patient_FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        [Display(Name = "Last Name")]
        public string Patient_LastName { get; set; }



        [Required(ErrorMessage = "Username is Required")]
        [Display(Name = "Username")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "User name must be a combination of letters and numbers only.")]
        public string Patient_UserName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Patient_Password { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Patient_Password", ErrorMessage = "Miss match")]
        [Display(Name = "Confirm_Password")]
        public string Patient_ConfirmPassword { get; set; }


        
        [Required(ErrorMessage = "Email address is required")]
        [Display(Name = "Email_Address")]
        [DataType(DataType.EmailAddress)]
        public string Patient_Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        [Display(Name = "Phone Number")]

        [DataType(DataType.PhoneNumber)]
        public int Patient_PhoneNumber { get; set; }

        [Required(ErrorMessage = "PatientAge is required")]
        [Display(Name = "Age")]

        public int Patient_Age { get; set; }


        [Required]
        [Display(Name = "Gender")]

        public Gender Patient_Gender { get; set; }

        [Required]
        [Display(Name = "City")]

        public LocationState Patient_Location { get; set; }
        [Display(Name = "About Me")]
        public string Patient_Aboutme { get; set; }
        public string Patient_SendNoteToDoctor { get; set; }

        public List<Reservation> Patient_Reservations { get; set; }

        public double Amount { get; set; }

    }

}
