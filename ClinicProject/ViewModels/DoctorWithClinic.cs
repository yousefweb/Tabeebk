using ClinicProject.Models;
using Microsoft.Graph.Models;
using System.Collections.Generic;

namespace ClinicProject.ViewModels
{
    public class DoctorWithClinic
    {
        public List<Doctor> doctors { get; set; }
        public List<Clinic> clinics { get; set; }

        public List<DoctorWithClinic> doctorWithClinics(List<Doctor> doctors,List<Clinic> clinics)
        {
            List<DoctorWithClinic> doctorWithClinics = new List<DoctorWithClinic>();

            foreach(Doctor doctor in doctors)
            {

            }



            return doctorWithClinics;
        }

    }
}
