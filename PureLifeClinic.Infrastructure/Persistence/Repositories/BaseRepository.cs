﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Exceptions;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Persistence.Data;
using System.Linq.Expressions;

namespace PureLifeClinic.Infrastructure.Persistence.Repositories
{
    //Unit of Work Pattern
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        protected DbSet<T> DbSet => _dbContext.Set<T>();

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
        {
            var data = await _dbContext.Set<T>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return data;
        }

        public async Task<IEnumerable<T>> GetAll(
            List<Expression<Func<T, object>>> includeExpressions, 
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (includeExpressions != null)
            {
                query = includeExpressions.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
            }

            var entities = await query.AsNoTracking().ToListAsync(cancellationToken);
            return entities;
        }

        public async Task<IEnumerable<T>> GetAll(
            List<Expression<Func<T, object>>>? includeExpressions, 
            List<ExpressionFilter> filters, 
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if (includeExpressions != null)
            {
                query = includeExpressions.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
            }
            if (filters != null && filters.Any())
            {
                var expressionTree = ExpressionBuilder.ConstructAndExpressionTree<T>(filters);
                query = query.Where(expressionTree);
            }
            var entities = await query.AsNoTracking().ToListAsync(cancellationToken);
            return entities;
        }

        public async Task<IEnumerable<T>> GetAll(
            List<ExpressionFilter> filters, 
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if (filters != null && filters.Any())
            {
                var expressionTree = ExpressionBuilder.ConstructAndExpressionTree<T>(filters);
                query = query.Where(expressionTree);
            }
            var entities = await query.AsNoTracking().ToListAsync(cancellationToken);
            return entities;
        }


        public async Task<IEnumerable<T>> GetAll(
            List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> includeExpressions,
            List<ExpressionFilter>? filters,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (includeExpressions != null && includeExpressions.Any())
            {
                foreach (var includeExpression in includeExpressions)
                {
                    query = includeExpression(query);
                }
            }

            if (filters != null && filters.Any())
            {
                var expressionTree = ExpressionBuilder.ConstructAndExpressionTree<T>(filters);
                query = query.Where(expressionTree);
            }

            var entities = await query.AsNoTracking().ToListAsync(cancellationToken);
            return entities;
        }

        public virtual async Task<PaginatedData<T>> GetPaginatedData(
            int pageNumber, 
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            var data = await query.ToListAsync(cancellationToken);
            var totalCount = await query.CountAsync(cancellationToken);

            return new PaginatedData<T>(data, totalCount);
        }

        public async Task<PaginatedData<T>> GetPaginatedData(
            int pageNumber, 
            int pageSize, 
            List<ExpressionFilter> filters, 
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsNoTracking();

            // Apply search criteria if provided
            if (filters != null && filters.Any())
            {
                var expressionTree = ExpressionBuilder.ConstructAndExpressionTree<T>(filters);
                query = query.Where(expressionTree);
            }

            // Pagination
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalCount = await query.CountAsync(cancellationToken);

            return new PaginatedData<T>(data, totalCount);
        }

        public virtual async Task<PaginatedData<T>> GetPaginatedData(
            List<Expression<Func<T, object>>> includeExpressions, 
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken, 
            List<ExpressionFilter>? filters = null)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (filters != null && filters.Any())
            {
                var filterExpression = ExpressionBuilder.ConstructAndExpressionTree<T>(filters);
                if (filterExpression != null)
                {
                    query = query.Where(filterExpression);
                }
            }

            if (includeExpressions != null && includeExpressions.Any())
            {
                query = includeExpressions.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
            }

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var totalCount = await query.CountAsync(cancellationToken);

            return new PaginatedData<T>(data, totalCount);
        }

        public async Task<PaginatedData<T>> GetPaginatedData(
            int pageNumber, 
            int pageSize, 
            List<ExpressionFilter> filters, 
            string sortBy, 
            string sortOrder, 
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsNoTracking();

            // Apply search criteria if provided
            if (filters != null && filters.Any())
            {
                var expressionTree = ExpressionBuilder.ConstructAndExpressionTree<T>(filters);
                query = query.Where(expressionTree);
            }

            // Add sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                var orderByExpression = GetOrderByExpression<T>(sortBy);
                query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(orderByExpression) : query.OrderBy(orderByExpression);
            }

            // Pagination
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalCount = await query.CountAsync(cancellationToken);

            return new PaginatedData<T>(data, totalCount);
        }

        private Expression<Func<T, object>> GetOrderByExpression<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var conversion = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(conversion, parameter);
        }

        public virtual async Task<T> GetById<Tid>(Tid id, CancellationToken cancellationToken = default)
        {
            var data = await _dbContext.Set<T>()
                .FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken);
            return data == null ? throw new NotFoundException("No data found") : data;
        }

        public async Task<T> GetById<Tid>(
            Tid id, 
            List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> includeExpressions, 
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (includeExpressions != null && includeExpressions.Any())
            {
                foreach (var includeExpression in includeExpressions)
                {
                    query = includeExpression(query);
                }
            }

            // Apply ID filter
            query = query.Where(e => EF.Property<T>(e, "Id").Equals(id));
            // Retrieve the entity
            var data = await query.FirstOrDefaultAsync(cancellationToken);

            return data == null ? throw new NotFoundException("No data found") : data;
        }

        public virtual async Task<T> GetById<Tid>(
            List<Expression<Func<T, object>>> includeExpressions, 
            Tid id, 
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (includeExpressions != null)
            {
                query = includeExpressions.Aggregate(query, (current, include) => current.Include(include));
            }

            var data = await query.SingleOrDefaultAsync(x => EF.Property<Tid>(x, "Id").Equals(id), cancellationToken);

            return data ?? throw new NotFoundException("No data found");
        }

        public async Task<bool> IsExists<Tvalue>(string key, Tvalue value, CancellationToken cancellationToken = default)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return await _dbContext.Set<T>().AnyAsync(lambda, cancellationToken);
        }

        //Before update existence check
        public async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value, CancellationToken cancellationToken = default)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(property, constant);

            var idProperty = Expression.Property(parameter, "Id");
            var idEquality = Expression.NotEqual(idProperty, Expression.Constant(id));

            var combinedExpression = Expression.AndAlso(equality, idEquality);
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

            return await _dbContext.Set<T>().AnyAsync(lambda, cancellationToken);
        }

        public async Task<T> Create(T model, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(model, cancellationToken);
            //await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task CreateRange(List<T> model, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddRangeAsync(model, cancellationToken);
            //await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(T model, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Update(model);
            //await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(T model, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(model);
            //await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRange(List<T> models, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().RemoveRange(models);
            //await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
