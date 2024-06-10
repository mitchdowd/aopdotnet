namespace AopPoc.Example;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class LoggingAttribute : InterceptorAttribute
{
    private readonly string _name;

    public LoggingAttribute(string name)
    {
        _name = name;
    }

    public override async Task InterceptAsync(IInterceptionContext context)
    {
        Console.WriteLine($"Before method call: {_name}");
        await Task.Delay(1000);
        await context.NextAsync();
        await Task.Delay(1000);
        Console.WriteLine($"After method call: {_name}");
    }
}
