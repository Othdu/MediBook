using MediBook.Application.Interfaces;
using MediBook.Domain.Entities;
using MediBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly MediBookDbContext _db;

        public AppointmentRepository(MediBookDbContext db)
        {
            _db = db;
        }

        public List<Appointment> GetAll() =>
            _db.Appointments.Include(a => a.Doctor).Include(a => a.Patient).ToList();

        public Appointment? GetById(int id) =>
            _db.Appointments.Include(a => a.Doctor).Include(a => a.Patient)
                .FirstOrDefault(a => a.Id == id);

        public List<Appointment> GetByDoctorAndDate(int doctorId, DateTime date) =>
            _db.Appointments
                .Where(a => a.DoctorId == doctorId && a.ScheduledAt.Date == date.Date)
                .ToList();

        public void Add(Appointment appointment) => _db.Appointments.Add(appointment);

        public void Update(Appointment appointment) => _db.Appointments.Update(appointment);

        public void SaveChanges() => _db.SaveChanges();
    }
}