using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using PatientsList.Model.Entities;

namespace PatientsList.Model.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        void AddOrUpdate(T entity);
        T Add(T entity);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        IQueryable<T> Query();
        void Commit();
        void Rollback();
        void Flush();
        T Get(int id);
        IQueryable<T> QueryRandom(int? n = null);
    }

    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IUnitOfWork unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Commit()
        {
            unitOfWork.Commit();
        }

        public void Rollback()
        {
            unitOfWork.Rollback();
        }

        public void Flush()
        {
            unitOfWork.Flush();
        }

        protected IQueryable<T> TableFast
        {
            get { return unitOfWork.Session.Query<T>(); }
        }

        public T Get(int id)
        {
            return unitOfWork.Session.Get<T>(id);
        }

        public IQueryable<T> QueryRandom(int? n = null)
        {
            var r = new Random();
            if (n.HasValue)
            {
                return TableFast
                    .ToList()
                    .Take(n.Value)
                    .OrderBy(x => r.NextDouble())
                    .AsQueryable();
            }
            return TableFast
                .ToList()
                .OrderBy(x => r.NextDouble())
                .AsQueryable();
        }

        public virtual void AddOrUpdate(T entity)
        {
            unitOfWork.Session.SaveOrUpdate(entity);
        }

        public virtual T Add(T entity)
        {
            var id = (int)unitOfWork.Session.Save(entity);
            return unitOfWork.Session.Get<T>(id);
        }

        public void Delete(T entity)
        {
            unitOfWork.Session.Delete(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                unitOfWork.Session.Delete(entity);
            }
        }

        public IQueryable<T> Query()
        {
            return TableFast;
        }
    }
}
