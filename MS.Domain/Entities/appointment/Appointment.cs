using MS.Domain.Entities.doctors;
using MS.Domain.Entities.patients;
using MS.Domain.Exceptions;

namespace MS.Domain.Entities.appointment;

public class Appointment
{
    public int Id { get; private set; }
    public int PatientId { get; private set; }
    public Patient Patient { get; private set; }
    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly Time { get; private set; }
    public AppointmentStatus Status { get; private set; }
    private void Checking(DateOnly date)
    {
        if (date < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new DateInPastException();
        }
        if (date == DateOnly.FromDateTime(DateTime.Today)
            && Time < TimeOnly.FromDateTime(DateTime.Now))
        {
            throw new TimeInPastException();
        }
    }
    public Appointment(int patientId, int doctorId, DateOnly date, TimeOnly time)
    {
        Checking(date);
        PatientId = patientId;
        DoctorId = doctorId;
        Date = date;
        Time = time;
        Status = AppointmentStatus.Reserved;
    }

    public void Update(DateOnly date, TimeOnly time)
    {
        Checking(date);        
        Date = date;
        Time = time;
        Status = AppointmentStatus.Reserved;
    }
    public void CancelAppointment()
    {
        Status = AppointmentStatus.Cancelled;
    }
}