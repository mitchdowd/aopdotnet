namespace AopPoc;

public abstract class InterceptorAttribute : Attribute
{
    internal void Invoke(IInterceptionContext context)
    {
        Intercept(context).Wait(); // CANCER

        if (context is InterceptionContext ic)
        {
            ic.WrapReturnValue();
        }
    }

    public abstract Task Intercept(IInterceptionContext context);
}
