using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TheBoringTeam.CIAssistant.BusinessEntities.Interfaces;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;
using TheBoringTeam.CIAssistant.DataAccess.Interfaces;

namespace TheBoringTeam.CIAssistant.BusinessLogic.Entities
{
    public class BaseBusinessLogic<TBaseEntity> : IBaseBusinessLogic<TBaseEntity>
        where TBaseEntity : IIdentifiable, ITrackable
    {

        private IBaseMongoRepository<TBaseEntity> _repository;

        public BaseBusinessLogic(IBaseMongoRepository<TBaseEntity> repository)
        {
            _repository = repository;
        }

        public void Delete(string id)
        {
            _repository.Delete(id);
        }

        public void Delete(IEnumerable<string> ids)
        {
            _repository.Delete(ids);
        }

        public void Delete(TBaseEntity entity)
        {
            _repository.Delete(entity);
        }

        public void Delete(IEnumerable<TBaseEntity> entities)
        {
            _repository.Delete(entities);
        }

        public TBaseEntity GetById(string id)
        {
            return _repository.Get(f => f.Id == id).FirstOrDefault();
        }

        public void Insert(IEnumerable<TBaseEntity> entities)
        {
            _repository.Insert(entities);
        }

        public void Insert(TBaseEntity entity)
        {
            _repository.Insert(entity);
        }

        public IEnumerable<TBaseEntity> Search(Expression<Func<TBaseEntity, bool>> filter)
        {
            return _repository.Get(filter);
        }

        public void Update(TBaseEntity entity)
        {
            _repository.Update(entity);
        }
    }
}
