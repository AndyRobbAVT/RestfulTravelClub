using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
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
        private HttpSelfHostServer _server;
        private WindsorContainer _container;
        [SetUp]
        public void SetUp()
        {
            //Due to bug in self hosting scenario we need to load the type into memory
            Type type = typeof (TripsController);
            // Setup _server configuration 
            var config = new HttpSelfHostConfiguration(Constants.BaseUri);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            _server = new HttpSelfHostServer(config);
            BootstrapWindsorContainer();
            Database.SetInitializer(new TripContextInitializerForTests());
            SetResolver();
            _server.OpenAsync().Wait();
        }

        private void SetResolver()
        {
            _server.Configuration.ServiceResolver.SetResolver(t =>
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
                        .ImplementedBy<Repository.TravelClubEntitiesContext>());
            _container.Install(FromAssembly.This());

        }

        [TearDown]
        public void TearDown()
        {
            _server.CloseAsync();
            _server.Dispose();
        }
    }
}