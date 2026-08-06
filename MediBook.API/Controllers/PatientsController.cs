using MediBook.Application.Dtos;
using MediBook.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.API.Controllers
{
    

        [ApiController]
        [Route("api/[controller]")]
        public class PatientsController : ControllerBase
        {
            private readonly PatientService _patientService;
            public PatientsController(PatientService patientService) => _patientService = patientService;
            [HttpGet]
            public ActionResult GetAll() => Ok(_patientService.GetAll());
            [HttpPost]
            public ActionResult Create(PatientDto dto)=> Ok(_patientService.Create(dto));
    }
}
