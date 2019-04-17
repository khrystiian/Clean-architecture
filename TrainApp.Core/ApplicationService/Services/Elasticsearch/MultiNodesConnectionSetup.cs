using Elasticsearch.Net;
using Nest;
using System;

namespace TrainApp.Core.ApplicationService.Services.Elasticsearch
{
    public class MultiNodesConnectionSetup
    {
        private static readonly string _uri = "http://localhost:9200";
        private static readonly object LevelLock = new object();
        private static readonly object ConnectionLock = new object();
        private static volatile StaticConnectionPool _multiNodeConnection;
        private static volatile ConnectionSettings _connectionConfiguration;
        private static volatile IElasticClient _nestClient;

        public static StaticConnectionPool MultiNodesConnection
        {
            get
            {
                if (_multiNodeConnection != null) { return _multiNodeConnection; }
                lock (ConnectionLock)
                {
                    if (_multiNodeConnection != null) { return _multiNodeConnection; }

                    var uri = new Uri[]
                    {
                        new Uri(_uri)
                    };
                    _multiNodeConnection = new StaticConnectionPool(uri);
                }
                return _multiNodeConnection;
            }
        }


        public static IElasticClient ElasticClient
        {
            get
            {
                if (_nestClient != null) { return _nestClient; }
                lock (LevelLock)
                {
                    if (_nestClient != null) { return _nestClient; }

                    _connectionConfiguration = new ConnectionSettings(MultiNodesConnection)
                        .RequestTimeout(TimeSpan.FromMinutes(2))
                        .ThrowExceptions();
                    _nestClient = new ElasticClient(_connectionConfiguration);
                }
                return _nestClient;
            }
        }

    }
}
