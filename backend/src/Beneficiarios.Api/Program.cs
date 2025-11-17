using Microsoft.EntityFrameworkCore;
using Beneficiarios.Api.Infrastructure.Db;
using Beneficiarios.Api.Infrastructure.Interfaces;
using Beneficiarios.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
