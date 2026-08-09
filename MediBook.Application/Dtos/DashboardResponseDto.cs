namespace MediBook.Application.Dtos
{
    public class DashboardResponseDto
    {
        public int TodayAppointmentCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public string BusiestDoctorName { get; set; } = string.Empty;
        public int BusiestDoctorAppointmentCount { get; set; }
    }
}