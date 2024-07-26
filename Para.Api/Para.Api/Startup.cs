using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Para.Business;
using Para.Business.Cqrs;
using Para.Business.Validations;
using Para.Data.Context;
using Para.Data.Domain;
using Para.Data.UnitOfWork;

namespace Para.Api;

public class Startup
{
    public IConfiguration Configuration;
    
    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }
    
    
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Para.Api", Version = "v1" });
        });

        var connectionStringSql = Configuration.GetConnectionString("MsSqlConnection");
        services.AddDbContext<ParaDbContext>(options => options.UseSqlServer(connectionStringSql));
        //services.AddDbContext<ParaDbContext>(options => options.UseNpgsql(connectionStringPostgre));
        
        //FluentValidation
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerValidator>());
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerAddressValidator>());
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerDetailValidator>());
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerPhoneValidator>());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MapperConfig());
        });
        services.AddSingleton(config.CreateMapper());

        services.AddMediatR(typeof(CreateCustomerCommand).GetTypeInfo().Assembly);
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Para.Api v1"));
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}