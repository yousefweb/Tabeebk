using ClinicProject.Data;
using ClinicProject.Data.Enum;
using ClinicProject.Migrations;
using ClinicProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Graph.Communications.CallRecords.CallRecordsGetDirectRoutingCallsWithFromDateTimeWithToDateTime;
using Microsoft.Graph.Models;
using Microsoft.Graph.Reports.GetOffice365ActivationsUserDetail;
using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Tavis.UriTemplates;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.FindB;

//[Authorize]
public class PatientController : Controller
{
    private UserManager<IdentityUser> _userManager;
    private SignInManager<IdentityUser> _signinManager;
    private ClinicProjectDbContext _db;
    public void SendMail(Patient patient)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Tabeebk", "tabeebksite@gmail.com"));
        email.To.Add(new MailboxAddress("" + patient.Patient_LastName + "", "" + patient.Patient_Email + ""));
        email.Subject = "About your Signup request";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = "Mr."+patient.Patient_FirstName+" "+patient.Patient_LastName+" Your reservation has been accepted\n\t\t\t Thank you for joining us"
        };
        using (var smtp = new SmtpClient())
        {
            smtp.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            smtp.Connect("smtp.gmail.com", 587, false);
            smtp.Authenticate("tabeebksite@gmail.com", "pqmxevlqhwcbdjkr");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }

    public PatientController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager, ClinicProjectDbContext db)
    {
        _userManager = userManager;
        _signinManager = signinManager;
        _db = db;
    }
    #region Register

    [HttpGet]
    [AllowAnonymous]
    public IActionResult RegisterPatient()
    {
        ViewBag.gender = Enum.GetValues(typeof(Gender));
        ViewBag.locationState = Enum.GetValues(typeof(LocationState));
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterPatient(Patient model)
    {
        ViewBag.gender = Enum.GetValues(typeof(Gender));
        ViewBag.locationState = Enum.GetValues(typeof(LocationState));
        Guid id=Guid.NewGuid();
        model.PatientId=id; 

        if (ModelState.IsValid)
        {
            var IsEmialInUsersTable = await _userManager.FindByEmailAsync(model.Patient_Email);
            var IsUserNameInUserTable = await _userManager.FindByNameAsync(model.Patient_UserName);
            var AlreadyTaken = _db.Patients.FirstOrDefault(P => P.Patient_Email == model.Patient_Email);

            if (IsEmialInUsersTable == null && AlreadyTaken == null && IsUserNameInUserTable == null)
            {
                Patient patient = new Patient
                {
                    PatientId = model.PatientId,
                    Patient_FirstName = model.Patient_FirstName,
                    Patient_LastName = model.Patient_LastName,
                    Patient_Age = model.Patient_Age,
                    Patient_Gender = model.Patient_Gender,
                    Patient_Location = model.Patient_Location,
                    Patient_PhoneNumber = model.Patient_PhoneNumber,
                    Patient_UserName = model.Patient_UserName,
                    Patient_Email = model.Patient_Email,
                    Patient_Aboutme = model.Patient_Aboutme,
                    Amount = 100,

                };


                IdentityUser user = new IdentityUser
                {
                    Email = model.Patient_Email,
                    UserName = model.Patient_UserName,
                    PhoneNumber = model.Patient_PhoneNumber.ToString(),

                };


                var result = await _userManager.CreateAsync(user, model.Patient_Password);


                if (result.Succeeded)
                {
                    _db.Patients.Add(patient);
                    _db.SaveChanges();

                    await _userManager.AddToRoleAsync(user, "Patient");

                    return RedirectToAction("LoginPatient");

                }

                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                        ViewBag.gender = Enum.GetValues(typeof(Gender));
                        ViewBag.locationState = Enum.GetValues(typeof(LocationState));
                    }

                }

            }


            else
            {
                ModelState.AddModelError("", "Email or PhoneNumber is already taken");
                ViewBag.gender = Enum.GetValues(typeof(Gender));
                ViewBag.locationState = Enum.GetValues(typeof(LocationState));
                return View(model);
            }

        }

        return View(model);
    }

    #endregion

    #region Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult LoginPatient()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]

    public async Task<IActionResult> LoginPatient(PatientLogin P)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(P.Patient_Email);

            if (user != null && await _userManager.IsInRoleAsync(user, "Patient") && await _userManager.CheckPasswordAsync(user, P.Patient_Password.ToString()))
            {
                await _signinManager.SignInAsync(user, false);
                HttpContext.Session.SetString(user.Id, user.Id);
                return RedirectToAction("Index", "Home");
            }

            else
            {

                ModelState.AddModelError("", "Invalid User or Pass or Unorthorized");
                return View(P);
            }

        }
        return View(P);

    }

    #endregion

    #region profile
    public async Task<IActionResult> profile()
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);

            if (isPatientAuthenticatedAsync == true)
            {

                List<Patient> patients = new List<Patient>();
                patients.AddRange(_db.Patients);

                foreach (var p in patients)
                {
                    if (user.Email == p.Patient_Email && user.UserName == p.Patient_UserName)
                    {
                        return View(p);

                    }

                }


                return View();

            }

        }

        return RedirectToAction("LoginPatient", "Patient");
    }
    #endregion


    private async Task<bool> IsPatientAuthenticatedAsync(string Key)
    {
        var PatientId = HttpContext.Session.GetString(Key);

        if (string.IsNullOrEmpty(PatientId))
        {
            return false;
        }

        var user = await _userManager.FindByIdAsync(PatientId);

        if (null == user)
        {
            return false;
        }

        if (User.Identity.IsAuthenticated && await _userManager.IsInRoleAsync(user, "Patient"))
        {
            return true;
        }

        return false;

    }

    #region Logout 
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await _signinManager.SignOutAsync();
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> LogoutPatient()
    {
        await _signinManager.SignOutAsync();
        return RedirectToAction("LoginPatient", "Patient");
    }

    #endregion


    [HttpPost]
    public async Task<IActionResult> BookAppointment(Guid TimeSlotOfBooking , bool isOnlinePayment)
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);

            if (isPatientAuthenticatedAsync)
            {

                var Pateint = _db.Patients.FirstOrDefault(p => p.Patient_Email == user.Email && p.Patient_UserName == user.UserName);

                if (null != Pateint)
                {
                    bool ChckDuplidateReservationTime = CheckIfPatientTakeTwoReservationAtTheSameHour(Pateint, TimeSlotOfBooking);

                    if (ChckDuplidateReservationTime)
                    {
                        // Chenage Here ----------------------

                        bool DonePay = await CheckingTheAppoitmentProcess(Pateint, TimeSlotOfBooking);

                        if (DonePay)
                        {
                            var reservation = _db.Reservations.FirstOrDefault(R => R.TimeSlotId_reservation == TimeSlotOfBooking);
                            SendMail(Pateint);
                            TempData["Message"] = "You have another appointment at the same time.";
                            return RedirectToAction("Index", "Home");

                        }


                        return RedirectToAction("ClinicAppointmentTimes", "Clinic");

                    }

                    else
                    {
                        // Chenage Here ----------------------

                        bool DonePay = await CheckingTheAppoitmentProcess(Pateint, TimeSlotOfBooking);

                        if (DonePay)
                        {
                            SendMail(Pateint);
                            return RedirectToAction("DetailsOfDoctor", "Home");

                        }

                        return RedirectToAction("ClinicAppointmentTimes", "Clinic");


                    }

                }
            }

            return RedirectToAction("LoginPatient", "Patient");
        }

        return RedirectToAction("LoginPatient", "Patient");

    }

    private async Task<bool> CheckingTheAppoitmentProcess(Patient patient , Guid TimeSlotOfBookingId)
    {
        Reservation reservation;

        var TimeSlot = _db.timeSlots.Find(TimeSlotOfBookingId);

        if (null != TimeSlot)
        {
            var ClinicDay = _db.ClinicDays.Find(TimeSlot.ClinicDayId);

            if (null != ClinicDay)
            {
                var clinic = _db.Clinics.Find(ClinicDay.ClinicId);

                if (null != clinic)
                {
                    var doctor = _db.Doctors.Find(clinic.Clinic_DoctorId);


                        reservation = new Reservation
                        {
                            ReservationId = Guid.NewGuid(),
                            Reservation_DoctorId = clinic.Clinic_DoctorId,
                            Reservation_PatientId = patient.PatientId,
                            Strat_reservation = TimeSlot.StartTime,
                            End_reservation = TimeSlot.EndTime,
                            Reservation_Day = TimeSlot.ClinicDay.DayOfWeek,
                            Reservation_Specialization = doctor.Doctor_Specialization,
                            situationOfReservation = SituationOfReservation.Accept,
                            TimeSlotId_reservation = TimeSlot.TimeSlotId,
                            Reservation_AppointmentType = doctor.Doctor_AppointmentType,
                            ReservationAmount = clinic.clinc_Price,
                        };



                        bool isDoneThePymentProcess =  UpdatePatientAmountForAddNewReservation(patient, clinic.clinc_Price , doctor.DoctorId);

                        if (isDoneThePymentProcess)
                        {
                            TimeSlot.IsAvailable = false;
                            _db.Reservations.Add(reservation);
                            _db.SaveChanges();
                            return true;
                        }

                       
                }

            }
        }


       return false;
    }

    public bool UpdatePatientAmountForAddNewReservation(Patient patient , double clinc_Price , Guid DoctorId)
    {
        var doctor = _db.Doctors.Find(DoctorId);
        

        if (patient.Amount > 0 && doctor != null)
        {

            double NewPatientAmount = patient.Amount - clinc_Price;

            if (NewPatientAmount >= 0)
            {
                var owners = _db.Owners.ToList();

                double ThePercentageOfTheWebSite = clinc_Price * 0.10;
                double ThePercentageOfTheDoctr  = clinc_Price - ThePercentageOfTheWebSite;

                foreach(Owner owner in owners)
                {
                    double OwnerAmount =  owner.Owner_Amount;
                    owner.Owner_Amount = OwnerAmount + ThePercentageOfTheWebSite;
                    _db.Update(owner);
                    _db.SaveChanges();
                }

                double DoctorAmount =   doctor.DoctorAmount;
                doctor.DoctorAmount = DoctorAmount + ThePercentageOfTheDoctr;

                patient.Amount = NewPatientAmount;


                _db.SaveChanges();
                return true;
            }
            return false;
        }

        return false;

    }


    public IActionResult AppointmentConfirmation()
    {
        return View();
    }
    #region  Edit Profile
    [HttpGet]
    public async Task<IActionResult> EditProfileP(Guid id)
    { 
        // this page to Faild Payment
        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);

            if (isPatientAuthenticatedAsync)
            {
                ViewBag.gender = Enum.GetValues(typeof(Gender));
                ViewBag.locationState = Enum.GetValues(typeof(LocationState));
                Patient P = _db.Patients.Find(id);
                return View(P);
            }

            return RedirectToAction("LoginPatient", "Patient");
        }

        return RedirectToAction("LoginPatient", "Patient");
    }
    [HttpPost]
    public async Task<IActionResult> EditProfileP(Patient P)
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);

            if (isPatientAuthenticatedAsync)
            {
                _db.Patients.Update(P);
                _db.SaveChanges();
                return RedirectToAction("profile");
            }

            return RedirectToAction("LoginPatient", "Patient");
        }

        return RedirectToAction("LoginPatient", "Patient");

    }
    #endregion

    #region ContactAdmin
    [HttpGet]
    public async Task<IActionResult> PatientContactAdmin()
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);

            if (isPatientAuthenticatedAsync)
            {
                var patient = _db.Patients.FirstOrDefault(p => p.Patient_Email == user.Email);

                if (patient != null)
                {

                    PatientNoteToAdmin patientNote = new PatientNoteToAdmin();
                    patientNote.id = Guid.NewGuid();
                    patientNote.Discription = "";


                    return View(patientNote);
                }
            }

            return RedirectToAction("LoginPatient", "Patient");

        }

        return RedirectToAction("LoginPatient", "Patient");
    }

    [HttpPost]
    public async Task<IActionResult> PatientContactAdmin(PatientNoteToAdmin P)
    {

        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);

            if (isPatientAuthenticatedAsync)
            {
                var patient = _db.Patients.FirstOrDefault(p => p.Patient_Email == user.Email);

                if (patient != null)
                {
                    PatientNoteToAdmin patientNoteToAdmin = new PatientNoteToAdmin();

                    patientNoteToAdmin.id = P.id;
                    patientNoteToAdmin.Discription = P.Discription;
                    patientNoteToAdmin.PatientId = patient.PatientId;
                    patientNoteToAdmin.MessageTime = DateTime.Now;
                    _db.patientNoteToAdmins.Add(patientNoteToAdmin);

                    _db.SaveChanges();

                    return RedirectToAction("Index", "Home");

                }
            }

            return RedirectToAction("LoginPatient", "Patient");

        }

        return RedirectToAction("LoginPatient", "Patient");
    }
    #endregion
    public async Task<IActionResult> ReservationForPatient(Guid id)
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);

            if (isPatientAuthenticatedAsync)
            {
                var RWithDName = new Dictionary<Reservation, (Doctor, Clinic)>();
                List<Reservation> reservations = new List<Reservation>();

                foreach (Reservation PR in _db.Reservations)
                {
                    if (PR.Reservation_PatientId == id)
                    {
                        var doctor = _db.Doctors.Find(PR.Reservation_DoctorId);
                        var clinic = _db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                        RWithDName.Add(PR, (doctor, clinic));
                    }
                }

                return View(RWithDName);

            }

            return RedirectToAction("LoginPatient", "Patient");

        }

        return RedirectToAction("LoginPatient", "Patient");

    }
    [HttpGet]
    public async Task<IActionResult> PatientContactDoctor()
    {

        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);

            if (isPatientAuthenticatedAsync)
            {
                var patient = _db.Patients.FirstOrDefault(p => p.Patient_Email == user.Email);
                var reservation = _db.Reservations.FirstOrDefault(r => r.ReservationId == r.ReservationId); // here what is this ((r => r.ReservationId == r.ReservationId)


                if (patient != null)
                {
                    PatientNoteToDoctor patientNote = new PatientNoteToDoctor
                    {
                        Id = Guid.NewGuid(),
                        Note = "",
                        PatientId = patient.PatientId,
                        DoctorId = reservation.Reservation_DoctorId,
                    };



                    return View(patientNote);
                }
            }

            return RedirectToAction("LoginPatient", "Patient");
        }

        return RedirectToAction("LoginPatient", "Patient");
    }
    [HttpPost]
    public async Task <IActionResult> PatientContactDoctor(PatientNoteToDoctor P)
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);

            if (isPatientAuthenticatedAsync)
            {
                var patient = _db.Patients.FirstOrDefault(p => p.Patient_Email == user.Email);
                var reservation = _db.Reservations.FirstOrDefault(r => r.ReservationId == r.ReservationId);

                if (patient != null)
                {
                    PatientNoteToDoctor patientNoteToDoctor = new PatientNoteToDoctor
                    {
                        Id = P.Id,
                        Note = P.Note,
                        PatientId = patient.PatientId,
                        DoctorId = reservation.Reservation_DoctorId,
                        MessageTime = DateTime.Now

                    };

                    _db.patientNoteToDoctors.Add(patientNoteToDoctor);
                    _db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("LoginPatient", "Patient");
        }

        return RedirectToAction("LoginPatient", "Patient");
    }

    // what is this page !!!!!!!!!!!!!!!!!
    public IActionResult FaildPaymentPage()
    {
        return View();
    }
    public bool CheckIfPatientTakeTwoReservationAtTheSameHour(Patient patient , Guid TimeSlotId)
    {
        var PateintReservation = _db.Reservations.Where(R => R.Reservation_PatientId == patient.PatientId).ToList();

        var TimeSlot = _db.timeSlots.FirstOrDefault(R => R.TimeSlotId == TimeSlotId);

        if (PateintReservation != null && TimeSlot != null && PateintReservation.Any())
        {
            foreach(var reservation in PateintReservation)
            {
                if(reservation.Strat_reservation == TimeSlot.StartTime)
                {
                    return true;
                }
            }
            
        }

        return false;
    }

    [HttpPost]
    public async Task<IActionResult> Rating(int rate , Guid clinicId)
    {
        var user = _userManager.GetUserAsync(User).Result;

        if (user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);

            if (isPatientAuthenticatedAsync)
            {
                var Patient = _db.Patients.FirstOrDefault(P => P.Patient_Email == user.Email);
                var clinic = _db.Clinics.FirstOrDefault(C => C.ClinicId == clinicId);

                if (clinic != null && Patient != null)
                {
                    var ClinicRates = _db.clinicRates.Where(PR => PR.ClinicId == clinic.ClinicId).ToList();
                    var PatientRateObejct = ClinicRates.FirstOrDefault(PR => PR.PatientId == Patient.PatientId);

                    if (PatientRateObejct == null)
                    {
                        ClinicRate NewClinicRate = new ClinicRate();

                        NewClinicRate.ClinicRateId = Guid.NewGuid();
                        NewClinicRate.PatientId = Patient.PatientId;
                        NewClinicRate.ClinicId = clinic.ClinicId;
                        NewClinicRate.RateOfPAtient = rate;

                        _db.clinicRates.Add(NewClinicRate);
                        _db.SaveChanges();

                    }

                    else
                    {
                        PatientRateObejct.RateOfPAtient = rate;
                        _db.SaveChanges();

                    }

                    var ClinicRatesAfterUpdate = _db.clinicRates.Where(PR => PR.ClinicId == clinic.ClinicId).ToList();
                    int sum = ClinicRatesAfterUpdate.Sum(PR => PR.RateOfPAtient);
                    int numberOfRates = ClinicRatesAfterUpdate.Count;

                    clinic.ClinicRate = sum / numberOfRates;
                    _db.Update(clinic);
                    _db.SaveChanges();

                }
            }

            return RedirectToAction("LoginPatient", "Patient");
        }

        return RedirectToAction("LoginPatient", "Patient");
    }

    public async Task<IActionResult> OldReservationForPatient(Guid id)
    {
        var user = _userManager.GetUserAsync(User).Result;
        if(user != null)
        {
            bool isPatientAuthenticatedAsync = await IsPatientAuthenticatedAsync(user.Id);
            {

                if(isPatientAuthenticatedAsync)
                {
                    var OldReservationWithClinicAndDoctor = new Dictionary<OldReservation, (Doctor, Clinic)>();
                    var OldResrvationsOfThePatient = _db.oldReservations.Where(OR=>OR.OldReservation_PatientId == id).ToList();

                    foreach (OldReservation OR in OldResrvationsOfThePatient)
                    {

                            var doctor = _db.Doctors.Find(OR.OldReservation_DoctorId);
                            var clinic = _db.Clinics.FirstOrDefault(c => c.Clinic_DoctorId == doctor.DoctorId);
                            OldReservationWithClinicAndDoctor.Add(OR, (doctor, clinic));
                        
                    }

                    return View(OldReservationWithClinicAndDoctor);

                }
            }

            return RedirectToAction("LoginPatient", "Patient");
        }

        return RedirectToAction("LoginPatient", "Patient");
    }


  
}