namespace HangmanMVC.Data.Common
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DbGenericRepository<T, TKey> : IDbGenericRepository<T, TKey>
        where T : class
    {
        public DbGenericRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", nameof(context));
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        private IDbSet<T> DbSet { get; }

        private DbContext Context { get; }

        public IQueryable<T> AllWithDeleted()
        {
            return this.DbSet;
        }

        public T GetById(TKey id)
        {
            return this.DbSet.Find(id);
        }

        public void Add(T entity)
        {
            this.DbSet.Add(entity);
        }

        public void HardDelete(T entity)
        {
            this.DbSet.Remove(entity);
        }

        public void Save()
        {
            this.Context.SaveChanges();
        }
    }
}
