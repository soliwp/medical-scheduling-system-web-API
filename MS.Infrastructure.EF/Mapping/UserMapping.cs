using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MS.Domain.Entities.authentication;

namespace MS.Infrastructure.EF.Mapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User(1, "bw@gmail.com" , "1939"),            
            new User(2, "soliwp@gmail.com", "1380")            
            );
    }
}
