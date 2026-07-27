using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;
using MediBook.Infrastructure.Data;

namespace MediBook.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly MediBookDbContext _db;

        public DoctorRepository(MediBookDbContext db)
        {
            _db = db;
        }

        public List<Doctor> GetAll() => _db.Doctors.ToList();

        public Doctor? GetById(int id) => _db.Doctors.FirstOrDefault(d => d.Id == id);

        public void Add(Doctor doctor) => _db.Doctors.Add(doctor);

        public void SaveChanges() => _db.SaveChanges();
    }
}