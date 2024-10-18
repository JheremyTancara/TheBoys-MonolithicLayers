using Api.Data;
using Api.DTOs;
using Api.Models;
using Api.Models.Interface;
using Api.Repositories.Interface;
using Api.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Streaming Project", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new LowercaseControllerModelConvention());
});

var connectionString = builder.Configuration.GetConnectionString("MySQLConnection");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25))));

builder.Services.AddScoped<IRepository<User, UserDTO>, UserRepository>();
builder.Services.AddScoped<IRepository<IMovie, MovieDTO>, MovieRepository>();
builder.Services.AddScoped<IRepository<Actor, ActorDTO>, ActorRepository>();
//builder.Services.AddScoped<IRepository<Actor, ActorDTO>, JsonActorRepository>(provider => 
//    new JsonActorRepository("actors.json"));
builder.Services.AddScoped<IRepository<Director, DirectorDTO>, DirectorRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.SeedData(); 
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();

public class LowercaseControllerModelConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        controller.ControllerName = controller.ControllerName.ToLower();

        foreach (var selectorModel in controller.Selectors)
        {
            var attributeRouteModel = selectorModel.AttributeRouteModel;

            if (attributeRouteModel != null)
            {
                attributeRouteModel.Template = attributeRouteModel.Template.ToLower();
            }
        }
    }
}