using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Infrastructure
{
    public class ContextCapturingControllerActivator : IHttpControllerActivator
    {
        private readonly IHttpControllerActivator _activator;
        private readonly Func<TaskCompletionSource<ContextWrapper>> _promiseFactory;

        public ContextCapturingControllerActivator(
            Func<TaskCompletionSource<ContextWrapper>> promiseFactory,
            IHttpControllerActivator activator)
        {
            _activator = activator;
            _promiseFactory = promiseFactory;
        }

        public IHttpController Create(HttpControllerContext controllerContext,Type controllerType)
        {
            var url = controllerContext.Request.RequestUri;
            var baseUri = new UriBuilder(url.Scheme, url.Host, url.Port).Uri;
            _promiseFactory().SetResult(new ContextWrapper { BaseUri = baseUri, Context = controllerContext });
            return this._activator.Create(controllerContext, controllerType);
        }
    }
}