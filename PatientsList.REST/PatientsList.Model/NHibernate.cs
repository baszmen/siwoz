﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using PatientsList.Model.Entities;

namespace PatientsList.Model
{
    public static class NHibernateInMemoryCleanup
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    Initialize();
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            var session =  SessionFactory.OpenSession();
            new SchemaExport(_configuration).Execute(false, true, false, session.Connection, null);
            return session;
        }

        public static void Initialize()
        {
            _sessionFactory = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.InMemory().ShowSql)
                    .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Doctor>())
                    .ExposeConfiguration(config =>
                    {
                        _configuration = config;
                    })
                    .BuildSessionFactory();
        }
    }

    public static class NHibernateInMemory
    {
        private static ISessionFactory m_sessionFactory { get; set; }
        private static IDbConnection m_connection;
        private static Configuration m_config { get; set; }

        public static ISession OpenSession()
        {
            if (m_sessionFactory == null)
                InitializeSessionFactory();
            return m_connection == null ? m_sessionFactory.OpenSession() : m_sessionFactory.OpenSession(m_connection);
        }

        public static void InitializeSessionFactory()
        {
            m_sessionFactory = Fluently
                .Configure()
                .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .ExposeConfiguration(x => m_config = x)
                .BuildSessionFactory();

            m_connection = m_sessionFactory.OpenSession().Connection;
            var export = new SchemaExport(m_config);
            export.Execute(true, true, false, m_connection, null);
        }
    }
}
