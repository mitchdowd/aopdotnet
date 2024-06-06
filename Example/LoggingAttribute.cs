﻿namespace AopPoc.Example;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class LoggingAttribute : InterceptorAttribute
{
    private readonly string _name;

    public LoggingAttribute(string name)
    {
        _name = name;
    }

    public override async Task Intercept(IInterceptionContext context)
    {
        Console.WriteLine($"Before method call: {_name}");
        await Task.Delay(100);
        await context.Next();
        await Task.Delay(100);
        Console.WriteLine($"After method call: {_name}");
    }
}