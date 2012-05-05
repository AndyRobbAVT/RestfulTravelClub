using System;
using System.Web.Http.Controllers;

namespace Infrastructure
{
    public class ContextWrapper
    {
        public Uri BaseUri { get; set; }
        public HttpControllerContext Context { get; set; }
    }
}