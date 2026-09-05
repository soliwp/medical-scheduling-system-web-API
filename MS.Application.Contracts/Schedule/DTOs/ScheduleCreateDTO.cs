namespace MS.Application.Contracts.Schedule.DTOs;

public class ScheduleCreateDTO
{    
    public int DoctorId { get; set; }    
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
