using System.IO;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using Windows.ApplicationModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Sting.Units;

namespace Sting.Storage
{
    public class Database
    {
        private string _clusterConnectionString;
        private string _databaseName;
        private IMongoDatabase _database;

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
            _database = client.GetDatabase(_databaseName);
        }

        public void AddTelemetryData(TelemetryData telemetryData)
        {
            var collection = _database.GetCollection<BsonDocument>("TelemetryData");
            var document = new BsonDocument()
            {
                {"TimeStamp", telemetryData.UnixTimeStampMilliseconds },
                {"Temperature", telemetryData.Temperature },
                {"Humidity", telemetryData.Humidity },
                {"Air Pressure", telemetryData.Pressure }
            };

            collection.InsertOneAsync(document);
        }

        private void Ping()
        {
            _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
        }
    }
}
