using MS.Domain.Entities.doctors;

namespace MS.Infrastructure.EF.Repositories;
public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{
    private readonly MSContext _context;

    public DoctorRepository(MSContext context) : base(context)
    {
        _context = context;
    }
}
