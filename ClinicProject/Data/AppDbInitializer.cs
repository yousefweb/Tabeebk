using ClinicProject.Models;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using ClinicProject.Data.Enum;

namespace ClinicProject.Data
{
    public class AppDbInitializer
    {

        public static async void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ClinicProjectDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                context.Database.EnsureCreated();

                List<Guid> ListofGuidDoctorIds = new List<Guid>();

                for (int i = 0; i < 11; i++)
                {
                    ListofGuidDoctorIds.Add(Guid.NewGuid());
                }


                if (!context.Owners.Any())
                {
                    context.Owners.AddRange(new List<Owner>()
                    {
                        new Owner ()
                        {
                            OwnerId = Guid.NewGuid(),
                            Owner_Email = "khaledMelhem@gmail.com",
                            Owner_Name = "KhaledMelhem",
                            Onwer_Password = "khaledMelhem2001@#",
                            Owner_PhoneNumber = 0780308940,
                            Owner_Amount = 300
                        },

                        new Owner ()
                        {
                            OwnerId = Guid.NewGuid(),
                            Owner_Email = "loayShami@gmail.com",
                            Owner_Name = "LaoyShami",
                            Onwer_Password = "loayShami2000@#",
                            Owner_PhoneNumber = 0799857561,
                            Owner_Amount = 300
                        },

                        new Owner ()
                        {
                            OwnerId = Guid.NewGuid(),
                            Owner_Email = "YousefDeeb@gmail.com",
                            Owner_Name = "YousefAlDeeb",
                            Onwer_Password = "YousefDeeb2000@#",
                            Owner_PhoneNumber = 0795837513,
                            Owner_Amount = 300
                        },

                        new Owner ()
                        {
                            OwnerId = Guid.NewGuid(),
                            Owner_Email = "Abdalrahman@gmail.com",
                            Owner_Name = "Abdalrahman",
                            Onwer_Password = "Abdalrahman2000@#",
                            Owner_PhoneNumber = 0792942890,
                            Owner_Amount = 300
                        }

                    }); ;;

                    context.SaveChanges();


                    List<Owner> OnwerList = new List<Owner>();

                    OnwerList.AddRange(context.Owners);

                    foreach (var owner in OnwerList)
                    {

                        IdentityUser user = new IdentityUser
                        {
                            Email = owner.Owner_Email,
                            UserName = owner.Owner_Name,
                            PhoneNumber = owner.Owner_PhoneNumber.ToString(),

                        };


                        var result = await userManager.CreateAsync(user, owner.Onwer_Password);
						//var userRoles = await userManager.GetRolesAsync();

						if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "Owner");
                        }
                    }

                }

                if (!context.Patients.Any())
                {
                    context.Patients.AddRange(new List<Patient>()
                    {
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "James",
                        Patient_LastName = "Walter",
                        Patient_Age = 18,
                        Patient_Email = "jessh59@coughone.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Amman,
                        Patient_PhoneNumber = 0721904310,
                        Patient_UserName = "JamesWalt22",
                        Patient_Password = "3bQDDJ5Ur!0t",
                        Patient_ConfirmPassword ="3bQDDJ5Ur!0t",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Robert",
                        Patient_LastName = "Steven",
                        Patient_Age = 21,
                        Patient_Email = "robe.rt@gmail.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Irbid,
                        Patient_PhoneNumber = 0721204310,
                        Patient_UserName = "RobertSn11",
                        Patient_Password = "4LR8qf$9^yRIf#%L!0t",
                        Patient_ConfirmPassword = "4LR8qf$9^yRIf#%L!0t",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Michael",
                        Patient_LastName = "Christopher",
                        Patient_Age = 25,
                        Patient_Email = "christopher@gmail.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Salt,
                        Patient_PhoneNumber = 0821205091,
                        Patient_UserName = "MichaelChrist",
                        Patient_Password = "e5X^f2%f$enPaGfT",
                        Patient_ConfirmPassword = "e5X^f2%f$enPaGfT",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "William",
                        Patient_LastName = "Richard",
                        Patient_Age = 40,
                        Patient_Email = "willi.a.m@gmail.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Zarqa,
                        Patient_PhoneNumber = 0841025087,
                        Patient_UserName = "WilliamRic2",
                        Patient_Password = "BasfjmJZ)d&39c+r",
                        Patient_ConfirmPassword="BasfjmJZ)d&39c+r",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Joseph",
                        Patient_LastName = "Thomas",
                        Patient_Age = 40,
                        Patient_Email = "josep.h@gmail.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Amman,
                        Patient_PhoneNumber = 0921335054,
                        Patient_UserName = "Joseph323",
                        Patient_Password = "9hLw9vECjR9KKMAT",
                        Patient_ConfirmPassword = "9hLw9vECjR9KKMAT",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Charles",
                        Patient_LastName = "Matthew",
                        Patient_Age = 34,
                        Patient_Email = "charl.es@gmail.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Amman,
                        Patient_PhoneNumber = 0921035032,
                        Patient_UserName = "Charles65",
                        Patient_Password = "My$eWEH%5%J8V^t2",
                        Patient_ConfirmPassword = "My$eWEH%5%J8V^t2",
                        Amount =100,
                    }, new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "loay",
                        Patient_LastName = "shami",
                        Patient_Age = 34,
                        Patient_Email = "loayshami6@gmail.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Amman,
                        Patient_PhoneNumber = 0921035032,
                        Patient_UserName = "loayShami77",
                        Patient_Password = "My$eWEH%5%J8V^++t2",
                        Patient_ConfirmPassword = "My$eWEH%5%J8V^t2",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Mark",
                        Patient_LastName = "Donald",
                        Patient_Age = 54,
                        Patient_Email = "m.ar.k@gmail.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Madaba,
                        Patient_PhoneNumber = 0921038743,
                        Patient_UserName = "MarkDon87",
                        Patient_Password = "24gU5mD4%+&^3+X!",
                        Patient_ConfirmPassword = "24gU5mD4%+&^3+X!",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Steven",
                        Patient_LastName = "Paul",
                        Patient_Age = 16,
                        Patient_Email = "ste.v.e.n@gmail.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Karak,
                        Patient_PhoneNumber = 0921031209,
                        Patient_UserName = "StevenPa99",
                        Patient_Password = "V%u4UUXrZd^k@bLE",
                        Patient_ConfirmPassword = "V%u4UUXrZd^k@bLE",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Kevin",
                        Patient_LastName = "Brian",
                        Patient_Age = 27,
                        Patient_Email = "k.ev.i.n@gmail.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Irbid,
                        Patient_PhoneNumber = 0799131209,
                        Patient_UserName = "KevinBr3",
                        Patient_Password = "^rfGvURBdbNRcUhk",
                        Patient_ConfirmPassword = "^rfGvURBdbNRcUhk",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Jason",
                        Patient_LastName = "Ryan",
                        Patient_Age = 44,
                        Patient_Email = "jaso.n@gmail.com",
                        Patient_Gender = Enum.Gender.Male,
                        Patient_Location = Enum.LocationState.Irbid,
                        Patient_PhoneNumber = 0767131439,
                        Patient_UserName = "JasonRy1221",
                        Patient_Password = "f8s#Sz+JVCkead6!",
                        Patient_ConfirmPassword = "f8s#Sz+JVCkead6!",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Olivia",
                        Patient_LastName = "Robert",
                        Patient_Age = 20,
                        Patient_Email = "oliv.i.a@gmail.com",
                        Patient_Gender = Enum.Gender.Female,
                        Patient_Location = Enum.LocationState.Irbid,
                        Patient_PhoneNumber = 0767131001,
                        Patient_UserName = "Olivia12",
                        Patient_Password = "jm%M5HwG5HRuZ^LQ",
                        Patient_ConfirmPassword = "jm%M5HwG5HRuZ^LQ",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Emma",
                        Patient_LastName = "Emma",
                        Patient_Age = 24,
                        Patient_Email = "Emma221@gmail.com",
                        Patient_Gender = Enum.Gender.Female,
                        Patient_Location = Enum.LocationState.Ajloun,
                        Patient_PhoneNumber = 0763465001,
                        Patient_UserName = "EmmaEm3",
                        Patient_Password = "9kfyuP$cYS9WEPV+",
                        Patient_ConfirmPassword =  "9kfyuP$cYS9WEPV+",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Charlotte",
                        Patient_LastName = "Justin",
                        Patient_Age = 23,
                        Patient_Email = "Charlotte.Ju@gmail.com",
                        Patient_Gender = Enum.Gender.Female,
                        Patient_Location = Enum.LocationState.Jarash,
                        Patient_PhoneNumber = 0763465211,
                        Patient_UserName = "CharlotteJu1",
                        Patient_Password = "HnyNMFa9yIxT(5k7+",
                        Patient_ConfirmPassword =  "HnyNMFa9yIxT(5k7+",
                        Amount =100,
                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Amelia",
                        Patient_LastName = "Jonathan",
                        Patient_Age = 57,
                        Patient_Email = "Amelia.Jon2@gmail.com",
                        Patient_Gender = Enum.Gender.Female,
                        Patient_Location = Enum.LocationState.Jarash,
                        Patient_PhoneNumber = 0763465243,
                        Patient_UserName = "Amelia11Jon",
                        Patient_Password = "MhDANB*jn9q!Fx&Q(",
                        Patient_ConfirmPassword = "MhDANB*jn9q!Fx&Q(",
                        Amount =100,

                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Ava",
                        Patient_LastName = "Stephen",
                        Patient_Age = 43,
                        Patient_Email = "AvaStoh43@gmail.com",
                        Patient_Gender = Enum.Gender.Female,
                        Patient_Location = Enum.LocationState.Altafila,
                        Patient_PhoneNumber = 0770465243,
                        Patient_UserName = "AvaST21",
                        Patient_Password = "aD%IgH+th5zT*GL#",
                        Patient_ConfirmPassword = "aD%IgH+th5zT*GL#",
                        Amount =100,

                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Sofia",
                        Patient_LastName = "Alexander",
                        Patient_Age = 19,
                        Patient_Email = "SofiaAlex76@gmail.com",
                        Patient_Gender = Enum.Gender.Female,
                        Patient_Location = Enum.LocationState.Madaba,
                        Patient_PhoneNumber = 0770460067,
                        Patient_UserName = "SofiaA.24",
                        Patient_Password = "+CsShYP+E*tE(^F)",
                        Patient_ConfirmPassword = "+CsShYP+E*tE(^F)",
                        Amount =100,

                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Emily",
                        Patient_LastName = "Jack",
                        Patient_Age = 48,
                        Patient_Email = "EmilyJack6@gmail.com",
                        Patient_Gender = Enum.Gender.Female,
                        Patient_Location = Enum.LocationState.Madaba,
                        Patient_PhoneNumber = 0770460067,
                        Patient_UserName = "EmilyJJ2",
                        Patient_Password = "mqcCtY(ndpJ@5xNF",
                        Patient_ConfirmPassword = "mqcCtY(ndpJ@5xNF",
                        Amount =100,

                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Scarlett",
                        Patient_LastName = "Adam",
                        Patient_Age = 39,
                        Patient_Email = "ScarlettAD3@gmail.com",
                        Patient_Gender = Enum.Gender.Female,
                        Patient_Location = Enum.LocationState.Madaba,
                        Patient_PhoneNumber = 0770460067,
                        Patient_UserName = "Scarlett75",
                        Patient_Password = "(Ns4RDgtPDNbv+Jq",
                        Patient_ConfirmPassword = "(Ns4RDgtPDNbv+Jq",
                        Amount =100,

                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Lucy",
                        Patient_LastName = "Peter",
                        Patient_Age = 42,
                        Patient_Email = "LucyPeter@gmail.com",
                        Patient_Gender = Enum.Gender.Female,
                        Patient_Location = Enum.LocationState.Amman,
                        Patient_PhoneNumber = 0770469064,
                        Patient_UserName = "LucyPe1",
                        Patient_Password = "AjVBI4(BPRQ@YBLv",
                        Patient_ConfirmPassword ="AjVBI4(BPRQ@YBLv",
                        Amount =100,

                    },
                    new Patient()
                    {
                        PatientId = Guid.NewGuid(),
                        Patient_FirstName = "Ruby",
                        Patient_LastName = "Harold",
                        Patient_Age = 54,
                        Patient_Email = "Ruby.Har5@gmail.com",
                        Patient_Gender = Enum.Gender.Female,
                        Patient_Location = Enum.LocationState.Maan,
                        Patient_PhoneNumber = 0770469064,
                        Patient_UserName = "Ruby767",
                        Patient_Password = "^fGULywGgg74v!BD",
                        Patient_ConfirmPassword =  "^fGULywGgg74v!BD",
                        Amount =100,
                    },

                    });

                    await context.SaveChangesAsync();


                    List<Patient> PateintList = new List<Patient>();
                    PateintList.AddRange(context.Patients);


                    foreach (var patient in PateintList)
                    {

                        IdentityUser user = new IdentityUser
                        {
                            Email = patient.Patient_Email,
                            UserName = patient.Patient_UserName,
                            PhoneNumber = patient.Patient_PhoneNumber.ToString(),

                        };


                        var result = await userManager.CreateAsync(user, patient.Patient_Password);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "Patient");
                        }
                    }
                }

                if (!context.Doctors.Any())
                {
                    int counter = 0;

                    context.Doctors.AddRange(new List<Doctor>
                    {

                        new Doctor()
                        {
                            DoctorId = ListofGuidDoctorIds[counter++],
                            Doctor_Age = 30,
                            Doctor_FirstName = "Louis",
                            Doctor_LastName = "Philip",
                            Doctor_Gender = Enum.Gender.Male,
                            Doctor_Password = "B7uEkFXQ&jR8stD4",
                            Doctor_PhoneNumber = 0720046730,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Radiology,
                            Doctor_UserName = "LouisPhil",
                            Doctor_YearsOfExperience = 5,
                            Doctor_AppointmentType = Enum.AppointmentType.Nromal,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "2.jpg",
                            Doctor_Email = "loui.s@gmail.com",
                            DoctorAmount = 200
                            
                            
                        },
                        new Doctor()
                        {
                            DoctorId = ListofGuidDoctorIds[counter++],
                            Doctor_Age = 26,
                            Doctor_FirstName = "Russell",
                            Doctor_LastName = "Randy",
                            Doctor_Gender = Enum.Gender.Male,
                            Doctor_Password = "S*5er2Fjw6I)bdB8",
                            Doctor_PhoneNumber = 0721346730,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Cardiology,
                            Doctor_UserName = "RussellRy33",
                            Doctor_YearsOfExperience = 2,
                            Doctor_AppointmentType = Enum.AppointmentType.Nromal,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "3.jpg",
                            Doctor_Email = "russel.l@gmail.com",
                             DoctorAmount = 200
                        },
                        new Doctor()
                        {
                            DoctorId = ListofGuidDoctorIds[counter++],
                            Doctor_Age = 27,
                            Doctor_FirstName = "Dylan",
                            Doctor_LastName = "Arthur",
                            Doctor_Gender = Enum.Gender.Male,
                            Doctor_Password = "6cu2qx@QEv!3dL)S",
                            Doctor_PhoneNumber = 0721916723,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Dermatology,
                            Doctor_UserName = "DylanAt44",
                            Doctor_YearsOfExperience = 3,
                            Doctor_AppointmentType = Enum.AppointmentType.Nromal,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "4.jpg",
                            Doctor_Email = "dylan@gmail.com",
                            DoctorAmount = 200
                        },
                        new Doctor()
                        {
                            DoctorId = ListofGuidDoctorIds[counter++],
                            Doctor_Age = 28,
                            Doctor_FirstName = "Carl",
                            Doctor_LastName = "Terry",
                            Doctor_Gender = Enum.Gender.Male,
                            Doctor_Password = "ce3AR2bsS8g#ztd^",
                            Doctor_PhoneNumber = 0721346990,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Pediatrics,
                            Doctor_UserName = "Carl1993",
                            Doctor_YearsOfExperience = 4,
                            Doctor_AppointmentType = Enum.AppointmentType.Nromal,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "5.jpg",
                            Doctor_Email = "ca.r.l@gmail.com",
                            DoctorAmount = 200
                        },
                        new Doctor()
                        {
                            DoctorId = ListofGuidDoctorIds[counter++],
                            Doctor_Age = 29,
                            Doctor_FirstName = "Jeremy",
                            Doctor_LastName = "Kyle",
                            Doctor_Gender = Enum.Gender.Male,
                            Doctor_Password = "$97+N!3MFA5brtf%",
                            Doctor_PhoneNumber = 0781346732,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Psychiatry,
                            Doctor_UserName = "JeremyKe2",
                            Doctor_YearsOfExperience = 5,
                            Doctor_AppointmentType = Enum.AppointmentType.Nromal,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "6.jpg",
                            Doctor_Email = "jer.e.my@gmail.com",
                            DoctorAmount = 200
                        },
                        new Doctor()
                        {
                            DoctorId =ListofGuidDoctorIds[counter++],
                            Doctor_Age = 30,
                            Doctor_FirstName = "Steven",
                            Doctor_LastName = "Jeffrey",
                            Doctor_Gender = Enum.Gender.Male,
                            Doctor_Password = "P8hyb#%4cZQeHz6U",
                            Doctor_PhoneNumber = 0703246770,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Dermatology,
                            Doctor_UserName = "StevenJef.19",
                            Doctor_YearsOfExperience = 6,
                            Doctor_AppointmentType = Enum.AppointmentType.Nromal,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "12.jpg",
                            Doctor_Email = "stev.e.n@gmail.com",
                            DoctorAmount = 200
                        },
                        new Doctor()
                        {
                            DoctorId = ListofGuidDoctorIds[counter++],
                            Doctor_Age = 31,
                            Doctor_FirstName = "Matthew",
                            Doctor_LastName = "Scott",
                            Doctor_Gender = Enum.Gender.Male,
                            Doctor_Password = "tG^+pC7esw@h9kV*",
                            Doctor_PhoneNumber = 0733046750,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Family_medicine,
                            Doctor_UserName = "Matthew1990",
                            Doctor_YearsOfExperience = 8,
                            Doctor_AppointmentType = Enum.AppointmentType.Nromal,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "7.jpg",
                            Doctor_Email = "matth.ew@gmail.com",
                            DoctorAmount = 200
                        },
                        new Doctor()
                        {
                            DoctorId = ListofGuidDoctorIds[counter++],
                            Doctor_Age = 32,
                            Doctor_FirstName = "Andrew",
                            Doctor_LastName = "Jacob",
                            Doctor_Gender = Enum.Gender.Male,
                            Doctor_Password = "3LcGtg%@+#v4Qpr$",
                            Doctor_PhoneNumber = 0721346830,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Gastroenterology,
                            Doctor_UserName = "Andrew193",
                            Doctor_YearsOfExperience = 7,
                            Doctor_AppointmentType = Enum.AppointmentType.Nromal,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "1.jpg",
                            Doctor_Email = "and.r.ew@gmail.com",
                            DoctorAmount = 200
                        },
                        new Doctor()
                        {
                            DoctorId = ListofGuidDoctorIds[counter++],
                            Doctor_Age = 38,
                            Doctor_FirstName = "Brian",
                            Doctor_LastName = "Eric",
                            Doctor_Gender = Enum.Gender.Male,
                            Doctor_Password = "BG+*X$8wF^ETpe&6",
                            Doctor_PhoneNumber = 0721346986,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Family_medicine,
                            Doctor_UserName = "BrianEr11",
                            Doctor_YearsOfExperience = 10,
                            Doctor_AppointmentType = Enum.AppointmentType.Urgent,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "11.jpg",
                            Doctor_Email = "an.dre.w@gmail.com",
                            DoctorAmount = 200
                        },
                        new Doctor()
                        {
                            DoctorId = ListofGuidDoctorIds[counter++],
                            Doctor_Age = 26,
                            Doctor_FirstName = "Jason",
                            Doctor_LastName = "Frank",
                            Doctor_Gender = Enum.Gender.Male,
                            Doctor_Password = "&4%PpD^+5CJIArcW",
                            Doctor_PhoneNumber = 0721346736,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Family_medicine,
                            Doctor_UserName = "Jason.FK2",
                            Doctor_YearsOfExperience = 2,
                            Doctor_AppointmentType = Enum.AppointmentType.Urgent,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "14.jpg",
                            Doctor_Email = "Jason.w@gmail.com",
                            DoctorAmount = 200
                        },
                        new Doctor()
                        {
                            DoctorId = ListofGuidDoctorIds[counter++],
                            Doctor_Age = 54,
                            Doctor_FirstName = "Emilly",
                            Doctor_LastName = "Franky",
                            Doctor_Gender = Enum.Gender.Female,
                            Doctor_Password = "QZ!g)z5DC#S7bxIY",
                            Doctor_PhoneNumber = 0890348021,
                            Doctor_RegisterTime = DateTime.Now,
                            Doctor_Specialization = Enum.Specialization.Family_medicine,
                            Doctor_UserName = "EmillyFrnc",
                            Doctor_YearsOfExperience = 2,
                            Doctor_AppointmentType = Enum.AppointmentType.Urgent,
                            Doctor_Certification = "",
                            Doctor_ImageProfile = "15.jpg",
                            Doctor_Email = "EmillyFC@gmail.com",
                            DoctorAmount = 200
                        },

                    });



                    await context.SaveChangesAsync();

                    List<Doctor> DL = new List<Doctor>();

                    DL.AddRange(context.Doctors);

                    foreach (var doctor in DL)
                    {

                        IdentityUser user = new IdentityUser
                        {
                            Email = doctor.Doctor_Email,
                            UserName = doctor.Doctor_UserName,
                            PhoneNumber = doctor.Doctor_PhoneNumber.ToString(),
                        };


                        var result = await userManager.CreateAsync(user, doctor.Doctor_Password);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "Doctor");
                        }
                    }
                }

				if (!context.DoctorRequests.Any())
				{
					context.DoctorRequests.AddRange(new List<DoctorRequest>()
					{

						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 26,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Jack",
							DoctorRequest_LastName = "Benjamin",
							DoctorRequest_Email = "jack@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Male,
							DoctorRequest_Password = "5;hKr7Y5#D7gYpW",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "JackBn21",
							DoctorRequest_PhoneNumber = 079921545,
							DoctorRequest_Specialization = Enum.Specialization.Dermatology,
							DoctorRequest_YearsOfExperience = 2,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "1-1.jpg",


						},
						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 45,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Billy",
							DoctorRequest_LastName = "Albert",
							DoctorRequest_Email = "Billy299@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Male,
							DoctorRequest_Password = "7(#MWL4C:U[H7gm[",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "Billy.1919",
							DoctorRequest_PhoneNumber = 078821544,
							DoctorRequest_Specialization = Enum.Specialization.Gastroenterology,
							DoctorRequest_YearsOfExperience = 20,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "7-7.jpg",


						},
						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 30,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Louis",
							DoctorRequest_LastName = "Ethan",
							DoctorRequest_Email = "LouisE.12@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Male,
							DoctorRequest_Password = "JB/N7ygx7{tg$3-a",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "Louis.Eth32",
							DoctorRequest_PhoneNumber = 078821600,
							DoctorRequest_Specialization = Enum.Specialization.Dermatology,
							DoctorRequest_YearsOfExperience = 7,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "5-5.jpg",


						},
						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 50,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Frank",
							DoctorRequest_LastName = "George",
							DoctorRequest_Email = "Frank63@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Male,
							DoctorRequest_Password = "'!wZfd@Z3n84-EQG",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "Frank.Geor",
							DoctorRequest_PhoneNumber = 078821634,
							DoctorRequest_Specialization = Enum.Specialization.Internal_medicine,
							DoctorRequest_YearsOfExperience = 30,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "4-4.jpg",


						},
						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 50,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Charles",
							DoctorRequest_LastName = "Eric",
							DoctorRequest_Email = "Charles99@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Male,
							DoctorRequest_Password = "C?Rt9Y.9tQ-3D:t<",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "CharlesJe",
							DoctorRequest_PhoneNumber = 078821321,
							DoctorRequest_Specialization = Enum.Specialization.Oncology,
							DoctorRequest_YearsOfExperience = 25,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "8-8.jpg",


						},
						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 35,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Douglas",
							DoctorRequest_LastName = "Jose",
							DoctorRequest_Email = "Douglas.Er@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Male,
							DoctorRequest_Password = "Yz9W%4'=?{J`5k8Z",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "Douglas54",
							DoctorRequest_PhoneNumber = 078821321,
							DoctorRequest_Specialization = Enum.Specialization.Pulmonology,
							DoctorRequest_YearsOfExperience = 9,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "2-2.jpg",


						},
						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 27,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Thomas",
							DoctorRequest_LastName = "Kevin",
							DoctorRequest_Email = "ThomasKiv2@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Male,
							DoctorRequest_Password = "udnC6J><`C]}Y?T8",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "Thomas995",
							DoctorRequest_PhoneNumber = 072221654,
							DoctorRequest_Specialization = Enum.Specialization.Ophthalmology,
							DoctorRequest_YearsOfExperience = 3 ,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "12-12.jpg",


						},
						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 41,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Kenneth",
							DoctorRequest_LastName = "Raymond",
							DoctorRequest_Email = "KennethRy11@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Male,
							DoctorRequest_Password = "g'#Vwcd'/vXpJ8*s",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "KennethRad",
							DoctorRequest_PhoneNumber = 071901430,
							DoctorRequest_Specialization = Enum.Specialization.Psychiatry,
							DoctorRequest_YearsOfExperience = 14,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "13-g.jpg",


						},
						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 30,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Anna",
							DoctorRequest_LastName = "Raymond",
							DoctorRequest_Email = "AnnaRay2@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Female,
							DoctorRequest_Password = "%Qq?42~:agj$S%*m",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "AnnaRand",
							DoctorRequest_PhoneNumber = 071904432,
							DoctorRequest_Specialization = Enum.Specialization.Dermatology,
							DoctorRequest_YearsOfExperience = 5,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "9-g.jpg",


						},
						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 34,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Rose",
							DoctorRequest_LastName = "Carl",
							DoctorRequest_Email = "RoseCl23@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Female,
							DoctorRequest_Password = "tp{S$7F,8U>AfH;@",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "Rose99",
							DoctorRequest_PhoneNumber = 071906532,
							DoctorRequest_Specialization = Enum.Specialization.Neurology,
							DoctorRequest_YearsOfExperience = 9,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "5-g.jpg",


						},
						new DoctorRequest()
						{
							DoctorRequestId = Guid.NewGuid(),
							DoctorRequest_Age = 39,
							DoctorRequest_AppointmentType = Enum.AppointmentType.Nromal,
							DoctorRequest_FirstName = "Mary",
							DoctorRequest_LastName = "Russell",
							DoctorRequest_Email = "MaryRuus90@gmail.com",
							DoctorRequest_Gender = Enum.Gender.Female,
							DoctorRequest_Password = "S/?6%Shj?uX=!ru",
							DoctorRequest_RegisterTime = DateTime.Now,
							DoctorRequest_UserName = "Mary90",
							DoctorRequest_PhoneNumber = 070906567,
							DoctorRequest_Specialization = Enum.Specialization.Gastroenterology,
							DoctorRequest_YearsOfExperience = 12,
							DoctorRequest_Certification = "",
							DoctorRequest_ImageProfile = "10-g.jpg",


						},

					});

					context.SaveChanges();
				}

				if (!context.Clinics.Any())
				{

					int counter = 0;
					context.Clinics.AddRange(new List<Clinic>()
					{
						new Clinic()
						{

							ClinicId = Guid.NewGuid(),
							Clinic_DoctorId = ListofGuidDoctorIds[counter++],
							Clinic_Name = "Infra Xray",
							clinc_Price = 10,
							Clinc_Description = "At our clinic, we use state-of-the-art Infra X Ray technology to provide accurate and efficient imaging for our patients. Get a comprehensive look at your body and enjoy the benefits of our advanced equipment today",
							ClinicRate = 4,
							Clinic_Location = Enum.LocationState.Amman,


						},
						new Clinic()
						{

							ClinicId = Guid.NewGuid(),
							Clinic_DoctorId =ListofGuidDoctorIds[counter++],
							Clinic_Name = "Healing Clinic",
							clinc_Price = 15,
							Clinc_Description ="Visit our Healing Clinic for Cardiology and receive a personalized treatment plan tailored to your specific needs. Our experienced specialists use advanced technology and techniques to provide the best possible care.",
							ClinicRate = 5,
							Clinic_Location = Enum.LocationState.Irbid,


						},
						new Clinic()
						{

							ClinicId = Guid.NewGuid(),
							Clinic_DoctorId = ListofGuidDoctorIds[counter++],
							Clinic_Name = "Nova Clinic",
							clinc_Price = 8,
							Clinc_Description ="Experience top-notch skin care services and treatments at Nova Clinic in Dermatology. Our Clinic  will help you achieve healthy, glowing skin and provide personalized care for your individual needs.",
							ClinicRate = 4,
							Clinic_Location = Enum.LocationState.Zarqa,



						},
						new Clinic()
						{

							ClinicId =Guid.NewGuid(),
							Clinic_DoctorId = ListofGuidDoctorIds[counter++],
							Clinic_Name = "Tiny Tots Pediatrics",
							clinc_Price = 9,
							Clinc_Description ="Get the best care for your child's health at Tiny Tots Pediatrics. Our experienced doctor provide comprehensive care for children of all ages, from routine checkups to specialized treatments. Book an appointment now and receive a free vaccination for your child",
							ClinicRate = 5,
							Clinic_Location = Enum.LocationState.Salt,


						},
						new Clinic()
						{

							ClinicId =Guid.NewGuid(),
							Clinic_DoctorId = ListofGuidDoctorIds[counter++],
							Clinic_Name = "MindCare Clinic",
							clinc_Price = 11,
							Clinc_Description ="Take care of your mental health and wellbeing with the our expert psychiatrist at MindCare Clinic. Get personalized treatment and therapy to help you overcome your struggles and live a happier, healthier life.",
							ClinicRate = 3,
							Clinic_Location = Enum.LocationState.Ajloun,


						},
						new Clinic()
						{

							ClinicId =Guid.NewGuid(),
							Clinic_DoctorId =ListofGuidDoctorIds[counter++],
							Clinic_Name = "Smooth Skin Clinic",
							clinc_Price = 16,
							Clinc_Description ="Get smooth, beautiful skin at Smooth Skin Clinic! Our skilled doctor of professional offers a wide range of treatments, including laser hair removal, facials, and chemical peels, all tailored to your specific skin type and needs. Visit us today and experience the difference for yourself!",
							ClinicRate = 5,
							Clinic_Location = Enum.LocationState.Aqaba,

						},
						new Clinic()
						{

							ClinicId =Guid.NewGuid(),
							Clinic_DoctorId = ListofGuidDoctorIds[counter++],
							Clinic_Name = "Smooth Skin Clinic",
							clinc_Price = 16,
							Clinc_Description ="Experience comprehensive care for your entire family at Harmony Health Clinic. Our doctor provide personalized and compassionate care to help you achieve optimal health and wellness. Schedule your appointment today and take the first step towards a healthier you!",
							ClinicRate = 4,
							Clinic_Location = Enum.LocationState.Altafila,


						},
						new Clinic()
						{

							ClinicId =Guid.NewGuid(),
							Clinic_DoctorId = ListofGuidDoctorIds[counter++],
							Clinic_Name = "GastroCare Center",
							clinc_Price = 7,
							Clinc_Description ="Say goodbye to digestive issues with the GastroCare Center. Our doctor of experienced ts offers comprehensive diagnosis, treatment, and management of various gastrointestinal disorders. Book your appointment now and experience relief from discomfort and pain.",
							ClinicRate = 3,
							Clinic_Location = Enum.LocationState.Karak,


						},
						new Clinic()
						{

							ClinicId =Guid.NewGuid(),
							Clinic_DoctorId =ListofGuidDoctorIds[counter++],
							Clinic_Name = "Family Wellness Center",
							clinc_Price = 8,
							Clinc_Description ="Experience comprehensive family healthcare services at Family Wellness Center. Our doctor of experienced healthcare professionals provide personalized care for patients of all ages to promote overall wellness and healthy living. Visit us today to start your journey towards a healthier, happier life.",
							ClinicRate = 4,
							Clinic_Location = Enum.LocationState.Amman,


						},
						new Clinic()
						{

							ClinicId =Guid.NewGuid(),
							Clinic_DoctorId = ListofGuidDoctorIds[counter++],
							Clinic_Name = "Go Clinic Family.",
							clinc_Price = 6,
							Clinc_Description ="Get comprehensive healthcare for your entire family at Go Clinic Family. Our experienced physician provide personalized care to meet your unique healthcare needs. Visit us now to start your journey towards optimal health.",
							ClinicRate = 3,
							Clinic_Location = Enum.LocationState.Madaba,


						},
						new Clinic()
						{

							ClinicId =Guid.NewGuid(),
							Clinic_DoctorId = ListofGuidDoctorIds[counter++],
							Clinic_Name = "Go Clinic Family.",
							clinc_Price = 14,
							Clinc_Description ="At Unity Family Medicine, we prioritize your health and well-being. Our experienced doctor and staff provide comprehensive care and personalized treatment plans to help you and your family achieve optimal health. Contact us today to schedule an appointment and take the first step towards a healthier life.\r\n",
							ClinicRate = 5,
							Clinic_Location = Enum.LocationState.Jarash,


						},
					});

					context.SaveChanges();
				}

                if(!context.ClinicDays.Any())
                {
                   var Clinics = context.Clinics.ToList();
                    foreach (var clinic in Clinics)
                    {
                        if(clinic !=null)
                        {
                            context.ClinicDays.AddRange(new List<ClinicDay>()
                            {
                                new ClinicDay
                                {
                                    ClinicDayId = Guid.NewGuid(),
                                    ClinicId = clinic.ClinicId,
                                    DateOfWork = DateTime.Today,
                                    DayOfWeek = Days.Monday,
                                    StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8,0,0),
                                    EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14,0,0),
                                },

                                new ClinicDay
                                {
                                    ClinicDayId = Guid.NewGuid(),
                                    ClinicId = clinic.ClinicId,
                                    DateOfWork = DateTime.Today,
                                    DayOfWeek = Days.Tuesday,
                                    StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14,0,0),
                                    EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20,0,0),
                                },

                                new ClinicDay
                                {
                                    ClinicDayId = Guid.NewGuid(),
                                    ClinicId = clinic.ClinicId,
                                    DateOfWork = DateTime.Today,
                                    DayOfWeek = Days.Wednesday,
                                    StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,13,0,0),
                                    EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17,0,0),

                                },

                            });

                            await context.SaveChangesAsync();

                            Dictionary<Days, DateTime> ComingDictionry = new Dictionary<Days, DateTime>();
                            var daysOftheCLinic = context.ClinicDays.Where(C => C.ClinicId == clinic.ClinicId);
                            foreach (var day in daysOftheCLinic)
                            {
                                ComingDictionry = ClinicProject.Controllers.ClinicController.GetDateTimeForDays();

                                if (ComingDictionry.ContainsKey(day.DayOfWeek))
                                {
                                    day.DateOfWork = ComingDictionry[day.DayOfWeek].Date;
                                }
                            }
                            context.Update(clinic);
                            context.SaveChanges();
                        }
                    }
                      
                }

                if(!context.timeSlots.Any())
                {
                    
                    var listOfClinics = context.Clinics.ToList();

                    foreach(var clinic in listOfClinics)
                    {
                        GenerateClinicDayTimeSlots(context , clinic.ClinicId);
                    }
                }

				if ((context.Doctors != null && context.Clinics != null) && (context.Doctors.Count() == context.Clinics.Count()))
				{
					foreach (Doctor doctor in context.Doctors)
					{
						foreach (Clinic clinic in context.Clinics)
						{
							if (doctor.DoctorId == clinic.Clinic_DoctorId)
							{
								doctor.Doctor_Clinic = clinic;
							}

							context.SaveChanges();
						}
					}

				}
            
            }

           

        }

        public static void GenerateClinicDayTimeSlots(ClinicProjectDbContext context , Guid id)
        {
            var clinic = context.Clinics.Find(id);

            var clinicDays = new List<ClinicDay>();

            clinicDays = context.ClinicDays.Where(CD => CD.ClinicId == id).ToList();

            if (clinic != null && clinicDays.Any())
            {

                foreach (var day in clinicDays)
                {

                    DateTime currentTime = day.StartTime;

                    while (currentTime < day.EndTime)
                    {

                        TimeSlot timeSlot = new TimeSlot
                        {

                            TimeSlotId = Guid.NewGuid(),
                            StartTime = currentTime,
                            EndTime = currentTime.AddHours(1),
                            IsAvailable = true,
                            ClinicDayId = day.ClinicDayId,

                        };

                        context.timeSlots.Add(timeSlot);
                        day.TimeSlots.Add(timeSlot);
                        currentTime = currentTime.AddHours(1);

                    }




                }

                context.SaveChanges();

            }
        }
    }
}

