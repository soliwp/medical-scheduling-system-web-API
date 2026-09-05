namespace MS.Application.Contracts.Appointment.DTOs;

public class AppointmentViewDTO
{ 
    public int id {  get; set; }
    public string DoctorName { get; set; }
    public int DoctorId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Status { get; set; }
}