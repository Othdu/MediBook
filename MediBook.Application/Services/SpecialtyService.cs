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
    public class SpecialtyService
    {
        private readonly ISpecialtyRepository _Repository;
        public SpecialtyService(ISpecialtyRepository Repository)
        {
            _Repository = Repository;
        }
        public List<SpecialtyResponseDto> GetAll()
        {
            return _Repository.GetAll().Select(s => new SpecialtyResponseDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
      
        public SpecialtyResponseDto Create(SpecialtyDto dto)
        {
            var specialty = new Specialty
            {
                Name = dto.Name
            };
            _Repository.Add(specialty);
            _Repository.saveChanges();
            return new SpecialtyResponseDto
            {
                Id = specialty.Id,
                Name = specialty.Name
            };
        }
    }
}
