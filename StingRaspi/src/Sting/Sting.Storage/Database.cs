using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Sting.Storage
{
    public class Database
    {
        public void InitConnection()
        {
            var client = new MongoClient("mongodb+srv://Robin:QLo0iZM4SWklq6HkL6gW@stingmeasurements-2u2yu.mongodb.net/test?retryWrites=true");
            var database = client.GetDatabase("StingMeasurements");
            database.RunCommandAsync((Command<BsonDocument>) "{ping:1}" ).Wait();
        }
    }
}
