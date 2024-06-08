using Castle.DynamicProxy;

namespace AopPoc;

public class InterceptionContext
{
    internal IInvocation Invocation { get; }

    public required Func<Task> NextAsync { get; set; }

    internal InterceptionContext(IInvocation invocation)
    {
        Invocation = invocation;
    }
}
