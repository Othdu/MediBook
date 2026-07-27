using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Infrastructure.Data
{
    public class MediBookDbContext :DbContext
    {
        public MediBookDbContext(DbContextOptions<MediBookDbContext> options) : base(options) { }

        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Specialty> Specialties => Set<Specialty>();
        public DbSet<DoctorAvailability> DoctorAvailabilities => Set<DoctorAvailability>(); 
        public DbSet<User> Users => Set<User>();
    }
}
