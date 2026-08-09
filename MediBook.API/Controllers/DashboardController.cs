using MediBook.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;
        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet]
        public IActionResult Get() => Ok(_dashboardService.GetDashboard());





    }
}
