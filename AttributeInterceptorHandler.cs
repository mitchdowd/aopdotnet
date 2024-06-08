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

        var proceed = invocation.CaptureProceedInfo();

        attribute.Invoke(new InterceptionContext(invocation)
        {
            NextAsync = async () => {
                proceed.Invoke();

                if (invocation.ReturnValue is Task task)
                    await task;
            }
        });
    }

    public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        => methodInfo.GetCustomAttributes<InterceptorAttribute>().Any();

    public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo) {}

    public void MethodsInspected() {}
}
