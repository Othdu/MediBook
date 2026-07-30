using System.ComponentModel.DataAnnotations;

namespace MediBook.Application.Dtos
{
    public class DoctorDto
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int SpecialtyId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Consultation fee must be greater than 0")]
        public decimal ConsultationFee { get; set; }
    }

    public class DoctorResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SpecialtyName { get; set; } = string.Empty;
        public decimal ConsultationFee { get; set; }
    }
}