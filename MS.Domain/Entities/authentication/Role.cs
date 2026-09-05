namespace MS.Domain.Entities.authentication;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
