using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;

namespace MediBook.Application.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public List<PatientResponseDto> GetAll() =>
            _patientRepository.GetAll().Select(p => new PatientResponseDto { Id = p.Id, Name = p.Name }).ToList();


        public PatientResponseDto Create(PatientDto dto)
        {
            var patient = new Patient
            {
                Name = dto.Name
            };
               _patientRepository.Add(patient);
            _patientRepository.SaveChanges();
            return new PatientResponseDto
            {
                 Id = patient.Id, Name = patient.Name
            };
        }
    

    }
}
