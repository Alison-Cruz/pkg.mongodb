using Core.Contexts.Interfaces;

namespace Core.Contexts
{
    public class MongoContext : IMongoContext
    {
        public string DatabaseName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }
}
