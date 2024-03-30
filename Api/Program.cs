using Api;
using Application;
using Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseExceptionHandler(options => { });

app.Run();

public partial class Program { }
