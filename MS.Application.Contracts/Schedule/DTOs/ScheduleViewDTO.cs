namespace MS.Application.Contracts.Schedule.DTOs;

public class ScheduleViewDTO : ScheduleCreateDTO
{
    public int Id { get; set; }
    public string? DoctorName { get; set; }
}
