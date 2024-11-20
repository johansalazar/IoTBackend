using Asp.Versioning.ApiExplorer;
using IoTBackend.Aplication.Interfaces;
using IoTBackend.Aplication.Services;
using IoTBackend.Aplication.UseCases;
using IoTBackend.Infraestructure.Infraestructura.IOT;
using IoTBackend.Infraestructure.Infraestructura.User;
using IoTBackend.Infraestructure.Persistence.Contexts;
using IoTBackend.Infraestructure.Persistence.Migrations;
using IoTBackend.Services.WebAppi.Modules.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Devices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "IoTBackend API", Version = "v1" });

    // Configurar esquema de seguridad JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT en el formato: Bearer {token}"
    });

    // Requerir el esquema de seguridad en todas las operaciones
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

builder.Services.AddVersioning();
builder.Services.AddApplicationServices();

builder.Services.AddScoped<IIoTDeviceRepository, IoTDeviceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Configuración de otras dependencias

builder.Services.AddSingleton(sp =>
{
    var iotHubConnectionString = builder.Configuration.GetConnectionString("IoTHub");
    return RegistryManager.CreateFromConnectionString(iotHubConnectionString);
});

builder.Services.AddDbContext<IoTDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Registrar MigrationService
//builder.Services.AddTransient<MigrationService>();



// Configuración de JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });


// Agregar los casos de uso
builder.Services.AddScoped<UserUseCases>();
builder.Services.AddScoped<AuthService>();


var app = builder.Build();

// Asegurarse de que la base de datos esté creada y migrada
// Ejecutar las migraciones automáticamente en el inicio de la aplicación
//var migrationService = app.Services.GetRequiredService<MigrationService>();
//migrationService.MigrateDatabaseAsync().GetAwaiter().GetResult();

EnsureDatabaseCreatedAndMigrate(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // build a swagger endpoint for each discovered API version

        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}
else
{
    app.UseDeveloperExceptionPage();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // build a swagger endpoint for each discovered API version

        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Método para asegurar que la base de datos esté creada y migrada
void EnsureDatabaseCreatedAndMigrate(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<IoTDbContext>();

        // Verificar si la base de datos existe
        if (!context.Database.CanConnect())
        {
            try
            {
                // Si no existe, aplicar migraciones
                context.Database.Migrate();
                Console.WriteLine("Base de datos migrada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al migrar la base de datos: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("La base de datos ya existe y está conectada.");
        }
    }
}