using MS.Domain.Common;

namespace MS.Domain.Entities.appointment;
public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    Task<List<Appointment>> GetAppointmentByPatientIdAsync(int patientId);
    Task<List<Appointment>> GetReservedAppointments(int doctorId, DateOnly date);
}
