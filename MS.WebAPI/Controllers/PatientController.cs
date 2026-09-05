using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.Patient;
using MS.Application.Contracts.Patient.DTOs;
using MS.Domain.Entities.authentication;
using MS.Domain.Entities.patients;

namespace MS.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]

    public class PatientController : Controller
    {
        private readonly IPatientServices _patientServices;

        public PatientController(IPatientServices patientServices)
        {
            _patientServices = patientServices;
        }

        /// <summary>
        ///     add new patient
        /// </summary>
        /// <param name="patient">
        ///     we need patient first name and last name as fullname and patient mobile
        ///     mobile must valid and 11 number
        /// </param>
        /// <returns>
        ///     the details of new patient
        /// </returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "adminAndPatient")]
        [HttpPost]
        public async Task<ActionResult<APIResponse<PatientViewDTO>>> AddPatient([FromBody] PatientCreateDTO patient)
        {
            var newPatient = await _patientServices.CreateAsync(patient);
            if (newPatient.StatusCode != 201) 
            {
                return StatusCode(newPatient.StatusCode,newPatient.Errors);
            }
            return CreatedAtAction(nameof(GetPatientById), new { patientId = newPatient.Data.Id} , newPatient.Data);
        }

        /// <summary>
        ///     get patient details
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>
        ///     patient details
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "adminAndPatient")]
        [HttpGet("{patientId}")]
        public async Task<ActionResult<APIResponse<PatientViewDTO>>> GetPatientById(int patientId)
        {
            var patient = await _patientServices.GetPatientByIdAsync(patientId);
            if (patient.StatusCode != 200)
            {
                return StatusCode(patient.StatusCode, patient.Errors);
            }
            return Ok(patient);
        }

        /// <summary>
        ///     show all patients
        /// </summary>
        /// <returns>
        ///     patients list with ever patient details
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "adminOnly")]
        [HttpGet]
        public async Task<ActionResult<List<APIResponse<PatientViewDTO>>>> GetAllPatients()
        {
            var patient = await _patientServices.GetAllPatientAsync();
            if (patient.StatusCode != 200)
            {
                return StatusCode(patient.StatusCode, patient.Errors);
            }
            return Ok(patient);
        }

        /// <summary>
        ///     change patient datails
        /// </summary>
        /// <param name="newPatient"></param>
        /// <returns>
        ///     the update patient details
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "adminAndPatient")]
        [HttpPut]
        public async Task<ActionResult<APIResponse<PatientViewDTO>>> Update([FromBody] PatientUpdateDTO newPatient)
        {
            var patient = await _patientServices.UpdateAsync(newPatient);
            if (patient.StatusCode != 200)
            {
                return StatusCode(patient.StatusCode, patient.Errors);
            }
            return Ok(patient);
        }
        
    }
}
