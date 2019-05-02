using System.Collections.Generic;
using MongoDB.Driver;
using Sting.Models;

namespace Sting.Backend.Services.Filters
{
    public static class TelemetryDataFilter
    {
        private static readonly FilterDefinitionBuilder<TelemetryData> Builder = new FilterDefinitionBuilder<TelemetryData>();

        public static FilterDefinition<TelemetryData> CreateFilter(long? timeStampStart = null, long? timeStampStop = null, string deviceId = null)
        {
            var filterDefinitions = new List<FilterDefinition<TelemetryData>>();

            if (timeStampStart != null)
                filterDefinitions.Add(Builder.Gte(telemetryData => telemetryData.UnixTimeStamp, timeStampStart));

            if (timeStampStop != null)
                filterDefinitions.Add(Builder.Lte(telemetryData => telemetryData.UnixTimeStamp, timeStampStop));

            if(deviceId != null)
                filterDefinitions.Add(Builder.Eq(telemetryData => telemetryData.DeviceId, deviceId));

            var filter = Builder.Empty;

            foreach (var filterDefinition in filterDefinitions)
            {
                filter &= filterDefinition;
            }

            return filter;
        }
    }
}
