using MS.Domain.Entities.patients;

namespace MS.Infrastructure.EF.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    private readonly MSContext _context;

    public PatientRepository(MSContext context) : base(context)
    {
        _context = context;
    }
}
