using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MS.Application.Contracts.Appointment;
using MS.Application.Contracts.authentication;
using MS.Application.Contracts.Doctor;
using MS.Application.Contracts.Patient;
using MS.Application.Contracts.Schedule;
using MS.Application.Contracts.User;
using MS.Application.Services;
using MS.Domain.Common;
using MS.Domain.Entities.appointment;
using MS.Domain.Entities.authentication;
using MS.Domain.Entities.doctors;
using MS.Domain.Entities.patients;
using MS.Domain.Entities.Schedules;
using MS.Infrastructure.EF;
using MS.Infrastructure.EF.Repositories;

namespace MS.Infrastructure.Configuration;
public static class Bootstrapper
{
    public static IServiceCollection Wireup(this IServiceCollection services , string connentionString)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IUserRepository , UserRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        services.AddScoped<IDoctorServices , DoctorServices>();
        services.AddScoped<IPatientServices, PatientServices>();
        services.AddScoped<IUserServices , UserServices>();
        services.AddScoped<IScheduleServices, ScheduleServices>();
        services.AddScoped<IAppointmentServices, AppointmentServices>();
        services.AddScoped<ITokenService , TokenService>();

        services.AddDbContext<MSContext>(options => options.UseSqlServer(connentionString));

        return services;
    }
}
