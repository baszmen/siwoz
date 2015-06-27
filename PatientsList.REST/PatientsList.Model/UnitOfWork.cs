using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            Start(true);
        }

        public UnitOfWork(bool inMemory)
        {
            Start(inMemory);
        }

        public UnitOfWork(bool start = false, bool inMemory = false)
        {
            if (start)
                Start(inMemory);
        }

        public void Start(bool inMemory = false)
        {
            session = inMemory ? NHibernateInMemory.OpenSession() : NHibernateInMemoryCleanup.OpenSession();
            session.FlushMode = FlushMode.Commit;
            while (true)
                try
                {
                    transaction = session.BeginTransaction();
                    break;
                }
                catch
                {
                    Thread.Sleep(500);
                }
        }

        public void Start()
        {
            throw new NotImplementedException();
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
