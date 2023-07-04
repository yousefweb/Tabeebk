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
    public class Doctor
    {

        public Guid DoctorId { get; set; }

        public string Doctor_FirstName { get; set; }
        public string Doctor_LastName { get; set; }
        public string Doctor_UserName { get; set; }

        [NotMapped]
        public string Doctor_Password { get; set; }
        public string Doctor_Email { get; set; }
        public int Doctor_PhoneNumber { get; set; }
        public string Doctor_ImageProfile { get; set; }
        public string Doctor_Certification { get; set; }
        public int Doctor_Age { get; set; }
        public DateTime Doctor_RegisterTime { get; set; }
        public Gender Doctor_Gender{ get; set;}
        public AppointmentType Doctor_AppointmentType {get; set;}
        public int Doctor_YearsOfExperience { get; set; }
        public Specialization Doctor_Specialization { get; set; }
        public Clinic Doctor_Clinic { get; set; }

        public double DoctorAmount { get; set; }




    }
}
