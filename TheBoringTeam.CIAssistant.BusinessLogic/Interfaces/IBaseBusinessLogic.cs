using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TheBoringTeam.CIAssistant.BusinessEntities.Interfaces;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Interfaces
{
    public interface IBaseBusinessLogic<TBaseEntity> where TBaseEntity : IIdentifiable, ITrackable
    {
        IEnumerable<TBaseEntity> Search(Expression<Func<TBaseEntity, bool>> filter);
        TBaseEntity GetById(string id);
        void Update(TBaseEntity entity);
        void Insert(IEnumerable<TBaseEntity> entities);
        void Insert(TBaseEntity entity);
        void Delete(string id);
        void Delete(IEnumerable<string> ids);
        void Delete(TBaseEntity entity);
        void Delete(IEnumerable<TBaseEntity> entities);
    }
}
