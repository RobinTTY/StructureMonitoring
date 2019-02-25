using System.IO;
using System.Xml.Linq;
using Windows.ApplicationModel;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Sting.Storage
{
    public class Database
    {
        private string _clusterConnectionString;
        private string _databaseName;

        public Database()
        {
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            var xmlFilePath = Path.Combine(Package.Current.InstalledLocation.Path, "AppConfiguration.xml");
            var settings = XDocument.Load(xmlFilePath).Root?.Element("appSettings");

            _clusterConnectionString = settings?.Element("ClusterConnectionString")?.Value;
            _databaseName = settings?.Element("DatabaseName")?.Value;
        }

        public void InitConnection()
        {
            var client = new MongoClient(_clusterConnectionString);
            var database = client.GetDatabase(_databaseName);
            database.RunCommandAsync((Command<BsonDocument>) "{ping:1}" ).Wait();
        }


    }
}
