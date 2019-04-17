using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Text;
using TrainApp.Core.ApplicationService.Interfaces;
using Infrastructure.DataAccess;
using Infrastructure.Repositories;
using TrainApp.Core.ApplicationService.Services;
using TrainApp.Core.Entity;
using System.Linq;
using System.Collections.Generic;
using TrainApp.Core.ApplicationService.Services.Elasticsearch;

namespace ElasticSearchPractice
{
    public class Class1
    {
        private List<LegModel> trips = new List<LegModel>();
        public Class1()
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ITripService, TripService>()
                .AddSingleton<IEntityRepository<LegModel>, EntityRepository<TrainAppEntities, LegModel>>()
                .AddSingleton<TrainApp.Core.ApplicationService.IRepository<Trip>, TripRepository>()
                .BuildServiceProvider();

            //do the actual work here
            var bar = serviceProvider.GetService<ITripService>();
             trips = bar.GetAllTrips().ToList();
        }

        public void RunElasticsearch()
        {
            ElasticsearchConfigSingleNode();
            //ElasticsearchConfigMultiNodes();
        }


        #region singleNode
        private void ElasticsearchConfigSingleNode()
        {
            //Configuration
            var lowlevelClient = SingleNodeConnectionSetup.ElasticLowLevelClient;


            //Serialize and Index to Elasticsearch
             //IndexToElasticSearch(lowlevelClient);


            //search
            Search(lowlevelClient);
            Search2(lowlevelClient);
        }

        private void IndexToElasticSearch(IElasticLowLevelClient lowlevelClient)
        {
            for (int i = 0; i <= trips.Count-1; i++)
            {
                var routes = new List<StepModel>();
                for (int j = 0; j < trips[i].Steps.Count; j++)
                {
                    routes.Add(new StepModel
                    {
                        Distance = trips[i].Steps[j].Distance,
                        Duration = trips[i].Steps[j].Duration,
                        Price = trips[i].Steps[j].Price,
                        Travel_mode = trips[i].Steps[j].Travel_mode,
                        Transit = new TransitDetailModel
                        {
                            Arrival_stop = trips[i].Steps[j].Transit.Arrival_stop,
                            Departure_stop = trips[i].Steps[j].Transit.Departure_stop,
                            Arrival_time = trips[i].Steps[j].Transit.Arrival_time,
                            Departure_time = trips[i].Steps[j].Transit.Departure_time
                        }
                    });
                }

                var trip = new LegModel
                {
                    Start_address = trips[i].Start_address,
                    End_address = trips[i].End_address,
                    Arrival_time = trips[i].Arrival_time,
                    Departure_time = trips[i].Departure_time,
                    Distance = trips[i].Distance,
                    Duration = trips[i].Duration,
                    RoutePreference = trips[i].RoutePreference,
                    Price = trips[i].Price,
                    Steps = routes
                };
                var indexResponse = lowlevelClient.Index<BytesResponse>("legs", "trip", i.ToString(), PostData.Serializable(trip));
                var successful = indexResponse.Success;
               // byte[] responseBytes = indexResponse.Body;
               // var response = Encoding.UTF8.GetString(responseBytes);
            }
        }

        private void Search(IElasticLowLevelClient lowLevelClient)
        {
            var searchResponse = lowLevelClient.Search<BytesResponse>("legs", "trip", PostData.Serializable(new
            {
                from = 0,
                size = 5,
                query = new
                {
                    match = new
                    {
                        Start_address = "Vejle"
                    }
                }
            }));

            var successful = searchResponse.Success;
            var successOrKnownError = searchResponse.SuccessOrKnownError;
            var exception = searchResponse.OriginalException;
            var responseBytes = searchResponse.Body;
            var result = Encoding.UTF8.GetString(responseBytes);

        }

        private void Search2(IElasticLowLevelClient lowLevelClient)
        {
            var searchResponse = lowLevelClient.Search<BytesResponse>("legs", "trip", @"
{

    ""query"": {
        ""match"": {
            ""Start_address"": ""Vejle""
        }
    }
}");

            var responseBytes = searchResponse.Body;
            var result = Encoding.UTF8.GetString(responseBytes);
        }
        #endregion


        #region multiNode
        private void ElasticsearchConfigMultiNodes()
        {
            // Singleton configuration
            var client = MultiNodesConnectionSetup.ElasticClient;


            //Serialize and Index to Elasticsearch
            //MultiNodesIndex(client);


            //Search
             Search3(client);
             Search4(client);
        }

        private void MultiNodesIndex(IElasticClient client)
        {
            var tweet = new Tweet
            {
                Id = 1,
                User = "kimchy",
                PostDate = new DateTime(2009, 11, 15),
                Message = "Trying out NEST, so far so good?"
            };

            var indexResponse = client.Index(tweet, idx => idx.Index("mypersonindex"));
            var response = client.Get<Tweet>(1, idx => idx.Index("mytweetindex")); // returns an IGetResponse mapped 1-to-1 with the Elasticsearch JSON response
            Tweet newTweet = response.Source; // the original document
        }

        private void Search3(IElasticClient client)
        {
            var response = client.Search<Trip>(s => s
                                 .From(0) //results to skip
                                 .Size(10) //number of results
                                 .Query(q => q
                                 .Term(t => t.Start_address, "vejle") || q
                                 .Match(mq => mq
                                        .Field(f => f.Start_address)
                                        .Query("vejle"))
                                        )
                                 );
            var b = response;
        }

        private void Search4(IElasticClient client)
        {
            var request = new SearchRequest
            {
                From = 0,
                Size = 10,
                Query = new TermQuery { Field = "End_address", Value = "Bucharest" } // ||
            //new MatchQuery { Field = "description", Query = "nest" }
            };

            var response = client.Search<Trip>(request);
        }
        #endregion



    }
}
