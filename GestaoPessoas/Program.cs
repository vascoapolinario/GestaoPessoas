using GestaoPessoas.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.Development.LocalMachine.json", optional: true, reloadOnChange: true);
string? Implementation = builder.Configuration.GetValue<string>("WorkerService:Implementation");
if (string.IsNullOrEmpty(Implementation))
    throw new InvalidOperationException("WorkerService:Implementation is not configured.");
Implementation = Implementation.ToLower();
switch (Implementation)
{
    case "postgres":
        builder.Services.AddScoped<IWorkerService, WorkerServicePostGres>();
        break;
    case "jsonfile":
        builder.Services.AddScoped<IWorkerService, WorkerServiceJsonFile>();
        break;
    default:
        throw new NotSupportedException($"Implementation '{Implementation}' is not supported.");
}
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
