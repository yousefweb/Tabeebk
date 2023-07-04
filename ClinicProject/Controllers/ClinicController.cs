using ClinicProject.Data;
using ClinicProject.Data.Enum;
using ClinicProject.Models;
using ClinicProject.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Graph.DeviceManagement.DeviceConfigurations.Item.GetOmaSettingPlainTextValueWithSecretReferenceValueId;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Weibull_Dist;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace ClinicProject.Controllers
{
    public class ClinicController : Controller
    {
        private ClinicProjectDbContext db;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signinManager;
        private List<ClinicDay> day = new List<ClinicDay>();
        public void SendMailToDoctor(Patient patient,Doctor doctor)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Tabeebk", "tabeebksite@gmail.com"));
            email.To.Add(new MailboxAddress("" + doctor.Doctor_FirstName + "", "" + doctor.Doctor_Email + ""));
            email.Subject = "About your Signup request";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = "Mr." + patient.Patient_FirstName + " " + patient.Patient_LastName + " canceled his appointment\n\t\t\t Thank you for joining us"
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
        public void SendMailToPatient(Patient patient, Doctor doctor)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Tabeebk", "tabeebksite@gmail.com"));
            email.To.Add(new MailboxAddress("" + patient.Patient_FirstName + "", "" + patient.Patient_Email + ""));
            email.Subject = "About your Signup request";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = "Mr." + patient.Patient_FirstName + " " + patient.Patient_LastName + " Your appointment with "+doctor.Doctor_FirstName +" "+doctor.Doctor_LastName+" has been cancelled. \n\t\t\t Thank you for joining us"
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

        public ClinicController(ClinicProjectDbContext _db, UserManager<IdentityUser> userManager,
         SignInManager<IdentityUser> signinManager)
        {

            db = _db;
            _signinManager = signinManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddClinic()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if(user !=  null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if (isDoctorAuthenticatedAsync)
                {
                    ViewBag.Location = Enum.GetValues(typeof(LocationState));
                    return View();
                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return RedirectToAction("LogInDoctor", "Doctor");

        }

        [HttpPost]
        public async Task<IActionResult> AddClinic(Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserAsync(User).Result;

                if(user != null )
                {
                    bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                    if(isDoctorAuthenticatedAsync)
                    {
                        var doctor = db.Doctors.FirstOrDefault(D => D.Doctor_Email == user.Email);

                        if (doctor != null)
                        {
                            Clinic NewClinicForDoc;

                            NewClinicForDoc = new Clinic()
                            {

                                ClinicId = Guid.NewGuid(),
                                Clinic_DoctorId = doctor.DoctorId,
                                clinc_Price = clinic.clinc_Price,
                                ClinicRate = clinic.ClinicRate,
                                Clinic_Location = clinic.Clinic_Location,
                                Clinc_Description = clinic.Clinc_Description,
                                Clinic_Name = clinic.Clinic_Name,
                                Clinic_Days = clinic.Clinic_Days,
                                Days = clinic.Days,

                            };

                            db.Clinics.Add(NewClinicForDoc);
                            db.SaveChanges();

                            return RedirectToAction("TimeOfDay", NewClinicForDoc);
                        }
                        
                    }

                    return RedirectToAction("LogInDoctor", "Doctor");
                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return View(clinic);
        }

        [HttpGet]
        public async Task<IActionResult> TimeOfDay(Clinic clinic)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if(isDoctorAuthenticatedAsync)
                {
                    var doctor = db.Doctors.FirstOrDefault(D => D.Doctor_Email == user.Email);
                    if(doctor != null)
                    {
                        Dictionary<String, DateTime> ListOfTime = new Dictionary<string, DateTime>();
                        Dictionary<Days, DateTime> Coming = new Dictionary<Days, DateTime>();

                        if (clinic.Days == "Saturday-Monday-Wednesday")
                        {



                            clinic.Clinic_Days = new List<ClinicDay>();

                            ClinicDay Sat = new ClinicDay();
                            Sat.ClinicDayId = Guid.NewGuid();
                            Sat.DayOfWeek = (Days)DayOfWeek.Saturday;
                            Sat.ClinicId = clinic.ClinicId;



                            ClinicDay Mon = new ClinicDay();
                            Mon.ClinicDayId = Guid.NewGuid();
                            Mon.DayOfWeek = (Days)DayOfWeek.Monday;
                            Mon.ClinicId = clinic.ClinicId;

                            ClinicDay Wed = new ClinicDay();
                            Wed.ClinicDayId = Guid.NewGuid();
                            Wed.DayOfWeek = (Days)DayOfWeek.Wednesday;
                            Wed.ClinicId = clinic.ClinicId;

                            clinic.Clinic_Days.Add(Sat);
                            clinic.Clinic_Days.Add(Mon);
                            clinic.Clinic_Days.Add(Wed);

                            foreach (var day in clinic.Clinic_Days)
                            {

                                db.ClinicDays.Add(day);

                            }


                        }
                        else if (clinic.Days == "Sunday-Tuesday-Thursday")
                        {
                            clinic.Clinic_Days = new List<ClinicDay>();

                            ClinicDay Sun = new ClinicDay();
                            Sun.ClinicDayId = Guid.NewGuid();
                            Sun.DayOfWeek = (Days)DayOfWeek.Sunday;
                            Sun.ClinicId = clinic.ClinicId;

                            ClinicDay Tues = new ClinicDay();
                            Tues.ClinicDayId = Guid.NewGuid();
                            Tues.DayOfWeek = (Days)DayOfWeek.Tuesday;
                            Tues.ClinicId = clinic.ClinicId;

                            ClinicDay Thurs = new ClinicDay();
                            Thurs.ClinicDayId = Guid.NewGuid();
                            Thurs.DayOfWeek = (Days)DayOfWeek.Thursday;
                            Thurs.ClinicId = clinic.ClinicId;

                            clinic.Clinic_Days.Add(Sun);
                            clinic.Clinic_Days.Add(Tues);
                            clinic.Clinic_Days.Add(Thurs);

                            foreach (var day in clinic.Clinic_Days)
                            {

                                db.ClinicDays.Add(day);

                            }
                        }

                        else if (clinic.Days == "Sunday-Monday-Tuesday-Wednesday-Thursday")
                        {
                            clinic.Clinic_Days = new List<ClinicDay>();

                            ClinicDay Sun = new ClinicDay();
                            Sun.ClinicDayId = Guid.NewGuid();
                            Sun.DayOfWeek = (Days)DayOfWeek.Sunday;
                            Sun.ClinicId = clinic.ClinicId;

                            ClinicDay Mon = new ClinicDay();
                            Mon.ClinicDayId = Guid.NewGuid();
                            Mon.DayOfWeek = (Days)DayOfWeek.Monday;
                            Mon.ClinicId = clinic.ClinicId;

                            ClinicDay Tues = new ClinicDay();
                            Tues.ClinicDayId = Guid.NewGuid();
                            Tues.DayOfWeek = (Days)DayOfWeek.Tuesday;
                            Tues.ClinicId = clinic.ClinicId;

                            ClinicDay Wed = new ClinicDay();
                            Wed.ClinicDayId = Guid.NewGuid();
                            Wed.DayOfWeek = (Days)DayOfWeek.Wednesday;
                            Wed.ClinicId = clinic.ClinicId;

                            ClinicDay Thurs = new ClinicDay();
                            Thurs.ClinicDayId = Guid.NewGuid();
                            Thurs.DayOfWeek = (Days)DayOfWeek.Thursday;
                            Thurs.ClinicId = clinic.ClinicId;

                            clinic.Clinic_Days.Add(Sun);
                            clinic.Clinic_Days.Add(Mon);
                            clinic.Clinic_Days.Add(Tues);
                            clinic.Clinic_Days.Add(Wed);
                            clinic.Clinic_Days.Add(Thurs);

                            foreach (var day in clinic.Clinic_Days)
                            {

                                db.ClinicDays.Add(day);
                            }
                        }

                        else if (clinic.Days == "Saturday-Sunday-Monday-Tuesday-Wednesday-Thursday")
                        {
                            clinic.Clinic_Days = new List<ClinicDay>();

                            ClinicDay Sat = new ClinicDay();
                            Sat.ClinicDayId = Guid.NewGuid();
                            Sat.DayOfWeek = (Days)DayOfWeek.Saturday;
                            Sat.ClinicId = clinic.ClinicId;

                            ClinicDay Sun = new ClinicDay();
                            Sun.ClinicDayId = Guid.NewGuid();
                            Sun.DayOfWeek = (Days)DayOfWeek.Sunday;
                            Sun.ClinicId = clinic.ClinicId;

                            ClinicDay Mon = new ClinicDay();
                            Mon.ClinicDayId = Guid.NewGuid();
                            Mon.DayOfWeek = (Days)DayOfWeek.Monday;
                            Mon.ClinicId = clinic.ClinicId;

                            ClinicDay Tues = new ClinicDay();
                            Tues.ClinicDayId = Guid.NewGuid();
                            Tues.DayOfWeek = (Days)DayOfWeek.Tuesday;
                            Tues.ClinicId = clinic.ClinicId;

                            ClinicDay Wed = new ClinicDay();
                            Wed.ClinicDayId = Guid.NewGuid();
                            Wed.DayOfWeek = (Days)DayOfWeek.Wednesday;
                            Wed.ClinicId = clinic.ClinicId;

                            ClinicDay Thurs = new ClinicDay();
                            Thurs.ClinicDayId = Guid.NewGuid();
                            Thurs.DayOfWeek = (Days)DayOfWeek.Thursday;
                            Thurs.ClinicId = clinic.ClinicId;

                            clinic.Clinic_Days.Add(Sat);
                            clinic.Clinic_Days.Add(Sun);
                            clinic.Clinic_Days.Add(Mon);
                            clinic.Clinic_Days.Add(Tues);
                            clinic.Clinic_Days.Add(Wed);
                            clinic.Clinic_Days.Add(Thurs);

                            foreach (var day in clinic.Clinic_Days)
                            {
                                db.ClinicDays.Add(day);
                            }

                        }



                        foreach (var day in clinic.Clinic_Days)
                        {
                            Coming = GetDateTimeForDays();

                            if (Coming.ContainsKey(day.DayOfWeek))
                            {
                                day.DateOfWork = Coming[day.DayOfWeek];
                            }
                        }


                        db.Update(clinic);
                        db.SaveChanges();



                        return View(clinic);
                    }
                }
                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return RedirectToAction("LogInDoctor", "Doctor");
        }

        [HttpPost]
        public async Task<IActionResult> TimeOfDaydone(Clinic model)
        {

            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserAsync(User).Result;
                if (user != null)
                {
                    bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                    if (isDoctorAuthenticatedAsync)
                    {
                        var doctor = db.Doctors.FirstOrDefault(D => D.Doctor_Email == user.Email);
                        if (doctor != null)
                        {
                            var clinic = db.Clinics.Where(c => c.Clinic_DoctorId == doctor.DoctorId).FirstOrDefault();

                            if (clinic != null)
                            {

                                foreach (var day in model.Clinic_Days)
                                {
                                    var DayInDataBase = db.ClinicDays.FirstOrDefault(d => d.ClinicDayId == day.ClinicDayId);

                                    if (DayInDataBase != null)
                                    {

                                        DayInDataBase.StartTime = day.StartTime;
                                        DayInDataBase.EndTime = day.EndTime;
                                    }
                                }

                                db.SaveChanges();

                                foreach (var day in db.ClinicDays.Where(CD => CD.ClinicId == clinic.ClinicId))
                                {

                                    day.StartTime = new DateTime(day.DateOfWork.Year, day.DateOfWork.Month, day.DateOfWork.Day, day.StartTime.Hour, day.StartTime.Minute, day.StartTime.Second);
                                    day.EndTime = new DateTime(day.DateOfWork.Year, day.DateOfWork.Month, day.DateOfWork.Day, day.EndTime.Hour, day.EndTime.Minute, day.EndTime.Second);

                                }


                                db.SaveChanges();


                                GenerateClinicDayTimeSlots(clinic.ClinicId);
                                return RedirectToAction("Index", "Doctor");
                            }

                        }
                    }

                    return RedirectToAction("LogInDoctor", "Doctor");
                    

                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return View(model);
        }
        public async Task<IActionResult> times()
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (user != null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if (isDoctorAuthenticatedAsync)
                {
                    var doctor = db.Doctors.FirstOrDefault(D => D.Doctor_Email == user.Email);
                    if (doctor != null)
                    {
                        var clinic = db.Clinics.Where(c => c.Clinic_DoctorId == doctor.DoctorId).FirstOrDefault();


                        if (clinic != null)
                        {

                            List<ClinicDay> clinicDaysThatBelongsToClinic = await db.ClinicDays
                                .Where(cd => cd.ClinicId == clinic.ClinicId).ToListAsync();


                            return View(clinic);

                        }
                        return View();
                    }
                }

                return RedirectToAction("LogInDoctor", "Doctor");
            }
      
            return RedirectToAction("LogInDoctor", "Doctor");
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


        [HttpGet]
        public IActionResult ClinicAppointmentTimes(Guid id)
        {
            var clinic = db.Clinics.Find(id);

           var DaysAndTimesDictionary = new Dictionary<ClinicDay, List<TimeSlot>>();

            if (null != clinic)
            {
                var Days = db.ClinicDays.Where(D => D.ClinicId ==  clinic.ClinicId).ToList();

                if (Days.Any())
                {

                    // here to check and Update the DayOfwork in the ClinicDay
                    UpdateDayOfworkForEachDayClinic(clinic.ClinicId);


                    foreach (var Day in Days.OrderBy(D=>D.DayOfWeek))
                    {
                        List<TimeSlot> timeSlot = db.timeSlots.Where(T => T.ClinicDayId == Day.ClinicDayId).OrderBy(t=>t.StartTime).ToList();
                        DaysAndTimesDictionary.Add(Day, timeSlot);
                    }

                    
                    
                    return View(DaysAndTimesDictionary);

                }

            }

            return RedirectToAction("Index","Home");
        }

        public void GenerateClinicDayTimeSlots(Guid id)
        {
           var  clinic = db.Clinics.Find(id);
           
           var clinicDays = new List<ClinicDay>();

            clinicDays = db.ClinicDays.Where(CD => CD.ClinicId == id).ToList();   

            if (clinic != null && clinicDays.Any())
            {

                foreach(var day in clinicDays)
                {

                    DateTime currentTime = day.StartTime;

                    while(currentTime < day.EndTime)
                    {

                        TimeSlot timeSlot = new TimeSlot
                        {

                            TimeSlotId = Guid.NewGuid(),
                            StartTime = currentTime,
                            EndTime = currentTime.AddHours(1),
                            IsAvailable = true ,
                            ClinicDayId = day.ClinicDayId,
                            
                        };

                        db.timeSlots.Add(timeSlot);
                        day.TimeSlots.Add(timeSlot);
                        currentTime = currentTime.AddHours(1);
                        
                    }

                   


                }

                db.SaveChanges();

            }
        }

        public static Dictionary<Days, DateTime> GetDateTimeForDays()
        {
            DateTime today = DateTime.Today;
            DayOfWeek currentDayOfWeek = today.DayOfWeek;
            int diff = (7 + (currentDayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime startOfWeek = today.AddDays(-1 * diff);

            Dictionary<Days, DateTime> daysOfWeek = new Dictionary<Days, DateTime>();

            for (int i = 0; i < 7; i++)
            {
                Days dayOfWeek = (Days)((i + 1) % 7);
                DateTime date = startOfWeek.AddDays(i);
                daysOfWeek.Add(dayOfWeek, date);
            }

            return daysOfWeek;
        }

        public void UpdateDayOfworkForEachDayClinic(Guid id)
        {
            var CurrentDateTime = DateTime.Today;

            var AllCinicDays = db.ClinicDays.ToList();

            foreach (var day in AllCinicDays)
            {
               
                if (day.DateOfWork < CurrentDateTime)
                {
                    var nextWeekDate = day.DateOfWork.AddDays(7);

                    day.DateOfWork = new DateTime(nextWeekDate.Year, nextWeekDate.Month, nextWeekDate.Day, day.DateOfWork.Hour, day.DateOfWork.Minute, day.DateOfWork.Second);


                    DelateAllPastReservationsBasedOnDate(day.ClinicDayId);
                    UpdateTimeSlotsDates(day.ClinicDayId, nextWeekDate , day);

                }
                
            }
            
            db.SaveChanges();


        }

        public async Task<IActionResult> RemoveTheReservation(Guid reservationId)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if(user !=null)
            {
                bool isDoctorAuthenticatedAsync = await IsDoctorAuthenticatedAsync(user.Id);
                if (isDoctorAuthenticatedAsync)
                {
                    var reservation = db.Reservations.Find(reservationId);
                    var doctor = db.Doctors.Find(reservation.Reservation_DoctorId);
                    var patient = db.Patients.Find(reservation.Reservation_PatientId);

                    if (reservation != null)
                    {

                        bool thePatientPercentageIsreturnBack = RemoveReservationWithoutRefundThePercentageFromSite(reservationId);
                        bool RemoevReservationIsDone = checkForDelete(reservation, false);

                        if (RemoevReservationIsDone)
                        {
                            SendMailToPatient(patient, doctor);
                            return RedirectToAction("Index", "Doctor");

                        }
                    }

                    return View(reservation);

                }
                return RedirectToAction("LogInDoctor", "Doctor");
            }

            return RedirectToAction("LogInDoctor", "Doctor");


        }

        private bool checkForDelete(Reservation reservation ,bool IsTrue)
        {
            var TimeSlotOfReservation = db.timeSlots.Find(reservation.TimeSlotId_reservation);
            var Patient = db.Patients.Find(reservation.Reservation_PatientId);

            if(Patient != null && TimeSlotOfReservation !=null)
            {

                // this True of False to check if False that means Doctor Remove Reservation and we do not wanna to add him to Old reservation , and if True that means system delete the past reservation and we need to add them to old reservation 
                if(IsTrue) 
                {
                    AddRerservatioToOldReservationTable(reservation.ReservationId);
                }
                
                TimeSlotOfReservation.IsAvailable = true;
                
                db.Reservations.Remove(reservation);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        // this Code will be for Cancel Reservation from PAtient Side
        public IActionResult PatientCancelHisReservation(Guid id)
        {
            var reservation = db.Reservations.Find(id);

            if (reservation != null)
            {
                //bool Istrue = CheckIfTimeBetweenNowAndTheStartAppointmentMoreThen12Hour(reservation.ReservationId);

               
                    var TimeSlotOfReservation = db.timeSlots.Find(reservation.TimeSlotId_reservation);
                    var Patient = db.Patients.Find(reservation.Reservation_PatientId);
                    var doctor = db.Doctors.Find(reservation.Reservation_DoctorId);


                    if (Patient != null && TimeSlotOfReservation != null)
                    {
                       bool isDone =  CompleteTheRefundFromTheCencelReservationProcess(reservation.ReservationId);

                        if (isDone)
                        {
                         SendMailToDoctor(Patient, doctor);
                          TimeSlotOfReservation.IsAvailable = true;
                          db.Reservations.Remove(reservation);
                          db.SaveChanges();
                        }
                        


                        return RedirectToAction("ReservationForPatient", "Patient");
                    }
                
                
            }

          
            return RedirectToAction("ReservationForPatient", "Patient");
        }

        // this function to Update the Date of the Day if he become from the past
        public void UpdateTimeSlotsDates(Guid id, DateTime dateTime , ClinicDay day)
        {
            day.StartTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, day.StartTime.Hour, day.StartTime.Minute, day.StartTime.Second);
            day.EndTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, day.EndTime.Hour, day.EndTime.Minute, day.EndTime.Second);

            var ListOfTimeSLot = db.timeSlots.Where(TS => TS.ClinicDayId == id);

            foreach (var Slot in ListOfTimeSLot)
            {
                Slot.StartTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, Slot.StartTime.Hour, Slot.StartTime.Minute, Slot.StartTime.Second);
                Slot.EndTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, Slot.EndTime.Hour, Slot.EndTime.Minute, Slot.EndTime.Second);
            }

            db.SaveChanges();

        }

        // this funcation to Delete All the reservation that belongs to the past day and make the it new Availabe Aagin
        public void DelateAllPastReservationsBasedOnDate(Guid id)
        {

            var AllSlotsThatBelongsToThePastDay = db.timeSlots.Where(TS => TS.ClinicDayId == id).ToList();

            foreach (var Slot in AllSlotsThatBelongsToThePastDay)
            {
                var reservation = db.Reservations.FirstOrDefault(R => R.TimeSlotId_reservation == Slot.TimeSlotId);
                if (reservation != null)
                {

                    bool IsDone = checkForDelete(reservation , true);

                }
            }

        }

        private void AddRerservatioToOldReservationTable(Guid id)
        {
            var oldReservation = db.Reservations.Find(id);
            if (oldReservation != null)
            {
                var NewOldReservation = new OldReservation
                {
                    OldReservationId = oldReservation.ReservationId,
                    OldReservation_DoctorId = oldReservation.Reservation_DoctorId,
                    OldReservation_PatientId = oldReservation.Reservation_PatientId,
                    OldReservation_Specialization = oldReservation.Reservation_Specialization,
                    OldReservation_Day = oldReservation.Reservation_Day,
                    Strat_Oldreservation = oldReservation.Strat_reservation,
                    End_Oldreservation = oldReservation.End_reservation,

                };

                db.oldReservations.Add(NewOldReservation);

            }
        }

        private bool RemoveReservationWithoutRefundThePercentageFromSite(Guid id)
        {
            var reservation = db.Reservations.Find(id);

            if (reservation != null)
            {
                
                var doctor = db.Doctors.Find(reservation.Reservation_DoctorId);
                var patient = db.Patients.Find(reservation.Reservation_PatientId);

                double ValueOfRefund = reservation.ReservationAmount;
                double AmountOfDoctorAfterRefund = doctor.DoctorAmount - ValueOfRefund;

                if (AmountOfDoctorAfterRefund >=0) 
                {
                    doctor.DoctorAmount = AmountOfDoctorAfterRefund;
                    patient.Amount += ValueOfRefund;


                    db.Update(doctor);
                    db.Update(patient);
                    db.SaveChanges();
                }


            }

            return false;

        }

        private bool CheckIfTimeBetweenNowAndTheStartAppointmentMoreThen12Hour(Guid id)
        {
            var reservation = db.Reservations.Find(id);

            if(reservation != null)
            {
                DateTime TimeNow = DateTime.Now;
                TimeSpan timeDifference = reservation.Strat_reservation - TimeNow;
                if (timeDifference.TotalHours > 12)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CompleteTheRefundFromTheCencelReservationProcess(Guid id)
        {

            // edit here some thing ------------------------

            var reservation = db.Reservations.Find(id);
            var patient = db.Patients.Find(reservation.Reservation_PatientId);
            var doctor = db.Doctors.Find(reservation.Reservation_DoctorId);

            double ValueOfRefund = reservation.ReservationAmount;
            double PercentageOfSystem = ValueOfRefund * 0.10;
            double PercentageFromDoctor = ValueOfRefund - PercentageOfSystem;

            double checkDoctorAmount = doctor.DoctorAmount - PercentageFromDoctor;

            if (checkDoctorAmount >= 0)
            {
               
                doctor.DoctorAmount = checkDoctorAmount;
                patient.Amount += PercentageFromDoctor;
                db.Update(doctor);
                db.Update(patient);
                db.SaveChanges();
                return true;
            }
            
            
            return false;
            
        }
    }
}


