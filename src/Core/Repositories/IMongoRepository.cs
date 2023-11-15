using Core.Attributes;
using Core.Contexts.Interfaces;
using Core.Entities.Interfaces;
using Core.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Repositories
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument>
        where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IMongoContext mongoContext)
        {
            var database = new MongoClient(mongoContext.ConnectionString)
                .GetDatabase(mongoContext.DatabaseName);

            _collection = database
                .GetCollection<TDocument>(
                    MongoRepository<TDocument>.GetCollectionName(typeof(TDocument)));
        }

        protected static string GetCollectionName(Type documentType)
        {
            return (documentType
                .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault() as BsonCollectionAttribute)?
                .CollectionName!;
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection
                .Find(filterExpression)
                .ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection
                .Find(filterExpression)
                .Project(projectionExpression)
                .ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection
                .Find(filterExpression)
                .FirstOrDefault();
        }

        public virtual async Task<TDocument> FindOneAsync(
            Expression<Func<TDocument, bool>> filterExpression, 
            CancellationToken cancellationToken)
        {
            return await _collection
                .Find(filterExpression)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual async Task<TDocument> FindByIdAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var objectId = new ObjectId(id);
            
            var filter = Builders<TDocument>
                .Filter
                .Eq(doc => doc.Id, objectId);
           
            return await _collection
                .Find(filter)
                .SingleOrDefaultAsync(cancellationToken);  
        }

        public virtual void InsertOne(TDocument document)
        {
            _collection.InsertOne(document);
        }

        public virtual async Task InsertOneAsync(
            TDocument document, 
            InsertOneOptions? options = default, 
            CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(
                document, 
                options, 
                cancellationToken);
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }

        public virtual async Task InsertManyAsync(
            ICollection<TDocument> documents, 
            InsertManyOptions? options = default,
            CancellationToken cancellationToken = default)
        {
            await _collection.InsertManyAsync(
                documents, 
                options, 
                cancellationToken);
        }

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(
                filter, 
                document);
        }

        public virtual async Task ReplaceOneAsync(
            TDocument document,
            CancellationToken cancellationToken)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
           
            await _collection.FindOneAndReplaceAsync(
                filter, 
                document, 
                cancellationToken: cancellationToken);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public async Task DeleteOneAsync(
            Expression<Func<TDocument, bool>> filterExpression, 
            CancellationToken cancellationToken)
        {
            await _collection.FindOneAndDeleteAsync(filterExpression, cancellationToken: cancellationToken);
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
           
            var filter = Builders<TDocument>
                .Filter
                .Eq(doc => doc.Id, objectId);
           
            _collection.FindOneAndDelete(filter);
        }

        public async Task DeleteByIdAsync(
            string id, 
            CancellationToken cancellationToken)
        {       
            var objectId = new ObjectId(id);
                
            var filter = Builders<TDocument>
                .Filter
                .Eq(doc => doc.Id, objectId);

            await _collection.FindOneAndDeleteAsync(
                filter, 
                cancellationToken: cancellationToken);     
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public async Task DeleteManyAsync(
            Expression<Func<TDocument, bool>> filterExpression,
            CancellationToken cancellationToken)
        {
            await _collection.DeleteManyAsync(
                filterExpression, 
                cancellationToken);
        }
    }
}
