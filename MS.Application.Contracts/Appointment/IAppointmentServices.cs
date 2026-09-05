using MS.Application.Contracts.Appointment.DTOs;
using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.Schedule.DTOs;

namespace MS.Application.Contracts.Appointment;
public interface IAppointmentServices
{
    Task<APIResponse<AppointmentViewDTO>> CreateAsync (AppointmentCreateDTO command);
    Task<APIResponse<AppointmentViewDTO>> UpdateAsync (AppointmentUpdateDTO command);
    Task<APIResponse<AppointmentViewDTO>> DeleteAsync (int id);
    Task<APIResponse<List<AppointmentViewDTO>>> GetAllPatientAppointmentsAsync(int patientId);
    Task<APIResponse<List<AppointmentsTimeDTO>>> getDoctorAppointmentForSpecificDateAsync(int doctorId, DateOnly date);
}
