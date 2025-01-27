using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HUBT_Social_MongoDb_Service.Services
{
    internal class MongoService<Collection>(IMongoCollection<Collection> mongoCollection) : IMongoService<Collection>
        where Collection : class
    {
        private readonly IMongoCollection<Collection> _mongoCollection = mongoCollection;

        public async Task<bool> Create(Collection collection)
        {
            try
            {
                await _mongoCollection.InsertOneAsync(collection);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Collection collection)
        {
            try
            {
                var idProperty = typeof(Collection).GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttributes(typeof(BsonIdAttribute), true).Length != 0)
                    ?? throw new InvalidOperationException("No BsonId property found");
                var id = idProperty.GetValue(collection)?.ToString();
                if (string.IsNullOrEmpty(id))
                    throw new InvalidOperationException("Id value is null or empty");

                var filter = Builders<Collection>.Filter.Eq("_id", id);
                var result = await _mongoCollection.DeleteOneAsync(filter);

                return result.DeletedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Collection?> GetById(string id)
        {
            try
            {
                var filter = Builders<Collection>.Filter.Eq("_id", id);
                return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> Update(Collection collection)
        {
            try
            {
                var idProperty = typeof(Collection).GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttributes(typeof(BsonIdAttribute), true).Length != 0)
                    ?? throw new InvalidOperationException("No BsonId property found");

                var id = idProperty.GetValue(collection)?.ToString();
                if (string.IsNullOrEmpty(id))
                    throw new InvalidOperationException("Id value is null or empty");

                var filter = Builders<Collection>.Filter.Eq("_id", id);
                var updateResult = await _mongoCollection.ReplaceOneAsync(filter, collection);

                return updateResult.IsAcknowledged;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Collection>> GetAll()
        {
            try
            {
                return await _mongoCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<IEnumerable<Collection>> Find(Expression<Func<Collection, bool>> predicate)
        {
            try
            {
                return await _mongoCollection.Find(predicate).ToListAsync();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<bool> Exists(string id)
        {
            try
            {
                var filter = Builders<Collection>.Filter.Eq("_id", id);
                return await _mongoCollection.Find(filter).AnyAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<long> Count()
        {
            try
            {
                return await _mongoCollection.CountDocumentsAsync(_ => true);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}