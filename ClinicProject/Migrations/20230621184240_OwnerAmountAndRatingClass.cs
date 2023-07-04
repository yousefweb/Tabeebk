using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicProject.Migrations
{
    public partial class OwnerAmountAndRatingClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Patient_ConfirmPassword",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Patient_Password",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Onwer_Password",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Doctor_Password",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "Reservation_Date",
                table: "Reservations",
                newName: "Strat_reservation");

            migrationBuilder.AddColumn<DateTime>(
                name: "End_reservation",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "ReservationAmount",
                table: "Reservations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "TimeSlotId_reservation",
                table: "Reservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Patients",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Patient_Aboutme",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Owner_Amount",
                table: "Owners",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Owner_PhoneNumber",
                table: "Owners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "DoctorAmount",
                table: "Doctors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "DoctorRequest_Password",
                table: "DoctorRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoctorRequest_LastName",
                table: "DoctorRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoctorRequest_FirstName",
                table: "DoctorRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Days",
                table: "Clinics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfWork",
                table: "ClinicDays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "clinicRates",
                columns: table => new
                {
                    ClinicRateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RateOfPAtient = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinicRates", x => x.ClinicRateId);
                    table.ForeignKey(
                        name: "FK_clinicRates_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "ClinicId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_clinicRates_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "doctorNoteToAdmins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctorNoteToAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_doctorNoteToAdmins_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "oldReservations",
                columns: table => new
                {
                    OldReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldReservation_DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldReservation_PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldReservation_AppointmentType = table.Column<int>(type: "int", nullable: false),
                    Strat_Oldreservation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Oldreservation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldReservation_Day = table.Column<int>(type: "int", nullable: false),
                    OldReservation_Specialization = table.Column<int>(type: "int", nullable: false),
                    situationOfOldReservation = table.Column<int>(type: "int", nullable: false),
                    TimeSlotId_Oldreservation = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oldReservations", x => x.OldReservationId);
                    table.ForeignKey(
                        name: "FK_oldReservations_Doctors_OldReservation_DoctorId",
                        column: x => x.OldReservation_DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_oldReservations_Patients_OldReservation_PatientId",
                        column: x => x.OldReservation_PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "patientNoteToAdmins",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientNoteToAdmins", x => x.id);
                    table.ForeignKey(
                        name: "FK_patientNoteToAdmins_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "patientNoteToDoctors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientNoteToDoctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_patientNoteToDoctors_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_patientNoteToDoctors_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "timeSlots",
                columns: table => new
                {
                    TimeSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    ClinicDayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timeSlots", x => x.TimeSlotId);
                    table.ForeignKey(
                        name: "FK_timeSlots_ClinicDays_ClinicDayId",
                        column: x => x.ClinicDayId,
                        principalTable: "ClinicDays",
                        principalColumn: "ClinicDayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clinicRates_ClinicId",
                table: "clinicRates",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_clinicRates_PatientId",
                table: "clinicRates",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_doctorNoteToAdmins_DoctorId",
                table: "doctorNoteToAdmins",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_oldReservations_OldReservation_DoctorId",
                table: "oldReservations",
                column: "OldReservation_DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_oldReservations_OldReservation_PatientId",
                table: "oldReservations",
                column: "OldReservation_PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_patientNoteToAdmins_PatientId",
                table: "patientNoteToAdmins",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_patientNoteToDoctors_DoctorId",
                table: "patientNoteToDoctors",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_patientNoteToDoctors_PatientId",
                table: "patientNoteToDoctors",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_timeSlots_ClinicDayId",
                table: "timeSlots",
                column: "ClinicDayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clinicRates");

            migrationBuilder.DropTable(
                name: "doctorNoteToAdmins");

            migrationBuilder.DropTable(
                name: "oldReservations");

            migrationBuilder.DropTable(
                name: "patientNoteToAdmins");

            migrationBuilder.DropTable(
                name: "patientNoteToDoctors");

            migrationBuilder.DropTable(
                name: "timeSlots");

            migrationBuilder.DropColumn(
                name: "End_reservation",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservationAmount",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "TimeSlotId_reservation",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Patient_Aboutme",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Owner_Amount",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Owner_PhoneNumber",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "DoctorAmount",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Days",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "DateOfWork",
                table: "ClinicDays");

            migrationBuilder.RenameColumn(
                name: "Strat_reservation",
                table: "Reservations",
                newName: "Reservation_Date");

            migrationBuilder.AddColumn<string>(
                name: "Patient_ConfirmPassword",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Patient_Password",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Onwer_Password",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Doctor_Password",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoctorRequest_Password",
                table: "DoctorRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorRequest_LastName",
                table: "DoctorRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorRequest_FirstName",
                table: "DoctorRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
