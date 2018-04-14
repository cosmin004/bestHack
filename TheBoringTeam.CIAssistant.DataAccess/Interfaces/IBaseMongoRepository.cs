using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TheBoringTeam.CIAssistant.BusinessEntities.Interfaces;

namespace TheBoringTeam.CIAssistant.DataAccess.Interfaces
{
    public interface IBaseMongoRepository<TBaseEntity> where TBaseEntity : IIdentifiable, ITrackable
    {
        IEnumerable<TBaseEntity> Get();
        IEnumerable<TBaseEntity> Get(Expression<Func<TBaseEntity, bool>> predicate);
        void Insert(TBaseEntity entity);
        void Insert(IEnumerable<TBaseEntity> entities);
        void Update(TBaseEntity entity);
        void Delete(TBaseEntity entity);
        void Delete(IEnumerable<TBaseEntity> entities);
        void Delete(string id);
        void Delete(IEnumerable<string> ids);
    }
}
