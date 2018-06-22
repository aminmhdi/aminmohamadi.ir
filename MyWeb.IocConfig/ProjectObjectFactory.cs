﻿using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Threading;
using System.Web;
using AutoMapper;
using MyWeb.DataLayer.Context;
using StructureMap;
using StructureMap.Web;

namespace MyWeb.IocConfig
{
    public class ProjectObjectFactory
    {
        #region Fields
        private static readonly Lazy<Container> ContainerBuilder =
          new Lazy<Container>(DefaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        #endregion

        #region Container
        public static IContainer Container => ContainerBuilder.Value;

        #endregion

        #region DefaultContainer
        private static Container DefaultContainer()
        {
            var container = new Container(ioc =>
            {
                ioc.For<IUnitOfWork>()
                      .HybridHttpOrThreadLocalScoped()
                      .Use<MyWebContext>();

                ioc.For<IIdentity>()
                     .Use(
                         () =>
                             (HttpContext.Current != null && HttpContext.Current.User != null)
                                 ? HttpContext.Current.User.Identity
                                 : null);

                ioc.For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
                ioc.For<HttpServerUtilityBase>().Use(() => new HttpServerUtilityWrapper(HttpContext.Current.Server));
                ioc.For<HttpRequestBase>().Use(ctx => ctx.GetInstance<HttpContextBase>().Request);


                ioc.For<IRemotingFormatter>().Use(a => new BinaryFormatter());

                ioc.AddRegistry<AspNetIdentityRegistery>();
                ioc.AddRegistry<AutoMapperRegistery>();
                ioc.AddRegistry<ServiceLayerRegistery>();
                ioc.AddRegistry<TaskRegistry>();

                ioc.Scan(scanner => scanner.WithDefaultConventions());
                ioc.Policies.SetAllProperties(y => y.OfType<HttpContextBase>());
            });

            ConfigureAutoMapper(container);
            return container;
        }
        #endregion

        #region ConfigureAutoMapper
        private static void ConfigureAutoMapper(IContainer container)
        {
            var configuration = container.TryGetInstance<IConfiguration>();
            if (configuration == null) return;
            //saying AutoMapper how to resolve services
            configuration.ConstructServicesUsing(container.GetInstance);
            foreach (var profile in container.GetAllInstances<Profile>())
            {
                configuration.AddProfile(profile);
            }
        }
        #endregion
    }
}
