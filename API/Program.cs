using API.Data;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Register controllers
builder.Services.AddCors(); // Enable Cross-Origin Resource Sharing (CORS)
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddEndpointsApiExplorer(); // Enable API endpoints for Swagger
builder.Services.AddSwaggerGen(); // Enable Swagger for API documentation

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>(); // Custom exception middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger UI in development
    app.UseSwaggerUI();
}

// Configure CORS to allow requests from specified origins
app.UseCors(opt =>
{
    opt.AllowAnyHeader()
       .AllowAnyMethod()
       .WithOrigins("http://localhost:3000");
});

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enable routing
app.UseRouting();

// Map controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Maps controller routes
});

// Apply migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        context.Database.Migrate(); // Apply pending migrations
        DbInitializer.Initialize(context); // Seed the database
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during migration or database initialization");
    }
}

// Start the application
app.Run();
