using MS.Application.Contracts.Doctor;
using MS.Application.Contracts.Doctor.DTOs;
using MS.Application.Contracts.Helpers;
using MS.Domain.Entities.doctors;

namespace MS.Application.Services;

public class DoctorServices : IDoctorServices
{
    private readonly IDoctorRepository _doctorRepository;
    public DoctorServices(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<APIResponse<DoctorViewDTO>> CreateAsync(DoctorCreateDTO command)
    {         
        try
        {
            if (command is null)
            {
                return new APIResponse<DoctorViewDTO>(400, ErrorMessages.ValueMustNotBeNull);                
            }
            if(await _doctorRepository.ExistAsync(s => s.MedicalSystemNumber == command.MedicalSystemNumber))
            {
                return new APIResponse<DoctorViewDTO>(400, ErrorMessages.ValueCanNotBeDuplicated);
            }

            Doctor newDoctor = new Doctor(command.FullName.Trim(), command.Speciality.Trim(), command.MedicalSystemNumber.Trim());
            await _doctorRepository.CreateAsync(newDoctor);
            await _doctorRepository.SaveChangesAync();

            DoctorViewDTO data = new()
            {
                Id = newDoctor.Id,
                FullName = newDoctor.FullName,
                MedicalSystemNumber = newDoctor.MedicalSystemNumber,
                Speciality = newDoctor.Speciality
            };
            return new APIResponse<DoctorViewDTO>(201,data);
        }
        catch (Exception ex)
        {
            return new APIResponse<DoctorViewDTO>(500, ex.Message);
        }
    }
    public async Task<APIResponse<DoctorViewDTO>> UpdateAsync(DoctorUpdateDTO command)
    {        
        try
        {
            var OldDoctor = await _doctorRepository.GetByIdAsync(command.Id);
            if (OldDoctor is null)
            {
                return new APIResponse<DoctorViewDTO>(404, ErrorMessages.NotFound);
            }
            if (await _doctorRepository.ExistAsync(s => s.MedicalSystemNumber == command.MedicalSystemNumber
            && s.Id != command.Id))
            {                
                return new APIResponse<DoctorViewDTO>(400, ErrorMessages.ValueCanNotBeDuplicated);
            }
            OldDoctor.update(command.FullName.Trim(),command.Speciality.Trim(),command.MedicalSystemNumber.Trim());
            _doctorRepository.UpdateAsync(OldDoctor);
            await _doctorRepository.SaveChangesAync();

            DoctorViewDTO data = new()
            {
                Id = command.Id,
                FullName = command.FullName,
                MedicalSystemNumber = command.MedicalSystemNumber,
                Speciality = command.Speciality
            };
            return new APIResponse<DoctorViewDTO>(200, data);

        }
        catch (Exception ex)
        {
            return new APIResponse<DoctorViewDTO>(500, ex.Message);
        }
    }
    public async Task<APIResponse<List<DoctorViewDTO>>> GetAllDoctorsAsync()
    {
        try
        {
            var doctors = await _doctorRepository.GetAllAsync();
            if (!doctors.Any())
            {
                return new APIResponse<List<DoctorViewDTO>>(404, ErrorMessages.NotFound);
            }
            List<DoctorViewDTO> Result = doctors.Select(d => new DoctorViewDTO
            {
                Id = d.Id,
                FullName = d.FullName,
                MedicalSystemNumber = d.MedicalSystemNumber,
                Speciality = d.Speciality,
            }).ToList();
            
            return new APIResponse<List<DoctorViewDTO>>(200, Result);
        }
        catch (Exception ex)
        {
            return new APIResponse<List<DoctorViewDTO>>(500, ex.Message);
        }
    }
    public async Task<APIResponse<DoctorViewDTO>> GetDoctorByIdAsync(int id)
    {
        try
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor is null)
            {
                return new APIResponse<DoctorViewDTO>(404, ErrorMessages.NotFound);
            }
            DoctorViewDTO Result = new()
            {
                Id = id,
                FullName = doctor.FullName,
                Speciality = doctor.Speciality,
                MedicalSystemNumber = doctor.MedicalSystemNumber
            };
            return new APIResponse<DoctorViewDTO>(200 , Result);
        }
        catch (Exception ex)
        {
            return new APIResponse<DoctorViewDTO>(500, ex.Message);
        }        
    }
}
