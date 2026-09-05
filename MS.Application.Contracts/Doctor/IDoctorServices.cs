using MS.Application.Contracts.Doctor.DTOs;
using MS.Application.Contracts.Helpers;

namespace MS.Application.Contracts.Doctor;
public interface IDoctorServices
{
    Task<APIResponse<DoctorViewDTO>> CreateAsync(DoctorCreateDTO command);
    Task<APIResponse<DoctorViewDTO>> UpdateAsync (DoctorUpdateDTO command);
    Task<APIResponse<DoctorViewDTO>> GetDoctorByIdAsync (int id);
    Task<APIResponse<List<DoctorViewDTO>>> GetAllDoctorsAsync();
}
