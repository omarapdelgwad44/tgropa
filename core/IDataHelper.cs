namespace tgropa.Core
{
    public interface IDataHelper<T>
    {
        List<T> GetAllData();
        List<T> Search(string searchItem);
        T Find(int Id);
        int Add(T entity);
        int Update(T entity);
        int Delete(int Id);
    }
}
