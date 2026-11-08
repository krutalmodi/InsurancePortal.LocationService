using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using InsurancePortal.LocationService.Application.Abstractions;
using InsurancePortal.LocationService.Application.Commands;
using InsurancePortal.LocationService.Application.DTOs.Responses;
using InsurancePortal.LocationService.Application.Queries;
using InsurancePortal.LocationService.Application.Repositories;
using InsurancePortal.LocationService.Infrastructure.Data;
using InsurancePortal.LocationService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LocationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ICommandHandler<CreateLocationCommand, LocationResponse>, CreateLocationCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateLocationCommand, LocationResponse?>, UpdateLocationCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteLocationCommand, bool>, DeleteLocationCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetLocationByIdQuery, LocationResponse?>, GetLocationByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllLocationsQuery, IEnumerable<LocationResponse>>, GetAllLocationsQueryHandler>();

var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Secret"] ?? string.Empty))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

#region Database Initialization

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<LocationDbContext>().Database.Migrate();
}

#endregion

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
