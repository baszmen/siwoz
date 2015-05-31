using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsList.Model
{
    public interface INHibernate : IDisposable
    {
        ISession OpenSession();
    }

    public abstract class NHibernateInMemory : INHibernate
    {
        protected ISession Session { get; set; }

        public abstract ISession OpenSession();

        protected void BuildSchema(Configuration config)
        {
            var export = new SchemaExport(config);
            export.Execute(true, true, false);
        }

        protected void BuildSchema(Configuration config, IDbConnection connection)
        {
            var export = new SchemaExport(config);
            export.Execute(true, true, false, connection, null);
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }

    public class NHibernateInMemory : NHibernateInMemory
    {
        private static ISessionFactory _sessionFactory;
        protected static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    Initialize();
            }
        }
        protected static readonly object Locker = new object();

        protected static Configuration Configuration;

        public override ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        protected new void BuildSchema(Configuration config)
        {
            var export = new SchemaUpdate(config);
            export.Execute(false, true);
        }

        protected void Initialize()
        {
            return return Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateInMemoryCommonSession>())
                    .ExposeConfiguration(cfg => configuration = cfg)
                    .BuildSessionFactory();
        }
    }
}
