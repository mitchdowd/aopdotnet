using AopPoc.Example;

namespace AopPoc;

public class Program
{
    public static async Task Main(string[] args)
    {
        var interceptor = new ServiceInterceptor();

        var service = interceptor.Intercept(new ExampleService());

        var result = await service.DoSomethingAsync();

        Console.WriteLine($"Result: {result}");
    }
}
