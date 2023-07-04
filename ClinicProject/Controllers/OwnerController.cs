using ClinicProject.Data;
using ClinicProject.Data.Enum;
using ClinicProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MailKit.Net.Smtp;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.Graph.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ClinicProject.Controllers
{

    public class OwnerController : Controller
    {
        private ClinicProjectDbContext db;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signinManager;


        public void SendMail(DoctorRequest doctor)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Tabeebk", "tabeebksite@gmail.com"));
            email.To.Add(new MailboxAddress("" + doctor.DoctorRequest_FirstName + "", "" + doctor.DoctorRequest_Email + ""));
            email.Subject = "About your Signup request";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = "Congratulations " + doctor.DoctorRequest_FirstName + " You have been accepted to tabeebk site \n You can login now with \n Eamil:" + doctor.DoctorRequest_Email + "\n Password: " + doctor.DoctorRequest_Password + "\n\t\t\t Thank you for joining us"
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
        public OwnerController(ClinicProjectDbContext _db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager)
        {
            db = _db;
            _signinManager = signinManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(Owner model)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if(user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    return View();
                }
            }
           
            return RedirectToAction("LogIn", "Owner");

        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LogIn(Owner model)
        {


            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.Owner_Email);

                if (null != user && await _userManager.IsInRoleAsync(user, "Owner") && await _userManager.CheckPasswordAsync(user, model.Onwer_Password.ToString()))
                {

                    await _signinManager.SignInAsync(user, isPersistent: false);
                    HttpContext.Session.SetString(user.Id, user.Id);
                    return RedirectToAction("Index", "Owner");
                }

                else
                {
                    ModelState.AddModelError("", "Invalid Email or Password or or Unorthorized");
                    return View(model);
                }

            }
            return View(model);
        }

        #region Request
        [HttpGet]

        public async Task<IActionResult> AllDoctorsRequest()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated == true)
                {
                    return View(db.DoctorRequests.OrderBy(x => x.DoctorRequest_RegisterTime).ToList());
                }
            }
              

            return RedirectToAction("LogIn", "Owner");

        }


        [HttpGet]
        public async Task<IActionResult> DetailsDoctorRequest(Guid? id)
        {

            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    if (id != null)
                    {
                        DoctorRequest doctorRequest = db.DoctorRequests.Find(id);
                        return View(doctorRequest);
                    }

                    return RedirectToAction("AllDoctorsRequest");
                }

            }

            return RedirectToAction("LogIn", "Owner");
        }

        [HttpGet]
        public async Task<IActionResult> AcceptDoctor(Guid id)
        {

            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    DoctorRequest doctorRequest = new DoctorRequest();

                    doctorRequest = db.DoctorRequests.Find(id);

                    if (doctorRequest != null)
                    {
                        doctorRequest.StatusOfDoctor = StatusOfDoctor.Accept;
                        db.SaveChanges();

                        Doctor doctor = new Doctor()
                        {
                            DoctorId = doctorRequest.DoctorRequestId,
                            Doctor_FirstName = doctorRequest.DoctorRequest_FirstName,
                            Doctor_LastName = doctorRequest.DoctorRequest_LastName,
                            Doctor_UserName = doctorRequest.DoctorRequest_UserName,
                            Doctor_Email = doctorRequest.DoctorRequest_Email,
                            Doctor_PhoneNumber = doctorRequest.DoctorRequest_PhoneNumber,
                            Doctor_ImageProfile = doctorRequest.DoctorRequest_ImageProfile,
                            Doctor_Certification = doctorRequest.DoctorRequest_Certification,
                            Doctor_Age = doctorRequest.DoctorRequest_Age,
                            Doctor_RegisterTime = doctorRequest.DoctorRequest_RegisterTime,
                            Doctor_Gender = doctorRequest.DoctorRequest_Gender,
                            Doctor_AppointmentType = doctorRequest.DoctorRequest_AppointmentType,
                            Doctor_YearsOfExperience = doctorRequest.DoctorRequest_YearsOfExperience,
                            Doctor_Specialization = doctorRequest.DoctorRequest_Specialization,
                        };


                        IdentityUser userIdentity = new IdentityUser
                        {
                            Email = doctorRequest.DoctorRequest_Email,
                            PhoneNumber = doctorRequest.DoctorRequest_PhoneNumber.ToString(),
                            UserName = doctorRequest.DoctorRequest_UserName,

                        };


                        var result = await _userManager.CreateAsync(userIdentity, doctorRequest.DoctorRequest_Password);


                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(userIdentity, "Doctor");

                            db.Doctors.Add(doctor);
                            SendMail(doctorRequest);
                            db.DoctorRequests.Remove(doctorRequest);
                            db.SaveChanges();

                            return RedirectToAction("AllDoctorsRequest");

                        }

                        return RedirectToAction("AllDoctorsRequest");

                    }
                }
            }
              


            return RedirectToAction("LogIn", "Owner");
        }

        [HttpGet]

        public async Task<IActionResult> RejectDoctor(Guid id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    DoctorRequest doctorRequest = new DoctorRequest();

                    doctorRequest = db.DoctorRequests.Find(id);

                    if (doctorRequest != null)
                    {
                        db.DoctorRequests.Remove(doctorRequest);
                        db.SaveChanges();
                    }

                    return RedirectToAction("AllDoctorsRequest");
                }
            }

            return RedirectToAction("LogIn", "Owner");

        }

        public async Task<IActionResult> DeleteExistingDoctor(Guid id)
        {

            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    var doctor = db.Doctors.Find(id);

                    if (doctor != null)
                    {
                        db.Doctors.Remove(doctor);
                        db.SaveChanges();
                    }

                    return RedirectToAction("AllAcceptsDoctor");
                }
            }
                
            return RedirectToAction("LogIn", "Owner");

        }

        [HttpGet]
        public async Task<IActionResult> AcceptAllDoctor()
        {

            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    List<DoctorRequest> ListOfAllDoctorRequests = new List<DoctorRequest>();

                    ListOfAllDoctorRequests = db.DoctorRequests.ToList();

                    foreach (var Doc in ListOfAllDoctorRequests)
                    {

                        await AcceptDoctor(Doc.DoctorRequestId);

                    }

                    db.SaveChanges();
                    return RedirectToAction("AllDoctorsRequest");
                }
            }

            return RedirectToAction("LogIn", "Owner");
        }

        [HttpGet]
        public async Task<IActionResult> RejectAllDoctor()
        {

            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    List<DoctorRequest> ListOfAllDoctorRequests = new List<DoctorRequest>();

                    ListOfAllDoctorRequests = db.DoctorRequests.ToList();

                    foreach (var Doc in ListOfAllDoctorRequests)
                    {

                       await RejectDoctor(Doc.DoctorRequestId);

                    }

                    db.SaveChanges();

                    return RedirectToAction("AllDoctorsRequest");
                }
            }

            return RedirectToAction("LogIn", "Owner");
        }

        #endregion
        [HttpGet]
        public async Task<IActionResult> AllAcceptsDoctor()
        {


            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    return View(db.Doctors.OrderBy(x => x.Doctor_RegisterTime).ToList());
                }
            }

            return RedirectToAction("LogIn", "Owner");
        }

        [HttpGet]
        public async Task<IActionResult> DoctorDetails(Guid? id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    if (id != null)
                    {
                        Doctor doctor = db.Doctors.Find(id);
                        return View(doctor);
                    }

                    return RedirectToAction("AllAcceptsDoctor");
                }

            }


            return RedirectToAction("LogIn", "Owner");


        }

        [HttpGet]
        public async Task<IActionResult> AllClinics()
        {

            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    return View(db.Clinics.OrderBy(x => x.ClinicId).ToList());
                }
            }
              
            return RedirectToAction("LogIn", "Owner");
        }


        [HttpGet]
        public async Task<IActionResult> ClinicDetails(Guid? id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    if (id != null)
                    {
                        Clinic clinic = db.Clinics.Find(id);
                        return View(clinic);
                    }

                    return RedirectToAction("AllClinics");

                }

            }

            return RedirectToAction("LogIn", "Owner");

        }

        [HttpGet]
        public async Task<IActionResult> DoctorBelongsToClinic(Guid? id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {

                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {

                    if (id != null)
                    {
                        Clinic TheDesirdClinic = db.Clinics.Find(id);

                        Doctor doctor = db.Doctors.Find(TheDesirdClinic.Clinic_DoctorId);

                        if (doctor != null)
                        {
                            return View(doctor);
                        }

                        return View("AllClinics");
                    }

                    return View("AllClinics");
                }
            }

            return RedirectToAction("LogIn", "Owner");


        }

        private async Task<bool> IsOwnerAuthenticatedAsync(string Key)
        {
            var OwnerId = HttpContext.Session.GetString(Key);

            if (string.IsNullOrEmpty(OwnerId))
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(OwnerId);

            if (null == user)
            {
                return false;
            }

            if (User.Identity.IsAuthenticated && await _userManager.IsInRoleAsync(user, "Owner"))
            {
                return true;
            }

            return false;

        }


        public async Task<IActionResult> ReceiveNoteFromPatient()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    var notes = db.patientNoteToAdmins.Include(n => n.patient).ToList();
                    return View(notes);
                }
            }

            return RedirectToAction("LogIn", "Owner");

        }
        public async Task<IActionResult> ReceiveNoteFromDoctor()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                bool isOwnerAuthenticated = await IsOwnerAuthenticatedAsync(user.Id);
                if (isOwnerAuthenticated)
                {
                    var notes = db.doctorNoteToAdmins.Include(n => n.doctor).ToList();
                    return View(notes);
                }
            }

            return RedirectToAction("LogIn", "Owner");
        }
    }
}