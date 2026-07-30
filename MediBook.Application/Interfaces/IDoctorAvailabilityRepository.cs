using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediBook.Domain.Entities;

namespace MediBook.Application.Interfaces
{
    public interface IDoctorAvailabilityRepository
    {
        List<DoctorAvailability> GetByDoctorId(int doctorId);
        void Add(DoctorAvailability availability);

        void saveChanges();
    }

}
