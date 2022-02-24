using Kontaktnik.DATA;
using Kontaktnik.DATA.FileManager;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<KontaktnikDbContext>(o => o.UseSqlServer("name=ConnectionStrings:KontaktnikConnection"));
builder.Services.AddControllers();

builder.Services.AddScoped<ICustomerContactRepo, CustomerContactRepo>();
builder.Services.AddScoped<IContactsRepo, ContactsRepo>();
builder.Services.AddTransient<IFileManager,FileManager>();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
