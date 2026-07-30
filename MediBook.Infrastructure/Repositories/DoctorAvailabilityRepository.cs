using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;
using MediBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Infrastructure.Repositories
{
    public class DoctorAvailabilityRepository : IDoctorAvailabilityRepository
    {
        private readonly MediBookDbContext _db;
        public DoctorAvailabilityRepository(MediBookDbContext db) => _db = db;

        public List<DoctorAvailability> GetByDoctorId(int doctorId) =>
            _db.DoctorAvailabilities.Include(a => a.Doctor)
                .Where(a => a.DoctorId == doctorId)
                .ToList();

        public void Add(DoctorAvailability availability) => _db.DoctorAvailabilities.Add(availability);
        public void saveChanges() => _db.SaveChanges();
    }
}