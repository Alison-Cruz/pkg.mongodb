using Core.Entities.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Repositories.Interfaces
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();

        IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);

        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindOneAsync(
            Expression<Func<TDocument, bool>> filterExpression, 
            CancellationToken cancellationToken);

        TDocument FindById(string id);

        Task<TDocument> FindByIdAsync(
            string id, 
            CancellationToken cancellationToken);

        void InsertOne(TDocument document);

        Task InsertOneAsync(
            TDocument document,
            InsertOneOptions? options = default,
            CancellationToken cancellationToken = default);

        void InsertMany(ICollection<TDocument> documents);

        Task InsertManyAsync(
            ICollection<TDocument> documents,
            InsertManyOptions? options = default,
            CancellationToken cancellationToken = default);

        void ReplaceOne(TDocument document);

        Task ReplaceOneAsync(
            TDocument document, 
            CancellationToken cancellationToken);

        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteOneAsync(
            Expression<Func<TDocument, bool>> filterExpression, 
            CancellationToken cancellationToken);

        void DeleteById(string id);

        Task DeleteByIdAsync(
            string id, 
            CancellationToken cancellationToknen);

        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteManyAsync(
            Expression<Func<TDocument, bool>> filterExpression,
            CancellationToken cancellationToken);
    }
}
