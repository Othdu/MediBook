using MediBook.Application.Dtos;
using MediBook.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AppointmentsController : ControllerBase

    {

        private readonly AppointmentService _appointmentService;
        public AppointmentsController(AppointmentService appointmentService) => _appointmentService = appointmentService;
        [HttpGet]
        public ActionResult GetAll() => Ok(_appointmentService.GetAll());
        [HttpPost]
        public ActionResult Create(CreateAppointmentDto dto)=> Ok(_appointmentService.Create(dto));


        [HttpPatch("{id}/confirm")]
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult Confirm(int id) => Ok(_appointmentService.Confirm(id));

        [HttpPatch("{id}/cancel")]
        public ActionResult Cancel(int id) => Ok(_appointmentService.Cancel(id));

        [HttpPatch("{id}/complete")]
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult Complete(int id) => Ok(_appointmentService.Complete(id));
        [HttpGet("filter")]
        public ActionResult GetFiltered([FromQuery] AppointmentFilterDto filter) => Ok(_appointmentService.GetFiltered(filter));
    }
}
