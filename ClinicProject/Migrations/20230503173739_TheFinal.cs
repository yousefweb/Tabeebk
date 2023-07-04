using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicProject.Migrations
{
    public partial class TheFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Doctor_FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    Doctor_ImageProfile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_Certification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_Age = table.Column<int>(type: "int", nullable: false),
                    Doctor_RegisterTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Doctor_Gender = table.Column<int>(type: "int", nullable: false),
                    Doctor_AppointmentType = table.Column<int>(type: "int", nullable: false),
                    Doctor_YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    Doctor_Specialization = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DoctorId);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Owner_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner_Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Onwer_Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner_Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Onwer_Technical = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.OwnerId);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Patient_FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patient_PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    Patient_Age = table.Column<int>(type: "int", nullable: false),
                    Patient_Gender = table.Column<int>(type: "int", nullable: false),
                    Patient_Location = table.Column<int>(type: "int", nullable: false),
                    Patient_SendNoteToDoctor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    ClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Clinic_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clinic_Location = table.Column<int>(type: "int", nullable: false),
                    Clinc_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClinicRate = table.Column<int>(type: "int", nullable: false),
                    clinc_Price = table.Column<double>(type: "float", nullable: false),
                    Clinic_DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    situationOfReservation = table.Column<int>(type: "int", nullable: false),
                    DoctorRequest_ReceivePatientNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.ClinicId);
                    table.ForeignKey(
                        name: "FK_Clinics_Doctors_Clinic_DoctorId",
                        column: x => x.Clinic_DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicDays",
                columns: table => new
                {
                    ClinicDayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicDays", x => x.ClinicDayId);
                    table.ForeignKey(
                        name: "FK_ClinicDays_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "ClinicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorRequests",
                columns: table => new
                {
                    DoctorRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorRequest_FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorRequest_LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorRequest_UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorRequest_Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorRequest_Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorRequest_PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    DoctorRequest_ImageProfile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorRequest_Certification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorRequest_Age = table.Column<int>(type: "int", nullable: false),
                    DoctorRequest_RegisterTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoctorRequest_Gender = table.Column<int>(type: "int", nullable: false),
                    DoctorRequest_AppointmentType = table.Column<int>(type: "int", nullable: false),
                    DoctorRequest_YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    DoctorRequest_Specialization = table.Column<int>(type: "int", nullable: false),
                    StatusOfDoctor = table.Column<int>(type: "int", nullable: false),
                    DoctorRequest_ClinicClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorRequests", x => x.DoctorRequestId);
                    table.ForeignKey(
                        name: "FK_DoctorRequests_Clinics_DoctorRequest_ClinicClinicId",
                        column: x => x.DoctorRequest_ClinicClinicId,
                        principalTable: "Clinics",
                        principalColumn: "ClinicId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reservation_DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reservation_PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reservation_AppointmentType = table.Column<int>(type: "int", nullable: false),
                    Reservation_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reservation_Day = table.Column<int>(type: "int", nullable: false),
                    Reservation_Specialization = table.Column<int>(type: "int", nullable: false),
                    situationOfReservation = table.Column<int>(type: "int", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "ClinicId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Doctors_Reservation_DoctorId",
                        column: x => x.Reservation_DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Patients_Reservation_PatientId",
                        column: x => x.Reservation_PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicDays_ClinicId",
                table: "ClinicDays",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_Clinic_DoctorId",
                table: "Clinics",
                column: "Clinic_DoctorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorRequests_DoctorRequest_ClinicClinicId",
                table: "DoctorRequests",
                column: "DoctorRequest_ClinicClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ClinicId",
                table: "Reservations",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Reservation_DoctorId",
                table: "Reservations",
                column: "Reservation_DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Reservation_PatientId",
                table: "Reservations",
                column: "Reservation_PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClinicDays");

            migrationBuilder.DropTable(
                name: "DoctorRequests");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Doctors");
        }
    }
}
