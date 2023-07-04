using ClinicProject.Data.Enum;
using ClinicProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject.ViewModels
{
    public class DoctorRequestViewModel
    {
		public Guid DoctorRequestId { get; set; }

		[Required(ErrorMessage = "Please enter your first name")]
		[Display(Name = "First Name")]
		public string DoctorRequest_FirstName { get; set; }

		[Required(ErrorMessage = "Please enter your last name")]
		[Display(Name = "Last Name")]
		public string DoctorRequest_LastName { get; set; }

		[Required(ErrorMessage = "Username is Required")]
		[RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "User name must be a combination of letters and numbers only.")]
		[Remote("IsUserNameAvailable", "Doctor", HttpMethod = "POST", ErrorMessage = "User name already registered.")]
		public string DoctorRequest_UserName { get; set; }

		[Required(ErrorMessage = "Please enter your password")]
		[Display(Name = "Password")]
		public string DoctorRequest_Password { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		[Remote("IsEmailAvailable", "Doctor", HttpMethod = "POST", ErrorMessage = "Email already registered.")]
		public string DoctorRequest_Email { get; set; }

		[Required(ErrorMessage = "Please enter your phone number")]
		[Display(Name = "Phone Number")]
		public int DoctorRequest_PhoneNumber { get; set; }

		[Display(Name = "Image Profile")]
		public IFormFile DoctorRequest_ImageProfile { get; set; }


		[Display(Name = "Certification")]
		public IFormFile DoctorRequest_Certification { get; set; }

		[Required(ErrorMessage = "Please enter your age")]
		[Display(Name = "Age")]
		public int DoctorRequest_Age { get; set; }

		[Display(Name = "Register Time")]
		public DateTime DoctorRequest_RegisterTime { get; set; }
		public Gender DoctorRequest_Gender { get; set; }
		public AppointmentType DoctorRequest_AppointmentType { get; set; }

		[Required(ErrorMessage = "Please enter the number of years of experience")]
		[Display(Name = "Years of Experience")]
		public int DoctorRequest_YearsOfExperience { get; set; }
		public Specialization DoctorRequest_Specialization { get; set; }
		public StatusOfDoctor StatusOfDoctor { get; set; }
		public Clinic DoctorRequest_Clinic { get; set; }

	}
}
