using  MediBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediBook.Application.Interfaces
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAll();
        Appointment? GetById(int id);
        void Add(Appointment appointment);
        List <Appointment> GetByDoctorAndDate(int doctorId,DateTime date);
        void update(Appointment appointment);
        void SaveChanges();
    }
}
