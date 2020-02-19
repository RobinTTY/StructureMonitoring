namespace Sting.Core.Contracts
{
    public interface IDynamicComponentManager
    {
        /// <summary>
        /// Sets the currently used database for the application.
        /// </summary>
        /// <param name="database">The database which should be used.</param>
        void SetDatabase(IDatabase database);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        IDatabase GetDatabase(IDatabase database);
    }
}