using GestaoPessoas.Middlewares;
using GestaoPessoas.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.Development.LocalMachine.json", optional: true, reloadOnChange: true);
string? Implementation = builder.Configuration.GetValue<string>("WorkerService:Implementation");
if (string.IsNullOrEmpty(Implementation))
    throw new InvalidOperationException("WorkerService:Implementation is not configured.");
Implementation = Implementation.ToLower();
builder.Services.AddScoped<ICryptoService, AesCryptoService>();
switch (Implementation)
{
    case "postgres":
        builder.Services.AddScoped<IWorkerService, WorkerServicePostGres>();
        break;
    case "jsonfile":
        builder.Services.AddScoped<IWorkerService, WorkerServiceJsonFile>();
        break;
    default:
        throw new NotSupportedException($"Implementation '{Implementation}' for '{nameof(IWorkerService)}' is not supported.");
}
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
    else
    {
        throw new FileNotFoundException("Ficheiro de comentários XML para documentação swagger não encontrado");
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApplicationInformation();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
