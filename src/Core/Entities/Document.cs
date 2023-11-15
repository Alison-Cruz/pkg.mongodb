using Core.Entities.Interfaces;
using MongoDB.Bson;

namespace Core.Entities
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}
