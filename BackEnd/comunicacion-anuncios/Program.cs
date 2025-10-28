using comunicacion_anuncios.Application.Common;
using comunicacion_anuncios.Application.DTOs;
using comunicacion_anuncios.Config;
using comunicacion_anuncios.Infrastructure.Config;
using comunicacion_anuncios.Middleware;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(PagedRequest).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(AuthLoginRequest).Assembly);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Default", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .DisallowCredentials());
});

builder.Services.AddJwtAuth(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Default");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
