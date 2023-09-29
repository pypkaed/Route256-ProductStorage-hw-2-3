using FluentValidation;
using Products.GrpcServices;
using Products.Validators;
using ProductsBusiness.Extensions;
using ProductsDao.Extensions;

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
        builder.Services.AddGrpc();

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddScoped<RequestValidator>();
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.MapGrpcService<ProductsGrpcService>();

        app.Run();
    }
}