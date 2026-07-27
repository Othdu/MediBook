using MediBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediBook.Application.Interfaces
{
    public interface ISpecialtyRepository
    {
        List<Specialty> GetAll();
        Specialty? GetById(int id);
        void Add(Specialty specialty);
        void saveChanges();
    }
}
