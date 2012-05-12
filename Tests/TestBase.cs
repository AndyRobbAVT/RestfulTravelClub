using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Api.Controllers;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Infrastructure;
using Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TestBase
    {
        private WindsorContainer _container;
        protected WebApiClient client;
        private HttpServer _server;

        [SetUp]
        public void SetUp()
        {
            //Due to bug in self hosting scenario and in-memory hosting we need to load the type into memory
            Type type = typeof (TripsController);
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            _server = new HttpServer(config);
            BootstrapWindsorContainer();
            Database.SetInitializer(new TripContextInitializerForTests());
            SetResolver(config);
            
            client = new WebApiClient(Constants.BaseUri + "api/trips/").AgainstInMemoryServer(_server);

        }

        private void SetResolver(HttpConfiguration config)
        {
            config.ServiceResolver.SetResolver(t =>
                                                {
                                                    try
                                                    {
                                                        return _container.Resolve(t);
                                                    }
                                                    catch (ComponentNotFoundException)
                                                    {
                                                        return null;
                                                    }
                                                },
                                            t =>
                                                {
                                                    try
                                                    {
                                                        return _container.ResolveAll(t).Cast<object>();
                                                    }
                                                    catch (ComponentNotFoundException)
                                                    {
                                                        return new List<object>();
                                                    }
                                                });
        }

        private void BootstrapWindsorContainer()
        {
            _container = new WindsorContainer();

            _container.Register(Component.For<IHttpControllerFactory>()
                                         .ImplementedBy<WindsorHttpControllerFactory>()
                                         .LifeStyle.Singleton);
            _container.Register(Component.For<HttpConfiguration>().Instance(_server.Configuration));
            _container.Register(Component.For<ITravelClubEntitiesContext>()
                        .ImplementedBy<Fakes.FakeTravelClubContext>());
            _container.Install(FromAssembly.This());

        }
    }
}