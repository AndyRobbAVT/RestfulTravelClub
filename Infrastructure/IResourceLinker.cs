using System;
using System.Linq.Expressions;

namespace Infrastructure
{
    public interface IResourceLinker
    {
        Uri GetUri<T>(Expression<Action<T>> method);
    }
}