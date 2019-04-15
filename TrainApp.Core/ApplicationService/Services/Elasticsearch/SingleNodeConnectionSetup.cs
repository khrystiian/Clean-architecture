using Elasticsearch.Net;
using System;

namespace TrainApp.Core.ApplicationService.Services.Elasticsearch
{

    public class SingleNodeConnectionSetup
    {
        private static readonly string _uri = "http://localhost:9200";
        private static readonly object LowLevelLock = new object();
        private static readonly object ConnectionLock = new object();
        private static volatile SingleNodeConnectionPool _singleNodeConnection;
        private static volatile ConnectionConfiguration _connectionConfiguration;
        private static volatile IElasticLowLevelClient _lowlevelClient;

        public static SingleNodeConnectionPool SingleNodeConnection
        {
            get
            {
                if (_singleNodeConnection != null) { return _singleNodeConnection; }
                lock (ConnectionLock)
                {
                    if (_singleNodeConnection != null) { return _singleNodeConnection; }

                    var uri = new Uri(_uri);
                    _singleNodeConnection = new SingleNodeConnectionPool(uri);
                }
                return _singleNodeConnection;
            }
        }


        public static IElasticLowLevelClient ElasticLowLevelClient
        {
            get
            {
                if (_lowlevelClient != null) { return _lowlevelClient; }
                lock (LowLevelLock)
                {
                    if (_lowlevelClient != null) { return _lowlevelClient; }

                    _connectionConfiguration = new ConnectionConfiguration(SingleNodeConnection)
                        .RequestTimeout(TimeSpan.FromMinutes(2))
                        .ThrowExceptions();
                    _lowlevelClient = new ElasticLowLevelClient(_connectionConfiguration);
                }
                return _lowlevelClient;
            }
        }

     
    }
}
