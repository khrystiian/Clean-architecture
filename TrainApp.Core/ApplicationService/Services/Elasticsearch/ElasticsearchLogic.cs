using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elasticsearch.Net;
using Infrastructure.DataAccess;
using Nest;
using TrainApp.Core.ApplicationService.Interfaces;
using TrainApp.Core.DomainService;
using TrainApp.Core.Entity;

namespace TrainApp.Core.ApplicationService.Services.Elasticsearch
{
    public class ElasticsearchLogic : IElasticsearchLogic
    {
        private readonly ITripService tripService;
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        public ElasticsearchLogic(ITripService _tService)
        {
            tripService = _tService ?? throw new ArgumentNullException(nameof(_tService));
        }

        #region LowlevelElasticsearch
        public bool IndexToElasticsearch(IElasticLowLevelClient client)
        {
            bool successful = false;
            var trips = tripService.GetAllTrips().ToList();

            for (int i = 0; i <= trips.Count - 1; i++)
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
                var indexResponse = client.Index<BytesResponse>("legautocomplete", "trip", i.ToString(), PostData.Serializable(trip));
                successful = indexResponse.Success;
                if (!successful)
                {
                    log.Fatal("Elasticsearch indexing failure.");
                    break;
                }
                // byte[] responseBytes = indexResponse.Body;
                // var response = Encoding.UTF8.GetString(responseBytes);
            }
            return successful;
        }

        public string IndexSearch(string input) //TO DO. SPECIFY ANALYZER FOR SEARCH-TO-TYPE
        {
            var client = SingleNodeConnectionSetup.ElasticLowLevelClient;
            var successful = true;// IndexToElasticsearch(client);
            if (successful)
            {
                var searchResponse = client.Search<BytesResponse>("legindex", "trip", PostData.Serializable(new
                {
                    from = 0,
                    size = 5,
                    //analyzer ="autocomplete",
                    query = new
                    {
                        match = new
                        {
                            Start_address = input
                        }
                    }
                }));
                //var successful = searchResponse.Success;
                //var successOrKnownError = searchResponse.SuccessOrKnownError;
                //var exception = searchResponse.OriginalException;
                var responseBytes = searchResponse.Body;
                return Encoding.UTF8.GetString(responseBytes);
            }
            else
            {
                log.Fatal("Elasticsearch search failure.");
                return string.Empty;
            }
        }

        public string IndexSearch2()
        {
            var client = SingleNodeConnectionSetup.ElasticLowLevelClient;
            var successful = IndexToElasticsearch(client);
            if (successful)
            {
                var searchResponse = client.Search<BytesResponse>("legs", "trip", @"
{

    ""query"": {
        ""match"": {
            ""Start_address"": ""Vejle""
        }
    }
}");

                var responseBytes = searchResponse.Body;
                return Encoding.UTF8.GetString(responseBytes);
            }
            else
            {
                log.Fatal("Elasticsearch search failure.");
                return string.Empty;
            }
        }
        #endregion


        #region NEST
        public bool IndexToNestElasticsearch(IElasticClient client)
        {
            var tweet = new
            {
                Id = 1,
                User = "kimchy",
                PostDate = new DateTime(2009, 11, 15),
                Message = "Trying out NEST, so far so good?"
            };

            var indexResponse = client.Index(tweet, idx => idx.Index("mypersonindex"));
            return indexResponse.IsValid;
            // var response = client.Get<Tweet>(1, idx => idx.Index("mytweetindex")); // returns an IGetResponse mapped 1-to-1 with the Elasticsearch JSON response
            // Tweet newTweet = response.Source; // the original document
        }

        public void NestIndexSearch()
        {
            var client = MultiNodesConnectionSetup.ElasticClient;
            var successful = IndexToNestElasticsearch(client);
            if (successful)
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
            }
        }

        public void NestIndexSearch2()
        {
            var client = MultiNodesConnectionSetup.ElasticClient;
            var succesful = IndexToNestElasticsearch(client);
            if (succesful)
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
        }
        #endregion
    }
}
