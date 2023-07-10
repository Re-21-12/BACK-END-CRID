using CustomersApi.Repositories;
using Microsoft.EntityFrameworkCore;
using CustomersApi.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.

builder.Services.AddControllers();

// Agregar soporte para Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar el enrutamiento para URLs en minúsculas.
builder.Services.AddRouting(routing => routing.LowercaseUrls = true);

// Agregar acceso a la base de datos.
//este es como un context provider que proporciona los metodos al resto de la app
builder.Services.AddDbContext<CustomerDataBaseContext>(options =>
{
    // Configurar el proveedor de base de datos MySQL utilizando la cadena de conexión obtenida de la configuración.
    options.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection"));
});

// Agregar servicios de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//agregar caso de uso con inyeccion de dependencias
builder.Services.AddScoped<IUpdateCustomerUseCase, UpdateCustomerUseCase>();

var app = builder.Build();

// Configurar el pipeline de procesamiento de solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger y su interfaz de usuario (UI) solo en entorno de desarrollo.
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Agregar middleware de CORS
app.UseCors();

// Redireccionar las solicitudes HTTP a HTTPS.
app.UseHttpsRedirection();

// Agregar middleware de autorización.
app.UseAuthorization();

// Mapear los controladores a las rutas de URL correspondientes.
app.MapControllers();

// Iniciar la aplicación y comenzar a escuchar las solicitudes entrantes.
app.Run();
