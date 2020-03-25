using Sting.Core.Communication;

namespace Sting.Core.Contracts
{
    public interface IConfigurationLoader
    {
        /// <summary>
        /// Loads a <see cref="SystemConfiguration"/> and makes it ready for use.
        /// </summary>
        /// <param name="configuration">The <see cref="SystemConfiguration"/> to be applied.</param>
        void LoadConfiguration(SystemConfiguration configuration);
    }
}