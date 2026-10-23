using Apex.Portfolio.Features;
using Apex.Portfolio.Persistance;
using Apex.Shared.Core.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSharedControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSharedDependencies();
builder.Services.AddPersistenceDependencies(builder.Configuration);
builder.Services.AddFeaturesDependencies(builder.Configuration);
builder.Services.AddSharedCors();
builder.Services.AddSharedSwagger();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseSharedCors();
app.UseSharedSwagger();

app.Run();

