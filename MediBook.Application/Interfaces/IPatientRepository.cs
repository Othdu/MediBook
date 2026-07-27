using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediBook.Domain.Entities;
namespace MediBook.Application.Interfaces
{
    public interface IPatientRepository
    {
        List<Patient> GetAll();
        Patient? GetById(int id);
        void Add(Patient patient);
        void SaveChanges();
    }
}
