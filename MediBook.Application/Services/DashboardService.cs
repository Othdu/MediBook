using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using MediBook.Domain.Enums;

namespace MediBook.Application.Services
{
    public class DashboardService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public DashboardService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public DashboardResponseDto GetDashboard()
        {
            var allAppointments = _appointmentRepository.GetAll();

            var todayCount = allAppointments.Count(a => a.ScheduledAt.Date == DateTime.Today);

            var totalRevenue = allAppointments
                .Where(a => a.Status == AppointmentStatus.Completed)
                .Sum(a => a.Doctor?.ConsultationFee ?? 0);

            var busiest = allAppointments
                .GroupBy(a => a.Doctor)
                .Select(g => new { Doctor = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefault();

            return new DashboardResponseDto
            {
                TodayAppointmentCount = todayCount,
                TotalRevenue = totalRevenue,
                BusiestDoctorName = busiest?.Doctor?.Name ?? "N/A",
                BusiestDoctorAppointmentCount = busiest?.Count ?? 0
            };
        }
    }
}