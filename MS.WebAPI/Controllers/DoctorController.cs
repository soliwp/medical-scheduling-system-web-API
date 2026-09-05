using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Application.Contracts.Doctor;
using MS.Application.Contracts.Doctor.DTOs;
using MS.Application.Contracts.Helpers;
using MS.Domain.Entities.authentication;

namespace MS.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DoctorController : Controller
    {
        private readonly IDoctorServices _DoctorServices;

        public DoctorController(IDoctorServices doctorServices)
        {
            _DoctorServices = doctorServices;
        }

        /// <summary>
        ///     add a new doctor
        /// </summary>
        /// <param name="newDoctor">
        ///     we must enter doctor first name and last name as fullName , doctor speciality and doctor medical system number
        /// </param>
        /// <returns>
        ///     the details of new doctor
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize("adminOnly")]
        [HttpPost]
        public async Task<ActionResult<APIResponse<DoctorViewDTO>>> AddDoctor([FromBody] DoctorCreateDTO newDoctor)
        {
            var result = await _DoctorServices.CreateAsync(newDoctor);
            if(result.StatusCode != 201)
            {
                return StatusCode(result.StatusCode,result);
            }
            return CreatedAtAction(nameof(GetDoctorById) , new { doctorId = result.Data.Id } , result.Data);
        }

        /// <summary>
        ///     this action method is for changing doctor datails
        /// </summary>
        /// <param name="doctor">
        ///     we must enter doctor id , doctor first name and last name as full name , doctor speciality and doctor medical system number
        /// </param>
        /// <returns>
        ///     the updated doctor details 
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize("adminOnly")]
        [HttpPut]
        public async Task<ActionResult<APIResponse<DoctorViewDTO>>> UpdateDoctor([FromBody] DoctorUpdateDTO doctor)
        {
            var result = await _DoctorServices.UpdateAsync(doctor);
            if(result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode,result);
            }
            return Ok(result);
        }

        /// <summary>
        ///     get doctor details by doctor id
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns>
        ///     doctor datails
        /// </returns>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize("adminAndPatient")]
        [HttpGet("{doctorId}")]
        public async Task<ActionResult<APIResponse<DoctorViewDTO>>> GetDoctorById(int doctorId)
        {
            var result = await _DoctorServices.GetDoctorByIdAsync(doctorId);
            if(result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode,result);
            }
            return Ok(result);
        }

        /// <summary>
        ///     show all doctors with details
        /// </summary>
        /// <returns>
        ///     list of doctors
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize("adminAndPatient")]
        [HttpGet]
        public async Task<ActionResult<APIResponse<List<DoctorViewDTO>>>> GetAllDoctors()
        {
            var result = await _DoctorServices.GetAllDoctorsAsync();
            if(result.StatusCode != 200)
            {
                return StatusCode(result.StatusCode,result);
            }
            return Ok(result);
        }
    }
}
