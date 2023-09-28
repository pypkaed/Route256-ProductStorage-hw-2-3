using FluentValidation;
using ProductsBusiness.Extensions;
using ProductsBusiness.Services;
using ProductsDao.Extensions;
using ProductsDao.Repositories;
using ProductsDao.Validators.Entities;

namespace Products;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDao();
        builder.Services.AddBusiness();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddAutoMapper(typeof(Program));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}