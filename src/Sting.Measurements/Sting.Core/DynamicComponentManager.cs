using Sting.Core.Contracts;

namespace Sting.Core
{
    /// <summary>
    /// Manages the dynamic parts of the application that can be configured trough the front end
    /// </summary>
    public class DynamicComponentManager : IDynamicComponentManager
    {
        // Deliberate avoidance of properties
        private IDatabase _database;
        private ILogger _logger;

        public DynamicComponentManager(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Sets the currently used database for the application.
        /// </summary>
        /// <param name="database">The database which should be used.</param>
        public void SetDatabase(IDatabase database) => _database = database;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(IDatabase database) => _database;

    }
}
