using MyWeb.ServiceLayer.EFServices.Users;
using StructureMap.Configuration.DSL;

namespace MyWeb.IocConfig
{
    public class ServiceLayerRegistery : Registry
    {
        public ServiceLayerRegistery()
        {
            Scan(scanner =>
            {
                scanner.WithDefaultConventions();
                scanner.AssemblyContainingType<ApplicationUserManager>();
            });

            //Scan(scanner =>
            //{
            //    scanner.WithDefaultConventions();
            //    scanner.AssemblyContainingType<OutgoingMessageService>();
            //});

            //Scan(scanner =>
            //{
            //    scanner.WithDefaultConventions();
            //    scanner.AssemblyContainingType<ContactService>();
            //});
        }
    }
}
