using Microsoft.EntityFrameworkCore;
using MS.Domain.Entities.appointment;

namespace MS.Infrastructure.EF.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
    private readonly MSContext _context;

    public AppointmentRepository(MSContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Appointment>> GetAppointmentByPatientIdAsync(int patientId)
    {
        return await _context.appointments.Include(x => x.Doctor)
            .Where(a => a.PatientId == patientId && a.Date >= DateOnly.FromDateTime(DateTime.Today))
            .OrderBy(a => a.Date).AsNoTracking().ToListAsync();
    }
    public async Task<List<Appointment>> GetReservedAppointments(int doctorId , DateOnly date)
    {
        return await _context.appointments.Where(a => a.DoctorId == doctorId && a.Date == date 
        && a.Status == AppointmentStatus.Reserved).ToListAsync();
    }
}
