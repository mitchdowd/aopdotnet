# AOP.NET

This is a basic proof of concept for using C# attributes to intercept control flow
in services dynamically. It uses Castle DynamicProxy underneath.

*Disclaimer*: If you've randomly found this repository, please note that this is a
proof of concept and not intended for production use.

## Feature Goals

- [x] Interception of synchronous methods
- [x] Interception of asynchronous methods
- [x] Correctly passes return value in synchronous methods
- [x] Correctly passes return value in asynchronous methods
- [x] Handles multiple interceptors on a single method
- [ ] Both synchronous and asynchronous interceptors
- [ ] Validation on intercepting sync methods with an async interceptor
- [ ] Access to arguments passed to the intercepted method
- [ ] Able to change the return value of the intercepted method
- [ ] Do some performance benchmarking to see if it's viable

## Example Usage

```csharp

public class LoggingInterceptorAttribute : InterceptorAttribute
{
    public override async Task InterceptAsync(IInterceptionContext context)
    {
        Console.WriteLine("Before method call");
        await context.Next();
        Console.WriteLine("After method call");
    }
}

public class ExampleService
{
    [LoggingInterceptor]
    public virtual void DoSomething() => Console.WriteLine("Doing something...");
}

var interceptor = new ServiceInterceptor();
var service = interceptor.Intercept(new ExampleService());

service.DoSomething();
// Prints:
//  Before method call
//  Doing somemthing...
//  After method call
```
