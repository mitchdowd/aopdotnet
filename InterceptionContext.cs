using Castle.DynamicProxy;

namespace AopPoc;

public class InterceptionContext : IInterceptionContext
{
    internal IInvocation Invocation { get; }

    public required Func<Task> NextAsync { get; set; }

    public InterceptionContext(IInvocation invocation)
    {
        Invocation = invocation;
    }
}
