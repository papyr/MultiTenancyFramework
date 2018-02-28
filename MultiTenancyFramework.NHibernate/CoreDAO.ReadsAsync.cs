﻿using MultiTenancyFramework.Data;
using MultiTenancyFramework.Entities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiTenancyFramework.NHibernate
{
    public partial class CoreDAO<T, idT> : CoreGridPagingDAO<T, idT>, ICoreDAO<T, idT> where T : class, IBaseEntity<idT> where idT : IEquatable<idT>
    {
        public async Task<T> LoadAsync(idT id, CancellationToken token = default(CancellationToken))
        {
            var session = BuildSession();
            if (string.IsNullOrWhiteSpace(EntityName))
            {
                return await session.LoadAsync<T>(id, token);
            }
            return await session.LoadAsync(EntityName, id, token) as T;
        }

        public async Task<T> RetrieveAsync(idT id, CancellationToken token = default(CancellationToken))
        {
            var session = BuildSession();
            if (string.IsNullOrWhiteSpace(EntityName))
            {
                return await session.GetAsync<T>(id, token);
            }
            return await session.GetAsync(EntityName, id, token) as T;
        }

        /// <summary>
        /// Retrieve the first item found inthe db. This is useful for tables expected to have just one enty
        /// </summary>
        /// <returns></returns>
        public async Task<T> RetrieveOneAsync(CancellationToken token = default(CancellationToken))
        {
            var session = BuildSession();
            IQueryOver<T, T> query;
            if (string.IsNullOrWhiteSpace(EntityName))
            {
                query = session.QueryOver<T>();
            }
            else
            {
                query = session.QueryOver<T>(EntityName);
            }
            return await query.Take(1).SingleOrDefaultAsync(token);
        }

        public async Task<IList<idT>> RetrieveIDsAsync(CancellationToken token = default(CancellationToken))
        {
            var session = BuildSession();
            if (string.IsNullOrWhiteSpace(EntityName))
            {
                return await session.QueryOver<T>().Select(x => x.Id).ListAsync<idT>(token);
            }
            return await session.QueryOver<T>(EntityName).Select(x => x.Id).ListAsync<idT>(token);
        }

        public async Task<IList<T>> RetrieveAllAsync(string[] fields, CancellationToken token = default(CancellationToken))
        {
            var session = BuildSession();
            IQueryOver<T, T> query;
            if (string.IsNullOrWhiteSpace(EntityName))
            {
                query = session.QueryOver<T>();
            }
            else
            {
                query = session.QueryOver<T>(EntityName);
            }

            return await GetResultUsingProjectionAsync(query, fields, token);
        }

        public async Task<IList<T>> RetrieveAllActiveAsync(string[] fields, CancellationToken token = default(CancellationToken))
        {
            var session = BuildSession();
            IQueryOver<T, T> query;
            if (string.IsNullOrWhiteSpace(EntityName))
            {
                query = session.QueryOver<T>();
            }
            else
            {
                query = session.QueryOver<T>(EntityName);
            }
            query = query.Where(x => !x.IsDisabled);

            return await GetResultUsingProjectionAsync(query, fields, token);
        }

        public async Task<IList<T>> RetrieveAllDeletedAsync(string[] fields, CancellationToken token = default(CancellationToken))
        {
            var session = BuildSession();
            IQueryOver<T, T> query;
            if (string.IsNullOrWhiteSpace(EntityName))
            {
                query = session.QueryOver<T>();
            }
            else
            {
                query = session.QueryOver<T>(EntityName);
            }
            query = query.Where(x => x.IsDeleted);

            return await GetResultUsingProjectionAsync(query, fields, token);
        }

        public async Task<IList<T>> RetrieveAllInactiveAsync(string[] fields, CancellationToken token = default(CancellationToken))
        {
            var session = BuildSession();
            IQueryOver<T, T> query;
            if (string.IsNullOrWhiteSpace(EntityName))
            {
                query = session.QueryOver<T>();
            }
            else
            {
                query = session.QueryOver<T>(EntityName);
            }
            query = query.Where(x => x.IsDisabled);

            return await GetResultUsingProjectionAsync(query, fields, token);
        }

        public async Task<IList<T>> RetrieveByIDsAsync(idT[] IDs, string[] fields, CancellationToken token = default(CancellationToken))
        {
            var session = BuildSession();
            IQueryOver<T, T> query;
            if (string.IsNullOrWhiteSpace(EntityName))
            {
                query = session.QueryOver<T>();
            }
            else
            {
                query = session.QueryOver<T>(EntityName);
            }
            query = query.Where(x => x.Id.IsIn(IDs));

            return await GetResultUsingProjectionAsync(query, fields, token);
        }

        private Task<IList<T>> GetResultUsingProjectionAsync(IQueryOver<T, T> query, string[] fields, CancellationToken token = default(CancellationToken))
        {
            if (fields == null || fields.Length == 0)
            {
                return query.ListAsync<T>(token);
            }

            var projectionList = Projections.ProjectionList()
                     .Add(Projections.Id(), "Id");
            foreach (var prop in fields)
            {
                if (prop == "Id") continue;

                projectionList.Add(Projections.Property(prop), prop);
            }
            var results = query.Select(projectionList)
                .TransformUsing(Transformers.AliasToBean<T>())
                .ListAsync<T>(token);

            return results;
        }

    }
}
