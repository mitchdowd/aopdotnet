using Castle.DynamicProxy;

namespace AopPoc;

public class InterceptionContext : IInterceptionContext
{
    private readonly IInvocation _invocation;
    private readonly IInvocationProceedInfo _proceedInfo;
    private readonly Stack<InterceptorAttribute> _executionStack;

    public IInvocation Invocation => _invocation;

    public InterceptionContext(IInvocation invocation, IEnumerable<InterceptorAttribute> attributes)
    {
        _invocation = invocation;
        _proceedInfo = invocation.CaptureProceedInfo();
        _executionStack = new(attributes.Reverse());
    }

    public async Task NextAsync()
    {
        if (_executionStack.TryPop(out var next))
        {
            await next.InterceptAsync(this);
        }
        else
        {
            _proceedInfo.Invoke();

            if (_invocation.ReturnValue is Task task)
                await task;
        }
    }

    public void InvokeStack() => _executionStack.Pop().Invoke(this);
}
