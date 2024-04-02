using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestApiLK.data;
using RestApiLK.services; // Importer dine serviceklasser her

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Fjern standard konvertering af egenskabsnavne (camelCase)
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestApiLK", Version = "v1" });
});

builder.Services.AddDbContext<LakridsKompanigetDbContext>(opt =>
{
    opt.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
});

// Add your services to the container here
builder.Services.AddScoped<kundeService, kundeService>(); // Example assuming you have a service called KundeService
builder.Services.AddScoped<OrdreService, OrdreService>();
builder.Services.AddScoped<BetalingService, BetalingService>();
builder.Services.AddScoped<ForhandlerService, ForhandlerService>();
builder.Services.AddScoped<produkter, produkter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestApiLK v1");
    });
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowLocalhost4200");

app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
