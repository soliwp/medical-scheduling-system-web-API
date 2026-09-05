using MS.Domain.Common;

namespace MS.Domain.Entities.Schedules;

public interface IScheduleRepository : IGenericRepository<Schedule>
{
    Task<List<Schedule>> GetScheduleByDoctorIdAsync(int doctorId);
    Task<(TimeOnly WorkStart , TimeOnly WorkEnd)?> GetDoctorWorkingHoursForSpecificDateAsync(int doctorId , DateOnly date);
    Task<bool> IsWorkingHoursDefinatedAsync(int doctorId , DateOnly date , TimeOnly time);
}