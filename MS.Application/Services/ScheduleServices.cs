using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.Schedule;
using MS.Application.Contracts.Schedule.DTOs;
using MS.Domain.Entities.appointment;
using MS.Domain.Entities.Schedules;

namespace MS.Application.Services;

public class ScheduleServices : IScheduleServices
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public ScheduleServices(IScheduleRepository scheduleRepository, IAppointmentRepository appointmentRepository)
    {
        _scheduleRepository = scheduleRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<APIResponse<ScheduleViewDTO>> CreateAync(ScheduleCreateDTO command)
    {        
        try
        {
            if(command is null)
            {
                return new APIResponse<ScheduleViewDTO>(400, ErrorMessages.ValueMustNotBeNull);
            }
            if(await _scheduleRepository.ExistAsync(s => s.DoctorId == command.DoctorId &&
             s.Date == command.Date))
            {
                return new APIResponse<ScheduleViewDTO>(400, ErrorMessages.ValueCanNotBeDuplicated);                
            }

            Schedule schedule = new(command.DoctorId, command.Date, command.StartTime, command.EndTime);            
            await _scheduleRepository.CreateAsync(schedule);
            await _scheduleRepository.SaveChangesAync();

            ScheduleViewDTO result = new()
            {
                Id = schedule.Id,                
                DoctorId = schedule.DoctorId,                
                Date = schedule.Date,
                EndTime = schedule.EndTime,
                StartTime = schedule.StartTime
            };
            return new APIResponse<ScheduleViewDTO>(201, result);
        }
        catch (Exception ex)
        {
            return new APIResponse<ScheduleViewDTO>(500 , ex.Message);
        }
    }

    public async Task<APIResponse<ScheduleViewDTO>> UpdateAsync(ScheduleUpdateDTO command)
    {        
        try
        {
            var schedule = await _scheduleRepository.GetByIdAsync(command.Id);
            if(schedule is null)
            {
                return new APIResponse<ScheduleViewDTO>(404, ErrorMessages.NotFound);
            }
            if (await _scheduleRepository.ExistAsync(s => s.Date == command.Date 
            && s.DoctorId == command.DoctorId && s.Id != command.Id))
            {
                return new APIResponse<ScheduleViewDTO>(400, ErrorMessages.ValueCanNotBeDuplicated);
            }

            schedule.Update(command.DoctorId,command.Date,command.StartTime,command.EndTime);
            _scheduleRepository.UpdateAsync(schedule);
            await _scheduleRepository.SaveChangesAync();

            ScheduleViewDTO result = new()
            {
                Id = command.Id,
                DoctorId = command.DoctorId,
                Date = command.Date,
                EndTime = command.EndTime,
                StartTime = command.StartTime,
            };
            return new APIResponse<ScheduleViewDTO>(200, result);
        }
        catch (Exception ex)
        {
            return new APIResponse<ScheduleViewDTO>(500, ex.Message);
        }
    }

    public async Task<APIResponse<List<ScheduleViewDTO>>> GetScheduleByDoctorIdAsync(int Id)
    {
        try
        {
            var schedules = await _scheduleRepository.GetScheduleByDoctorIdAsync(Id);
            if(schedules is null)
            {
                return new APIResponse<List<ScheduleViewDTO>>(404,ErrorMessages.NotFound);
            }
            List<ScheduleViewDTO> doctorSchedules = schedules.Select(s => new ScheduleViewDTO
            {
                Id = s.Id,
                DoctorId = s.DoctorId,
                Date = s.Date,
                DoctorName = s.Doctor.FullName,
                StartTime = s.StartTime,
                EndTime = s.EndTime            
            }).ToList();

            return new APIResponse<List<ScheduleViewDTO>>(200, doctorSchedules);
        }
        catch (Exception ex)
        {
            return new APIResponse<List<ScheduleViewDTO>>(500, ex.Message);
        }
    }
}
