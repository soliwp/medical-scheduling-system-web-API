using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Domain.Entities.authentication;

namespace MS.Infrastructure.EF.Mapping;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role
            {
                RoleId = Convert.ToInt32(SystemRoles.admin),
                RoleName = "admin",
                UserId = 1                
            }            ,
            new Role
            {
                RoleId = Convert.ToInt32(SystemRoles.patient),
                RoleName = "patient",
                UserId = 2
            }            
            );
    }
}
