using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Infrastructure;
using Model;
using Repository;

namespace Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static WindsorContainer _container;

        protected void Application_Start()
        {
            BootstrapWindsorContainer();
            Database.SetInitializer(new TravelClubContextInitializer());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();
        }

        private void BootstrapWindsorContainer()
        {
            _container = new WindsorContainer();
            _container.AddFacility<TypedFactoryFacility>();

            _container.Register(Component.For<ITravelClubEntitiesContext>()
                                         .ImplementedBy<TravelClubEntitiesContext>());

            _container.Register(Component.For<IHttpControllerActivator>()
                                        .ImplementedBy<ContextCapturingControllerActivator>());

            _container.Register(Component.For<IHttpControllerActivator>()
                                        .ImplementedBy<DefaultHttpControllerActivator>());

            _container.Register(Component.For<Func<TaskCompletionSource<ContextWrapper>>>()
                                        .AsFactory());

            _container.Register(Component.For<TaskCompletionSource<ContextWrapper>>()
                                        .LifestylePerWebRequest());

            _container.Register(Component.For<ContextWrapper>()
                                        .UsingFactoryMethod(k => 
                                                            k.Resolve<TaskCompletionSource<ContextWrapper>>().Task.Result).LifestylePerWebRequest());

            _container.Register(Component.For<IResourceLinker>()
                                        .ImplementedBy<RouteLinker>());

            _container.Install(FromAssembly.This());

            _container.Register(Component.For<HttpConfiguration>().Instance(GlobalConfiguration.Configuration));
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            SetResolver();
        }

        private static void SetResolver()
        {
            GlobalConfiguration.Configuration.ServiceResolver.SetResolver(
                                                                        type =>
                                                                            {
                                                                                try
                                                                                {
                                                                                    return _container.Resolve(type);
                                                                                }
                                                                                catch (ComponentNotFoundException)
                                                                                {
                                                                                    return null;
                                                                                }
                                                                            },
                                                                        type =>
                                                                            {
                                                                                try
                                                                                {
                                                                                    return _container.ResolveAll(type).Cast<object>();
                                                                                }
                                                                                catch (ComponentNotFoundException)
                                                                                {
                                                                                    return new List<object>();
                                                                                }
                                                                            });
        }
    }
}