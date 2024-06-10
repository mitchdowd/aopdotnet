using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace AopPoc;

public abstract class InterceptorAttribute : Attribute
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> _cachedInvokeMethods = [];

    internal void Invoke(InterceptionContext context)
    {
        var returnType = context.Invocation.Method.ReturnType;

        if (returnType.IsAssignableTo(typeof(Task)))
        {
            if (returnType.IsGenericType)
            {
                // Need to handle return type value.
                GetTypedAsyncMethod(returnType).Invoke(this, [ context ]);
            }
            else
            {
                context.Invocation.ReturnValue = InterceptAsync(context);
            }
        }
        else
        {
            // Running an async interceptor for a synchronous method requires a wait operation.
            InterceptAsync(context).Wait();
        }
    }

    public abstract Task InterceptAsync(IInterceptionContext context);

    private void TypedAsyncInvoke<TResult>(InterceptionContext context)
    {
        context.Invocation.ReturnValue = InterceptAsync(context).ContinueWith(t =>
        {
            var result = context.Invocation.ReturnValue as Task<TResult>;

            Debug.Assert(result is not null);

            return result.Result;
        });
    }

    private static MethodInfo GetTypedAsyncMethod(Type returnType)
    {
        Debug.Assert(returnType.IsAssignableTo(typeof(Task)) && returnType.IsGenericType);

        return _cachedInvokeMethods.GetOrAdd(returnType, _ => typeof(InterceptorAttribute)
            .GetMethod(nameof(TypedAsyncInvoke), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(returnType.GetGenericArguments()[0]));
    }
}
