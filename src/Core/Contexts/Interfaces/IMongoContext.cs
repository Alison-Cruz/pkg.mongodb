namespace Core.Contexts.Interfaces
{
    public interface IMongoContext
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}
