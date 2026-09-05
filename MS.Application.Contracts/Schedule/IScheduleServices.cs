using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.Schedule.DTOs;

namespace MS.Application.Contracts.Schedule;
public interface IScheduleServices
{
    Task<APIResponse<ScheduleViewDTO>> CreateAync(ScheduleCreateDTO command);
    Task<APIResponse<ScheduleViewDTO>> UpdateAsync(ScheduleUpdateDTO command);
    Task<APIResponse<List<ScheduleViewDTO>>> GetScheduleByDoctorIdAsync(int Id);
}
