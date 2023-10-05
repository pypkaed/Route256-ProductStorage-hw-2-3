using FluentValidation;
using Products.GrpcServices;
using Products.Interceptors;
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
        builder.Services.AddGrpc(options => {
            options.Interceptors.Add<ErrorInterceptor>();
            options.Interceptors.Add<LogInterceptor>();
            options.Interceptors.Add<ValidationInterceptor>();
        })
            .AddJsonTranscoding();

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
        app.MapGrpcService<ProductsGrpcService>();

        app.Run();
    }
}