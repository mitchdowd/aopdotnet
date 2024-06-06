namespace AopPoc;

public interface IInterceptionContext
{
    Func<Task> Next { get; }
}
