using ClinicProject.Data.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System;
using System.Linq;
using ClinicProject.Models;
using ClinicProject.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ClinicProject.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Numerics;
using System.Collections.Generic;
using Microsoft.Graph.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;

namespace ClinicProject.Controllers
{

    public class DoctorController : Controller
    {
        private readonly IHostingEnvironment hosting;
        private ClinicProjectDbContext db;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signinManager;
        public DoctorController(ClinicProjectDbContext _db, IHostingEnvironment hos, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager)
        {
            db = _db;
            hosting = hos;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (user != null)
            {

                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);

                if (isDoctorAuthenticatedAsync)
                {

                    var doctor = db.Doctors.FirstOrDefault(d => d.Doctor_Email == user.Email);
                    if (doctor != null)
                    {

                        List<ClinicDay> clinicDays = new List<ClinicDay>();

                        var reservations = db.Reservations.Where(R => R.Reservation_DoctorId == doctor.DoctorId).ToList();

                        clinicDays = GetAllClinicDays(reservations);

                        var PatientReservationDictionary = new Dictionary<Reservation, Patient>();

                        var CLinicDaysDictionary = new Dictionary<Dictionary<Reservation, Patient>, List<ClinicDay>>();

                        foreach (Reservation R in reservations)
                        {

                            var Patient = db.Patients.Find(R.Reservation_PatientId);

                            if (Patient != null)
                            {

                                PatientReservationDictionary.Add(R, Patient);
                            }

                        }

                        CLinicDaysDictionary.Add(PatientReservationDictionary, clinicDays);



                        return View(CLinicDaysDictionary);
                    }
                    

                }
                return RedirectToAction("LogInDoctor", "Doctor");
            }
           

            return RedirectToAction("LogInDoctor", "Doctor");
        }


        public IActionResult IndexWithoutClinic()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterDoctor()
        {

            ViewBag.gender = Enum.GetValues(typeof(Gender));
            ViewBag.AppointmentType = Enum.GetValues(typeof(AppointmentType));
            ViewBag.Specialization = Enum.GetValues(typeof(Specialization));

            return View();
        }

      
        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(DoctorRequestViewModel model)
        {
            ViewBag.gender = Enum.GetValues(typeof(Gender));
            ViewBag.AppointmentType = Enum.GetValues(typeof(AppointmentType));
            ViewBag.Specialization = Enum.GetValues(typeof(Specialization));

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            model.DoctorRequest_RegisterTime = DateTime.Parse(time);

            if (ModelState.IsValid)
            {

                var IsEmialInUsersTable = await _userManager.FindByEmailAsync(model.DoctorRequest_Email);
                var IsUserNameInUserTable = await _userManager.FindByNameAsync(model.DoctorRequest_UserName);
                var AlreadyTakenInDoctorsTable = db.Doctors.FirstOrDefault(D => D.Doctor_Email == model.DoctorRequest_Email);
                var AlreadyTakenInDoctorRequestTable = db.DoctorRequests.FirstOrDefault(DR => DR.DoctorRequest_Email == model.DoctorRequest_Email);

                if (IsEmialInUsersTable == null && IsUserNameInUserTable== null && AlreadyTakenInDoctorsTable == null && AlreadyTakenInDoctorRequestTable==null) 
                {
                    try
                    {
                        string filenameProfile = string.Empty;
                        if (model.DoctorRequest_ImageProfile != null)
                        {
                            string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                            filenameProfile = model.DoctorRequest_ImageProfile.FileName;
                            string fullPath = Path.Combine(uploads, filenameProfile);
                            model.DoctorRequest_ImageProfile.CopyTo(new FileStream(fullPath, FileMode.Create));
                        }
                        string filenameCertification = string.Empty;
                        if (model.DoctorRequest_Certification != null)
                        {
                            string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                            filenameCertification = model.DoctorRequest_Certification.FileName;
                            string fullPath = Path.Combine(uploads, filenameCertification);
                            model.DoctorRequest_Certification.CopyTo(new FileStream(fullPath, FileMode.Create));
                        }

                       // var userEmails = await _userManager.Users.Select(u => u.Email).ToListAsync();

                        	
                        DoctorRequest doctorRequest = new DoctorRequest
                        {
                            DoctorRequestId = model.DoctorRequestId,
                            DoctorRequest_FirstName = model.DoctorRequest_FirstName,
                            DoctorRequest_LastName = model.DoctorRequest_LastName,
                            DoctorRequest_UserName = model.DoctorRequest_UserName,
                            DoctorRequest_Password = model.DoctorRequest_Password,
                            DoctorRequest_Email = model.DoctorRequest_Email,
                            DoctorRequest_PhoneNumber = model.DoctorRequest_PhoneNumber,
                            DoctorRequest_ImageProfile = filenameProfile,
                            DoctorRequest_Certification = filenameCertification,
                            DoctorRequest_Age = model.DoctorRequest_Age,
                            DoctorRequest_RegisterTime = model.DoctorRequest_RegisterTime,
                            DoctorRequest_Gender = model.DoctorRequest_Gender,
                            DoctorRequest_AppointmentType = model.DoctorRequest_AppointmentType,
                            DoctorRequest_YearsOfExperience = model.DoctorRequest_YearsOfExperience,
                            DoctorRequest_Specialization = model.DoctorRequest_Specialization,
                        };



                        db.DoctorRequests.Add(doctorRequest);
                        db.SaveChanges();

                        return RedirectToAction("index", "Home");


                    }

                    catch
                    {

                        ViewBag.gender = Enum.GetValues(typeof(Gender));
                        ViewBag.AppointmentType = Enum.GetValues(typeof(AppointmentType));
                        ViewBag.Specialization = Enum.GetValues(typeof(Specialization));

                        ModelState.AddModelError("Email", "Email already exists");
                        return View(model);
                    }
                }
                
            }

            return View();
        }


        [HttpGet]
        public IActionResult LogInDoctor()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LogInDoctor(DoctorLogIn doctorLogIn)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(doctorLogIn.Doctor_Email);

                if (user != null && await _userManager.IsInRoleAsync(user, "Doctor") && await _userManager.CheckPasswordAsync(user, doctorLogIn.Doctor_Password))
                {

                    await _signinManager.SignInAsync(user, false);
                    HttpContext.Session.SetString(user.Id, user.Id);


                    var doctor = db.Doctors
                   .FirstOrDefault(d => d.Doctor_Email == doctorLogIn.Doctor_Email);

                    var clinic = await db.Clinics.FirstOrDefaultAsync(c => c.Clinic_DoctorId == doctor.DoctorId);
                    if (clinic == null)
                    {
                        return RedirectToAction("IndexWithoutClinic", "Doctor");
                    }

                    else
                    {
                        return RedirectToAction("Index", "Doctor");
                    }


                }



                else
                {

                    ModelState.AddModelError("", "Invalid User or Pass or Unorthorized");
                    return View(doctorLogIn);
                }
            }

            else
            {

                return View(doctorLogIn);
            }


        }

        private async Task<bool> IsDoctorAuthenticatedAsync(string Key)
        {
            var DoctorId = HttpContext.Session.GetString(Key);

            if (string.IsNullOrEmpty(DoctorId))
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(DoctorId);

            if (null == user)
            {
                return false;
            }

            if (User.Identity.IsAuthenticated && await _userManager.IsInRoleAsync(user, "Doctor"))
            {
                return true;
            }

            return false;

		}

        private List<Reservation> GetAllReservation()
        {
            
            List<Reservation> reservations = new List<Reservation>();

            reservations = db.Reservations.ToList();

            return reservations;
        }

        private List<ClinicDay> GetAllClinicDays(List<Reservation> reservations)
        {

                if (reservations.Count > 0)
                {
                    List<ClinicDay> daysList = new List<ClinicDay>();
                    var firstReservation = db.Reservations.First();

                    var clinic = db.Clinics.FirstOrDefault(C => C.Clinic_DoctorId == firstReservation.Reservation_DoctorId);
                    if (clinic != null)
                    {
                        daysList = db.ClinicDays.Where(CD=>CD.ClinicId ==  clinic.ClinicId).ToList();
                        return daysList;
                    }

                }

                 return null;

            

        }


        #region ContactAdmin
        [HttpGet]
        public async Task<IActionResult> DoctorContactAdmin()
        {

            var user = _userManager.GetUserAsync(User).Result;

            if (user != null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);

                if (isDoctorAuthenticatedAsync)
                {
                    var doctor = db.Doctors.FirstOrDefault(d => d.Doctor_Email == user.Email);

                    if (doctor != null)
                    {

                        DoctorNoteToAdmin doctorNote = new DoctorNoteToAdmin();
                        doctorNote.Id = Guid.NewGuid();
                        doctorNote.Discription = "";


                        return View(doctorNote);
                    }


                   
                }

                return RedirectToAction("LogInDoctor", "Doctor");

            }

            return RedirectToAction("LogInDoctor", "Doctor");
        }

        [HttpPost]
        public async Task<IActionResult> DoctorContactAdmin(DoctorNoteToAdmin D)
        {

            var user = _userManager.GetUserAsync(User).Result;
            if(user !=null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if (isDoctorAuthenticatedAsync)
                {
                    var doctor = db.Doctors.FirstOrDefault(d => d.Doctor_Email == user.Email);

                    if (doctor != null)
                    {
                        DoctorNoteToAdmin doctorNote = new DoctorNoteToAdmin();

                        doctorNote.Id = D.Id;
                        doctorNote.Discription = D.Discription;
                        doctorNote.DoctorId = doctor.DoctorId;
                        doctorNote.MessageTime = DateTime.Now;

                        db.doctorNoteToAdmins.Add(doctorNote);

                        db.SaveChanges();

                        return RedirectToAction("Index", "Doctor");

                    }
                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return RedirectToAction("LogInDoctor", "Doctor");
        }
        #endregion

        public async Task<IActionResult> ReceiveNoteFromPatient()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if(isDoctorAuthenticatedAsync)
                {
                    var notes = db.patientNoteToDoctors.Include(n => n.patient).ToList();
                    return View(notes);
                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return RedirectToAction("LogInDoctor", "Doctor");

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> PatientsHistory (string DoctorUserName)
        {
            var user = _userManager.GetUserAsync(User).Result;
           
            if(user != null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if(isDoctorAuthenticatedAsync)
                {
                    var doctor = db.Doctors.FirstOrDefault(D => D.Doctor_Email == user.Email);
                    
                    var OldReservationWithClinicAndDoctor = new Dictionary<OldReservation, Patient>();
                    var TheHistoryOfMyReservations = db.oldReservations.Where(OR => OR.OldReservation_DoctorId == doctor.DoctorId).ToList();

                    foreach( OldReservation OldRes in TheHistoryOfMyReservations)
                    {
                        var pateint = db.Patients.Find(OldRes.OldReservation_PatientId);
                        OldReservationWithClinicAndDoctor.Add(OldRes,pateint);
                    }

                    return View(OldReservationWithClinicAndDoctor);
                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return RedirectToAction("LogInDoctor", "Doctor");
        }

        public async Task<IActionResult> DoctorProfile()
        {
            var ClinicWithDoctor = new Dictionary<Doctor, Clinic>();
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if (isDoctorAuthenticatedAsync)
                {
                    var doctor = db.Doctors.FirstOrDefault(d => d.Doctor_Email == user.Email && d.Doctor_UserName == user.UserName);
                    var clinic = db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                    ClinicWithDoctor.Add(doctor, clinic);
                    return View(ClinicWithDoctor);
                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return RedirectToAction("LogInDoctor", "Doctor");

        }
        [HttpGet]
        public async Task <IActionResult> EditProfileDoctor(Guid id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if(user != null)
            {
                bool isDoctorAuthenticatedAsync =await IsDoctorAuthenticatedAsync(user.Id);
                if(isDoctorAuthenticatedAsync)
                {
                    ViewBag.gender = Enum.GetValues(typeof(Gender));
                    ViewBag.Specialization = Enum.GetValues(typeof(Specialization));
                    var doctor = db.Doctors.Find(id);
                    return View(doctor);
                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return RedirectToAction("LogInDoctor", "Doctor");

        }
        [HttpPost]
        public async Task<IActionResult> EditProfileDoctor(Doctor doctor)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if(user != null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if(isDoctorAuthenticatedAsync)
                {

                    var d = db.Doctors.Find(doctor.DoctorId);

                    user.Email = doctor.Doctor_Email;
                    user.UserName = doctor.Doctor_UserName;

                    d.Doctor_UserName = doctor.Doctor_UserName;
                    d.Doctor_Email = doctor.Doctor_Email;
                    d.Doctor_FirstName = doctor.Doctor_FirstName;
                    d.Doctor_LastName = doctor.Doctor_LastName;
                    d.Doctor_Age = doctor.Doctor_Age;
                    d.Doctor_PhoneNumber = doctor.Doctor_PhoneNumber;
                    d.Doctor_Gender = doctor.Doctor_Gender;
                    d.Doctor_Specialization = doctor.Doctor_Specialization;

                    db.Doctors.Update(d);
                    await _userManager.UpdateAsync(user);
                    db.SaveChanges();

                }

                return RedirectToAction("DoctorProfile");
            }

            return RedirectToAction("DoctorProfile");

        }
        [HttpGet]
        public async Task <IActionResult> EditClinic(Guid id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if(user != null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if (isDoctorAuthenticatedAsync)
                {
                    ViewBag.location = Enum.GetValues(typeof(LocationState));
                    var clinic = db.Clinics.Find(id);
                    return View(clinic);
                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return RedirectToAction("LogInDoctor", "Doctor");

        }
        [HttpPost]
        public async Task<IActionResult> EditClinic(Clinic clinic)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if(user != null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if(isDoctorAuthenticatedAsync)
                {

                    var c = db.Clinics.Find(clinic.ClinicId);

                    c.Clinic_Name = clinic.Clinic_Name;
                    c.Clinc_Description = clinic.Clinc_Description;
                    c.clinc_Price = clinic.clinc_Price;
                    c.Clinic_Location = clinic.Clinic_Location;

                    db.Clinics.Update(c);
                    db.SaveChanges();
                    return RedirectToAction("DoctorProfile");
                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return RedirectToAction("LogInDoctor", "Doctor");


        }


        



    }
}
        

    
