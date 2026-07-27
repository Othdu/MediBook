using MediBook.Application.Dtos;

using MediBook.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtiesController : ControllerBase
    {
        private readonly SpecialtyService _specialtyService;
        public SpecialtiesController(SpecialtyService specialtyService) => _specialtyService = specialtyService;

        [HttpGet]
        public ActionResult GetAll() => Ok(_specialtyService.GetAll());

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(SpecialtyDto dto) => Ok(_specialtyService.Create(dto));
    }
}