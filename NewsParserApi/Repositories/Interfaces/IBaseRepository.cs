﻿using System.Linq.Expressions;

namespace NewsParserApi.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        void SaveChanges();
    }
}