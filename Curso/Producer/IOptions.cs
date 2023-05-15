namespace Curso.Producer
{
    public interface IOptions<out TOptions> where TOptions : class, new()
    {
        TOptions Value { get; }
    }
}
