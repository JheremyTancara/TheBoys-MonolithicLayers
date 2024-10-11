namespace Api.Models

{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
