using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

var logLevel = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "Information";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authorization", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Server.Kestrel", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Hosting.HealthChecks", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Hosting.Diagnostics", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Extensions.Hosting.Diagnostics", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Extensions.Diagnostics.HealthChecks", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Extensions.DependencyInjection", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Extensions.Http", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Extensions.Logging", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Extensions.Options", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStore API", Description = "Making the Pizzas you love", Version = "v1" });
     c.EnableAnnotations();
});

    
var app = builder.Build();

app.UseSerilogRequestLogging();
    
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNet Testing API V1");
});
    
app.MapGet("/", () => "Hello World!");
app.MapGet("/todo", () => new Todo { 
    Item = "Water plants", 
    Complete = false 
})
.WithMetadata(new SwaggerOperationAttribute(summary: "Summary", description: "Description Test"));

app.MapGet("/parse", async (HttpContext context, string dateInput) =>
{
    if (DateTime.TryParse(dateInput, out DateTime parsedDate))
    {
        Log.Information("Date input: {dateInput}", dateInput);
        await Task.Delay(1000);
        var iso8601 = parsedDate.ToString("o");  // ISO 8601
        var rfc3339 = parsedDate.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");  // RFC 3339

        using (var client = new HttpClient())
        {
            var apiResponse = Results.Json(new
            {
                iso8601 = iso8601,
                rfc3339 = rfc3339
            });

            var responsePretty = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions { WriteIndented = true });
            Log.Information("Response: {responsePretty}", responsePretty);
            Thread.Sleep(1000);
            return apiResponse;
        }
    }
    return Results.BadRequest(new
    {
        Error = "Invalid date string"
    });
});

    
app.Run();