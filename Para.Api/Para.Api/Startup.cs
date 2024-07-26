using System.Reflection;
using System.Text.Json.Serialization;
using Autofac;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using MediatR;
using Para.Api.Middlewares;
using Para.Business.Autofac;
using Para.Business.Cqrs;
using Para.Business.Validations;

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
        
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerValidator>());
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerAddressValidator>());
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerDetailValidator>());
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CustomerPhoneValidator>());

        services.AddMediatR(typeof(CreateCustomerCommand).GetTypeInfo().Assembly);
    }
    
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new AutofacBusinessModule(Configuration));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<LoggingMiddleware>();
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