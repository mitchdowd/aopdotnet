using Castle.DynamicProxy;

namespace AopPoc;

public class ServiceInterceptor
{
    private readonly ProxyGenerator _generator = new();

    public TService Intercept<TService>(TService instance) where TService : class
    {
        var handler = new AttributeInterceptorHandler();
        var options = new ProxyGenerationOptions(handler);

        if (typeof(TService).IsInterface)
            return _generator.CreateInterfaceProxyWithTarget(instance, options, handler);

        return _generator.CreateClassProxyWithTarget(instance, options, handler);
    }
}
