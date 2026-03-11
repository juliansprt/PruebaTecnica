var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Endpoints de la API: registrar controladores o minimal APIs según las reglas del proyecto.
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
