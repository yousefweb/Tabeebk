using ClinicProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ClinicProject.Data
{
    public class ClinicProjectDbContext:IdentityDbContext
    {
       

        public ClinicProjectDbContext(DbContextOptions<ClinicProjectDbContext> Option):base(Option)
        {
    }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorRequest> DoctorRequests { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ClinicDay> ClinicDays { get; set; }
        public DbSet<TimeSlot> timeSlots { get; set; }
        public DbSet<PatientNoteToAdmin> patientNoteToAdmins { get; set; }
        public DbSet<DoctorNoteToAdmin> doctorNoteToAdmins { get; set; }
        public DbSet<PatientNoteToDoctor> patientNoteToDoctors { get; set; }
        public DbSet<OldReservation> oldReservations { get; set; }
        public DbSet<ClinicRate> clinicRates { get; set; }


    }
}
