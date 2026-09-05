using MS.Domain.Entities.appointment;
using MS.Domain.Entities.Schedules;

namespace MS.Domain.Entities.doctors;
public class Doctor
{
    public int Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Speciality { get; private set; } = string.Empty;
    public string MedicalSystemNumber { get; private set; } = string.Empty;
    public List<Schedule> schedules { get; private set; }
    public List<Appointment> appointments { get; private set; }
    public Doctor(string fullName, string speciality, string medicalSystemNumber)
    {        
        FullName = fullName;
        Speciality = speciality;
        MedicalSystemNumber = medicalSystemNumber;
        schedules = new();
        appointments = new();
    }
    public void update(string fullName, string speciality, string medicalSystemNumber)
    {

        FullName = fullName;
        Speciality = speciality;
        MedicalSystemNumber = medicalSystemNumber;
    }
}