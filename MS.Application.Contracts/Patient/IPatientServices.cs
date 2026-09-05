using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.Patient.DTOs;

namespace MS.Application.Contracts.Patient;
public interface IPatientServices
{
    Task<APIResponse<PatientViewDTO>> CreateAsync (PatientCreateDTO command);
    Task<APIResponse<PatientViewDTO>> UpdateAsync (PatientUpdateDTO command);
    Task<APIResponse<PatientViewDTO>> GetPatientByIdAsync(int id);
    Task<APIResponse<List<PatientViewDTO>>> GetAllPatientAsync();

}
