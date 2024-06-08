namespace AopPoc;

public interface IInterceptionContext
{
    Func<Task> NextAsync { get; }
}
