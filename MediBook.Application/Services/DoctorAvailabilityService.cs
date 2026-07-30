using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;

namespace MediBook.Application.Services
{
    public class DoctorAvailabilityService
    {
        private readonly IDoctorAvailabilityRepository _availabilityRepository;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorAvailabilityService(IDoctorAvailabilityRepository availabilityRepository, IDoctorRepository doctorRepository)
        {
            _availabilityRepository = availabilityRepository;
            _doctorRepository = doctorRepository;
        }

        public List<DoctorAvailabilityResponseDto> GetByDoctorId(int doctorId)
        {
            return _availabilityRepository.GetByDoctorId(doctorId)
                .Select(a => new DoctorAvailabilityResponseDto
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor?.Name ?? string.Empty,
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                })
                .ToList();
        }

        public DoctorAvailabilityResponseDto? Create(DoctorAvailabilityDto dto)
        {
            var doctor = _doctorRepository.GetById(dto.DoctorId);
            if (doctor == null) return null;

            if (dto.EndTime <= dto.StartTime) return null; // invalid time range

            var availability = new DoctorAvailability
            {
                DoctorId = dto.DoctorId,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            _availabilityRepository.Add(availability);
            _availabilityRepository.saveChanges();

            return new DoctorAvailabilityResponseDto
            {
                Id = availability.Id,
                DoctorId = doctor.Id,
                DoctorName = doctor.Name,
                DayOfWeek = availability.DayOfWeek,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime
            };
        }
    }
}