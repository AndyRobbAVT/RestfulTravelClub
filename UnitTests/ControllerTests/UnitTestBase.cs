using System;
using System.Linq;
using System.Web.Http.Controllers;
using Castle.Core;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.Windsor;
using InstrumApi.Controllers;
using InstrumApi.Installers;
using Machine.Specifications;
using NUnit.Framework;

namespace UnitTests.ControllerTests
{
    [TestFixture]
    public class UnitTestBase
    {
        protected IWindsorContainer containerWithControllers;

        [SetUp]
        public void SetUp()
        {
            containerWithControllers = new WindsorContainer().Install(new ControllersInstaller());
        }

        protected IHandler[] GetAllHandlers(IWindsorContainer container)
        {
            return GetHandlersFor(typeof(object), container);
        }

        protected IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
        {
            return container.Kernel.GetAssignableHandlers(type);
        }

        protected Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
        {
            return GetHandlersFor(type, container)
                .Select(h => h.ComponentModel.Implementation)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        protected Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where)
        {
            return typeof(InstrumentsController).Assembly.GetExportedTypes()
                .Where(t => t.IsClass)
                .Where(t => t.IsAbstract == false)
                .Where(where.Invoke)
                .OrderBy(t => t.Name)
                .ToArray();
        }
    }

    [TestFixture]
    public class ControllerUnitTests : UnitTestBase
    {
        [Test]
        public void All_controllers_implement_IController()
        {
            var allHandlers = base.GetAllHandlers(containerWithControllers);
            var controllerHandlers = base.GetHandlersFor(typeof(IHttpController), containerWithControllers);

            allHandlers.ShouldNotBeEmpty();
            allHandlers.ShouldEqual(controllerHandlers);
        }

        [Test]
        public void All_controllers_are_registered()
        {
            // Is<TType> is an helper, extension method from Windsor in the Castle.Core.Internal namespace
            // which behaves like 'is' keyword in C# but at a Type, not instance level
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Is<IHttpController>());
            var registeredControllers = GetImplementationTypesFor(typeof(IHttpController), containerWithControllers);
            allControllers.ShouldEqual( registeredControllers);
        }

        [Test]
        public void All_and_only_controllers_have_Controllers_suffix()
        {
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
            var registeredControllers = GetImplementationTypesFor(typeof(IHttpController), containerWithControllers);
            allControllers.ShouldEqual(registeredControllers);
        }

        [Test]
        public void All_and_only_controllers_live_in_Controllers_namespace()
        {
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Controllers"));
            var registeredControllers = GetImplementationTypesFor(typeof(IHttpController), containerWithControllers);
            allControllers.ShouldEqual(registeredControllers);
        }

        [Test]
        public void All_controllers_are_transient()
        {
            var nonTransientControllers = GetHandlersFor(typeof(IHttpController), containerWithControllers)
                .Where(controller => controller.ComponentModel.LifestyleType != LifestyleType.Transient)
                .ToArray();

            nonTransientControllers.ShouldBeEmpty();
        }

        [Test]
        public void All_controllers_expose_themselves_as_service()
        {
            var controllersWithWrongName = GetHandlersFor(typeof(IHttpController), containerWithControllers)
                .Where(controller => controller.ComponentModel.Services.Single() != controller.ComponentModel.Implementation)
                .ToArray();

            controllersWithWrongName.ShouldBeEmpty();
        }
    }
}