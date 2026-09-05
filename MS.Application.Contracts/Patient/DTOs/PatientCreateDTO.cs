using System.ComponentModel.DataAnnotations;

namespace MS.Application.Contracts.Patient.DTOs;

public class PatientCreateDTO
{
    [Length(3, 50, ErrorMessage = "باید بین 3 تا 50 کاراکتر باشد")]
    public string FullName { get; set; } = string.Empty;
    [RegularExpression(@"^09\d{9}$" , ErrorMessage = "شماره نامعتبر است")]
    public string Mobile { get; set; } = string.Empty;
}
