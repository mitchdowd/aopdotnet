using System.Diagnostics;
using System.Reflection;
using Castle.DynamicProxy;

namespace AopPoc;

internal class AttributeInterceptorHandler : IInterceptor, IProxyGenerationHook
{
    public void Intercept(IInvocation invocation)
    {
        var attribute = invocation.Method.GetCustomAttribute<InterceptorAttribute>();

        Debug.Assert(attribute is not null);

        attribute.Invoke(new InterceptionContext(invocation)
        {
            Next = () => ProceedAsync(invocation)
        });
    }

    public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        => methodInfo.GetCustomAttributes<InterceptorAttribute>().Any();

    public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo) {}

    public void MethodsInspected() {}

    private static async Task ProceedAsync(IInvocation invocation)
    {
        invocation.Proceed();

        if (invocation.ReturnValue is Task task)
        {
            await task;

            if (task.GetType().IsGenericType)
            {
                invocation.ReturnValue = task.GetType()
                    .GetProperty(nameof(Task<object>.Result))!
                    .GetValue(task);
            }
        }
    }
}
