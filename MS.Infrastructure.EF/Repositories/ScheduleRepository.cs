using Microsoft.EntityFrameworkCore;
using MS.Domain.Entities.Schedules;

namespace MS.Infrastructure.EF.Repositories;

public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
{
    private readonly MSContext _context;

    public ScheduleRepository(MSContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(TimeOnly WorkStart, TimeOnly WorkEnd)?> GetDoctorWorkingHoursForSpecificDateAsync(int doctorId, DateOnly date)
    {
        var schedule = await _context.schedules
            .FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.Date == date);
        if(schedule is null)
        {
            return null;
        }
        return (schedule.StartTime, schedule.EndTime);
    }

    public async Task<List<Schedule>> GetScheduleByDoctorIdAsync(int doctorId)
    {
        return await _context.schedules.Include(s => s.Doctor)
            .Where(s => s.DoctorId == doctorId && s.Date >= DateOnly.FromDateTime(DateTime.Today))
            .OrderBy(s => s.Date).AsNoTracking().ToListAsync();
    }

    public async Task<bool> IsWorkingHoursDefinatedAsync(int doctorId, DateOnly date, TimeOnly time)
    {
        return await _context.schedules.AnyAsync(s => s.DoctorId == doctorId && s.Date == date &&
        s.StartTime <= time && s.EndTime > time);
    }
}
