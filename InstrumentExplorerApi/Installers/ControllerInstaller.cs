using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Api.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<ApiController>()
                                   .Configure(registration=>registration.Named(registration.Implementation.Name.ToLower().Replace("controller", "")))
                                   .LifestyleTransient());
        }
    }
}