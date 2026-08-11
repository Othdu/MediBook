using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using MediBook.Application.Services;
using MediBook.Domain.Entities;
using MediBook.Domain.Enums;
using MediBook.Domain.Exceptions;
using Moq;
using Xunit;

namespace MediBook.UnitTests.Services
{
    public class AppointmentServiceTests
    {
        [Fact]
        public void Create_Succeeds_WhenSlotIsAvailableAndNoConflict()
        {
            // Arrange
            var doctor = new Doctor { Id = 1, Name = "Dr. Test", SpecialtyId = 1, ConsultationFee = 100 };
            var patient = new Patient { Id = 1, Name = "Test Patient" };
            var requestedTime = new DateTime(2026, 8, 3, 10, 0, 0); // Monday, 10 AM

            var availability = new DoctorAvailability
            {
                DoctorId = 1,
                DayOfWeek = DayOfWeek.Monday,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(14, 0)
            };

            var doctorRepoMock = new Mock<IDoctorRepository>();
            doctorRepoMock.Setup(r => r.GetById(1)).Returns(doctor);

            var patientRepoMock = new Mock<IPatientRepository>();
            patientRepoMock.Setup(r => r.GetById(1)).Returns(patient);

            var availabilityRepoMock = new Mock<IDoctorAvailabilityRepository>();
            availabilityRepoMock.Setup(r => r.GetByDoctorId(1)).Returns(new List<DoctorAvailability> { availability });

            var appointmentRepoMock = new Mock<IAppointmentRepository>();
            appointmentRepoMock.Setup(r => r.GetByDoctorAndDate(1, requestedTime))
                .Returns(new List<Appointment>()); // empty — no conflict

            var service = new AppointmentService(
                appointmentRepoMock.Object,
                doctorRepoMock.Object,
                patientRepoMock.Object,
                availabilityRepoMock.Object);

            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                PatientId = 1,
                ScheduledAt = requestedTime
            };

            // Act
            var result = service.Create(dto);

            // Assert
            Assert.Equal("Dr. Test", result.DoctorName);
            Assert.Equal("Test Patient", result.PatientName);
            Assert.Equal("Pending", result.Status);
        }
        [Fact]
        public void Confirm_Succeeds_WhenAppointmentIsPending()
        {
            // Arrange
            var appointment = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                ScheduledAt = new DateTime(2026, 8, 3, 10, 0, 0),
                Status = AppointmentStatus.Pending,
                Doctor = new Doctor { Id = 1, Name = "Dr. Test" },
                Patient = new Patient { Id = 1, Name = "Test Patient" }
            };

            var appointmentRepoMock = new Mock<IAppointmentRepository>();
            appointmentRepoMock.Setup(r => r.GetById(1)).Returns(appointment);

            var service = new AppointmentService(
                appointmentRepoMock.Object,
                Mock.Of<IDoctorRepository>(),
                Mock.Of<IPatientRepository>(),
                Mock.Of<IDoctorAvailabilityRepository>());

            // Act
            var result = service.Confirm(1);

            // Assert
            Assert.Equal("Confirmed", result.Status);
        }
        [Fact]
        public void Cancel_Succeeds_WhenAppointmentIsPending()
        {
            var appointment = new Appointment
            {
                Id = 1,
                Status = AppointmentStatus.Pending,
                Doctor = new Doctor { Id = 1, Name = "Dr. Test" },
                Patient = new Patient { Id = 1, Name = "Test Patient" }
            };

            var appointmentRepoMock = new Mock<IAppointmentRepository>();
            appointmentRepoMock.Setup(r => r.GetById(1)).Returns(appointment);

            var service = new AppointmentService(
                appointmentRepoMock.Object,
                Mock.Of<IDoctorRepository>(),
                Mock.Of<IPatientRepository>(),
                Mock.Of<IDoctorAvailabilityRepository>());

            var result = service.Cancel(1);

            Assert.Equal("Cancelled", result.Status);
        }

        [Fact]
        public void Cancel_ThrowsInvalidStatusTransitionException_WhenAppointmentIsCompleted()
        {
            var appointment = new Appointment
            {
                Id = 1,
                Status = AppointmentStatus.Completed,
                Doctor = new Doctor { Id = 1, Name = "Dr. Test" },
                Patient = new Patient { Id = 1, Name = "Test Patient" }
            };

            var appointmentRepoMock = new Mock<IAppointmentRepository>();
            appointmentRepoMock.Setup(r => r.GetById(1)).Returns(appointment);

            var service = new AppointmentService(
                appointmentRepoMock.Object,
                Mock.Of<IDoctorRepository>(),
                Mock.Of<IPatientRepository>(),
                Mock.Of<IDoctorAvailabilityRepository>());

            Assert.Throws<InvalidStatusTransitionException>(() => service.Cancel(1));
        }

        [Fact]
        public void Complete_Succeeds_WhenAppointmentIsConfirmed()
        {
            var appointment = new Appointment
            {
                Id = 1,
                Status = AppointmentStatus.Confirmed,
                Doctor = new Doctor { Id = 1, Name = "Dr. Test" },
                Patient = new Patient { Id = 1, Name = "Test Patient" }
            };

            var appointmentRepoMock = new Mock<IAppointmentRepository>();
            appointmentRepoMock.Setup(r => r.GetById(1)).Returns(appointment);

            var service = new AppointmentService(
                appointmentRepoMock.Object,
                Mock.Of<IDoctorRepository>(),
                Mock.Of<IPatientRepository>(),
                Mock.Of<IDoctorAvailabilityRepository>());

            var result = service.Complete(1);

            Assert.Equal("Completed", result.Status);
        }

        [Fact]
        public void Complete_ThrowsInvalidStatusTransitionException_WhenAppointmentIsPending()
        {
            var appointment = new Appointment
            {
                Id = 1,
                Status = AppointmentStatus.Pending, // never confirmed
                Doctor = new Doctor { Id = 1, Name = "Dr. Test" },
                Patient = new Patient { Id = 1, Name = "Test Patient" }
            };

            var appointmentRepoMock = new Mock<IAppointmentRepository>();
            appointmentRepoMock.Setup(r => r.GetById(1)).Returns(appointment);

            var service = new AppointmentService(
                appointmentRepoMock.Object,
                Mock.Of<IDoctorRepository>(),
                Mock.Of<IPatientRepository>(),
                Mock.Of<IDoctorAvailabilityRepository>());

            Assert.Throws<InvalidStatusTransitionException>(() => service.Complete(1));
        }

        [Fact]
        public void Confirm_ThrowsInvalidStatusTransitionException_WhenAppointmentIsNotPending()
        {
            // Arrange
            var appointment = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                ScheduledAt = new DateTime(2026, 8, 3, 10, 0, 0),
                Status = AppointmentStatus.Cancelled, // already cancelled
                Doctor = new Doctor { Id = 1, Name = "Dr. Test" },
                Patient = new Patient { Id = 1, Name = "Test Patient" }
            };

            var appointmentRepoMock = new Mock<IAppointmentRepository>();
            appointmentRepoMock.Setup(r => r.GetById(1)).Returns(appointment);

            var service = new AppointmentService(
                appointmentRepoMock.Object,
                Mock.Of<IDoctorRepository>(),
                Mock.Of<IPatientRepository>(),
                Mock.Of<IDoctorAvailabilityRepository>());

            // Act & Assert
            Assert.Throws<InvalidStatusTransitionException>(() => service.Confirm(1));
        }


        [Fact]

        public void Create_ThrowsBookingConflictException_WhenDoctorAlreadyBookedAtSameTime()
        {
            // Arrange
            var doctor = new Doctor { Id = 1, Name = "Dr. Test", SpecialtyId = 1, ConsultationFee = 100 };
            var patient = new Patient { Id = 1, Name = "Test Patient" };
            var requestedTime = new DateTime(2026, 8, 3, 10, 0, 0); // a Monday, 10 AM

            var availability = new DoctorAvailability
            {
                DoctorId = 1,
                DayOfWeek = DayOfWeek.Monday,
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(14, 0)
            };

            var existingAppointment = new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 2,
                ScheduledAt = requestedTime,
                Status = AppointmentStatus.Pending
            };

            var doctorRepoMock = new Mock<IDoctorRepository>();
            doctorRepoMock.Setup(r => r.GetById(1)).Returns(doctor);

            var patientRepoMock = new Mock<IPatientRepository>();
            patientRepoMock.Setup(r => r.GetById(1)).Returns(patient);

            var availabilityRepoMock = new Mock<IDoctorAvailabilityRepository>();
            availabilityRepoMock.Setup(r => r.GetByDoctorId(1)).Returns(new List<DoctorAvailability> { availability });

            var appointmentRepoMock = new Mock<IAppointmentRepository>();
            appointmentRepoMock.Setup(r => r.GetByDoctorAndDate(1, requestedTime))
                .Returns(new List<Appointment> { existingAppointment });

            var service = new AppointmentService(
                appointmentRepoMock.Object,
                doctorRepoMock.Object,
                patientRepoMock.Object,
                availabilityRepoMock.Object);

            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                PatientId = 1,
                ScheduledAt = requestedTime
            };

            // Act & Assert
            Assert.Throws<BookingConflictException>(() => service.Create(dto));
        }
    }
}