using Facturas_simplified.Database;
using Facturas_simplified.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Invoices.xml"));
});
builder.Services.AddProblemDetails();
builder.Services.AddControllers(options =>
    {
      options.Filters.Add<FluentValidationFilter>();
    });
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(options =>
    {
      options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
      // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });

// builder.Services.Configure<ApiBehaviorOptions>(options =>
// {
//   options.InvalidModelStateResponseFactory = context =>
//   {
//     var problemDetails = new ValidationProblemDetails(context.ModelState);
//     return new BadRequestObjectResult(problemDetails)
//     {
//       ContentTypes = { "application/problem+json" }
//     };
//   };
// });

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowFrontend", policy =>
  {
    policy.WithOrigins("http://localhost:9090") // Cambia si usas otro puerto
            .AllowAnyHeader()
            .AllowAnyMethod();
  });
});

builder.Services.AddAutoMapper(typeof(Program));
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  // SeedData.Seed(services);
  var context = services.GetRequiredService<AppDbContext>();

  // Asegúrate de que la base de datos y tablas existen
  context.Database.EnsureCreated(); // <-- ¡Esta línea es clave!

  // Ahora ejecuta el seeding
  SeedData.Seed(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }
