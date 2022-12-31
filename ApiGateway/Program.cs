using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddJsonFile("configuration.json", false, true)
            .AddEnvironmentVariables();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOcelot();
        builder.Services.AddSwaggerForOcelot(builder.Configuration);

        var app = builder.Build();

        //Configure the HTTP request pipeline.
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