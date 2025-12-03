using CourtReservation_Core.CustomEntities;
using CourtReservation_Core.Interfaces;
using CourtReservation_Core.Interfaces.Services_Interfaces;
using CourtReservation_Core.Services;
using CourtReservation_Infraestructure.Context;
using CourtReservation_Infraestructure.Filters;
using CourtReservation_Infraestructure.Repositories;
using CourtReservation_Infraestructure.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace CourtReservation_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.Sources.Clear();
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
                    optional: true, reloadOnChange: true);
                

            // Configurar configuración para diferentes entornos
            if (builder.Environment.IsDevelopment())
            {
                // En desarrollo: cargar User Secrets
                builder.Configuration.AddUserSecrets<Program>();
                Console.WriteLine("User Secrets habilitados para desarrollo");
            }
            // En producción, los secrets vendrán de Variables de Entorno o Azure Key Vault
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddApiVersioning(options =>
            {
                // Reporta las versiones soportadas y obsoletas en encabezados de respuesta
                options.ReportApiVersions = true;

                // Versión por defecto si no se especifica
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

                // Soporta versionado mediante URL, Header o QueryString
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),       // Ejemplo: /api/v1/...
                    new HeaderApiVersionReader("x-api-version"), // Ejemplo: Header ? x-api-version: 1.0
                    new QueryStringApiVersionReader("api-version") // Ejemplo: ?api-version=1.0
                );
            });

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSqlServer")));


            builder.Services.AddAutoMapper(typeof(CourtReservation_Infraestructure.Mappings.MappingProfile));


            builder.Services.AddScoped<IReservaService, ReservaService>();
            builder.Services.AddTransient<ISecurityService, SecurityService>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
          //  builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
      
       
            
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


            builder.Services.Configure<PasswordOptions>
                (builder.Configuration.GetSection("PasswordOptions"));

            builder.Services.AddSingleton<IPasswordService, PasswordService>();

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

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(
                                builder.Configuration["Authentication:SecretKey"]
                            )
                        )
                    };
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

            app.UseAuthentication();
            app.UseAuthorization();

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

