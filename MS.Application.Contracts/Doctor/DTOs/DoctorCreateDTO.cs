using System.ComponentModel.DataAnnotations;

namespace MS.Application.Contracts.Doctor.DTOs;

public class DoctorCreateDTO
{
    
    [Length(3,50,ErrorMessage ="باید بین 3 تا 50 کاراکتر باشد")]
    public string FullName { get; set; } = string.Empty;
    
    [Length(3,50,ErrorMessage =" باید بین 3 تا 50 کاراکتر باشد")]
    public string Speciality { get; set; } = string.Empty;
    
    [Length(10 , 10 , ErrorMessage = "باید 10 کاراکتر باشد")]    
    public string MedicalSystemNumber { get; set; } = string.Empty;
}
