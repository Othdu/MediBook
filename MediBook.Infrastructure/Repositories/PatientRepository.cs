using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;
using MediBook.Infrastructure.Data;

namespace MediBook.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly MediBookDbContext _db;

        public PatientRepository(MediBookDbContext db)
        {
            _db = db;
        }

        public List<Patient> GetAll() => _db.Patients.ToList();

        public Patient? GetById(int id) => _db.Patients.FirstOrDefault(p => p.Id == id);

        public void Add(Patient patient) => _db.Patients.Add(patient);

        public void SaveChanges() => _db.SaveChanges();
    }
}