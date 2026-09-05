using MS.Application.Contracts.Appointment;
using MS.Application.Contracts.Appointment.DTOs;
using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.Schedule;
using MS.Application.Contracts.Schedule.DTOs;
using MS.Domain.Entities.appointment;
using MS.Domain.Entities.patients;
using MS.Domain.Entities.Schedules;

namespace MS.Application.Services;

public class AppointmentServices : IAppointmentServices
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IScheduleRepository _scheduleRepository;

    public AppointmentServices(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository, IScheduleRepository scheduleRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<APIResponse<AppointmentViewDTO>> CreateAsync(AppointmentCreateDTO command)
    {        
        try
        {
            if(command is null)
            {
                return new APIResponse<AppointmentViewDTO>(400, ErrorMessages.ValueMustNotBeNull);
            }
            // check patient is existed
            if(!await _patientRepository.ExistAsync(p => p.Id == command.PatientId))
            {
                return new APIResponse<AppointmentViewDTO>(400, ErrorMessages.PatientNotFound);                
            }
            // check the appointment not reserved for someone else
            if(await _appointmentRepository.ExistAsync(a => a.DoctorId == command.DoctorId &&
            a.Date == command.Date && a.Time == command.Time && a.Status == AppointmentStatus.Reserved))
            {
                return new APIResponse<AppointmentViewDTO>(400, ErrorMessages.AppointmentCanNotBeReserved);                                
            }
            // check the time is in doctor times
            if(!await _scheduleRepository.IsWorkingHoursDefinatedAsync(command.DoctorId , command.Date , command.Time))
            {
                return new APIResponse<AppointmentViewDTO>(400, ErrorMessages.AppointmentCanNotBeReserved);
            }

            //check appointment time is in doctor schedules
            var times = await getDoctorAppointmentForSpecificDateAsync(command.DoctorId, command.Date);
            if(!times.Data.Any(a => a.Time == command.Time))
            {
                return new APIResponse<AppointmentViewDTO>(400, ErrorMessages.AppointmentCanNotBeReserved);
            }

            Appointment appointment = new(command.PatientId, command.DoctorId, command.Date, command.Time);

            await _appointmentRepository.CreateAsync(appointment);
            await _appointmentRepository.SaveChangesAync();

            AppointmentViewDTO result = new()
            {
                id = appointment.Id,
                Status = appointment.Status.ToString(),
                DoctorId = command.DoctorId,
                Date = command.Date,
                Time = command.Time
            };
            return new APIResponse<AppointmentViewDTO>(201, result);
        }
        catch (Exception ex)
        {
            return new APIResponse<AppointmentViewDTO>(500 , ex.Message);
        }
    }
    public async Task<APIResponse<AppointmentViewDTO>> UpdateAsync(AppointmentUpdateDTO command)
    {
        try
        {
            Appointment appointment = await _appointmentRepository.GetByIdAsync(command.Id);
            // check appointment is existed
            if (appointment is null)
            {
                return new APIResponse<AppointmentViewDTO>(404, ErrorMessages.NotFound);
            }
            // check the appointment not reserved for someone else
            if (await _appointmentRepository.ExistAsync(a => a.DoctorId == command.DoctorId &&
            a.Date == command.Date && a.Time == command.Time && a.Status == AppointmentStatus.Reserved))
            {
                return new APIResponse<AppointmentViewDTO>(400, ErrorMessages.AppointmentCanNotBeReserved);
            }
            // check the time is in doctor times
            if (!await _scheduleRepository.IsWorkingHoursDefinatedAsync(command.DoctorId, command.Date, command.Time))
            {
                return new APIResponse<AppointmentViewDTO>(400, ErrorMessages.AppointmentCanNotBeReserved);
            }

            appointment.Update(command.Date,command.Time);
            _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAync();

            AppointmentViewDTO result = new()
            {
                id = appointment.Id,
                Status = appointment.Status.ToString(),
                DoctorId = command.DoctorId,
                Date = command.Date,
                Time = command.Time
            };
            return new APIResponse<AppointmentViewDTO>(200, result);
        }
        catch (Exception ex)
        {
            return new APIResponse<AppointmentViewDTO>(500, ex.Message);
        }
    }

    public async Task<APIResponse<AppointmentViewDTO>> DeleteAsync(int id)
    {        
        try
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if(appointment is null)
            {
                return new APIResponse<AppointmentViewDTO>(404, ErrorMessages.NotFound);
            }
            appointment.CancelAppointment();
            _appointmentRepository.UpdateAsync(appointment);
            await _appointmentRepository.SaveChangesAync();

            AppointmentViewDTO result = new()
            {
                id = appointment.Id,
                Date = appointment.Date,
                Time = appointment.Time,
                DoctorId = appointment.DoctorId,
                Status = appointment.Status.ToString()
            };
            return new APIResponse<AppointmentViewDTO>(200, result);
        }
        catch (Exception ex)
        {
            return new APIResponse<AppointmentViewDTO>(500, ex.Message);
        }
    }

    public async Task<APIResponse<List<AppointmentViewDTO>>> GetAllPatientAppointmentsAsync(int patientId)
    {
        try
        {
            var appointments = await _appointmentRepository.GetAppointmentByPatientIdAsync(patientId);
            if (!appointments.Any())
            {
                return new APIResponse<List<AppointmentViewDTO>>(404, ErrorMessages.NotFound);
            }
            List<AppointmentViewDTO> result = appointments.Select(a => new AppointmentViewDTO
            {
                id = a.Id,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor.FullName,
                Date = a.Date,
                Time = a.Time,
                Status = a.Status.ToString()
            }).ToList();

            return new APIResponse<List<AppointmentViewDTO>>(200, result);
        }
        catch (Exception ex)
        {
            return new APIResponse<List<AppointmentViewDTO>>(500, ex.Message);            
        }
    }
    public async Task<APIResponse<List<AppointmentsTimeDTO>>> getDoctorAppointmentForSpecificDateAsync(int doctorId, DateOnly date)
    {
        try
        {
            var workingHours = await _scheduleRepository.GetDoctorWorkingHoursForSpecificDateAsync(doctorId, date);
            List<AppointmentsTimeDTO> times = new();
            if (workingHours is null)
            {
                return new APIResponse<List<AppointmentsTimeDTO>>(404, ErrorMessages.NotFound);
            }
            var reservedAppointments = await _appointmentRepository.GetReservedAppointments(doctorId, date);
            var currentTime = workingHours.Value.WorkStart;
            while (currentTime < workingHours.Value.WorkEnd)
            {
                var time = new AppointmentsTimeDTO()
                {
                    Time = currentTime,
                    IsReserved = reservedAppointments.Any(a => a.Time == currentTime)
                };
                times.Add(time);
                currentTime = currentTime.AddMinutes(20);
            }
            return new APIResponse<List<AppointmentsTimeDTO>>(200, times);
        }
        catch (Exception ex)
        {
            return new APIResponse<List<AppointmentsTimeDTO>>(500, ex.Message);
        }
    }
}
