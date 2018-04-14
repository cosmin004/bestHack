using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TheBoringTeam.CIAssistant.BusinessEntities.Interfaces;
using TheBoringTeam.CIAssistant.DataAccess.Interfaces;

namespace TheBoringTeam.CIAssistant.DataAccess
{
    public class BaseMongoRepository<TBaseEntity> : IBaseMongoRepository<TBaseEntity>
        where TBaseEntity : IIdentifiable, ITrackable
    {
        private IMongoDatabase _database;
        private IMongoCollection<TBaseEntity> _collection;
        private IMongoClient _client;
        private string _collectionName;

        public BaseMongoRepository(string connectionString, string databaseName, bool isSSL)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

            if(isSSL)
            {
                settings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                };
            }

            _client = new MongoClient(settings);
            _database = _client.GetDatabase(databaseName);
            _collectionName = typeof(TBaseEntity).Name;
            _collection = _database.GetCollection<TBaseEntity>(_collectionName);
        }

        public void Delete(TBaseEntity entity)
        {
            this.Delete(new List<string> { entity.Id });
        }

        public void Delete(IEnumerable<TBaseEntity> entities)
        {
            IEnumerable<string> ids = entities.Select(f => f.Id);
            this.Delete(ids);
        }

        public void Delete(string id)
        {
            this.Delete(new List<string> { id });
        }

        public void Delete(IEnumerable<string> ids)
        {
            this._collection.DeleteMany(f => ids.Contains(f.Id));
        }

        public IEnumerable<TBaseEntity> Get()
        {
            return this.Get(f => true);
        }

        public IEnumerable<TBaseEntity> Get(Expression<Func<TBaseEntity, bool>> predicate)
        {
            return this._collection.Find(predicate).ToList();
        }

        public void Insert(TBaseEntity entity)
        {
            this.Insert(new List<TBaseEntity>() { entity });
        }

        public void Insert(IEnumerable<TBaseEntity> entities)
        {
            foreach(var entity in entities)
            {
                entity.Id = ConvertGuidToObjectId(Guid.NewGuid()).ToString();
                entity.DateCreation = DateTime.UtcNow;
            }

            this._collection.InsertMany(entities);
        }

        public void Update(TBaseEntity entity)
        {
            entity.DateModification = DateTime.UtcNow;
            this._collection.ReplaceOne(f => f.Id == entity.Id, entity);
        }

        private ObjectId ConvertGuidToObjectId(Guid gid)
        {
            var bytes = gid.ToByteArray().Take(12).ToArray();
            var oid = new ObjectId(bytes);
            return oid;
        }
    }
}
