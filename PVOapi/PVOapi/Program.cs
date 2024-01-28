using PVOapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IPvoService, PvoService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowAnyOrigin"); // Corrected to match the policy name

// Other middleware configurations
// app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapControllers();

// Specify the single URL the application should listen on
var url = "http://0.0.0.0:5196";

// Start the application
app.Run(url);
