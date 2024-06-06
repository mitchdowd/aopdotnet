
using Castle.DynamicProxy;

namespace AopPoc;

internal class InterceptionContext : IInterceptionContext
{
    private readonly IInvocation _invocation;

    public Func<Task> Next { get; set; }

    public InterceptionContext(IInvocation invocation)
    {
        _invocation = invocation;
    }

    public void WrapReturnValue()
    {
        _invocation.ReturnValue = CoerceType(_invocation.Method.ReturnType, _invocation.ReturnValue);
    }

    private static Task CoerceType(Type type, object value)
    {
        if (type.IsAssignableTo(typeof(Task)))
        {
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Task<>))
                return Task.CompletedTask; // Can't return a value

            return (Task)typeof(Task)
                .GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(type.GetGenericArguments()[0])
                .Invoke(null, [value])!;
        }

        return (Task) typeof(Task)
            .GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(type)
            .Invoke(null, [value])!;
    }
}
