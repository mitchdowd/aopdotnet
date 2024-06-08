namespace AopPoc.Example;

public class ExampleService : IExampleService
{
    [Logging("First")]
    [Logging("Second")]
    public virtual async Task<int> DoSomethingAsync()
    {
        Console.Write("Doing something");

        for (int i = 0; i < 5; i++)
        {
            Console.Write(".");
            await Task.Delay(500);
        }

        Console.WriteLine(" Done!");

        return 10;
    }
}
