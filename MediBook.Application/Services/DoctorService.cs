using MediBook.Application.Dtos;

using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;

namespace MediBook.Application.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISpecialtyRepository _specialtyRepository;

        public DoctorService(IDoctorRepository doctorRepository, ISpecialtyRepository specialtyRepository)
        {
            _doctorRepository = doctorRepository;
            _specialtyRepository = specialtyRepository;
        }

        public List<DoctorResponseDto> GetAll()
        {
            return _doctorRepository.GetAll()
                .Select(MapToDto)
                .ToList();
        }

        public DoctorResponseDto? GetById(int id)
        {
            var doctor = _doctorRepository.GetById(id);
            return doctor == null ? null : MapToDto(doctor);
        }

        public DoctorResponseDto? Create(DoctorDto dto)
        {
            var specialty = _specialtyRepository.GetById(dto.SpecialtyId);
            if (specialty == null) return null; // invalid SpecialtyId

            var doctor = new Doctor
            {
                Name = dto.Name,
                SpecialtyId = dto.SpecialtyId,
                ConsultationFee = dto.ConsultationFee
            };

            _doctorRepository.Add(doctor);
            _doctorRepository.SaveChanges();

            doctor.Specialty = specialty; // attach for the response mapping below
            return MapToDto(doctor);
        }

        private static DoctorResponseDto MapToDto(Doctor doctor)
        {
            return new DoctorResponseDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                SpecialtyName = doctor.Specialty?.Name ?? string.Empty,
                ConsultationFee = doctor.ConsultationFee
            };
        }
    }
}