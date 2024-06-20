using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Persistence;
using RoomBookingApp.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRoomBookingRequestProcessor, RoomBookingRequestProcessor>();
builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();  
var connString = "Filename=:memory:";
var conn = new SqliteConnection(connString);
conn.Open();
builder.Services.AddDbContext<RoomBookingAppDbContext>(options => options.UseSqlite(conn));
var dbBuilder = new DbContextOptionsBuilder<RoomBookingAppDbContext>();
dbBuilder.UseSqlite(conn);
var context = new RoomBookingAppDbContext(dbBuilder.Options);
context.Database.EnsureCreated();

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
