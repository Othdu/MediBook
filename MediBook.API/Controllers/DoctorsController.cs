using MediBook.Application.Dtos;
using MediBook.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorService _doctorService;
        public DoctorsController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [HttpGet]
        public ActionResult GetALl()=> Ok(_doctorService.GetAll());
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var doctor = _doctorService.GetById(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create( DoctorDto dto)
        {
            
            var result = _doctorService.Create(dto);
            if (result == null) return BadRequest("SpecialtyId Not Found");
            return Ok(result);
        }
    }
}
