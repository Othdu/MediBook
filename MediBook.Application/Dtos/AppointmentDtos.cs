using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MediBook.Application.Dtos
{
    public class CreateAppointmentDto
    {
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]      
        public DateTime ScheduledAt { get; set; }
        
    }
    public class AppointmentResponseDto
    {
        public int Id { get; set; }
        public string DoctorName { get; set; } = string.Empty;

        public string PatientName { get; set; } = string.Empty;


        public DateTime ScheduledAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
