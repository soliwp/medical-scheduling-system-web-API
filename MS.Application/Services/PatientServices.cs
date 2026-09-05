using MS.Application.Contracts.Doctor.DTOs;
using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.Patient;
using MS.Application.Contracts.Patient.DTOs;
using MS.Domain.Entities.patients;

namespace MS.Application.Services;

public class PatientServices : IPatientServices
{
    private readonly IPatientRepository _patientRepository;

    public PatientServices(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<APIResponse<PatientViewDTO>> CreateAsync(PatientCreateDTO command)
    {        
        try
        {
            if(command is null)
            {
                return new APIResponse<PatientViewDTO>(400, ErrorMessages.ValueCanNotBeDuplicated);
            }
            if(await _patientRepository.ExistAsync(p => p.Mobile == command.Mobile))
            {
                return new APIResponse<PatientViewDTO>(400, ErrorMessages.ValueCanNotBeDuplicated);                
            }
            Patient newPatent = new(command.FullName.Trim(),command.Mobile.Trim());
            await _patientRepository.CreateAsync(newPatent);
            await _patientRepository.SaveChangesAync();

            PatientViewDTO result = new()
            {
                Id = newPatent.Id,
                FullName = newPatent.FullName,
                Mobile = newPatent.Mobile
            };
            return new APIResponse<PatientViewDTO>(201, result);
        }
        catch (Exception ex)
        {
            return new APIResponse<PatientViewDTO>(500, ex.Message);
        }
    }
    public async Task<APIResponse<PatientViewDTO>> UpdateAsync(PatientUpdateDTO command)
    {        
        try
        {
            var patient = await _patientRepository.GetByIdAsync(command.Id);
            if(patient is null)
            {
                return new APIResponse<PatientViewDTO>(404, ErrorMessages.NotFound);
            }
            if (await _patientRepository.ExistAsync(p => p.Mobile == command.Mobile
            && p.Id != command.Id)) 
            {
                return new APIResponse<PatientViewDTO>(400, ErrorMessages.ValueCanNotBeDuplicated);
            }

            patient.Update(command.FullName.Trim() , command.Mobile.Trim());
            _patientRepository.UpdateAsync(patient);
            await _patientRepository.SaveChangesAync();

            PatientViewDTO result = new()
            {
                Id = command.Id,
                FullName = command.FullName,
                Mobile = command.Mobile
            };
            return new APIResponse<PatientViewDTO>(200, result);
        }
        catch (Exception ex)
        {
            return new APIResponse<PatientViewDTO>(500, ex.Message);
        }
    }

    public async Task<APIResponse<PatientViewDTO>> GetPatientByIdAsync(int id)
    {
        try
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if(patient is null)
            {
                return new APIResponse<PatientViewDTO>(404, ErrorMessages.NotFound);
            }
            PatientViewDTO result = new()
            {
                Id = id,
                FullName = patient.FullName,
                Mobile = patient.Mobile
            };
            return new APIResponse<PatientViewDTO>(200, result);
        }
        catch (Exception ex)
        {
            return new APIResponse<PatientViewDTO>(500, ex.Message);
        }
    }

    public async Task<APIResponse<List<PatientViewDTO>>> GetAllPatientAsync()
    {
        try
        {
            var patients = await _patientRepository.GetAllAsync();
            if(!patients.Any())
            {
                return new APIResponse<List<PatientViewDTO>>(404 , ErrorMessages.NotFound);
            }
            var result = patients.Select(p => new PatientViewDTO
            {
                Id=p.Id,
                FullName=p.FullName,
                Mobile = p.Mobile
            }).ToList();

            return new APIResponse<List<PatientViewDTO>>(200,result);
        }
        catch (Exception ex)
        {
            return new APIResponse<List<PatientViewDTO>>(500, ex.Message);
        }
    }
}
