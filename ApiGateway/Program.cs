using ApiGateway.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var authSection = builder.Configuration.GetSection(AuthConfig.Position);
        var authConfig = authSection.Get<AuthConfig>();

        // load config fo ocelot
        builder.Configuration.AddJsonFile("configuration.json", false, true)
            .AddEnvironmentVariables();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // ocelot
        builder.Services.AddOcelot();
        builder.Services.AddSwaggerForOcelot(builder.Configuration);

        // auth
        builder.Services.AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authConfig.Issuer,
                    ValidAudience = authConfig.Audience,
                    IssuerSigningKey = authConfig.SymmetricSecurityKey(),
                };
            });

        var app = builder.Build();

        // swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerForOcelotUI(options => options.PathToSwaggerGenerator = "/swagger/docs");
        }

        //app.UseHttpsRedirection();
        app.UseOcelot().Wait();

        app.Run();
    }
}