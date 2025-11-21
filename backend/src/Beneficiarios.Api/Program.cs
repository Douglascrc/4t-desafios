using Microsoft.EntityFrameworkCore;
using Beneficiarios.Api.Infrastructure.Db;
using Beneficiarios.Api.Infrastructure.Interfaces;
using Beneficiarios.Api.Infrastructure.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSwaggerGen(c => 
{
    c.EnableAnnotations();
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IPlano, PlanoRepository>();
builder.Services.AddTransient<IBeneficiario, BeneficiarioRepository>();
    
var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddSingleton(conn);
builder.Services.AddDbContext<ApplicationDbContext>(opts => 
    opts.UseNpgsql(conn));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

DatabaseInitializer.Initialize(app.Services);

app.Run();
