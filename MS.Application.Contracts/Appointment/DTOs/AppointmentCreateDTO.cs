using System.ComponentModel;

namespace MS.Application.Contracts.Appointment.DTOs;

public class AppointmentCreateDTO
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }    
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
}
