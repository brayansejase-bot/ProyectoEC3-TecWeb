using CourtReservation_Core.Interfaces;
using CourtReservation_Core.Interfaces.Services_Interfaces;
using CourtReservation_Core.Services;
using CourtReservation_Infraestructure.Context;
using CourtReservation_Infraestructure.Filters;
using CourtReservation_Infraestructure.Repositories;
using CourtReservation_Infraestructure.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace CourtReservation_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSqlServer")));


            //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddScoped<IReservaService, ReservaService>();

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
          //  builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
      
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddAutoMapper(typeof(CourtReservation_Infraestructure.Mappings.MappingProfile));
            // Registrar tu ValidationService
            builder.Services.AddScoped<IValidationService, ValidationService>();

            // Registrar todos los validadores del ensamblado
            builder.Services.AddValidatorsFromAssemblyContaining<GetByIdRequestValidator>();
            // Registrar todos los validadores del ensamblado automáticamente
            builder.Services.AddValidatorsFromAssemblyContaining<ReservaDtoValidator>();

            builder.Services.AddOpenApi();

            // Registrar IDbConnectionFactory, UnitOfWork, DapperContext y repos
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<IDapperContext, DapperContext>();

            // Configurar Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Backend Social Media API",
                    Version = "v1",
                    Description = "Documentación de la API de Social Media - .NET 9",
                    Contact = new()
                    {
                        Name = "Equipo de Desarrollo UCB",
                        Email = "desarrollo@ucb.edu.bo"
                    }
                });
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                // Configurar para que muestre los parámetros de objetos complejos
                options.EnableAnnotations();

            });



            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Usar Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Social Media API v1");
                    options.RoutePrefix = string.Empty; // Swagger será accesible en la raíz
                });

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

    }
}

