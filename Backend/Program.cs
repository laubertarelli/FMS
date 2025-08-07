using Backend.Data;
using Backend.Services;
using dotenv.net;
using System.Text.Json.Serialization;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
    .AddDependencies()
    .AddIdentity()
    .AddSwagger()
    .AddMyCors()
    .AddJwtAuthentication(builder.Configuration)
    .AddControllers()
    .AddNewtonsoftJson()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FmsContext>();
    context.EnsureDatabaseMigrated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
