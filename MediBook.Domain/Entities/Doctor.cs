using MediBook.Domain.Enums;

namespace MediBook.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int SpecialtyId { get; set; }
        public Specialty? Specialty { get; set; }

        public decimal ConsultationFee { get; set; }
        public List<Appointment> Appointments { get; set; } = new();
        public List<DoctorAvailability> Availabilities { get; set; } = new();
    }
}