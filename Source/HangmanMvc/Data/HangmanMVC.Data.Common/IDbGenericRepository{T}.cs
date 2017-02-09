namespace HangmanMVC.Data.Common
{
    using System.Linq;

    public interface IDbGenericRepository<T, TKey>
    {
        IQueryable<T> AllWithDeleted();

        T GetById(TKey id);

        void Add(T entity);

        void HardDelete(T entity);

        void Save();
    }
}
