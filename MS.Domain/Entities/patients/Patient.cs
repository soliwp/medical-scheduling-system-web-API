using MS.Domain.Entities.appointment;

namespace MS.Domain.Entities.patients;
public class Patient
{
    public int Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Mobile { get; private set; } = string.Empty;
    public List<Appointment> Appointments { get; private set; }
    public Patient(string fullName , string mobile)
    {
        FullName = fullName;
        Mobile = mobile;
        Appointments = new();
    }
    public void Update(string fullName , string mobile)
    {
        FullName = fullName;
        Mobile = mobile;        
    }
}