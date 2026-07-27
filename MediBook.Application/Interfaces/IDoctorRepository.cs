using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediBook.Domain.Entities;

namespace MediBook.Application.Interfaces
{
    public interface IDoctorRepository
    {
        List <Doctor> GetAll();
        Doctor? GetById(int id);
        void Add(Doctor doctor);
        void SaveChanges();
    }
}
