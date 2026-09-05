using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Application.Contracts.Appointment;
using MS.Application.Contracts.Appointment.DTOs;
using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.Schedule.DTOs;
using MS.Domain.Entities.authentication;

namespace MS.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "adminAndPatient")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentServices _AppointmentServices;

        public AppointmentController(IAppointmentServices appointmentServices)
        {
            _AppointmentServices = appointmentServices;
        }

        /// <summary>
        ///     create a appointment for one patient        
        /// </summary>
        /// <param name="Appointment">
        ///     we need patient id , doctor id , appointment date and time
        ///     appointment time and date must be in doctor working hours
        /// </param>
        /// <returns>
        ///     the created appointment details
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<APIResponse<AppointmentViewDTO>>> Create([FromBody] AppointmentCreateDTO Appointment)
        {
            var result = await _AppointmentServices.CreateAsync(Appointment);
            if (result.StatusCode != 201)
            {
                return StatusCode(result.StatusCode, result.Errors);
            }            
            return CreatedAtAction(nameof(GetPatientAppointments) , new { patinetId = result.Data.id} , result.Data);
        }

        /// <summary>
        ///     chenge details of a appointment
        /// </summary>
        /// <param name="Appointment">
        ///     we need patient id , doctor id , appointment date and time , appointment id
        ///     appointment time and date must be in doctor working hours
        /// </param>
        /// <returns>
        ///     the updated appointment details
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<APIResponse<AppointmentViewDTO>>> Update([FromBody] AppointmentUpdateDTO Appointment)
        {
            var result = await _AppointmentServices.UpdateAsync(Appointment);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result.Errors);
            }
            return Ok(result);
        }

        /// <summary>
        ///     cancel a appointment
        ///     by changin appointment status
        /// </summary>
        /// <param name="appointmentId">
        /// </param>
        /// <returns>
        ///     the cancelled appointment details
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{appointmentId}")]
        public async Task<ActionResult<APIResponse<AppointmentViewDTO>>> Cancel(int appointmentId)
        {
            var result = await _AppointmentServices.DeleteAsync(appointmentId);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result.Errors);
            }
            return Ok(result);
        }
        /// <summary>
        ///     get details of patient appointments
        /// </summary>
        /// <param name="patinetId">
        /// </param>
        /// <returns>
        ///     patient appointments List
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{patinetId}")]
        public async Task<ActionResult<APIResponse<List<AppointmentViewDTO>>>> GetPatientAppointments(int patinetId)
        {
            var result = await _AppointmentServices.GetAllPatientAppointmentsAsync(patinetId);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result.Errors);
            }
            return Ok(result);
        }

        /// <summary>
        ///     show the working dates and hours for a doctor in specefic date and time
        /// </summary>
        /// <param name="doctorId"></param>
        /// <param name="date"></param>
        /// <returns>
        ///     the doctor appoinment times , the appointment time is 20 minutes
        /// </returns>        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<APIResponse<List<AppointmentsTimeDTO>>>> GetScheduleForSpecificDate(int doctorId, DateOnly date)
        {
            var result = await _AppointmentServices.getDoctorAppointmentForSpecificDateAsync(doctorId, date);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result.Errors);
            }
            return Ok(result);
        }
    }
}
