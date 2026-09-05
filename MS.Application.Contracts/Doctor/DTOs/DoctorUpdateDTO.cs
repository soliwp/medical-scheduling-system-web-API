using System.ComponentModel.DataAnnotations;

namespace MS.Application.Contracts.Doctor.DTOs;

public class DoctorUpdateDTO : DoctorCreateDTO
{    
    public int Id { get; set; }
}
