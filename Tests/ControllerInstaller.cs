using System.Web.Http;
using Api.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Tests
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<TripsController>()
                                   .BasedOn<ApiController>()
                                   .Configure(registration=>registration.Named(registration.Implementation.Name.ToLower().Replace("controller", "")))
                                   .LifestyleTransient());
        }
    }
}