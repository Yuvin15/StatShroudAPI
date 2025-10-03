var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var pathToJson = Path.Combine(AppContext.BaseDirectory, "leaguestats-9a390-firebase-adminsdk-fbsvc-761d832650.json");
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToJson);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Redirect root URL to Swagger
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

// Use CORS
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
