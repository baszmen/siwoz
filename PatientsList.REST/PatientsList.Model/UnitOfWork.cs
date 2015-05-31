using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace PatientsList.Model
{
    public interface IUnitOfWork : IDisposable
    {
        ISession Session { get; }
        void Commit();
        void Rollback();
        void Start();
        void Flush();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private ISession session;
        private ITransaction transaction;

        public UnitOfWork()
        {
            Start();
        }

        public UnitOfWork(bool start = false)
        {
            if (start)
                Start();
        }

        public void Start()
        {
            session = NHibernateInMemory.OpenSession();
            session.FlushMode = FlushMode.Commit;
            transaction = session.BeginTransaction();
        }

        public void Flush()
        {
            session.Flush();
        }

        public ISession Session
        {
            get { return session; }
        }

        public void Commit()
        {
            if (transaction.IsActive)
                transaction.Commit();
        }

        public void Rollback()
        {
            if (transaction.IsActive)
                transaction.Rollback();
        }

        public void Dispose()
        {
            if (transaction != null)
                transaction.Dispose();
            if (session != null)
                session.Dispose();
        }
    }
}
