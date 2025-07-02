using DedicadoEstudo.Data.AutoMapper;
using DedicadoEstudo.Data.Infraestrutura;
using DedicadoEstudo.Data.Repository;
using DedicadoEstudo.Data.Repository.Interface;
using DedicadoEstudo.Service.CRiptografia;
using DedicadoEstudo.Service.Service;
using DedicadoEstudo.Service.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PasswordHasher>();

builder.Services.AddDbContext<DedicadoEstudoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var chaveJwt = builder.Configuration["Jwt:ChaveSecreta"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,  // Ajuste conforme seu cenário
        ValidateAudience = false, // Ajuste conforme seu cenário
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


builder.Services.AddScoped<IUsuarioRepsitory, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddAutoMapper(typeof(UsuarioProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("PermitirAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
