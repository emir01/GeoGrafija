namespace Model.Interfaces
{
    public interface IDatabaseRepository
    {
        /// <summary>
        /// Method used to Persist Datasource Changes
        /// </summary>
        void SaveChanges();
    }
}
