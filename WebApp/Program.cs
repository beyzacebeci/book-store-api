using Microsoft.EntityFrameworkCore;
using WebApp.Repositories;
using Microsoft.AspNetCore.Cors;

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.WithOrigins("http://localhost:3000")
//                          .AllowAnyMethod()
//                          .AllowAnyHeader()
//                          .AllowCredentials();
//                      });
//});




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RepositoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

//react conneciton


//-----------------------

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
