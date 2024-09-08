using CityInfo.API;
using CityInfo.API.DbContext;
using CityInfo.API.Repositories;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityInfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var str = builder.Configuration["CityInfoConnectionString"];

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(
    options => options.ReturnHttpNotAcceptable = true
    )
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddScoped<IMailService,LocalMailService>();
builder.Services.AddSingleton<CitiesDataStore>();
builder.Services.AddDbContext<CityInfoDbContext>(optionsAction =>
      optionsAction.UseSqlServer(str)
    );
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(option =>
    option.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
             Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]

             ))
    }

    ); 

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

});

app.Run();
