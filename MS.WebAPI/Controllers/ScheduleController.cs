using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.Schedule;
using MS.Application.Contracts.Schedule.DTOs;
using MS.Domain.Entities.authentication;

namespace MS.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ScheduleController : Controller
    {
        private readonly IScheduleServices _scheduleServices;

        public ScheduleController(IScheduleServices scheduleServices)
        {
            _scheduleServices = scheduleServices;
        }

        /// <summary>
        ///     create working hours and date for a doctor
        /// </summary>
        /// <param name="schedule">
        ///     we need doctor id , date , start work and finish work
        /// </param>
        /// <returns>
        ///     the new working hours and date with doctor id
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "adminOnly")]
        [HttpPost]
        public async Task<ActionResult<APIResponse<ScheduleViewDTO>>> Create([FromBody] ScheduleCreateDTO schedule)
        {
            var result = await _scheduleServices.CreateAync(schedule);
            if(result.StatusCode != 201)
            {
                return StatusCode(result.StatusCode , result.Errors);
            }
            return CreatedAtAction(nameof(GetDoctorSchedules) , new { doctorId = result.Data.DoctorId} , result.Data);
        }

        /// <summary>
        ///     change date or working hours for a doctor with doctor id
        /// </summary>
        /// <param name="schedule">
        ///     we need doctor id , date , start work and finish work and schedule id 
        /// </param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "adminOnly")]
        [HttpPut]
        public async Task<ActionResult<APIResponse<ScheduleViewDTO>>> Update([FromBody] ScheduleUpdateDTO schedule)
        {
            var result = await _scheduleServices.UpdateAsync(schedule);
            if(result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode , result.Errors);
            }
            return Ok(result);
        }

        /// <summary>
        ///     show doctor start work time and finish work time for dates are registerd
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns>
        ///     show the List of doctor working hours with dates
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "adminAndPatient")]
        [HttpGet("{doctorId}")]
        public async Task<ActionResult<APIResponse<List<ScheduleViewDTO>>>> GetDoctorSchedules(int doctorId)
        {
            var result = await _scheduleServices.GetScheduleByDoctorIdAsync(doctorId);
            if (result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode, result.Errors);
            }
            return Ok(result);
        }
    }
}
