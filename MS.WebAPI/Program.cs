using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MS.Domain.Entities.authentication;
using MS.Infrastructure.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("connection");
// Add services to the container.
builder.Services.Wireup(connectionString);

builder.Services.AddControllers();

// JWT configurations
var issuer = builder.Configuration.GetValue<string>("ValidIssuer");
var audience = builder.Configuration.GetValue<string>("Audience");
var secretKey = builder.Configuration.GetValue<string>("secretKey");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,

        ValidIssuer = issuer,
        ValidAudience = audience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("adminOnly", policy =>
    {
        policy.RequireRole(SystemRoles.admin.ToString());
    });
    options.AddPolicy("adminAndPatient", policy =>
    {
        policy.RequireRole(SystemRoles.admin.ToString(), SystemRoles.patient.ToString());
    });
});


// swagger configurations
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.SwaggerDoc("medicalSchedulingSystemOpenAPISpecification", new()
    {
        Title = "medical schedulin system",
        Version = "v1",
        Description = "add doctor , add patient , add doctor schedule or work time , add appointment for patient",
        Contact = new()
        {
            Email = "m.soleymani.1939@gmail.com",
            Name = "mohammad hosein soleymani",
            Url = new Uri("http://t.me/soliwp")
        }
    });
    var XmlDocumentaionFile = "MS.WebAPI.xml";
    var XMLDocumentationFilePath = Path.Combine(AppContext.BaseDirectory, XmlDocumentaionFile);
    setupAction.IncludeXmlComments(XMLDocumentationFilePath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(setupAction =>
{
    setupAction.SwaggerEndpoint("/swagger/medicalSchedulingSystemOpenAPISpecification/Swagger.json", "medical scheduling system Web API");
    setupAction.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
