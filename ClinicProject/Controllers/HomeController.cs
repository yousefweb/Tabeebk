using ClinicProject.ViewModels;
using ClinicProject.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ClinicProject.Models;

namespace ClinicProject.Controllers
{
    public class HomeController : Controller
    {

        private ClinicProjectDbContext db;
        public HomeController(ClinicProjectDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var doctorClinicDictionary = new Dictionary<Doctor, Clinic>();

            List<Doctor> doctors = new List<Doctor>();
            doctors.AddRange(db.Doctors);
            foreach (var doctor in doctors)
            {
                var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                if (clinic != null)
                {
                    doctorClinicDictionary.Add(doctor, clinic);
                }
            }

            return View(doctorClinicDictionary);

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DetailsOfDoctor(Guid id)
        {
            var doctorClinicDictionary = new Dictionary<Doctor, Clinic>();
            doctorClinicDictionary.Clear();
            bool isEmpty = doctorClinicDictionary.Count == 0;
            if (isEmpty)
            {
                var doctor = db.Doctors.FirstOrDefault(c => c.DoctorId == id);

                if (doctor != null)
                {
                    var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                    if (clinic != null)
                    {
                        doctorClinicDictionary.Add(doctor, clinic);

                        foreach (Clinic c in db.Clinics)
                        {

                            if (c != clinic && c.Clinic_Location.Equals(clinic.Clinic_Location))
                            {
                                Doctor nearbyD = db.Doctors.FirstOrDefault(x => x.DoctorId == c.Clinic_DoctorId);
                                doctorClinicDictionary.Add(nearbyD, c);
                                if (doctorClinicDictionary.Count == 3) { break; }
                            }
                        }

                      

                        return View(doctorClinicDictionary);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult NearbyDoctors()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(Search search)
        {
            var doctorClinicDictionary = new Dictionary<Doctor, Clinic>();
            string location = search.Location;
            string name = search.Name;
            string orderBy = search.OrderBy;
            string specialization=search.Specialization;

            //for name
            if (location == null && orderBy == "0" && name != null && specialization == "0")
            {

                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);
                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_FirstName.Equals(name))
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null)
                        {

                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }
                return View(doctorClinicDictionary);
            }
            //for orders
            else if (location == null && orderBy != "0" && name == null && specialization == "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);
                foreach (var doctor in doctors)
                {
                    var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                    if (clinic != null)
                    {
                        doctorClinicDictionary.Add(doctor, clinic);
                    }
                }
                if (orderBy == ("Price(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Price(highest first)"))
                {               
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(Lowest first)"))
                {                   
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(highest first)"))
                { 
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
            }
            //for location
            else if (location != null && orderBy == "0" && name == null && specialization == "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);
                foreach (var doctor in doctors)
                {
                    var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                    if (clinic != null && clinic.Clinic_Location.ToString().Equals(location))
                    {

                        doctorClinicDictionary.Add(doctor, clinic);
                    }

                }
                return View(doctorClinicDictionary);
            }
            //for name and location
            else if (location != null && orderBy == "0" && name != null && specialization == "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);

                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_FirstName.Equals(name))
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null && clinic.Clinic_Location.ToString().Equals(location))
                        {
                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }

                return View(doctorClinicDictionary);

            }
            //for name and orders
            else if (location == null && orderBy != "0" && name != null && specialization == "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);
                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_FirstName.Equals(name))
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null)
                        {

                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                   
                }
                if (orderBy == ("Price(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Price(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }


            }
            //for orders and location
            else if (location != null && orderBy != "0" && name == null && specialization == "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);
                foreach (var doctor in doctors)
                {
                    var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                    if (clinic != null && clinic.Clinic_Location.ToString().Equals(location))
                    {

                        doctorClinicDictionary.Add(doctor, clinic);
                    }

                }
                if (orderBy == ("Price(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Price(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }

            }
            //for name and orders and location
            else if (location != null && orderBy != "0" && name != null && specialization == "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);

                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_FirstName.Equals(name))
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null && clinic.Clinic_Location.ToString().Equals(location))
                        {
                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }
                if (orderBy == ("Price(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Price(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
            }
            //for Specialization
            else if (location == null && orderBy == "0" && name == null && specialization != "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);
                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_Specialization.ToString()==specialization)
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null)
                        {

                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }
                return View(doctorClinicDictionary);
            }
            //for Specialization and name
            else if (location == null && orderBy == "0" && name != null && specialization != "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);

                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_FirstName.Equals(name)&&doctor.Doctor_Specialization.ToString() == specialization)
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null)
                        {
                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }
                return View(doctorClinicDictionary);

            }
            //for Specialization and order 
            else if (location == null && orderBy != "0" && name == null && specialization != "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);
                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_Specialization.ToString() == specialization)
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null)
                        {

                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }
                if (orderBy == ("Price(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Price(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
            }
            //for Specialization and location
            else if (location != null && orderBy == "0" && name == null && specialization != "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);
                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_Specialization.ToString() == specialization)
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null && clinic.Clinic_Location.ToString().Equals(location))
                        {

                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }
                return View(doctorClinicDictionary);
            }
            //for Specialization and name and order
            else if (location == null && orderBy != "0" && name != null && specialization != "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);

                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_FirstName.Equals(name) && doctor.Doctor_Specialization.ToString() == specialization)
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null)
                        {
                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }
                if (orderBy == ("Price(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Price(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
            }
            //for Specialization and name and location
            else if (location != null && orderBy == "0" && name != null && specialization != "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);

                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_FirstName.Equals(name) && doctor.Doctor_Specialization.ToString() == specialization)
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null && clinic.Clinic_Location.ToString().Equals(location))
                        {
                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }
                return View(doctorClinicDictionary);
            }
            //for Specialization and orders and location
            else if (location != null && orderBy != "0" && name == null && specialization != "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);
                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_Specialization.ToString() == specialization)
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null && clinic.Clinic_Location.ToString().Equals(location))
                        {

                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }
                if (orderBy == ("Price(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Price(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
            }
            //for Specialization and orders and location and name
            else if (location != null && orderBy != "0" && name != null && specialization != "0")
            {
                List<Doctor> doctors = new List<Doctor>();
                doctors.AddRange(db.Doctors);

                foreach (var doctor in doctors)
                {
                    if (doctor.Doctor_FirstName.Equals(name) && doctor.Doctor_Specialization.ToString() == specialization)
                    {
                        var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        if (clinic != null && clinic.Clinic_Location.ToString().Equals(location))
                        {
                            doctorClinicDictionary.Add(doctor, clinic);
                        }
                    }
                }
                if (orderBy == ("Price(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Price(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.clinc_Price).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(Lowest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderBy(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
                else if (orderBy == ("Rate(highest first)"))
                {
                    var sortedDictionary = doctorClinicDictionary.OrderByDescending(x => x.Value.ClinicRate).ToDictionary(x => x.Key, x => x.Value);
                    return View(sortedDictionary);
                }
            }
            //for all null
            else if (location == null && orderBy == "0" && name == null && specialization == "0")
            { return RedirectToAction("Index"); }
                return View();
        }





       
    }
}
