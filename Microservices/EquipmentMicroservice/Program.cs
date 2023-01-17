using EquipmentMicroservice.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EquipmentMicroservice;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (connectionString == null)
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

        builder.Services.AddSwaggerGen(SetupSwaggerAction);
        builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

        var app = builder.Build();

        // update database
        using (var scope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            if (scope != null)
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                db.Database.Migrate();
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void SetupSwaggerAction(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Description = "Enter the Bearer Authorization string",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                        Scheme = "oauth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
    }
}