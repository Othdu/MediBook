using MediBook.Application.Dtos;
using MediBook.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorAvailabilitiesController : ControllerBase
    {
        private readonly DoctorAvailabilityService _availabilityService;
        public DoctorAvailabilitiesController(DoctorAvailabilityService availabilityService) => _availabilityService = availabilityService;

        [HttpGet("doctor/{doctorId}")]
        public ActionResult GetByDoctorId(int doctorId) => Ok(_availabilityService.GetByDoctorId(doctorId));

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult Create(DoctorAvailabilityDto dto)
        {
            var result = _availabilityService.Create(dto);
            if (result == null) return BadRequest("Invalid doctor or time range.");
            return Ok(result);
        }
    }
}