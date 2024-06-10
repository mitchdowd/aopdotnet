using System.Diagnostics;
using System.Reflection;
using Castle.DynamicProxy;

namespace AopPoc;

internal class AttributeInterceptorHandler : IInterceptor, IProxyGenerationHook
{
    public void Intercept(IInvocation invocation)
    {
        var attributes = invocation.Method.GetCustomAttributes<InterceptorAttribute>();

        Debug.Assert(attributes.Any());

        var context = new InterceptionContext(invocation, attributes);
        context.InvokeStack();
    }

    public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        => methodInfo.GetCustomAttributes<InterceptorAttribute>().Any();

    public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo) {}

    public void MethodsInspected() {}
}
