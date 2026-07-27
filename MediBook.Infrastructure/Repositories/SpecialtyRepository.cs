using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;
using MediBook.Infrastructure.Data;
namespace MediBook.Infrastructure.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly MediBookDbContext _db;
        public SpecialtyRepository(MediBookDbContext db)
        {
            _db = db;
        }
        public List<Specialty> GetAll()
        {
            return _db.Specialties.ToList();
        }
        public Specialty? GetById(int id)
        {
            return _db.Specialties.FirstOrDefault(s => s.Id == id);
        }
        public void Add(Specialty specialty)
        {
            _db.Specialties.Add(specialty);
        }
        public void saveChanges()
        {
            _db.SaveChanges();
        }

    }
}
