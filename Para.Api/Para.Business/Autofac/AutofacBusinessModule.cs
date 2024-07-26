using Autofac;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Para.Business.Cqrs;
using Para.Data.Context;
using Para.Data.UnitOfWork;

namespace Para.Business.Autofac;

public class AutofacBusinessModule : Module
{
    private readonly IConfiguration configuration;

    public AutofacBusinessModule(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(x =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ParaDbContext>();
            
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MsSqlConnection"));
            
            return new ParaDbContext(optionsBuilder.Options);
        }).InstancePerLifetimeScope();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MapperConfig());
        });

        builder.RegisterInstance(mapperConfig.CreateMapper()).As<IMapper>().SingleInstance();

        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
    }
}