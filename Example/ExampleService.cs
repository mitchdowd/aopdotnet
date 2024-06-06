namespace AopPoc.Example;

public class ExampleService : IExampleService
{
    [Logging("First")]
    public virtual async Task<int> DoSomething()
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
