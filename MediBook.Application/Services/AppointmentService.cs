using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;
using MediBook.Domain.Enums;
using MediBook.Domain.Exceptions;

namespace MediBook.Application.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorAvailabilityRepository _availabilityRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository, IPatientRepository patientRepository, IDoctorAvailabilityRepository availabilityRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _availabilityRepository = availabilityRepository;
        }
        public AppointmentResponseDto Confirm(int id)
        {
            var appointment = _appointmentRepository.GetById(id)
                ?? throw new SlotUnavailableException("Appointment not found.");

            if (appointment.Status != AppointmentStatus.Pending)
                throw new InvalidStatusTransitionException($"Cannot confirm an appointment with status '{appointment.Status}'.");

            appointment.Status = AppointmentStatus.Confirmed;
            _appointmentRepository.Update(appointment);
            _appointmentRepository.SaveChanges();

            return MapToDto(appointment);
        }

        public AppointmentResponseDto Cancel(int id)
        {
            var appointment = _appointmentRepository.GetById(id)
                ?? throw new SlotUnavailableException("Appointment not found.");

            if (appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Cancelled)
                throw new InvalidStatusTransitionException($"Cannot cancel an appointment with status '{appointment.Status}'.");

            appointment.Status = AppointmentStatus.Cancelled;
            _appointmentRepository.Update(appointment);
            _appointmentRepository.SaveChanges();

            return MapToDto(appointment);
        }

        public AppointmentResponseDto Complete(int id)
        {
            var appointment = _appointmentRepository.GetById(id)
                ?? throw new SlotUnavailableException("Appointment not found.");

            if (appointment.Status != AppointmentStatus.Confirmed)
                throw new InvalidStatusTransitionException($"Cannot complete an appointment with status '{appointment.Status}'.");

            appointment.Status = AppointmentStatus.Completed;
            _appointmentRepository.Update(appointment);
            _appointmentRepository.SaveChanges();

            return MapToDto(appointment);
        }
        public AppointmentResponseDto Create(CreateAppointmentDto dto)
        {
            var doctor = _doctorRepository.GetById(dto.DoctorId) ?? throw new SlotUnavailableException($"Doctor with ID {dto.DoctorId} not found.");
            var patient = _patientRepository.GetById(dto.PatientId) ?? throw new SlotUnavailableException($"Patient Not Found");
            var requestedDay = dto.ScheduledAt.DayOfWeek;
            var requestedTime = TimeOnly.FromDateTime(dto.ScheduledAt);

            var availabilities = _availabilityRepository.GetByDoctorId(dto.DoctorId);
            bool isWithinAvailability = availabilities.Any(a =>
                a.DayOfWeek == requestedDay &&
                requestedTime >= a.StartTime &&
                requestedTime < a.EndTime);

            if (!isWithinAvailability)
                throw new SlotUnavailableException("The doctor is not available at the requested time.");
            var sameDayAppointments = _appointmentRepository.GetByDoctorAndDate(dto.DoctorId, dto.ScheduledAt);
            bool hasConflict = sameDayAppointments.Any(a =>
                a.ScheduledAt == dto.ScheduledAt &&
                a.Status != AppointmentStatus.Cancelled);


            if (hasConflict)
                throw new BookingConflictException("This doctor already has an appointment at the requested time.");


            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                ScheduledAt = dto.ScheduledAt,
                Status = AppointmentStatus.Pending
            };
            _appointmentRepository.Add(appointment);
            _appointmentRepository.SaveChanges();
            return MapToDto(appointment);
        }

        public List<AppointmentResponseDto> GetAll() =>
            _appointmentRepository.GetAll().Select(MapToDto).ToList();


        private static AppointmentResponseDto MapToDto(Appointment a) => new()
        {
            Id = a.Id,
            DoctorName = a.Doctor?.Name ?? string.Empty,
            PatientName = a.Patient?.Name ?? string.Empty,
            ScheduledAt = a.ScheduledAt,
            Status = a.Status.ToString()
        };
    }
}