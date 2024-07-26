using System.Data;
using Autofac;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        var connectionString = configuration.GetConnectionString("MsSqlConnection");

        builder.Register(c => new SqlConnection(connectionString))
            .As<IDbConnection>().InstancePerLifetimeScope();

        builder.Register(c => new ParaDbContext(new DbContextOptionsBuilder<ParaDbContext>()
                .UseSqlServer(connectionString)
                .Options))
            .AsSelf()
            .InstancePerLifetimeScope();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MapperConfig());
        });

        builder.RegisterInstance(mapperConfig.CreateMapper()).As<IMapper>().SingleInstance();

        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
    }
}