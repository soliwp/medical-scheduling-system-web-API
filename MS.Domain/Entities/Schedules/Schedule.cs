    using MS.Domain.Entities.doctors;
using MS.Domain.Entities.patients;
using MS.Domain.Exceptions;
using System.Security.Cryptography.X509Certificates;

namespace MS.Domain.Entities.Schedules;
public class Schedule
{
    public int Id { get; private set; }
    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    private void Chacking(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        if (endTime <= startTime)
        {
            throw new DateException();
        }
        if (date < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new DateInPastException();
        }
        if (date == DateOnly.FromDateTime(DateTime.Today)
            && startTime < TimeOnly.FromDateTime(DateTime.Now))
        {
            throw new TimeInPastException();
        }
    }
    public Schedule(int doctorId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        Chacking(date, startTime, endTime);

        DoctorId = doctorId;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;        
    }
    public void Update(int doctorId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        Chacking(date, startTime, endTime);

        DoctorId = doctorId;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }
}
