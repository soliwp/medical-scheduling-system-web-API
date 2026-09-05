namespace MS.Application.Contracts.Schedule.DTOs;

public class AppointmentsTimeDTO
{
    public TimeOnly Time { get; set; }
    public bool IsReserved { get; set; }
}